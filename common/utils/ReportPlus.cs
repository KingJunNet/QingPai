using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.common.utils
{
    public class ReportPlus
    {
        private XWPFDocument document;

        //通过模板创建新文档
        public void CreateNewDocument(string filePath) {
            FileStream stream = File.OpenRead(filePath);
            this.document = new XWPFDocument(stream);
        }

        public void SaveDocument(string filePath) {
            FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            document.Write(file);
            file.Close();
        }

        public TableReport createTableReport(int tableIndex) {
            XWPFTable table = document.Tables[tableIndex];

            return new TableReport(this.document, table);
        }
    }

    public class TableReport {
        private XWPFDocument document;
        private XWPFTable table;

        public TableReport(XWPFDocument myDoc, XWPFTable table)
        {
            this.document = myDoc;
            this.table = table;
        }

        public void InsertValue(int rowIndex,int cellIndex,string value) {
            table.GetRow(rowIndex).GetCell(cellIndex).SetParagraph(this.buildParagraph(value));
        }

        private XWPFParagraph buildParagraph(string text)
        {
            var para = new CT_P();
            XWPFParagraph cellParagraph = new XWPFParagraph(para, table.Body);
            cellParagraph.Alignment = ParagraphAlignment.CENTER; //字体居中
            cellParagraph.VerticalAlignment = TextAlignment.CENTER; //字体居中      
            XWPFRun run = cellParagraph.CreateRun();
            run.SetText(text);
            run.FontSize = 12;

            return cellParagraph;
        }

        public void AddRow(int targetRowIndex,int count)
        {
            if (count <= 0) {
                return;
            }
            XWPFTableRow ntr = table.GetRow(targetRowIndex);   //获得要复制的表格    
            for (int index = 0; index < count; index++)
            {
                table.AddRow(ntr); //添加上去
            }
        }

        public void AddRowEx(int startIndex, int count)
        {
            if (count <= 0)
            {
                return;
            }
            for (int index = 0; index < count; index++)
            {
                table.InsertNewTableRow(startIndex + index);
            }
        }

        public void InsertCell(int rowIndex, string[] values)
        {
            XWPFTableRow row = table.GetRow(rowIndex);
            for (int index = 0; index < values.Length; index++)
            {
                row.GetCell(index).SetParagraph(this.buildParagraph(values[index]));
                //row.GetCell(index).SetText(values[index]);
            }
        }

        public void RemoveRow(int startIndex,int endIndex) {
            for (int index = endIndex; index >= startIndex; index--) {
                table.RemoveRow(index);
            }
        }
    }
}
