using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.Data.AR
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class Criteria
    {
        private string _alias = null;
        private string _where = null;
        private string _func = null;
        private string _select = null;
        private string _group = null;
        private string _having = null;
        private string _join = null;
        private int _limit = 0;
        private int _offset = 0;
        private string _order = null;

        /// <summary>
        /// 当前表的别名（as）
        /// </summary>
        public string Alias
        {
            get
            {
                return this._alias;
            }
            set
            {
                this._alias = value;
            }
        }

        /// <summary>
        /// where 条件
        /// </summary>
        public string Where
        {
            get
            {
                return this._where;
            }
            set
            {
                this._where = value;
            }
        }

        /// <summary>
        /// FindOne时使用的函数（例：count）
        /// </summary>
        public string Func
        {
            get
            {
                return this._func;
            }
            set
            {
                this._func = value;
            }
        }

        /// <summary>
        /// Select 的字段
        /// </summary>
        public string Select
        {
            get
            {
                return this._select;
            }
            set
            {
                this._select = value;
            }
        }

        /// <summary>
        /// Group by
        /// </summary>
        public string Group
        {
            get
            {
                return this._group;
            }
            set
            {
                this._group = value;
            }
        }

        /// <summary>
        /// Having
        /// </summary>
        public string Having
        {
            get
            {
                return this._having;
            }
            set
            {
                this._having = value;
            }
        }

        /// <summary>
        /// Join
        /// </summary>
        public string Join
        {
            get
            {
                return this._join;
            }
            set
            {
                this._join = value;
            }
        }

        /// <summary>
        /// 显示记录数
        /// </summary>
        public int Limit
        {
            get
            {
                return this._limit;
            }
            set
            {
                this._limit = value;
            }
        }

        /// <summary>
        /// 偏移记录
        /// </summary>
        public int Offset
        {
            get
            {
                return this._offset;
            }
            set
            {
                this._offset = value;
            }
        }

        /// <summary>
        /// Order by
        /// </summary>
        public string Order
        {
            get
            {
                return this._order;
            }
            set
            {
                this._order = value;
            }
        }

    }
}
