using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace TaskManager
{   
    public delegate void MyDelegate(object sender, EventArgs e);//
    public partial class FileView1 : UserControl
    {      
        public event MyDelegate Button1Even_click, Button2Even_click, Button3Even_click;


        public bool Authority
        {
            set
            {
                if (DesignMode)
                    return;             
            }
        }

        private string folder;
        public string Folder
        {
            get => folder;
            set
            {
                if (DesignMode)
                    return;
                folder = value;               
            }
        }
        public FileView1()
        {
            InitializeComponent();
            if (DesignMode)
                return;
        }       
        private void FileView1_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;          

            button1.Enabled = button3.Enabled = button2.Enabled = false;
            radioButton1.Checked = true;     
        }
    
        /// <summary>
        /// 文件夹打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void folderOpen_Click(object sender, EventArgs e)
        {
            Folder = @label1.Text.Trim().Substring(6, label1.Text.Trim().Length-6);
            if (Directory.Exists(Folder))
                System.Diagnostics.Process.Start("Explorer.exe", Folder);
            else
                MessageBox.Show($"{Folder}文件夹不存在", "提醒");
        }
        /// <summary>
        /// 证书
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void button1_Click(object sender, EventArgs e)
        {
            if (Button1Even_click != null)
            {
                Button1Even_click(sender, e);                
            }
        }        
        /// <summary>
        /// 确认表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (Button2Even_click != null)
            {
                Button2Even_click(sender, e);
            }
        }
        /// <summary>
        /// 报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (Button3Even_click != null)
            {
                Button3Even_click(sender, e);
            }          
        }
    }
}
