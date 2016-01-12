using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.WinForm
{
    /// <summary>
    /// 用于保存Winform的异步状态
    /// </summary>
    public class AsyncStatus
    {
        private object _WorkObject = null;
        private byte[] _Buffer = null;
        private int _BufferSize = 1024;
        private Dictionary<string, object> _OtherObject = new Dictionary<string, object>();

        /// <summary>
        /// 实例化AsyncStatus
        /// </summary>
        public AsyncStatus()
        {
            
        }
        /// <summary>
        /// 获取或设置工作对象
        /// </summary>
        public object WorkObject
        {
            get
            {
                return this._WorkObject;
            }
            set
            {
                this._WorkObject = value;
            }
        }
        /// <summary>
        /// 获取或设置缓冲区
        /// </summary>        
        public byte[] Buffer
        {
            get
            {
                if (this._Buffer == null)
                {
                    this._Buffer = new byte[this._BufferSize];                    
                }
                return this._Buffer;
            }
            set
            {
                this._Buffer = value;
            }
        }
        /// <summary>
        /// 获取或设置缓冲区大小
        /// </summary>
        public int BufferSize
        {
            get
            {
                return this._BufferSize;
            }
            set
            {
                this._BufferSize = value;
            }
        }
        /// <summary>
        /// 获取其他对象(此对象集合可修改)
        /// </summary>
        public Dictionary<string, object> OtherObject
        {
            get
            {
                return this._OtherObject;
            }
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        public void ResetData()
        {
            this._WorkObject = null;
            this._Buffer = null;
            this._BufferSize = 1024;
            this._OtherObject.Clear();
        }

    }
}
