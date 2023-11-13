using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using ExpertLib.Controls;
using LabSystem.DAL;

namespace TaskManager
{
    public partial class EquipEditDialog : BaseEditDialog
    {
        private DataControl sql = new DataControl();
        private TitleCombox EquipState;
        private TitleCombox CheckType;

        private TitleDate CheckDate;
        private TitleDate CheckEndDate;

        private TitleTextBox Period;
        private TitleCombox EquipName;
        private TitleCombox Code;
        Frmreport f1;

        private static string g_EquipName = "";
        public const string RootFolder = "轻排参数表服务器";
        public const string Category = "设备管理";

        private static string g_Code1 = "";

        public string Folder => $"\\\\{Server}{RootFolder}\\{Category}\\{EquipName1 + "{" + Code1 + "}"}\\{CheckType1}";
        public string PriorFolder => $"\\\\{Server}{RootFolder}\\{Category}\\{EquipName1 + "{" + Code1 + "}"}\\{CheckType1}";

        private int _id;
        [Description("ID"), Category("自定义属性")]
        public int Id
        {
            get => _id;
            set
            {
                if (DesignMode)
                    return;

                _id = value;
                fileView1.label1.Text = $"文件夹地址:{Folder}";
            }
        }     

        private string equipname1;
        [Description("设备名称"), Category("自定义属性")]
        public string EquipName1
        {
            get => equipname1;
            set
            {
                if (DesignMode)
                    return;
                equipname1 = value;
                fileView1.label1.Text = $"文件夹地址:{Folder}";
            }
        }

        private string code1;
        [Description("设备编码"), Category("自定义属性")]
        public string Code1
        {
            get => code1;
            set
            {
                if (DesignMode)
                    return;
                code1 = value;
                fileView1.label1.Text = $"文件夹地址:{Folder}";
            }
        }

        private string checktype1;
        [Description("检验方式"), Category("自定义属性")]
        public string CheckType1
        {
            get => checktype1;
            set
            {
                if (DesignMode)
                    return;
                checktype1 = value;
                fileView1.label1.Text = $"文件夹地址:{Folder}";
            }
        }

        public EquipEditDialog()
        {
            InitializeComponent();
            if (DesignMode)
                return;
        }

        public EquipEditDialog(bool authorityEdit, GridView theView,  int theHand, List<DataField> fields, FormType Type1)
            : base(authorityEdit, theView,theHand, fields, Type1)
        {
            InitializeComponent();
            if (DesignMode)
                return;
            fileView1.Authority = authorityEdit;
            fileView1.label1.Text = Folder;
           
            
        }
     
        protected override void BaseEditDialog_Load(object sender, EventArgs e)
        {
            if(DesignMode)
                return;

            

           flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            controls = GetAllControls(out Panel);
            InitControls();

            //if (btnUpdate.Text=="新增")
            //{
                if (GetControlByFieldName("group1") is TitleCombox group1 &&
               GetControlByFieldName("locationnum") is TitleCombox locationnum &&
               GetControlByFieldName("regpeople") is TitleCombox regpeople
             )
                {
                    
                    string sql = $"select Locationnumber from UserStructure where Group1 ='{FormSignIn.CurrentUser.Department}' and Username='{FormSignIn.CurrentUser.Name}'";
                    DataTable data = SqlHelper.GetList(sql);

                    if (data.Rows.Count > 0)
                    {
                        List<string> list1 = new List<string>();
                        foreach (DataRow row in data.Rows)
                        {
                            list1.Add(row[0].ToString());
                        }
                        locationnum.SetItems(list1);
                        
                    }

                }
            //}

            if (btnUpdate.Text == "新增")
            {
                titleCombox13.SetValue(FormSignIn.CurrentUser.Name);
                titleCombox11.SetValue(FormSignIn.CurrentUser.Department);
                if (titleCombox12.comboBox1.Items.Count > 0)
                {
                    titleCombox12.SetValue(titleCombox12.comboBox1.Items[0].ToString());
                }
               
            }


            //if (getcontrolbyfieldname("usecharge") is titlecombox usecharge)
            //{
            //    string sql = $"select locationnumber from userstructure";

            //    datatable data = sqlhelper.getlist(sql);

            //    if (data.rows.count > 0)
            //    {
            //        list<string> list1 = new list<string>();
            //        foreach (datarow row in data.rows)
            //        {
            //            list1.add(row[0].tostring());
            //        }
            //        usecharge.setitems(list1);
            //    }

            //}


            g_EquipName = view.GetRowCellValue(hand, "EquipName").ToString();
            g_Code1 = view.GetRowCellValue(hand, "EquipCode").ToString();
            int.TryParse(view.GetRowCellValue(hand, "ID").ToString(), out var id);
            if (id == -1) id = 0;         

            if (GetControlByFieldName("EquipState") is TitleCombox equipState &&
                GetControlByFieldName("CheckType") is TitleCombox checktype &&
                GetControlByFieldName("EquipName") is TitleCombox equipname &&
                GetControlByFieldName("EquipCode") is TitleCombox code &&
                GetControlByFieldName("CheckDate") is TitleDate checkdate &&
                GetControlByFieldName("CheckEndDate") is TitleDate checkEndDate &&
                GetControlByFieldName("Period") is TitleTextBox period)
            {
                EquipState = equipState;
                CheckType = checktype;
                EquipName = equipname;
                Code = code;
                CheckDate = checkdate;
                CheckEndDate = checkEndDate;
                Period = period;                

                LoadView();
                EquipState.SetTextChange(StateChangeHandler);
                CheckType.SetTextChange(StateChangeHandler);
                EquipName.SetTextChange(StateChangeHandler);
                Code.SetTextChange(StateChangeHandler);
                Period.SetTextChange(StateChangeHandler);               
                CheckDate.SetValueChange(StateChangeHandler);                

                StateChangeHandler(null, null);
            }         
            flowLayoutPanel1.ResumeLayout(true);
            ResumeLayout(true);

            CheckDataFile();
            Log.e("BaseEditDialog_Load");
        }
        private void StateChangeHandler(object sender, EventArgs e)
        {
            List<string> lsitem = new List<string>();

            if (GetControlByFieldName("CheckType").Value() == "外部校准")
            {
                fileView1.button1.Enabled = true;
                fileView1.button3.Enabled = false;
                fileView1.button2.Enabled = true;
                lsitem.Add("校准中");
                lsitem.Add("使用");
                lsitem.Add("停用");
                GetControlByFieldName("EquipState").SetItems(lsitem);
            }
            if (GetControlByFieldName("CheckType").Value() == "外部检定")
            {
                fileView1.button1.Enabled = true;
                fileView1.button3.Enabled = false;
                fileView1.button2.Enabled = true;
                lsitem.Add("检定中");
                lsitem.Add("使用");
                lsitem.Add("停用");
                GetControlByFieldName("EquipState").SetItems(lsitem);

            }
            if (GetControlByFieldName("CheckType").Value() == "内部核查")
            {
                fileView1.button1.Enabled = false;
                fileView1.button3.Enabled = true;
                fileView1.button2.Enabled = false;
                lsitem.Add("核查中");
                lsitem.Add("使用");
                lsitem.Add("停用");
                GetControlByFieldName("EquipState").SetItems(lsitem);
            }
            equipname1 = EquipName.Text.Trim();
            code1 = Code.Text.Trim();


            checktype1 = GetControlByFieldName("CheckType").Value();
            fileView1.label1.Text = $"文件夹地址:{Folder}";
            SetSurfaceColor(GetControlByFieldName("CheckType").Value());          
        }   
        private void StateChangeHandler1(object sender, EventArgs e)
        {
            if (MessageBox.Show($"是否修改检定日期?", "提示", MessageBoxButtons.OKCancel,
                               MessageBoxIcon.Warning) == DialogResult.OK)
            {

                if (!int.TryParse(Period.Text, out var period))
                    period = 1;

                var months = period * 12;
                CheckEndDate.SetValue(CheckDate.Date.AddMonths(months).AddDays(-1));  
            }
        }
        public static void DelectDir(string srcPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                if (!dir.Exists) return;
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        new FileInfo(i.FullName).Attributes = FileAttributes.Normal;
                        File.Delete(i.FullName);//删除指定文件
                    }
                }
                dir.Delete(true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 根据硬盘中文件进行数据中数据校对
        /// </summary>
        private void CheckDataFile()
        {
            string strCheckDate = "";
            string strTempPath = "";
            string f1 = "N", f2="N", f3 = "N";
            bool bl = false;
            try
            {
                DirectoryInfo dir = new DirectoryInfo(Folder);
                if (!dir.Exists)
                {
                    sql.ExecuteNonQuery("delete from EquipmentFileTable where EquipName='" + equipname1+ code1 + "' and CheckType = '" + checktype1 + "'");
                    return;
                }
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    f1 = "N"; f2 = "N"; f3 = "N";
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);  //获取日期文件夹
                        strCheckDate = subdir.Name;

                        //f1,f2,f3
                        if (CheckType1.Contains("内部"))
                        {
                            strTempPath = subdir.FullName + "\\f3";
                            DirectoryInfo subdir1 = new DirectoryInfo(strTempPath);
                            if (subdir1.Exists)
                            {
                                int length = Directory.GetFiles(strTempPath + "\\", "*.*", SearchOption.AllDirectories).Length;
                                if (length == 0) f3 = "";
                            }
                            else f3 = "";
                            //
                            if (f3 == "") 
                            {
                                sql.ExecuteNonQuery("delete from EquipmentFileTable where EquipName='"+equipname1+ code1 + "' and Checkdate = '"+ strCheckDate + "' and CheckType = '"+checktype1+"'");
                            }
                        }
                        else
                        {
                            strTempPath = subdir.FullName + "\\f1";               
                            DirectoryInfo subdir1 = new DirectoryInfo(strTempPath);
                            if (subdir1.Exists)
                            {
                                int length = Directory.GetFiles(strTempPath + "\\", "*.*", SearchOption.AllDirectories).Length;
                                if (length == 0) f1 = "";
                            }
                            else f1 = "";
                            //
                            strTempPath = subdir.FullName + "\\f2";                         
                            subdir1 = new DirectoryInfo(strTempPath);
                            if (subdir1.Exists)
                            {
                                int length = Directory.GetFiles(strTempPath + "\\", "*.*", SearchOption.AllDirectories).Length;
                                if (length == 0) f2 = "";
                            }
                            else f2 = "";

                            if ((f1 != "")&&(f2==""))
                            {
                                sql.ExecuteNonQuery("update EquipmentFileTable set f2=''where EquipName='" + equipname1+ code1 + "' and Checkdate = '" + strCheckDate + "' and CheckType = '" + checktype1 + "'");
                            }
                            if ((f1 == "") && (f2 != ""))
                            {
                                sql.ExecuteNonQuery("update EquipmentFileTable set f1='' where EquipName='" + equipname1+ code1 + "' and Checkdate = '" + strCheckDate + "' and CheckType = '" + checktype1 + "'");
                            }

                            if ((f1 == "") && (f2 == ""))
                            {
                                sql.ExecuteNonQuery("delete from EquipmentFileTable  where EquipName='" + equipname1+ code1 + "' and Checkdate = '" + strCheckDate + "' and CheckType = '" + checktype1 + "'");
                            }
                        }
                    }
                   
                }

                //
                if (fileinfo.Length == 0) bl = true;

                if (bl)
                {
                    sql.ExecuteNonQuery("delete from EquipmentFileTable where EquipName='" + equipname1+ code1 + "' and CheckType = '" + checktype1 + "'");
                }
            }
            catch (Exception e)
            {
                throw;
            }

        }
        public void LoadView()
        {
            fileView1.listView1.Items.Clear();

            DataSet d1 = sql.ExecuteQuery("select * from EquipmentFileTable where EquipName = '" + g_EquipName+g_Code1 + "' and CheckType = '"+CheckType.Value().Trim()+ "' order by Checkdate desc");
            for (int i = 0; i < d1.Tables[0].Rows.Count; i++)
            {
                ListViewItem lvi = new ListViewItem();


                lvi.Text = (i + 1).ToString().Trim();
                lvi.SubItems.Add(d1.Tables[0].Rows[i]["Checkdate"].ToString().Trim());
                lvi.SubItems.Add(d1.Tables[0].Rows[i]["UploadDate"].ToString().Trim());
                lvi.SubItems.Add(d1.Tables[0].Rows[i]["f1"].ToString().Trim());
                lvi.SubItems.Add(d1.Tables[0].Rows[i]["f2"].ToString().Trim());
                lvi.SubItems.Add(d1.Tables[0].Rows[i]["f3"].ToString().Trim());

                fileView1.listView1.Items.Add(lvi);
            }
        }
        /// <summary>
        /// 设置界面颜色
        /// </summary>
        public void SetSurfaceColor(string stype)
        {
            if (!int.TryParse(Period.Text, out var period))
                period = 1;

            var months = period * 12;
            if(f1 != null)
                CheckDate.SetValue(f1.dateTimePicker1.Value.Date.ToString("yyyy-MM-dd"));
            CheckEndDate.SetValue(CheckDate.Date.AddMonths(months).AddDays(-1));
            
            //
            var days = ReminderDialog.EquipWarning;//30天 
            var date1 = DateTime.Now.Date.AddDays(days);

            var intcount = fileView1.listView1.Items.Count;
            //if (intcount > 0 && EquipState.Text.Contains("检定"))
            //{
            if (date1 >= CheckEndDate.Date) //                
            {
                BackColor = Color.Orange;
                if (EquipState.Text != null && EquipState.Text.Contains("中"))
                {
                    BackColor = Color.Yellow;
                    if (intcount > 0)
                    {
                        if ((CheckType.Value() == "外部检定") && (fileView1.listView1.Items[0].SubItems[3].Text.Trim() != "") && (fileView1.listView1.Items[0].SubItems[4].Text.Trim() == ""))
                            BackColor = Color.LightGreen;

                        if ((CheckType.Value() == "外部检定") && (fileView1.listView1.Items[0].SubItems[3].Text.Trim() != "") && (fileView1.listView1.Items[0].SubItems[4].Text.Trim() != ""))
                            BackColor = Color.White;

                        /////////////
                        if ((CheckType.Value() == "外部校准") && (fileView1.listView1.Items[0].SubItems[3].Text.Trim() != "") && (fileView1.listView1.Items[0].SubItems[4].Text.Trim() == ""))
                            BackColor = Color.LightGreen;

                        if ((CheckType.Value() == "外部校准") && (fileView1.listView1.Items[0].SubItems[3].Text.Trim() != "") && (fileView1.listView1.Items[0].SubItems[4].Text.Trim() != ""))
                            BackColor = Color.White;

                        /////
                        if ((CheckType.Value() == "内部核查") && (fileView1.listView1.Items[0].SubItems[5].Text.Trim() != ""))
                            BackColor = Color.White;
                    }

                }
                

            }
            else {

                if (EquipState.Text != null && EquipState.Text.Contains("中"))
                {
                    BackColor = Color.Yellow;
                    if (intcount > 0)
                    {
                        if ((CheckType.Value() == "外部检定") && (fileView1.listView1.Items[0].SubItems[3].Text.Trim() != "") && (fileView1.listView1.Items[0].SubItems[4].Text.Trim() == ""))
                            BackColor = Color.LightGreen;

                        if ((CheckType.Value() == "外部检定") && (fileView1.listView1.Items[0].SubItems[3].Text.Trim() != "") && (fileView1.listView1.Items[0].SubItems[4].Text.Trim() != ""))
                            BackColor = Color.White;

                        /////////////
                        if ((CheckType.Value() == "外部校准") && (fileView1.listView1.Items[0].SubItems[3].Text.Trim() != "") && (fileView1.listView1.Items[0].SubItems[4].Text.Trim() == ""))
                            BackColor = Color.LightGreen;

                        if ((CheckType.Value() == "外部校准") && (fileView1.listView1.Items[0].SubItems[3].Text.Trim() != "") && (fileView1.listView1.Items[0].SubItems[4].Text.Trim() != ""))
                            BackColor = Color.White;

                        /////
                        if ((CheckType.Value() == "内部核查") && (fileView1.listView1.Items[0].SubItems[5].Text.Trim() != ""))
                            BackColor = Color.White;
                    }

                }
                else if (EquipState.Text != null && EquipState.Text.Contains("停"))
                {
                    BackColor = Color.DarkGray;
                }
                else if (EquipState.Text != null && EquipState.Text.Contains("使用"))
                {
                    BackColor = Color.White;
                }
                else
                {
                    BackColor = Color.Orange;
                }

            } 
            //}
            //else BackColor = Color.Yellow;


            g_EquipName = EquipName.Text.Trim();

            if((stype == "确认表")||(stype == "核查报告"))
                EquipState.SetValue("使用");
        }
        /// <summary>
        ///点击证书按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileView1_Button1Even_click(object sender, EventArgs e)
        {         
            Boolean bldownload = false;
            string strEquipState = "";

            if (fileView1.radioButton1.Checked)
            {
                if (CheckType.Text.Trim() == "外部检定") strEquipState = "检定中";
                if (CheckType.Text.Trim() == "外部校准") strEquipState = "校准中";
                if (CheckType.Text.Trim() == "内部核查") strEquipState = "核查中";
                EquipState.SetValue(strEquipState);
                #region //*****上传文件****              

                if (Id < 0)
                {
                    MessageBox.Show("请先保存再上传文件", "提示");
                    return;
                }
                if (EquipState.Text == "")
                {
                    MessageBox.Show("请先选择设备状态", "提示");
                    return;
                }

                if (EquipName.Text == "")
                {
                    MessageBox.Show("设备名称不能为空！", "信息提示");
                    return;
                }

                if (f1 == null || f1.IsDisposed)
                {
                    f1 = new Frmreport("证书",Folder, EquipName.Text, "", CheckType.Value().Trim(),Code.Text);
                    f1.refreshview += new RefreshView(LoadView);
                    f1.refreshcolor += new RefreshColor(SetSurfaceColor); 
                    f1.Show();
                }
                else
                {
                    f1.Activate();
                }
                #endregion
            }            
            else if (fileView1.radioButton2.Checked)
            {
                #region //*****下载文件****           

                if (Id < 0)
                {
                    MessageBox.Show("请先保存再下载文件");
                    return;
                }

                var items = fileView1.listView1.SelectedItems;
                if (items.Count == 0)
                {
                    MessageBox.Show("请选择文件", "提示");
                    return;
                }

                foreach (ListViewItem item in items)
                {
                    var name = item.SubItems[3].Text;
                    var cdate = item.SubItems[1].Text;
                    var sourcePath = $"{PriorFolder}\\{cdate}\\f1\\{name}";
                    var fileDialog = new SaveFileDialog
                    {
                        Title = $"备份 {name}",
                        FileName = name,
                        RestoreDirectory = true,
                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                    };
                    if (fileDialog.ShowDialog() != DialogResult.OK) continue;

                    var targetPath = fileDialog.FileName;
                    File.Copy(sourcePath, targetPath, true);
                    bldownload = true;
                }
                if (bldownload)
                {
                    MessageBox.Show("下载成功！", "信息");
                }
                #endregion
            }
            else
            {
                #region *****删除文件******
                if (Id < 0)
                {
                    MessageBox.Show("请先保存再删除文件");
                    return;
                }

                if (MessageBox.Show("删除后无法恢复,是否继续", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    return;

                try
                {
                    if (fileView1.listView1.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("请选中要删出的信息！", "信息提示");
                        return;
                    }

                    string strdate = fileView1.listView1.SelectedItems[0].SubItems[1].Text.Trim();
                    DelectDir(PriorFolder + "\\" + strdate + "\\f1");
                   
                    //
                    DataSet d1 = sql.ExecuteQuery("select * from EquipmentFileTable where EquipName='" + EquipName.Text.Trim()+ Code.Text.Trim() + "' and  Checkdate='" +strdate+ "' and CheckType = '"+CheckType.Value().Trim()+"'");

                    if ((d1.Tables[0].Rows[0]["f1"].ToString().Trim() != "") && (d1.Tables[0].Rows[0]["f2"].ToString().Trim() != ""))
                    {
                        sql.ExecuteNonQuery("update EquipmentFileTable set f1 = '' where EquipName='" + EquipName.Text.Trim()+ Code.Text.Trim() + "' and  Checkdate='" + strdate + "' and CheckType = '" + CheckType.Value().Trim() + "'");
                    }
                    if ((d1.Tables[0].Rows[0]["f1"].ToString().Trim() != "") && (d1.Tables[0].Rows[0]["f2"].ToString().Trim() == ""))
                    {
                        sql.ExecuteNonQuery("delete EquipmentFileTable  where EquipName='" + EquipName.Text.Trim()+ Code.Text.Trim() + "' and  Checkdate='" + strdate + "' and CheckType = '" + CheckType.Value().Trim() + "'");
                    }

                    //修改设备状态
                    if (CheckType.Text.Trim() == "外部检定") strEquipState = "检定中";
                    if (CheckType.Text.Trim() == "外部校准") strEquipState = "校准中";

                    sql.ExecuteNonQuery("update EquipmentTable set EquipState = '"+ strEquipState + "' where EquipName='" + EquipName.Text.Trim()+ Code.Text.Trim() + "' and CheckType = '" + CheckType.Value().Trim() + "'");
                    //
                    EquipState.SetValue(strEquipState);                    
                    LoadView();
                    SetSurfaceColor("NNN");
                    MessageBox.Show("删除信息成功！");

                }
                catch (Exception e1)
                {
                    Log.e($"删除ToolStripMenuItem_Click {e1}");
                }

                #endregion
            }
            LoadView();
            
        }      
        /// <summary>
        /// 点击确认表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileView1_Button2Even_click(object sender, EventArgs e)
        {           
            Boolean bldownload = false;
            string strEquipState = "";

            if (fileView1.radioButton1.Checked)
            {
                if (CheckType.Text.Trim() == "外部检定") strEquipState = "检定中";
                if (CheckType.Text.Trim() == "外部校准") strEquipState = "校准中";
                if (CheckType.Text.Trim() == "内部核查") strEquipState = "核查中";
                EquipState.SetValue(strEquipState);
                #region //*****上传文件****              

                if (Id < 0)
                {
                    MessageBox.Show("请先保存再上传文件", "提示");
                    return;
                }
                //
                if (EquipState.Text == "")
                {
                    MessageBox.Show("请先选择设备状态", "提示");
                    return;
                }
                //判定证书是否上传
                DataSet d1 = sql.ExecuteQuery("select * from EquipmentFileTable where EquipName='" + EquipName.Text.Trim()+ Code.Text.Trim() + "' and Checkdate ='" + CheckDate.Date.ToString("yyyy-MM-dd") + "' and CheckType = '" + CheckType.Value().Trim() + "'");
                if ((d1.Tables[0].Rows.Count == 0) || (d1.Tables[0].Rows[0]["f1"].ToString().Trim() == ""))
                {
                    MessageBox.Show("请先上传证书", "信息提示");
                    return;
                }
                //
                if (f1 == null || f1.IsDisposed)
                {
                    f1 = new Frmreport("确认表", Folder, EquipName.Text.Trim(),  d1.Tables[0].Rows[0]["Checkdate"].ToString(), CheckType.Value().Trim(), Code.Text.Trim());
                    f1.refreshview += new RefreshView(LoadView);
                    f1.refreshcolor += new RefreshColor(SetSurfaceColor);      
                    f1.Show();
                }
                else
                {
                    f1.Activate();
                }

                #endregion
            }
            else if (fileView1.radioButton2.Checked)
            {
                #region //*****下载文件****           
                
                if (Id < 0)
                {
                    MessageBox.Show("请先保存再下载文件");
                    return;
                }

                var items = fileView1.listView1.SelectedItems;
                if (items.Count == 0)
                {
                    MessageBox.Show("请选择文件", "提示");
                    return;
                }

                foreach (ListViewItem item in items)
                {
                    var name = item.SubItems[4].Text;
                    var cdate = item.SubItems[1].Text;
                    var sourcePath = $"{PriorFolder}\\{cdate}\\f2\\{name}";
                    var fileDialog = new SaveFileDialog
                    {
                        Title = $"备份 {name}",
                        FileName = name,
                        RestoreDirectory = true,
                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                    };
                    if (fileDialog.ShowDialog() != DialogResult.OK) continue;

                    var targetPath = fileDialog.FileName;
                    File.Copy(sourcePath, targetPath, true);
                    bldownload = true;
                }
                if (bldownload)
                {
                    MessageBox.Show("下载成功！", "信息");
                }
                #endregion
            }
            else
            {
                #region *****删除文件******
                if (Id < 0)
                {
                    MessageBox.Show("请先保存再删除文件");
                    return;
                }

                if (MessageBox.Show("删除后无法恢复,是否继续", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    return;

                try
                {
                    if (fileView1.listView1.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("请选中要删出的信息！", "信息提示");
                        return;
                    }

                    string strdate = fileView1.listView1.SelectedItems[0].SubItems[1].Text.Trim();
                    DelectDir(PriorFolder + "\\" + strdate + "\\f2");
                  

                    //
                    DataSet d1 = sql.ExecuteQuery("select * from EquipmentFileTable where EquipName='" + EquipName.Text.Trim()+ Code.Text.Trim() + "' and  Checkdate='" + strdate + "' and CheckType = '" + CheckType.Value().Trim() + "'");
                    if ((d1.Tables[0].Rows[0]["f1"].ToString().Trim() != "") && (d1.Tables[0].Rows[0]["f2"].ToString().Trim() != ""))
                    {
                        sql.ExecuteNonQuery("update EquipmentFileTable set f2 = '' where EquipName='" + EquipName.Text.Trim()+ Code.Text.Trim() + "' and  Checkdate='" + strdate + "' and CheckType = '" + CheckType.Value().Trim() + "'");
                    }
                    if ((d1.Tables[0].Rows[0]["f1"].ToString().Trim() == "") && (d1.Tables[0].Rows[0]["f2"].ToString().Trim() != ""))
                    {
                        sql.ExecuteNonQuery("delete EquipmentFileTable  where EquipName='" + EquipName.Text.Trim()+ Code.Text.Trim() + "' and  Checkdate='" + strdate + "' and CheckType = '" + CheckType.Value().Trim() + "'");
                    }

                    //修改设备状态
                    if (CheckType.Text.Trim() == "外部检定") strEquipState = "检定中";
                    if (CheckType.Text.Trim() == "外部校准") strEquipState = "校准中";
                    sql.ExecuteNonQuery("update EquipmentTable set EquipState = '" + strEquipState + "' where EquipName='" + EquipName.Text.Trim()+ Code.Text.Trim() + "' and CheckType = '" + CheckType.Value().Trim() + "'");
                    //
                    EquipState.SetValue(strEquipState);
                    LoadView();
                    SetSurfaceColor("NNN");                   
                    MessageBox.Show("删除信息成功！");

                }
                catch (Exception e1)
                {
                    Log.e($"删除ToolStripMenuItem_Click {e1}");
                }

                #endregion
            }
            LoadView();
        }
        /// <summary>
        /// 点击报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileView1_Button3Even_click(object sender, EventArgs e)
        {           
            Boolean bldownload = false;
            string strEquipState = "";
            if (fileView1.radioButton1.Checked)
            {
                if (CheckType.Text.Trim() == "外部检定") strEquipState = "检定中";
                if (CheckType.Text.Trim() == "外部校准") strEquipState = "校准中";
                if (CheckType.Text.Trim() == "内部核查") strEquipState = "核查中";
                EquipState.SetValue(strEquipState);
                #region //*****上传文件****              

                if (Id < 0)
                {
                    MessageBox.Show("请先保存再上传文件", "提示");
                    return;
                }
                //
                if (EquipState.Text == "")
                {
                    MessageBox.Show("请先选择设备状态", "提示");
                    return;
                }
                //
                if (EquipName.Text == "")
                {
                    MessageBox.Show("设备名称不能为空！", "信息提示");
                    return;
                }

                if (f1 == null || f1.IsDisposed)
                {
                    f1 = new Frmreport("核查报告", Folder, EquipName.Text, "",CheckType.Value().Trim(), Code.Text);
                    f1.refreshview += new RefreshView(LoadView);
                    f1.refreshcolor += new RefreshColor(SetSurfaceColor);
                    f1.Show();
                }
                else
                {
                    f1.Activate();
                }
                #endregion
            }
            else if (fileView1.radioButton2.Checked)
            {                
                #region //*****下载文件****           

                if (Id < 0)
                {
                    MessageBox.Show("请先保存再下载文件");
                    return;
                }

                var items = fileView1.listView1.SelectedItems;
                if (items.Count == 0)
                {
                    MessageBox.Show("请选择文件", "提示");
                    return;
                }

                foreach (ListViewItem item in items)
                {
                    var name = item.SubItems[5].Text;
                    var cdate = item.SubItems[1].Text;
                    var sourcePath = $"{PriorFolder}\\{cdate}\\f3\\{name}";
                    var fileDialog = new SaveFileDialog
                    {
                        Title = $"备份 {name}",
                        FileName = name,
                        RestoreDirectory = true,
                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                    };
                    if (fileDialog.ShowDialog() != DialogResult.OK) continue;

                    var targetPath = fileDialog.FileName;
                    File.Copy(sourcePath, targetPath, true);
                    bldownload = true;
                }
                if (bldownload)
                {
                    MessageBox.Show("下载成功！", "信息");
                }
                #endregion
            }
            else
            {
                #region *****删除文件******
                if (Id < 0)
                {
                    MessageBox.Show("请先保存再删除文件");
                    return;
                }

                if (MessageBox.Show("删除后无法恢复,是否继续", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    return;

                try
                {
                    if (fileView1.listView1.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("请选中要删出的信息！", "信息提示");
                        return;
                    }

                    string strdate = fileView1.listView1.SelectedItems[0].SubItems[1].Text.Trim();
                    DelectDir(PriorFolder + "\\" + strdate + "\\f3");                  

                    //
                    sql.ExecuteNonQuery("delete from EquipmentFileTable  where EquipName='" + EquipName.Text.Trim()+ Code.Text.Trim() + "' and  Checkdate='" + strdate + "' and CheckType = '" + CheckType.Value().Trim() + "'");

                    //修改设备状态
                    sql.ExecuteNonQuery("update EquipmentTable set EquipState = '核查中' where EquipName='" + EquipName.Text.Trim()+ Code.Text.Trim() + "' and CheckType = '" + CheckType.Value().Trim() + "'");

                    EquipState.SetValue("核查中");                    
                    LoadView();
                    SetSurfaceColor("NNN");
                    MessageBox.Show("删除信息成功！");

                }
                catch (Exception e1)
                {
                    Log.e($"删除ToolStripMenuItem_Click {e1}");
                }
                #endregion
            }
            LoadView();
        }
        private void EquipEditDialog_Load(object sender, EventArgs e)
        {
            LoadView();
        }

        private void fileView1_Load(object sender, EventArgs e)
        {

        }
    }
}
