using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExpertLib.Controls
{
    public partial class MultiComboBox : UserControl
    {
        #region 属性
        public ComboBox ComboBox { get; set; }
        public CheckedListBox CheckedListBox { get; set; }
        public ComboBox.ObjectCollection Items
        {
            get
            {
                return ComboBox?.Items;
            }
        }

        public List<CheckItem> CheckItems { get; set; }

        public List<string> SelectedItems { get; set; }

        public string SelectedItemsText { get; set; }

        #endregion

        /// <summary>
        /// 多选下拉框
        /// </summary>
        public MultiComboBox()
        {
            // 面板
            this.VerticalScroll.Enabled = true;
            this.AutoSize = true;

            // 多选列表
            CheckedListBox = new CheckedListBox();
            CheckedListBox.CheckOnClick = true;
            CheckedListBox.BorderStyle = BorderStyle.Fixed3D;
            CheckedListBox.Visible = true;
            CheckedListBox.Margin = new Padding(0);
            //CheckedListBox.Dock = System.Windows.Forms.DockStyle.Fill;


            CheckedListBox.MouseUp += (ss, se) =>
            {
                this.syncCheckedItemText();
            };
            CheckedListBox.MouseLeave += (ss, se) =>
            {
                // 隐藏下拉多选框
                CheckedListBox.Hide();
            };

            // 下拉框
            ComboBox = new ComboBox();
            ComboBox.Size = new System.Drawing.Size(352, 23);
            ComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            ComboBox.IntegralHeight = false;
            ComboBox.DroppedDown = false;
            ComboBox.DropDownHeight = 1;
            ComboBox.Margin = new Padding(0);
            ComboBox.Location = new Point(0, 0);
            ComboBox.DropDownStyle = ComboBoxStyle.DropDown;
            ComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBox.BackColor = Color.White;
        
            // 显示下拉框
            //showCheckBox();
            ComboBox.MouseDown += (ss, se) =>
            {
                ComboBox.DroppedDown = false;
            };
            ComboBox.MouseLeave += (ss, se) =>
            {
                // 不在下拉区则隐藏下拉
                var curMousePos = this.PointToClient(Control.MousePosition);
                var downArea = CheckedListBox.Location;
                if (curMousePos.X < downArea.X || curMousePos.X > (downArea.X + CheckedListBox.Width)
                || curMousePos.Y < downArea.Y || curMousePos.Y > (downArea.Y + CheckedListBox.Height))
                {
                    CheckedListBox.Hide();
                }
            };
            ComboBox.DropDown += (ss, se) =>
            {
                //显示下拉多选框
                ComboBox.BeginUpdate();

                //LoadCheckedItms();
                CheckedListBox.Show();

                ComboBox.EndUpdate();
            };

            // 添加控件
            this.Controls.Add(ComboBox);
        }

        public void LoadCheckedItms()
        {
            //加载CheckedListBox项目
            CheckedListBox.Items.Clear();
            foreach (var checkItem in this.CheckItems)
            {
                CheckedListBox.Items.Add(checkItem.Value, checkItem.Checked);
            }

            CheckedListBox.Width = ComboBox.Width;
            CheckedListBox.ItemHeight = ComboBox.ItemHeight;
            //CheckedListBox.Size = new Size(ComboBox.DropDownWidth, CheckedListBox.Items.Count * 10);
            CheckedListBox.Location = new Point(ComboBox.Left, ComboBox.Height);
            this.Controls.Add(CheckedListBox);
            CheckedListBox.Visible = true;
            CheckedListBox.Hide();

            //设置文本
            this.syncCheckedItemText();
        }

        private void syncCheckedItemText()
        {
            // 更新ComboBox显示文本
            this.SelectedItems = new List<string>();
            foreach (var item in CheckedListBox.CheckedItems)
            {
                this.SelectedItems.Add(item.ToString());
            }
            this.SelectedItemsText = string.Join(",", SelectedItems);
            ComboBox.Text = SelectedItemsText;
            ComboBox.Tag = SelectedItems;
        }
            

        private void showCheckBox()
        {
            CheckedListBox.Items.Add("052a", true);
            CheckedListBox.Items.Add("052b", true);
            CheckedListBox.Items.Add("052c", true);
            CheckedListBox.Items.Add("052d", true);
            CheckedListBox.Items.Add("052e", true);
            CheckedListBox.Items.Add("052f", true);
            CheckedListBox.Items.Add("052g", true);
            CheckedListBox.Width = ComboBox.Width;
            CheckedListBox.ItemHeight = ComboBox.ItemHeight;
            CheckedListBox.Size = new Size(ComboBox.DropDownWidth, CheckedListBox.Items.Count * 18);
            CheckedListBox.Location = new Point(ComboBox.Left, ComboBox.Height);
            this.Controls.Add(CheckedListBox);
            CheckedListBox.Visible = true;

        }

        private void showCheckBoxBack()
        {
            // 添加并设置选中项
            var lastChecked = ComboBox.Tag as List<string>;
            ComboBox.BeginUpdate();
            foreach (var v in this.Items)
            {
                var ck = false;
                if (lastChecked != null && lastChecked.IndexOf(v.ToString()) >= 0)
                {
                    ck = true;
                }
                CheckedListBox.Items.Add(v, ck);
            }
            // 显示下拉框
            CheckedListBox.Width = ComboBox.Width;
            CheckedListBox.ItemHeight = ComboBox.ItemHeight;
            CheckedListBox.Size = new Size(ComboBox.DropDownWidth, this.Items.Count * 18);
            CheckedListBox.Location = new Point(ComboBox.Left, ComboBox.Height);
            this.Controls.Add(CheckedListBox);
            CheckedListBox.Visible = true;
        }
    }
}
