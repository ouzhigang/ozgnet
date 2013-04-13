using System;
using System.Collections.Generic;
using System.Text;

namespace OZGNet.WebFormControls.ThirdPart.Winson.AlertAndPopup
{
    /// <summary>
    /// 类 编 号：JavaScriptWriter
    /// 类 名 称：JavaScriptWriter
    /// 内容摘要：   主要功能是实现日期控键客户端脚本输出。
    /// </summary>
    internal class JavaScriptWriter
    {
        public JavaScriptWriter() { }

        #region 变量区

        private StringBuilder sb = new StringBuilder();
        private int currIndent = 0;
        private int openBlocks = 0;
        private bool format = false;

        #endregion

        #region 方法区

        /// <summary>
        /// 在书入到页面时是否需要格式
        /// </summary>
        /// <param name="Formatted">需要格式?</param>
        public JavaScriptWriter(bool Formatted)
        {
            format = Formatted;
        }

        /// <summary>
        /// 当前的缩进层次
        /// </summary>
        public int Indent
        {
            get { return currIndent; }
            set { currIndent = value; }
        }

        /// <summary>
        /// 新增一行javascript代码
        /// </summary>
        /// <param name="parts">代码字串的数组</param>
        public void AddLine(params string[] parts)
        {
            // 如果有格式设置，则加入缩进的行
            if (format)
                for (int i = 0; i < currIndent; i++)
                    sb.Append("\t");

            foreach (string part in parts)
                sb.Append(part);

            if (format)
                sb.Append(Environment.NewLine);
            else
                if (parts.Length > 0)
                    sb.Append(" ");
        }

        /// <summary>
        /// 输入"{"，并使层次缩进一层
        /// </summary>
        public void OpenBlock()
        {
            AddLine("{");
            currIndent++;
            openBlocks++;
        }

        /// <summary>
        /// 输入"{"，并使层次扩展一层
        /// </summary>
        public void CloseBlock()
        {
            // 检查一个function有没有"{"
            if (openBlocks < 1)
                throw new InvalidOperationException("在调用JavaScriptWriter.CloseBlock()时没有先前的JavaScriptWriter.OpenBlock()调用");

            currIndent--;
            openBlocks--;
            AddLine("}");
        }

        /// <summary>
        /// 加入注解的(为javascript加入注解)
        /// </summary>
        /// <param name="CommentText">注解的字串数组.</param>
        public void AddCommentLine(params string[] CommentText)
        {
            if (format)
            {
                for (int i = 0; i < currIndent; i++)
                    sb.Append("\t");

                sb.Append("// ");

                foreach (string part in CommentText)
                    sb.Append(part);

                sb.Append(Environment.NewLine);
            }
        }

        /// <summary>
        /// 转换开发和结束的javascript的标记，并在中间加入已加入的javascrpt的代码
        /// </summary>
        /// <returns>返回标准的javascript代码</returns>
        public override string ToString()
        {
            if (openBlocks > 0)
                throw new InvalidOperationException("JavaScriptWriter: 没有相应的关闭标识");

            return String.Format(
                "<script language=\"javascript\" type=\"text/javascript\">{0}{1}</script>",
                Environment.NewLine,
                sb
                );
        }

        #endregion
    }
}
