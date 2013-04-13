using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.Framework
{
    /// <summary>
    /// Spring帮助类
    /// </summary>
    public class SpringHelper
    {
        #region 获取Spring.NET配置文件的objects节里面的object对象
        /// <summary>
        /// 获取Spring.NET配置文件的objects节里面的object对象
        /// </summary>
        /// <param name="BeanID">指定ID</param>
        /// <returns></returns>
        public static Object GetSpringBeanObject(string BeanID)
        {
            Spring.Context.IApplicationContext context = Spring.Context.Support.ContextRegistry.GetContext();
            return context.GetObject(BeanID);
        }
        #endregion
        #region 获取XML文件的objects节里面的object对象
        /// <summary>
        /// 获取XML文件的objects节里面的object对象
        /// </summary>
        /// <param name="XmlPath">XML文件路径</param>
        /// <param name="BeanID">指定ID</param>
        /// <returns></returns>
        public static Object GetSpringBeanObject(string XmlPath, string BeanID)
        {
            Spring.Context.IApplicationContext context = new Spring.Context.Support.XmlApplicationContext(XmlPath);
            return context.GetObject(BeanID);
        }
        #endregion
    }
}
