using System;
using System.Data;
using System.Collections.Generic;

namespace OZGNet.WebForm.Cart
{
    /// <summary>
    /// 购物车接口
    /// </summary>
    public interface ICart
    {
        /// <summary>
        /// 获取cookie购物车,Hashtable,Key为产品ID,Value为产品个数
        /// </summary>
        /// <returns></returns>
        Dictionary<object, int> GetList();
        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <returns></returns>
        Dictionary<object, object> GetObjects();
        /// <summary>
        /// 添加产品进购物车
        /// </summary>
        /// <param name="pid">产品ID</param>
        /// <param name="pnum">产品个数</param>
        void Add(object pid, int pnum);
        /// <summary>
        /// 添加产品进购物车
        /// </summary>
        /// <param name="pid">产品ID</param>
        /// <param name="pnum">产品个数</param>
        /// <param name="obj">其他对象</param>
        void Add(object pid, int pnum, object obj);
        /// <summary>
        /// 删除购物车的产品
        /// </summary>
        /// <param name="pid">产品ID</param>
        void Remove(object pid);
        /// <summary>
        /// 清空购物车里面的所有产品
        /// </summary>
        void RemoveAll();
        /// <summary>
        /// 修改购物车产品的个数
        /// </summary>
        /// <param name="pid">产品ID</param>
        /// <param name="pnum">产品个数</param>
        void Edit(object pid, int pnum);
        /// <summary>
        /// 修改购物车产品的个数
        /// </summary>
        /// <param name="pid">产品ID</param>
        /// <param name="pnum">产品个数</param>
        /// <param name="obj">其他对象</param>
        void Edit(object pid, int pnum, object obj);
        /// <summary>
        /// 判断该产品是否存在购物车
        /// </summary>
        /// <param name="pid">产品ID</param>
        /// <returns></returns>
        bool Exist(object pid);
        /// <summary>
        /// 获取购物车字符(格式为:产品ID_产品个数,产品ID_产品个数,产品ID_产品个数)
        /// </summary>
        /// <returns></returns>
        string GetString();
        /// <summary>
        /// 获取购物车产品的总数
        /// </summary>
        /// <returns></returns>
        int GetProductsTotal();
        /// <summary>
        /// 获取产品ID的对应索引
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        int GetIndex(object pid);
        /// <summary>
        /// 获取对应产品ID的项
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        KeyValuePair<object, int> Get(object pid);
        /// <summary>
        ///  获取对应产品ID的其他对象
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        KeyValuePair<object, object> GetObject(object pid);
        /// <summary>
        /// 获取或设置购物车的名称
        /// </summary>
        string CartName { get; set; }
        /// <summary>
        /// 获取或设置购物车的对象名称
        /// </summary>
        string CartObjectsName { get; set; }
        
    }
}
