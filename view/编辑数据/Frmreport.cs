using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManager
{
    public delegate void RefreshView();
    public delegate void RefreshColor(string stype);
    public partial class Frmreport : Form
    {        
        private DataControl sql = new DataControl();
        
        private static string g_Type = "";
        private static string g_Folder = "";
        private static string g_EquipName = "";
        private static string g_CheckDate = "";
        private static string g_CheckType = "";

        private static string g_Code1 = "";

        public event RefreshView refreshview;
        public event RefreshColor refreshcolor;     
        public Frmreport()
        {
            InitializeComponent();
        }
        public Frmreport(string strType,string strFolder,string strEquipName,string strCheckDate,string strCheckType,string Code1)
        {
            InitializeComponent();
            g_Type = strType;
            g_Folder = strFolder;
            g_EquipName = strEquipName;
            g_CheckType = strCheckType;
            g_Code1 = Code1;

            if ((g_Type == "证书")|| (g_Type == "核查报告"))
            {
                dateTimePicker1.Enabled = true;
                g_CheckDate = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");
            }
            else
            {
                g_CheckDate = strCheckDate;
                dateTimePicker1.Value = Convert.ToDateTime(strCheckDate);
                dateTimePicker1.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Folder = "";

            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("请选择要上传的文件","信息");
                return;
            }
            try
            {
                if (g_Type == "证书")
                {
                    Folder = g_Folder + "\\" + g_CheckDate + "\\f1";
                }
                else if (g_Type == "确认表")
                {
                    Folder = g_Folder + "\\" + g_CheckDate + "\\f2";
                }
                else
                {
                    Folder = g_Folder + "\\" + g_CheckDate + "\\f3";
                }
                //
                Form1.ShowWaitForm();
                
                var dir = new DirectoryInfo(Folder);
                if (!dir.Exists)
                    dir.Create();

                var name = Path.GetFileName(textBox1.Text.Trim());
                var target = $"{Folder}\\{name}";

                if (File.Exists(target))
                {
                    if (MessageBox.Show($"{name}已存在是否覆盖?", "提示", MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Warning) != DialogResult.OK)
                        return;
                }
                File.Copy(textBox1.Text.Trim(), target, true);
                //
                #region 修改数据库
                if (g_Type == "证书")
                {
                    DataSet d1 = sql.ExecuteQuery("select * from EquipmentFileTable where EquipName='" + g_EquipName+ g_Code1 + "' and  Checkdate='" + g_CheckDate + "' and CheckType = '" + g_CheckType + "'");
                    if (d1.Tables[0].Rows.Count == 0)
                    {
                        sql.ExecuteNonQuery("insert into EquipmentFileTable(EquipName,Checkdate,UploadDate,CheckType,f1) values('" + g_EquipName+ g_Code1 + "','" + g_CheckDate + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','"+g_CheckType+"','" + name + "')");
                    }
                    else
                    {
                        sql.ExecuteNonQuery("update EquipmentFileTable set UploadDate='" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "' and f1 = '" + name + "' where EquipName='" + g_EquipName+ g_Code1 + "' and  Checkdate='" + g_CheckDate + "' and CheckType = '" + g_CheckType + "'");
                    }
                }
                else if (g_Type == "确认表")//确认表
                {
                    //修改数据库中的字段
                    sql.ExecuteNonQuery("update EquipmentFileTable set f2='" + name + "' where EquipName='" + g_EquipName+ g_Code1 + "' and Checkdate ='" + g_CheckDate + "' and CheckType = '"+g_CheckType+"'");
                    //
                }
                else
                {
                    DataSet d1 = sql.ExecuteQuery("select * from EquipmentFileTable where EquipName='" + g_EquipName+ g_Code1 + "' and  Checkdate='" + g_CheckDate + "' and CheckType = '" + g_CheckType + "'");
                    if (d1.Tables[0].Rows.Count == 0)
                    {
                        sql.ExecuteNonQuery("insert into EquipmentFileTable(EquipName,Checkdate,UploadDate,CheckType,f3) values('" + g_EquipName+ g_Code1 + "','" + g_CheckDate + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','"+g_CheckType+"','" + name + "')");
                    }
                    else
                    {
                        sql.ExecuteNonQuery("update EquipmentFileTable set UploadDate='" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "' and f3 = '" + name + "' where EquipName='" + g_EquipName+ g_Code1 + "' and  Checkdate='" + g_CheckDate + "' and CheckType = '" + g_CheckType + "'");
                    }
                }
                #endregion
                //   
                refreshview();
                MessageBox.Show("上传成功！","信息");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"请联系管理员设置服务器共享文件夹属性", "提示");
                Log.e($"上传文件 {ex}");
            }
            finally
            {
                Form1.CloseWaitForm();
            }

            //
            refreshcolor(g_Type);
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            var fileDialog = new OpenFileDialog
            {              
                Multiselect = false,
                Title = "选择文件",
            };
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            textBox1.Text = fileDialog.FileName;
           
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            g_CheckDate = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
