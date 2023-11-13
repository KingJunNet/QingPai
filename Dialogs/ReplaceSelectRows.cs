using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;

namespace TaskManager.Dialogs
{
    public partial class ReplaceSelectRows : Form
    {
        private GridView currentView;
        private List<DataField> field;
        private int colmun;
        public ReplaceSelectRows()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="authorityEdit"></param>
        /// <param name="theView"></param>
        /// <param name="theHand"></param>
        /// <param name="fields"></param>
        /// <param name="type"></param>
        public ReplaceSelectRows(bool authorityEdit, GridView theView, int theHand, List<DataField> fields,
           FormType type)
        {           
            InitializeComponent();
            if(DesignMode)
                return;
            replace.Enabled = authorityEdit;
            currentView = theView;          
            field = fields;
            colmun = field.Count;
            
        }


        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void replace_Click(object sender, EventArgs e)
        {
            string oldcontent = Oldcontent.Text;
            string newcontent = Newcontent.Text;
            int index = filedName.SelectedIndex;
            int rowCount = currentView.DataRowCount;
            int rowHandle;
            //currentView.BeginUpdate();
            try
            {
                if (index == 0)
                {
                    for (rowHandle = rowCount - 1; rowHandle >= 0; rowHandle--)
                    {
                        for (int i = 0; i < colmun; i++)
                        {
                            if (currentView.GetRowCellValue(rowHandle, field[i].Eng.ToString()).ToString() == oldcontent)
                            {
                                currentView.SetRowCellValue(rowHandle, field[i].Eng.ToString(), newcontent);
                            }

                        }
                    }
                }
                else
                {
                    for (rowHandle = rowCount - 1; rowHandle >= 0; rowHandle--)
                    {
                        if (currentView.GetRowCellValue(rowHandle, field[index - 1].Eng.ToString()).ToString() == oldcontent)
                        {
                            currentView.SetRowCellValue(rowHandle, field[index - 1].Eng.ToString(), newcontent);
                        }
                    }
                }
               
                MessageBox.Show("替换成功");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //currentView.EndUpdate();
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void ReplaceSelectRows_Load(object sender, EventArgs e)
        {
            filedName.Properties.Items.Clear();
            filedName.Properties.Items.Add("全部");
            foreach(var name in field)
            {
                filedName.Properties.Items.Add(name.Chs);
            }
            filedName.SelectedIndex = 0;
            
        }
    }
}
