using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Word;
using System.Diagnostics;

/*
 * 
生成文档实例:
OZGNet.Office.WordApp doc = new OZGNet.Office.WordApp();
doc.CreateDocument();
doc.AppendText("sdgdsgh");
doc.SaveAs(@"E:\web\web_aspnet\WEB\1.doc");
doc.Close(false);
doc.Dispose();
 * 


打开保存文档实例 
OZGNet.Office.WordApp doc = new OZGNet.Office.WordApp();
doc.OpenDocument(@"E:\web\web_aspnet\WEB\1.doc", false);
doc.AppendText("sdgdsgh");
doc.Save();
doc.Close(false);
doc.Dispose();
 * 
 */

namespace OZGNet.Office
{
    /// <summary>
    /// 操作Word的帮助类
    /// </summary>
    public class WordApp : IDisposable
    {
        #region 字段
        private _Application m_WordApp = null;
        private _Document m_Document = null;
        private object missing = System.Reflection.Missing.Value;
        #endregion
        #region 构造函数与析构函数
        /// <summary>
        /// 实例化WordApp
        /// </summary>
        public WordApp()
        {
            m_WordApp = new ApplicationClass();
        }
        /// <summary>
        /// 实例化WordApp
        /// </summary>
        ~WordApp()
        {
            try
            {
                if (m_WordApp != null)
                    m_WordApp.Quit(ref missing, ref missing, ref missing);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }
        #endregion
        #region 属性
        /// <summary>
        /// 获取文档
        /// </summary>
        public _Document Document
        {
            get
            {
                return m_Document;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public _Application WordApplication
        {
            get
            {
                return m_WordApp;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int WordCount
        {
            get
            {
                if (m_Document != null)
                {
                    Range rng = m_Document.Content;
                    rng.Select();
                    return m_Document.Characters.Count;
                }
                else
                    return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public object Missing
        {
            get
            {
                return missing;
            }
        }
        #endregion
        #region 基本任务
        #region CreateDocument
        /// <summary>
        /// 生成(临时)文档
        /// </summary>
        /// <param name="template">路径</param>
        public void CreateDocument(string template)
        {
            object obj_template = template;
            if (template.Length <= 0) obj_template = missing;
            m_Document = m_WordApp.Documents.Add(ref obj_template, ref missing, ref missing, ref missing);
        }
        /// <summary>
        /// 生成(临时)文档
        /// </summary>
        public void CreateDocument()
        {
            this.CreateDocument("");
        }
        #endregion
        #region OpenDocument
        /// <summary>
        /// 打开文档
        /// </summary>
        /// <param name="fileName">路径</param>
        /// <param name="readOnly">是否只读</param>
        public void OpenDocument(string fileName, bool readOnly)
        {
            object obj_FileName = fileName;
            object obj_ReadOnly = readOnly;
            m_Document = m_WordApp.Documents.Open(ref obj_FileName, ref missing, ref obj_ReadOnly, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing);
        }
        /// <summary>
        /// 打开文档
        /// </summary>
        /// <param name="fileName">路径</param>
        public void OpenDocument(string fileName)
        {
            this.OpenDocument(fileName, false);
        }
        #endregion
        #region Save & SaveAs
        /// <summary>
        /// 保存文档(对应OpenDocument方法)
        /// </summary>
        public void Save()
        {
            if (m_Document != null)
                m_Document.Save();
        }
        /// <summary>
        /// 文档另存为
        /// </summary>
        /// <param name="fileName">路径</param>
        public void SaveAs(string fileName)
        {
            object obj_FileName = fileName;
            if (m_Document != null)
            {
                m_Document.SaveAs(ref obj_FileName, ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing);
            }
        }
        #endregion
        #region Close
        /// <summary>
        /// 关闭文档
        /// </summary>
        /// <param name="isSaveChanges"></param>
        public void Close(bool isSaveChanges)
        {
            object saveChanges = WdSaveOptions.wdDoNotSaveChanges;
            if (isSaveChanges)
                saveChanges = WdSaveOptions.wdSaveChanges;
            if (m_Document != null)
                m_Document.Close(ref saveChanges, ref missing, ref missing);
        }
        #endregion
        #region 添加数据
        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="picName"></param>
        public void AddPicture(string picName)
        {
            if (m_WordApp != null)
                m_WordApp.Selection.InlineShapes.AddPicture(picName, ref missing, ref missing, ref missing);
        }
        /// <summary>
        /// 插入页眉
        /// </summary>
        /// <param name="text"></param>
        /// <param name="align"></param>
        public void SetHeader(string text, WdParagraphAlignment align)
        {
            this.m_WordApp.ActiveWindow.View.Type = WdViewType.wdOutlineView;
            this.m_WordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekPrimaryHeader;
            this.m_WordApp.ActiveWindow.ActivePane.Selection.InsertAfter(text); //插入文本
            this.m_WordApp.Selection.ParagraphFormat.Alignment = align;  //设置对齐方式
            this.m_WordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekMainDocument; // 跳出页眉设置
        }
        /// <summary>
        /// 插入页脚
        /// </summary>
        /// <param name="text"></param>
        /// <param name="align"></param>
        public void SetFooter(string text, WdParagraphAlignment align)
        {
            this.m_WordApp.ActiveWindow.View.Type = WdViewType.wdOutlineView;
            this.m_WordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekPrimaryFooter;
            this.m_WordApp.ActiveWindow.ActivePane.Selection.InsertAfter(text); //插入文本
            this.m_WordApp.Selection.ParagraphFormat.Alignment = align;  //设置对齐方式
            this.m_WordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekMainDocument; // 跳出页眉设置
        }
        #endregion
        #region Print
        /// <summary>
        /// 
        /// </summary>
        public void PrintOut()
        {
            object copies = "1";
            object pages = "";
            object range = WdPrintOutRange.wdPrintAllDocument;
            object items = WdPrintOutItem.wdPrintDocumentContent;
            object pageType = WdPrintOutPages.wdPrintAllPages;
            object oTrue = true;
            object oFalse = false;
            this.m_Document.PrintOut(
                ref oTrue, ref oFalse, ref range, ref missing, ref missing, ref missing,
                ref items, ref copies, ref pages, ref pageType, ref oFalse, ref oTrue,
                ref missing, ref oFalse, ref missing, ref missing, ref missing, ref missing);
        }
        /// <summary>
        /// 
        /// </summary>
        public void PrintPreview()
        {
            if (m_Document != null)
                m_Document.PrintPreview();
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public void Paste()
        {
            try
            {
                if (m_Document != null)
                {
                    m_Document.ActiveWindow.Selection.Paste();
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }
        #endregion
        #region 文档中的文本和对象
        /// <summary>
        /// 插入文本
        /// </summary>
        /// <param name="text"></param>
        public void AppendText(string text)
        {
            Selection currentSelection = this.m_WordApp.Selection;
            // Store the user's current Overtype selection
            bool userOvertype = this.m_WordApp.Options.Overtype;
            // Make sure Overtype is turned off.
            if (this.m_WordApp.Options.Overtype)
            {
                this.m_WordApp.Options.Overtype = false;
            }
            // Test to see if selection is an insertion point.
            if (currentSelection.Type == WdSelectionType.wdSelectionIP)
            {
                currentSelection.TypeText(text);
                currentSelection.TypeParagraph();
            }
            else
                if (currentSelection.Type == WdSelectionType.wdSelectionNormal)
                {
                    // Move to start of selection.
                    if (this.m_WordApp.Options.ReplaceSelection)
                    {
                        object direction = WdCollapseDirection.wdCollapseStart;
                        currentSelection.Collapse(ref direction);
                    }
                    currentSelection.TypeText(text);
                    currentSelection.TypeParagraph();
                }
                else
                {
                    // Do nothing.
                }
            // Restore the user's Overtype selection
            this.m_WordApp.Options.Overtype = userOvertype;
        }
        #endregion
        #region 搜索和替换文档中的文本
        /// <summary>
        /// 替换文档中的文本
        /// </summary>
        /// <param name="oriText">源文本</param>
        /// <param name="replaceText">替换文本</param>
        public void Replace(string oriText, string replaceText)
        {
            object replaceAll = WdReplace.wdReplaceAll;
            this.m_WordApp.Selection.Find.ClearFormatting();
            this.m_WordApp.Selection.Find.Text = oriText;
            this.m_WordApp.Selection.Find.Replacement.ClearFormatting();
            this.m_WordApp.Selection.Find.Replacement.Text = replaceText;
            this.m_WordApp.Selection.Find.Execute(
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref replaceAll, ref missing, ref missing, ref missing, ref missing);
        }

        #endregion
        #region 文档中的Word表格
        /// <summary>
        /// 向文档中插入表格
        /// </summary>
        /// <param name="startIndex">开始位置</param>
        /// <param name="endIndex">结束位置</param>
        /// <param name="rowCount">行数</param>
        /// <param name="columnCount">列数</param>
        /// <returns></returns>
        public Table AppendTable(int startIndex, int endIndex, int rowCount, int columnCount)
        {
            object start = startIndex;
            object end = endIndex;
            Range tableLocation = this.m_Document.Range(ref start, ref end);
            return this.m_Document.Tables.Add(tableLocation, rowCount, columnCount, ref missing, ref missing);
        }
        /// <summary>
        /// 添加行
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public Row AppendRow(Table table)
        {
            object row = table.Rows[1];
            return table.Rows.Add(ref row);
        }
        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public Column AppendColumn(Table table)
        {
            object column = table.Columns[1];
            return table.Columns.Add(ref column);
        }
        /// <summary>
        /// 设置单元格的文本和对齐方式
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="text">文本</param>
        /// <param name="align">对齐方式</param>
        public void SetCellText(Cell cell, string text, WdParagraphAlignment align)
        {
            cell.Range.Text = text;
            cell.Range.ParagraphFormat.Alignment = align;
        }
        #endregion
        #region IDisposable 成员
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (m_WordApp != null)
                    m_WordApp.Quit(ref missing, ref missing, ref missing);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
            }
        }
        #endregion
    }
}