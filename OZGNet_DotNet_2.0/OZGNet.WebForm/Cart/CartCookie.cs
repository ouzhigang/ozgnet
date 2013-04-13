using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;

namespace OZGNet.WebForm.Cart
{
    /// <summary>
    /// Cookie购物车,实现ICart接口
    /// </summary>
    public class CartCookie : ICart
    {
        private string _CartName;
        private string _CartObjectsName;

        #region 构造函数
        /// <summary>
        /// 实例化CartCookie
        /// </summary>
        public CartCookie()
        {
            this._CartName = "OZG_Cart";
            this._CartObjectsName = "OZG_Cart_Objects";
        }
        /// <summary>
        /// 实例化CartCookie
        /// </summary>
        /// <param name="_CartName">购物车名称</param>
        /// <param name="_CartObjectsName"></param>
        public CartCookie(string _CartName, string _CartObjectsName)
        {
            this._CartName = _CartName;
            this._CartObjectsName = _CartObjectsName;
        }
        #endregion

        #region 获取cookie购物车
        /// <summary>
        /// 获取cookie购物车
        /// </summary>
        /// <returns></returns>
        public Dictionary<object, int> GetList()
        {
            Dictionary<object, int> map = null;
            if (OZGNet.WebForm.Utility.ExistsCookie(this._CartName))
            {
                map = OZGNet.WebForm.CookieHelper.GetCookieSerialize(this._CartName) as Dictionary<object, int>;
            }
            else
            {
                map = new Dictionary<object, int>();
                OZGNet.WebForm.CookieHelper.SetCookieSerialize(this._CartName, map);
            }
            return map;
        }
        #endregion

        #region 获取购物车对象
        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <returns></returns>
        public Dictionary<object, object> GetObjects()
        {
            Dictionary<object, object> map = null;
            if (OZGNet.WebForm.Utility.ExistsCookie(this._CartObjectsName))
            {
                map = OZGNet.WebForm.CookieHelper.GetCookieSerialize(this._CartObjectsName) as Dictionary<object, object>;
            }
            else
            {
                map = new Dictionary<object, object>();
                OZGNet.WebForm.CookieHelper.SetCookieSerialize(this._CartObjectsName, map);
            }
            return map;
        } 
        #endregion

        #region 添加产品进购物车
        /// <summary>
        /// 添加产品进购物车
        /// </summary>
        /// <param name="pid">产品ID</param>
        /// <param name="pnum">产品个数</param>
        public void Add(object pid, int pnum)
        {
            Add(pid, pnum, null);
        }
        /// <summary>
        /// 添加产品进购物车
        /// </summary>
        /// <param name="pid">产品ID</param>
        /// <param name="pnum">产品个数</param>
        /// <param name="obj"></param>
        public void Add(object pid, int pnum, object obj)
        {
            Dictionary<object, int> CartList = GetList();
            Dictionary<object, object> CartObjects = GetObjects();

            bool has = Exist(pid);

            if (!has)
            {
                CartList.Add(pid, pnum);
                //保存购物车
                OZGNet.WebForm.CookieHelper.SetCookieSerialize(this._CartName, CartList);

                if (obj != null)
                {
                    CartObjects.Add(pid, obj);
                    OZGNet.WebForm.CookieHelper.SetCookieSerialize(this._CartObjectsName, CartObjects);
                }
            }
            else
            {
                int tmp = CartList[pid];
                tmp += pnum;

                if (obj != null)
                {
                    Edit(pid, tmp, obj);
                }
                else
                {
                    Edit(pid, tmp);
                }                
            }
        }
        #endregion

        #region 删除购物车的产品
        /// <summary>
        /// 删除购物车的产品
        /// </summary>
        /// <param name="pid">产品ID</param>
        public void Remove(object pid)
        {
            Dictionary<object, int> CartList = GetList();
            Dictionary<object, object> CartObjects = GetObjects();
            bool has = Exist(pid);
            if (has)
            {
                CartList.Remove(pid);
                //保存购物车
                OZGNet.WebForm.CookieHelper.SetCookieSerialize(this._CartName, CartList);

                if (CartObjects.ContainsKey(pid))
                {
                    CartObjects.Remove(pid);
                    OZGNet.WebForm.CookieHelper.SetCookieSerialize(this._CartObjectsName, CartObjects);
                }
            }
        }
        #endregion

        #region 清空购物车里面的所有产品
        /// <summary>
        /// 清空购物车里面的所有产品
        /// </summary>
        public void RemoveAll()
        {
            Dictionary<object, int> CartList = GetList();
            Dictionary<object, object> CartObjects = GetObjects();
            CartList.Clear();
            CartObjects.Clear();

            //保存购物车
            OZGNet.WebForm.CookieHelper.SetCookieSerialize(this._CartName, CartList);

            OZGNet.WebForm.CookieHelper.SetCookieSerialize(this._CartObjectsName, CartObjects);
        }
        #endregion

        #region 修改购物车产品的个数
        /// <summary>
        /// 修改购物车产品的个数
        /// </summary>
        /// <param name="pid">产品ID</param>
        /// <param name="pnum">产品个数</param>
        public void Edit(object pid, int pnum)
        {
            Edit(pid, pnum, null);
        }
        /// <summary>
        /// 修改购物车产品的个数
        /// </summary>
        /// <param name="pid">产品ID</param>
        /// <param name="pnum">产品个数</param>
        /// <param name="obj"></param>
        public void Edit(object pid, int pnum, object obj)
        {
            Dictionary<object, int> CartList = GetList();
            Dictionary<object, object> CartObjects = GetObjects();
            
            bool has = Exist(pid);

            if (has)
            {
                if (CartList[pid] != pnum)
                {
                    CartList[pid] = pnum;
                    //保存购物车
                    OZGNet.WebForm.CookieHelper.SetCookieSerialize(this._CartName, CartList);
                }

                if (obj != null)
                {
                    if (CartObjects.ContainsKey(pid))
                    {
                        CartObjects[pid] = obj;
                    }
                    else
                    {
                        CartObjects.Add(pid, obj);
                    }                    
                    OZGNet.WebForm.CookieHelper.SetCookieSerialize(this._CartObjectsName, CartObjects);
                }                
            }
        }
        #endregion

        #region 判断该产品是否存在购物车
        /// <summary>
        /// 判断该产品是否存在购物车
        /// </summary>
        /// <param name="pid">产品ID</param>
        /// <returns></returns>
        public bool Exist(object pid)
        {
            Dictionary<object, int> CartList = GetList();
            bool has = false;
            foreach (KeyValuePair<object,int> item in CartList)
            {
                if (item.Key.Equals(pid))
                {
                    has = true;
                    break;
                }
            }
            return has;
        }
        #endregion

        #region 获取购物车字符(格式为:产品ID_产品个数,产品ID_产品个数,产品ID_产品个数)
        /// <summary>
        /// 获取购物车字符(格式为:产品ID_产品个数,产品ID_产品个数,产品ID_产品个数)
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            string tmp = "";
            Dictionary<object, int> CartList = GetList();
            foreach (KeyValuePair<object, int> item in CartList)
            {
                tmp += item.Key.ToString() + "_" + item.Value.ToString() + ",";
            }

            try
            {
                return tmp.Substring(0, tmp.Length - 1);
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region 获取购物车产品的总数
        /// <summary>
        /// 获取购物车产品的总数
        /// </summary>
        /// <returns></returns>
        public int GetProductsTotal()
        {
            int tmp = 0;
            Dictionary<object, int> CartList = GetList();
            foreach (KeyValuePair<object,int> item in CartList)
            {
                tmp += item.Value;
            }

            return tmp;
        }
        #endregion
        
        #region 相关属性
        /// <summary>
        /// 获取或设置购物车的Cookie名称
        /// </summary>
        public string CartName
        {
            get { return this._CartName; }
            set { this._CartName = value; }
        }
        /// <summary>
        /// 获取或设置购物车的对象名称
        /// </summary>
        public string CartObjectsName
        {
            get { return this._CartObjectsName; }
            set { this._CartObjectsName = value; }
        }
        #endregion

        #region 获取产品ID的对应索引
        /// <summary>
        /// 获取产品ID的对应索引
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public int GetIndex(object pid)
        {
            int i = 0;
            Dictionary<object, int> list = GetList();
            foreach (KeyValuePair<object, int> item in list)
            {
                if (item.Key.Equals(pid))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }
        #endregion

        #region 获取对应产品ID的项
        /// <summary>
        /// 获取对应产品ID的项
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public KeyValuePair<object, int> Get(object pid)
        {
            Dictionary<object, int> list = GetList();
            foreach (KeyValuePair<object, int> item in list)
            {
                if (item.Key.Equals(pid))
                {
                    return item;
                }
            }

            return new KeyValuePair<object, int>();
        }
        #endregion

        #region 获取对应产品ID的对象
        /// <summary>
        /// 获取对应产品ID的对象
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public KeyValuePair<object, object> GetObject(object pid)
        {
            Dictionary<object, object> list = GetObjects();
            foreach (KeyValuePair<object, object> item in list)
            {
                if (item.Key.Equals(pid))
                {
                    return item;
                }
            }

            return new KeyValuePair<object, object>();
        } 
        #endregion

    }
}
