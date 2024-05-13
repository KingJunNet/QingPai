using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using ExpertLib.IO;
using ExpertLib.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraBars.Navigation;
using LabSystem.DAL;
using System.Diagnostics;
using TaskManager.编辑数据;
using TaskManager.Properties;
using IWshRuntimeLibrary;
using System.Text.RegularExpressions;
using TaskManager.domain.valueobject;
using TaskManager.controller;
using TaskManager.common.utils;
using TaskManager.Model;

namespace TaskManager
{
    //1、timer1点击事件（判断更新、修改密码）
    //2、版本号
    //3、更新lims数据
    //4、首次进入判断更新
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public static double curVersion = 2024.0003;
        public static double newestVersion = 2024.0003;


        public const string RootFolder = "轻排参数表服务器";
        public readonly string Server;

        //public string Folder => $"\\\\{Server}{RootFolder}\\轻排更新包.exe";

        public string Folder => @"D:\code\TaskMangerSetup\Debug\setup.exe";

        private Dictionary<string, ThirdAppCfgItem> thirdAppCfgItemMap;

        public static DateTimeFormatInfo DTFormat
        {
            get
            {
                var dtFormat = new DateTimeFormatInfo
                {
                    ShortDatePattern = "yyyy/MM/dd",
                    LongDatePattern = "yyyy/MM/dd HH:mm:ss"
                };
                return dtFormat;
            }
        }

        private readonly DataControl Sql = new DataControl();

        #region 构造函数 & 显示与关闭

        public Form1()
        {
            InitializeComponent();
            if (DesignMode)
                return;

            InitComboxDictionary();

            var sql = new DataControl();
            Server = sql.ServerIP;
            if (!Server.EndsWith("\\"))
                Server += "\\";
            //InitReportTypeDic();

            if (FormSignIn.CurrentUser.Role.Contains("管理"))
            {


            }

            else
            {

                EquipmentForms = new Dictionary<string, EquipmentForm>
                {
                    {
                        FormSignIn.CurrentUser.Department,
                        new EquipmentForm(FormType.EquipmentDepartment, FormSignIn.CurrentUser.Department)
                    }
                };
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            string userRole = FormSignIn.CurrentUser.Role;
            if (!userRole.Equals(Role.超级管理员.ToString()))
            {
                btnSubmmitExe.Visible = false;
                btnSubmmitCode.Visible = false;
                btnDownloadCode.Visible = false;
            }
            ShowUserInfo();
            ShowInstallPath();

            InitReminder();

            string sql = $"select password from UserTable where userName ='{FormSignIn.CurrentUser.Name}'";
            if (SqlHelper.GetList(sql).Rows[0][0].ToString() == "Catarc@123")
            {
                MessageBox.Show("当前密码为初始密码，请及时修改！");
            }
        }

        public static void ShowWaitForm()
        {
            try
            {
                SplashScreenManager.ShowForm(typeof(WaitForm1));
            }
            catch (Exception ex)
            {
                Log.e($"ShowWaitForm {ex}");
            }
        }

        public static void CloseWaitForm()
        {
            try
            {
                SplashScreenManager.CloseForm();
            }
            catch (Exception ex)
            {
                Log.e($"CloseWaitForm {ex}");
            }
        }

        /// <summary>
        /// 用户信息显示
        /// </summary>
        private void ShowUserInfo()
        {
            barTextUser.Caption = FormSignIn.CurrentUser.Name;
            barTextDepartment.Caption = FormSignIn.CurrentUser.Department;
            barTextRole.Caption = FormSignIn.CurrentUser.Role;
            lblUser.Text = FormSignIn.CurrentUser.Describe;
            lblDepartment.Text = FormSignIn.CurrentUser.Department;
            lblRole.Text = FormSignIn.CurrentUser.Role;
            lblVersion.Caption = "版本号:" + curVersion;
            //lblVersion.Caption = "版本号:20220428.01";
            lblServer.Caption = "服务器:" + Sql.ServerIP;
        }

        #endregion

        #region BackView

        private void backstageViewButtonItem2_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            FormLog log = new FormLog();
            log.ShowDialog();
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backstageViewButtonItem1_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 数据备份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backstageViewButtonItem3_ItemClick(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
            try
            {
                var fileDialog = new SaveFileDialog
                {
                    Title = "备份 NewTaskManager 数据库",
                    Filter = "数据备份文件(*.bak)|*.bak",
                    FileName = "NewTaskManager-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"),
                    RestoreDirectory = true
                };
                if (fileDialog.ShowDialog() != DialogResult.OK) return;

                var file = new FileInfo(fileDialog.FileName);
                var strsql = $"BACKUP DATABASE NewTaskManager TO DISK = '{file.FullName}' ";
                Sql.ExecuteNonQuery(strsql);
                //MessageBox.Show("备份完成, 请继续备份ORVR数据库");

                //fileDialog = new SaveFileDialog
                //{
                //    Title = "备份 ORVR 数据库",
                //    Filter = "数据备份文件(*.bak)|*.bak",
                //    FileName = "ORVR-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"),
                //    RestoreDirectory = true
                //};
                //if (fileDialog.ShowDialog() != DialogResult.OK) return;

                //file = new FileInfo(fileDialog.FileName);
                //strsql = $"BACKUP DATABASE ORVR TO DISK = '{file.FullName}' ";
                //Sql.ExecuteNonQuery(strsql);
                MessageBox.Show("备份完成");
            }
            catch
            {
                MessageBox.Show("无法存放该路径，请更换路径");
            }

        }

        /// <summary>
        /// 判断是否包含数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(String str)
        {
            bool result = false;
            for (int i = 0; i < str.Length; i++)
            {
                if (Char.IsNumber(str, i))
                {
                    return true;
                }
            }
            return result;
        }

        /// <summary>
        /// 判断是否包含字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isLetter(string str)
        {
            return Regex.Matches(str, "[a-zA-Z]").Count > 0;
        }

        /// <summary>
        /// 判断是否包含特殊字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isExe(string str)
        {
            return Regex.Replace(str, "[\\u4e00-\\u9FA5A-Za-z0-9]", "", RegexOptions.IgnoreCase).Length > 0;

        }


        /// <summary>
        /// 更换密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyPwd_Click(object sender, EventArgs e)
        {
            #region 判定

            if (!txtOldPwd.Text.Trim().Equals(FormSignIn.CurrentUser.Password))
            {
                MessageBox.Show("密码错误");
                txtOldPwd.Text = "";
                txtOldPwd.Focus();
                return;
            }

            if (txtNewPwd.Text.Trim().Equals(""))
            {
                MessageBox.Show("不能为空");
                txtNewPwd.Text = "";
                txtNewPwd.Focus();
                return;
            }

            if (!(txtNewPwd.Text.Trim().Length >= 8 && IsNumber(txtNewPwd.Text.Trim()) && isLetter(txtNewPwd.Text.Trim()) && isExe(txtNewPwd.Text.Trim())))
            {
                MessageBox.Show("新密码为8位（包括）以上字符，包含数字、大小写字母、特殊字符；");
                txtNewPwd2.Text = "";
                txtNewPwd2.Focus();
                return;
            }


            if (!txtNewPwd.Text.Trim().Equals(txtNewPwd2.Text.Trim()))
            {
                MessageBox.Show("新密码两次输入不同");
                txtNewPwd2.Text = "";
                txtNewPwd2.Focus();
                return;
            }

            #endregion

            FormSignIn.CurrentUser.Password = txtNewPwd.Text.Trim();
            if (FormSignIn.RememberedUsers.ContainsKey(FormSignIn.CurrentUser.ID))
            {
                FormSignIn.RememberedUsers.Remove(FormSignIn.CurrentUser.ID);
            }
            FormSignIn.RememberedUsers.Add(FormSignIn.CurrentUser.ID, FormSignIn.CurrentUser);
            //FormSignIn.RememberedUsers.WriteSerializable("data.bin");

            #region SQL UPDATE

            var strsql = "update UserTable set password='" + FormSignIn.CurrentUser.Password + "' where userID='" + FormSignIn.CurrentUser.ID + "'";
            Sql.ExecuteNonQuery(strsql);

            #endregion

            MessageBox.Show("修改成功");
        }

        #region 更改姓名和缩写

        private void textEdit3_TextChanged(object sender, EventArgs e)
        {
            var value = textEdit3.Text.Trim();
            textEdit2.Text = value.GetFirstPinyin();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var userName = textEdit3.Text.Trim();
            var letter = textEdit2.Text.Trim();

            if (userName == "" || !userName.IsAllChinese())
            {
                MessageBox.Show("姓名不能为空而且必须为汉字", "警告");
                return;
            }
            if (letter == "")
            {
                MessageBox.Show("缩写不能为空", "警告");
                return;
            }

            #region UPDATE

            FormSignIn.CurrentUser.Name = userName;
            FormSignIn.CurrentUser.Letter = letter;
            string strsql = "update UserTable set userName='" + userName + "',firstLetter='" + letter + "' where userID='" + FormSignIn.CurrentUser.ID + "'";
            Sql.ExecuteNonQuery(strsql);

            FormSignIn.RememberedUsers.Remove(FormSignIn.CurrentUser.ID);
            FormSignIn.RememberedUsers.WriteSerializable("data.bin");

            #endregion

            ShowUserInfo();
        }

        #endregion

        #endregion

        #region 导航栏点击




        #region 综合管理



        private EquipmentForm formEquipmentAll;

        private EquipmentUsageRecordForm EquipmentUsageRecordForm;
        private ConfigItemForm configItemForm;

        private CreateTaskForm CreateTaskForm;

        private StatisticForm formStatistic;







        /// <summary>
        /// 设备管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accordionControlElement37_Click(object sender, EventArgs e)
        {
            try
            {
                Log.e("OpenForm formEquipmentAll");
                ShowWaitForm();

                if (formEquipmentAll == null || formEquipmentAll.IsDisposed)
                {
                    formEquipmentAll = new EquipmentForm(FormType.Equipment, null)
                    {
                        MdiParent = this,
                        WindowState = FormWindowState.Maximized
                    };
                    formEquipmentAll.Show();
                }
                else
                {
                    formEquipmentAll.MdiParent = this;
                    formEquipmentAll.WindowState = FormWindowState.Maximized;
                    formEquipmentAll.Show();
                    formEquipmentAll.Activate();
                }
            }
            catch (Exception ex)
            {
                Log.e($"OpenForm formEquipmentAll {ex}");
            }
            finally
            {
                Log.e("OpenForm formEquipmentAll Finish");
                CloseWaitForm();
            }
        }

        /// <summary>
        /// 设备使用记录管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accordionControlElement39_Click(object sender, EventArgs e)
        {
            try
            {
                Log.e("OpenForm EquipmentUsageRecord");
                ShowWaitForm();

                if (EquipmentUsageRecordForm == null || EquipmentUsageRecordForm.IsDisposed)
                {
                    EquipmentUsageRecordForm = new EquipmentUsageRecordForm(FormType.EquipmentUsageRecord, null)
                    {
                        MdiParent = this,
                        WindowState = FormWindowState.Maximized
                    };
                    EquipmentUsageRecordForm.Show();
                }
                else
                {
                    EquipmentUsageRecordForm.MdiParent = this;
                    EquipmentUsageRecordForm.WindowState = FormWindowState.Maximized;
                    EquipmentUsageRecordForm.Show();
                    EquipmentUsageRecordForm.Activate();
                }
            }
            catch (Exception ex)
            {
                Log.e($"OpenForm EquipmentUsageRecordForm {ex}");
            }
            finally
            {
                Log.e("OpenForm EquipmentUsageRecordForm Finish");
                CloseWaitForm();
            }
        }

        private void CreateTask_Click(object sender, EventArgs e)
        {
            try
            {
                Log.e("OpenForm CreateTaskForm");
                ShowWaitForm();

                if (CreateTaskForm == null || CreateTaskForm.IsDisposed)
                {
                    CreateTaskForm = new CreateTaskForm(CreateTestTaskFrom.MAIN_FORM)
                    {
                        MdiParent = this,
                        WindowState = FormWindowState.Maximized
                    };
                    CreateTaskForm.Show();
                }
                else
                {
                    CreateTaskForm.MdiParent = this;
                    CreateTaskForm.WindowState = FormWindowState.Maximized;
                    CreateTaskForm.Show();
                    CreateTaskForm.Activate();
                }
            }
            catch (Exception ex)
            {
                Log.e($"OpenForm CreateTaskForm {ex}");
            }
            finally
            {
                Log.e("OpenForm CreateTaskForm Finish");
                CloseWaitForm();
            }
        }


        private void accordionControlElement40_Click(object sender, EventArgs e)
        {
            if (formStatistic == null || formStatistic.IsDisposed)
                formStatistic = new StatisticForm();

            formStatistic.Show();
            formStatistic.Activate();
        }
        #endregion

        #region 试验组



        private readonly Dictionary<string, EquipmentForm> EquipmentForms = new Dictionary<string, EquipmentForm>();





        private void OpenDepartmentEquipmentFormClick(object sender, EventArgs e)
        {
            if (!(sender is AccordionControlElement button))
                return;

            var department = button.Tag.ToString().Trim();
            OpenDepartmentEquipmentForm(department);
        }





        private void OpenDepartmentEquipmentForm(string department)
        {
            Log.i($"Open EquipmentForms:{department}", "设备管理");
            try
            {
                ShowWaitForm();

                var form = EquipmentForms.ContainsKey(department)
                    ? EquipmentForms[department]
                    : new EquipmentForm(FormType.EquipmentDepartment, department);

                if (form == null || form.IsDisposed)
                {
                    form = new EquipmentForm(FormType.EquipmentDepartment, department)
                    {
                        MdiParent = this,
                        WindowState = FormWindowState.Maximized
                    };
                    form.Show();
                }
                else
                {
                    form.MdiParent = this;
                    form.WindowState = FormWindowState.Maximized;
                    form.Show();
                    form.Activate();
                }

                if (EquipmentForms.ContainsKey(department))
                    EquipmentForms[department] = form;
                else
                    EquipmentForms.Add(department, form);

                Log.i($"Open EquipmentForms:{department}", "设备管理");
            }
            catch (Exception ex)
            {
                Log.e($"Open EquipmentForms:{department} {ex}");
            }
            finally
            {
                CloseWaitForm();
            }
        }

        #endregion

        #region 系统维护

        private FormUser frmUser;

        private void OpenUserForm(object sender, EventArgs e)
        {
            this.Text = "";
            if (frmUser == null || frmUser.IsDisposed)
            {
                frmUser = new FormUser { MdiParent = this, WindowState = FormWindowState.Maximized };
                frmUser.Show();
            }
            else
            {


                frmUser.MdiParent = this;
                frmUser.WindowState = FormWindowState.Maximized;
                frmUser.Show();
                frmUser.Activate();
            }
        }

        private void OpenRoleForm(object sender, EventArgs e)
        {
            var dialog = new AuthorityForm();
            dialog.ShowDialog();
        }

        #endregion

        #endregion

        #region 托盘菜单

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.Show();
        }

        #endregion


        private bool YNinstall;

        #region Exe更新

        ///更新方法说明：
        ///适用对象：开发者在多个客户端更新修改成熟后版本
        ///原理：启动时候ShowInstallPath()检查当前版本号和数据库version表中的版本号是否匹配，然后弹出对话框下载更新文件
        ///方法：
        ///step1. 修改public static double VERSION的值(在本文件最上方定义)
        ///step2. 将更新的文件打包rar
        ///step3. 点击提交更新按钮，选择step2打包的文件
        ///step4. 打开SQL2008,手动修改version表中的值 = public static double VERSION
        ///step5. 别的客户端重新打开软件，就会弹出提示下载更新的窗口，下载该更新包（此处是客户自行操作）
        ///step6. 按照提示，将更新文件解压，然后覆盖自己电脑上的同名文件

        /// <summary>
        /// 判定并下载更新
        /// </summary>
        private void ShowInstallPathBack()
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory;

            //DirectoryInfo info0 = new DirectoryInfo(Application.StartupPath);
            //string debugpath = info0.Parent.FullName;

            var info = new FileInfo(exePath);

            newestVersion = Sql.GetExpr1("select top 1 version as Expr1 from Version", 1000.00);
            var count = Sql.GetExpr1("select count(*) as Expr1 from FileTable where category='APP_EXE'", 0);
            if (!(newestVersion > curVersion) || count <= 0) return;
            //if (!(NEWEST_Version > VERSION)) return;
            YNinstall = true;
            //if (MessageBox.Show("更新方法：点击确定进行更新" +
            //                    "\n版本号：" + NEWEST_Version +               
            //                    "\n点击取消忽略本次更新",
            //        "有新版本请更新", MessageBoxButtons.OKCancel) != DialogResult.OK) {                
            //    YNinstall = false;
            //    return;
            //}
            //else
            //{
            //    YNinstall = false;
            //}
            MessageBox.Show("更新方法：点击确定进行更新" +
                                "\n版本号：" + newestVersion, "有新版本请更新");
            YNinstall = false;
            //var dialog = new FolderBrowserDialog { Description = "请选择下载目标文件夹" };
            //if (dialog.ShowDialog() != DialogResult.OK) return;



            try
            {
                Process.Start(Folder);
            }
            catch
            {
                MessageBox.Show("无法访问安装文件，请确认此电脑是否正常访问服务器共享文件夹！");
            }
            finally
            {

                System.Environment.Exit(0);
            }


            //try
            //{
            //    ShowWaitForm();

            //    var strsql = "select top 1 [file] as Expr1,name as Expr2 from FileTable where category='APP_EXE'";
            //    var dt = Sql.ExecuteQuery(strsql).Tables[0];
            //    if (dt.Rows.Count == 0) return;

            //    var picBytes = (byte[])dt.Rows[0]["Expr1"];
            //    //var stream0 = new FileStream(fileDialog.FileName, FileMode.Open);
            //    //var picBytes = stream0.StreamToBytes();

            //    using (var stream = new FileStream(dialog.SelectedPath + "\\轻排业务管理系统更新包.zip", FileMode.Create))
            //    {
            //        stream.Write(picBytes, 0, picBytes.Length);
            //    }

            //    MessageBox.Show("更新包已下载");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString(), "下载失败");
            //}
            //finally
            //{
            //    Form1.CloseWaitForm();
            //}
        }

        /// <summary>
        /// 判定并下载更新
        /// </summary>
        private void ShowInstallPath()
        {
            //是否需要下载
            if (!this.isNeedUpdateApp())
            {
                return;
            }

            MessageBox.Show("更新方法：点击确定进行更新" +
                                "\n版本号：" + newestVersion, "有新版本请更新");
            try
            {
                string appFilePath = "";
                if (ServerConfig.Instance.IsCanConnectBlobServer())
                {
                    appFilePath = ServerConfig.Instance.AppExePath;
                }
                else
                {
                    FileDownloadResult result = this.downloadFile(FileCategory.APP_EXE);
                    if (!result.IsSuccess)
                    {
                        MessageBox.Show($"更新包下载失败,失败原因：{result.ErrorMessage}", "错误信息", MessageBoxButtons.OK);
                        return;
                    }
                    appFilePath = result.FilePath;
                }

                DialogResult downResult = MessageBox.Show("更新包已下载,开始安装");
                if (downResult == DialogResult.OK)
                {
                    Process.Start(appFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "更新包安装失败");
            }
            finally
            {
                System.Environment.Exit(0);
            }
        }

        private bool isNeedUpdateApp()
        {
            var strsql = $"select top 1 version,userScope from Version";
            var dt = Sql.ExecuteQuery(strsql).Tables[0];
            if (dt.Rows.Count == 0)
            {
                return false;
            }
            newestVersion = double.Parse(dt.Rows[0]["version"].ToString());
            string userScope = DbHelper.dataColumn2String(dt.Rows[0]["userScope"]);
            var count = Sql.GetExpr1("select count(*) as Expr1 from FileTable where category='APP_EXE'", 0);
            if ((curVersion >= newestVersion) || count <= 0)
            {
                return false;
            }
            //未设置范围
            if (string.IsNullOrWhiteSpace(userScope))
            {
                return true;
            }

            string curUser = FormSignIn.CurrentUser.Name;
            List<string> users = new List<string>(userScope.Split('，'));
            bool isNeed = users.Exists(item => item.Equals("ALL") || item.Equals(curUser));

            return isNeed;
        }

        /// <summary>
        /// 提交更新文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmitUpdate_Click(object sender, EventArgs e)
        {
            string value = XtraInputBox.Show("格式如：20170901.01", "请输入新版本号", curVersion.ToString());
            if (!double.TryParse(value, out newestVersion))
            {
                MessageBox.Show("版本号格式不合法");
                return;
            }
            else if (newestVersion < curVersion)
            {
                MessageBox.Show("版本号比当前版本号小");
                return;
            }

            var fileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = "请选择更新包",
                //Filter = "更新包(*.rar)|*.rar"
            };

            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                Form1.ShowWaitForm();
                var filePath = fileDialog.FileName;
                Sql.ExecuteNonQuery("delete FileTable where category='APP_EXE'");
                var stream = new FileStream(fileDialog.FileName, FileMode.Open);
                var paras = new[] {
                        new SqlParameter("name",Path.GetFileName(filePath) ),
                        new SqlParameter("foreignKey",(curVersion*100).ToString() ),
                        new SqlParameter("file",stream.StreamToBytes())
                        };
                var strsql = "insert into FileTable(name,[file],category,foreignKey) values( ";
                strsql += "@name,@file,'APP_EXE',@foreignKey)";
                Sql.ExecuteNonQuery(strsql, paras);
                Sql.ExecuteNonQuery("update Version set [version]=" + newestVersion);
                MessageBox.Show("更新包提交成功", "提示", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Form1.CloseWaitForm();
            }
        }

        private void btnSubmmitCode_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = "请选择项目源代码",
                Filter = "压缩文件|*.zip;*.rar"
            };
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                Form1.ShowWaitForm();
                string key = DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                var filePath = fileDialog.FileName;
                Sql.ExecuteNonQuery("delete FileTable where category='PROJECT_CODE'");
                var stream = new FileStream(fileDialog.FileName, FileMode.Open);
                var paras = new[] {
                        new SqlParameter("name",Path.GetFileName(filePath) ),
                        new SqlParameter("foreignKey", key),
                        new SqlParameter("file",stream.StreamToBytes())
                        };
                var strsql = "insert into FileTable(name,[file],category,foreignKey) values( ";
                strsql += "@name,@file,'PROJECT_CODE',@foreignKey)";
                Sql.ExecuteNonQuery(strsql, paras);
                MessageBox.Show("项目源代码提交成功", "提示", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                Form1.CloseWaitForm();
            }
        }

        private void btnDownloadExe_Click(object sender, EventArgs e)
        {
            if (this.downloadFromShareFolder(FileCategory.APP_EXE))
            {
                return;
            }

            FileDownloadResult result = this.downloadFile(FileCategory.APP_EXE);
            if (!result.IsSuccess)
            {
                MessageBox.Show($"更新包下载失败,失败原因：{result.ErrorMessage}", "错误信息", MessageBoxButtons.OK);
                return;
            }
            MessageBox.Show($"更新包已下载,请转至路径'{result.FilePath}'查看", "提示", MessageBoxButtons.OK);
        }

        private bool downloadFromShareFolder(FileCategory category)
        {
            if (!ServerConfig.Instance.IsCanConnectBlobServer())
            {
                return false;
            }
            try
            {
                this.openFileFromShareDirectory(category);
            }
            catch (Exception ex)
            {
                return false;
            }

            //MessageBox.Show($"文件目录已打开，请直接拷贝文件", "提示", MessageBoxButtons.OK);
            return true;
        }

        private void btnDownloadCode_Click(object sender, EventArgs e)
        {
            if (this.downloadFromShareFolder(FileCategory.PROJECT_CODE))
            {
                return;
            }

            FileDownloadResult result = this.downloadFile(FileCategory.PROJECT_CODE);
            if (!result.IsSuccess)
            {
                MessageBox.Show($"项目源代码下载失败,失败原因：{result.ErrorMessage}", "错误信息", MessageBoxButtons.OK);
                return;
            }
            MessageBox.Show($"项目源代码已下载,请转至路径'{result.FilePath}'查看", "提示", MessageBoxButtons.OK);
        }

        private string downloadAppFile()
        {
            string appFilePath = "";

            var dialog = new FolderBrowserDialog { Description = "请选择下载目标文件夹" };
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return appFilePath;
            }
            ShowWaitForm();
            var strsql = "select top 1 [file] as Expr1,name as Expr2 from FileTable where category='APP_EXE'";
            var dt = Sql.ExecuteQuery(strsql).Tables[0];
            if (dt.Rows.Count == 0)
            {
                return appFilePath;
            }

            var picBytes = (byte[])dt.Rows[0]["Expr1"];
            //var stream0 = new FileStream(fileDialog.FileName, FileMode.Open);
            //var picBytes = stream0.StreamToBytes();

            appFilePath = dialog.SelectedPath + "\\TaskMangerSetup.msi";
            using (var stream = new FileStream(appFilePath, FileMode.Create))
            {
                stream.Write(picBytes, 0, picBytes.Length);
            }
            CloseWaitForm();

            return appFilePath;
        }

        private FileDownloadResult downloadFile(FileCategory category)
        {
            FileDownloadResult result = new FileDownloadResult();

            var dialog = new FolderBrowserDialog { Description = "请选择下载目标文件夹" };
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return result.failed("未选择文件夹");
            }
            result = this.executeDownloadFile(category, dialog.SelectedPath);

            return result;
        }

        private FileDownloadResult executeDownloadFile(FileCategory category, string fileDir)
        {
            FileDownloadResult result = new FileDownloadResult();

            ShowWaitForm();
            try
            {
                if (ServerConfig.Instance.IsCanConnectBlobServer())
                {
                    result = this.downloadFileFromShareDirectory(category, fileDir);
                }
                else
                {
                    result = this.downloadFileFromDb(category, fileDir);
                }
            }
            catch (Exception ex)
            {
                result = result.failed(ex.Message);
            }
            finally
            {
                CloseWaitForm();
            }

            return result;
        }

        private FileDownloadResult downloadFileFromDb(FileCategory category, string fileDir)
        {
            FileDownloadResult result = new FileDownloadResult();

            //查询文件
            var strsql = $"select top 1 [file] as Expr1,name as Expr2 from FileTable where category='{category.ToString()}'";
            var dt = Sql.ExecuteHighCostQuery(strsql).Tables[0];
            if (dt.Rows.Count == 0)
            {
                return result.failed("文件不存在"); ;
            }

            //写入文件
            var picBytes = (byte[])dt.Rows[0]["Expr1"];
            string fileName = dt.Rows[0]["Expr2"].ToString();
            //var stream0 = new FileStream(fileDialog.FileName, FileMode.Open);
            //var picBytes = stream0.StreamToBytes();

            string filePath = fileDir + $"\\{fileName}";
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                stream.Write(picBytes, 0, picBytes.Length);
            }

            return result.success(filePath);
        }

        private FileDownloadResult downloadFileFromShareDirectory(FileCategory category, string fileDirectory)
        {
            FileDownloadResult result = new FileDownloadResult();

            //查询文件
            string serverFileDirectory = "";
            string fileName = "";
            switch (category)
            {
                case FileCategory.APP_EXE:
                    serverFileDirectory = ServerConfig.Instance.AppExeDirectory;
                    fileName = ServerConfig.TASK_MANAGER_APP_EXE_NAME;
                    break;
                case FileCategory.PROJECT_CODE:
                    serverFileDirectory = ServerConfig.Instance.CodeDirectory;
                    fileName = ServerConfig.PROJECT_CODE_NAME;
                    break;
                default:
                    break;
            }

            string serverFilePath = serverFileDirectory + $"\\{fileName}";
            string filePath = fileDirectory + $"\\{fileName}";
            System.IO.File.Copy(serverFilePath, filePath, true);

            return result.success(filePath);
        }

        private void openFileFromShareDirectory(FileCategory category)
        {
            //查询文件
            string serverFileDirectory = "";
            string fileName = "";
            switch (category)
            {
                case FileCategory.APP_EXE:
                    serverFileDirectory = ServerConfig.Instance.AppExeDirectory;
                    fileName = ServerConfig.TASK_MANAGER_APP_EXE_NAME;
                    break;
                case FileCategory.PROJECT_CODE:
                    serverFileDirectory = ServerConfig.Instance.CodeDirectory;
                    fileName = ServerConfig.PROJECT_CODE_NAME;
                    break;
                default:
                    break;
            }
            System.Diagnostics.Process.Start("Explorer.exe", serverFileDirectory);
        }


        #endregion

        #region Other | 下拉框

        /// <summary>
        /// 点击状态条用户名显示账号界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barTextUser_ItemDoubleClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ribbonControl1.ShowApplicationButtonContentControl();
            backstageViewControl1.SelectedTab = backstageViewTabItem1;
        }

        /// <summary>
        /// 下拉框备选项
        /// </summary>
        public static Dictionary<string, List<string>> ComboxDictionary = new Dictionary<string, List<string>>();

        /// <summary>
        /// 设备报警灯
        /// </summary>
        public static bool IsLight;

        /// <summary>
        /// 初始化下拉框备选项
        /// </summary>
        public static void InitComboxDictionary()
        {
            var sql = new DataControl();
            var formats = new[]
            {

                "报告类型",
                "国环视同",
                "任务备注",
                "任务类型简称",
                "项目经理",
                "项目经理：初校状态",
                "项目经理：个人备注",

                "车辆类型",

                "定位编号",
                "实验地点",
                "是否报国环",
                "数据合格状态",
                "项目类型",

                "完成状态",
                "变速器形式",
                "发动机生产厂",
                "燃油标号",
                "生产厂家",

                "检验依据1",
                "项目名称",
                "标准阶段",
                "项目简称",
                "动力类型",
                "是否直喷",
                "样品类型",
                "燃料类型",
                "供油种类",
                "驱动形式",
                "项目备注",


                "标准",
                "检定方式",
                "排放限值档",
                "情况等级",            
                //"设备状态",
          
                "试验项目",
                "数据状态",
                "转鼓",

                "项目代码",
                "试验里程数",
                "预处理里程数",
                "价格单位",
                "单价",
                "测试周期",

                "费用确认",
                "胎压",

                "检定地点",
                "设备状态",
                "设备使用状况"

            };

            foreach (var format in formats)
            {
                //var list = sql.GetStringList("select distinct item from ComboxTable " +
                //                             $"where format='{format}' order by item");
                var list = sql.GetStringList("select distinct Value from ConfigItemTable " +
                                            $"where Name='{format}' order by Value");
                ComboxDictionary.Add(format, list);
            }

            //选项名称
            List<string> optionNames = sql.GetStringList("select distinct Name from ConfigItemTable order by Name");
            ComboxDictionary.Add("配置项名称", optionNames);
        }

        /// <summary>
        /// 任务单号-报告类型对应 - 用于ReportEditDialog
        /// </summary>
        public static Dictionary<string, string> ReportTypeDic = new Dictionary<string, string>();

        public static void InitReportTypeDic()
        {
            var sql = new DataControl();
            var dt = sql.ExecuteQuery("select * from ReportTypeTable").Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                var prefix = dr[0].ToString().ToUpper().Trim();
                var reportType = dr[1].ToString().Trim();
                if (ReportTypeDic.ContainsKey(prefix))
                    continue;

                ReportTypeDic.Add(prefix, reportType);
            }
        }

        #endregion

        #region 定时检查

        private void InitReminder()
        {
            //var dialog = new ReminderDialog();
            //dialog.ShowDialog();

            //barRemainder.Caption = $"任务提醒[{dialog.no_read_no_finish + dialog.equipment}]";

            //timer1.Interval = (int)(ReminderDialog.Interval * 60 * 1000);
            //timer1.Enabled = true;
            //timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (YNinstall == false)
            //{
            //    ShowInstallPath();
            //}


            //string sql = $"select password from UserTable where userName ='{FormSignIn.CurrentUser.Name}'";
            //if(SqlHelper.GetList(sql).Rows[0][0].ToString()== "Catarc@123")
            //{
            //    MessageBox.Show("当前密码为初始密码，请及时修改！");
            //}



            //timer1.Stop();
            //var dialog = new ReminderDialog();
            //barRemainder.Caption = $"任务提醒[{dialog.no_read_no_finish + dialog.equipment}]";

            //if (dialog.IsReminder)
            //    dialog.ShowDialog();
            //timer1.Interval = (int)(ReminderDialog.Interval * 60 * 1000);
            //timer1.Enabled = true;
            //timer1.Start();
        }

        private void barRemainder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            timer1_Tick(null, null);

        }


        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            DataBind("任务管理", 任务管理); //任务管理
            DataBind("样品信息", 样品管理);
            DataBind("试验统计", 试验统计);
            DataBind("项目报价", 项目报价);

            WebsiteBind(网址链接);



            Filter.filterText = "";
            //accordionControlElement23.Elements.Add();
            //accordionControlElement23.Elements[1].Text = "sssss";
            //accordionControlElement23.Elements[1].Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
        }

        #region 自定义模板

        private AlertTemplate alertTemplate;
        /// <summary>
        /// 自定义模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accordionControlElement24_Click(object sender, EventArgs e)
        {
            if (alertTemplate == null || alertTemplate.IsDisposed)
            {
                alertTemplate = new AlertTemplate();
                alertTemplate.Show();
            }
            else
            {
                alertTemplate.Show();
                alertTemplate.Activate();
            }

        }




        public void DataBind(string moudle, AccordionControlElement name)
        {
            string sql = $"select distinct templatename,index1 from definetemplate where type ='{moudle}' order by index1";
            DataTable dt = SqlHelper.GetList(sql);
            int i = 1;
            foreach (DataRow row in dt.Rows)
            {
                name.Elements.Add();
                name.Elements[i].Text = row[0].ToString();
                name.Elements[i].Style = ElementStyle.Item;

                switch (name.Name.ToString())
                {
                    case "任务管理":
                        name.Elements[i].Click += 任务管理_ElementClick;
                        break;
                    case "样品管理":
                        name.Elements[i].Click += 样品信息_ElementClick;
                        break;
                    case "试验统计":
                        name.Elements[i].Click += 试验统计_ElementClick;
                        break;
                    case "项目报价":
                        name.Elements[i].Click += 项目报价_ElementClick;
                        break;
                }


                i++;

            }

        }

        public void SelectTemplate(string moudle, object sender)
        {
            string sql = $"select templatecolumn from definetemplate where type ='{moudle}' and templatename ='{((AccordionControlElement)sender).Text}'";
            DataTable da = SqlHelper.GetList(sql);
            string columns = da.Rows[0][0].ToString();
            string[] column = columns.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            Templatecolumn.column = column;

            for (int j = 1; j < taskForm._control._view.Columns.Count; j++)
            {
                taskForm._control._view.Columns[j].Visible = false;
            }
            for (int i = Templatecolumn.column.Length - 1; i >= 0; i--)
            {
                taskForm._control._view.Columns[Templatecolumn.column[i]].Visible = true;
                // _control._view.Columns[Templatecolumn.column[i]].VisibleIndex = i;
            }
        }

        //注册点击事件

        //private string moudle;
        public void 任务管理_ElementClick(object sender, EventArgs e)
        {

            accordionControlElement34_Click(accordionControlElement34, e);


            string sql = $"select templatecolumn from definetemplate where type ='任务管理' and templatename ='{((AccordionControlElement)sender).Text}'";

            this.Text = ((AccordionControlElement)sender).Text;

            Templatecolumn.name = ((AccordionControlElement)sender).Text;

            DataTable da = SqlHelper.GetList(sql);
            if (da.Rows.Count > 0)
            {
                string columns = da.Rows[0][0].ToString();
                string[] column = columns.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Templatecolumn.column = column;

                for (int j = 1; j < taskForm._control._view.Columns.Count; j++)
                {
                    taskForm._control._view.Columns[j].Visible = false;
                }
                for (int i = Templatecolumn.column.Length - 1; i >= 0; i--)
                {
                    taskForm._control._view.Columns[Templatecolumn.column[i]].Visible = true;
                    // _control._view.Columns[Templatecolumn.column[i]].VisibleIndex = i;
                }
            }
            else
            {
                MessageBox.Show("模板不存在");
            }


        }

        public void 样品信息_ElementClick(object sender, EventArgs e)
        {
            accordionControlElement42_Click(accordionControlElement42, e);
            string sql = $"select templatecolumn from definetemplate where type ='样品信息' and templatename ='{((AccordionControlElement)sender).Text}'";
            this.Text = ((AccordionControlElement)sender).Text;
            Templatecolumn.name = ((AccordionControlElement)sender).Text;
            DataTable da = SqlHelper.GetList(sql);
            if (da.Rows.Count > 0)
            {
                string columns = da.Rows[0][0].ToString();
                string[] column = columns.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Templatecolumn.column = column;

                for (int j = 1; j < sampleForm._control._view.Columns.Count; j++)
                {
                    sampleForm._control._view.Columns[j].Visible = false;
                }
                for (int i = Templatecolumn.column.Length - 1; i >= 0; i--)
                {
                    sampleForm._control._view.Columns[Templatecolumn.column[i]].Visible = true;
                    // _control._view.Columns[Templatecolumn.column[i]].VisibleIndex = i;
                }
            }
            else
            {
                MessageBox.Show("模板不存在");
            }


        }
        public void 试验统计_ElementClick(object sender, EventArgs e)
        {
            accordionControlElement44_Click(accordionControlElement42, e);
            string sql = $"select templatecolumn from definetemplate where type ='试验统计' and templatename ='{((AccordionControlElement)sender).Text}'";
            this.Text = ((AccordionControlElement)sender).Text;
            Templatecolumn.name = ((AccordionControlElement)sender).Text;
            DataTable da = SqlHelper.GetList(sql);
            if (da.Rows.Count > 0)
            {
                string columns = da.Rows[0][0].ToString();
                string[] column = columns.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Templatecolumn.column = column;

                for (int j = 1; j < teststatistic._control._view.Columns.Count; j++)
                {
                    teststatistic._control._view.Columns[j].Visible = false;
                }
                for (int i = Templatecolumn.column.Length - 1; i >= 0; i--)
                {
                    teststatistic._control._view.Columns[Templatecolumn.column[i]].Visible = true;
                    // _control._view.Columns[Templatecolumn.column[i]].VisibleIndex = i;
                }
            }
            else
            {
                MessageBox.Show("模板不存在");
            }


        }
        public void 项目报价_ElementClick(object sender, EventArgs e)
        {
            accordionControlElement43_Click(accordionControlElement42, e);
            string sql = $"select templatecolumn from definetemplate where type ='项目报价' and templatename ='{((AccordionControlElement)sender).Text}'";
            this.Text = ((AccordionControlElement)sender).Text;
            Templatecolumn.name = ((AccordionControlElement)sender).Text;
            DataTable da = SqlHelper.GetList(sql);
            if (da.Rows.Count > 0)
            {
                string columns = da.Rows[0][0].ToString();
                string[] column = columns.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                Templatecolumn.column = column;

                for (int j = 1; j < projectprice._control._view.Columns.Count; j++)
                {
                    projectprice._control._view.Columns[j].Visible = false;
                }
                for (int i = Templatecolumn.column.Length - 1; i >= 0; i--)
                {
                    projectprice._control._view.Columns[Templatecolumn.column[i]].Visible = true;
                    // _control._view.Columns[Templatecolumn.column[i]].VisibleIndex = i;
                }
            }
            else
            {
                MessageBox.Show("模板不存在");
            }

        }

        #endregion





        private NewTaskForm taskForm;

        /// <summary>
        /// 任务管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accordionControlElement34_Click(object sender, EventArgs e)
        {
            try
            {
                Log.i("进入任务管理模块", "任务管理");
                ShowWaitForm();

                if (taskForm == null || taskForm.IsDisposed)
                {
                    taskForm = new NewTaskForm(FormType.NewTask, null)
                    {
                        MdiParent = this,
                        WindowState = FormWindowState.Maximized
                    };
                    taskForm.Show();
                }
                else
                {
                    taskForm.MdiParent = this;
                    taskForm.WindowState = FormWindowState.Maximized;
                    taskForm.Show();
                    taskForm.Activate();
                }


            }
            catch (Exception ex)
            {
                Log.e($"OpenForm {ex}");

            }
            finally
            {

                Log.e("OpenForm Finish");
                CloseWaitForm();
                this.Text = "默认模板";
                Templatecolumn.name = "默认模板";

                for (int j = 1; j < taskForm._control._view.Columns.Count; j++)
                {
                    taskForm._control._view.Columns[j].Visible = false;
                }
                for (int j = 1; j < taskForm._control._view.Columns.Count; j++)
                {
                    taskForm._control._view.Columns[j].Visible = true;
                    taskForm._control._view.Columns[j].VisibleIndex = j;
                }


            }
        }

        private SampleForm sampleForm;
        /// <summary>
        /// 样品信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accordionControlElement42_Click(object sender, EventArgs e)
        {
            try
            {
                Log.i("进入样品信息模块", "样品信息");
                ShowWaitForm();

                if (sampleForm == null || sampleForm.IsDisposed)
                {
                    sampleForm = new SampleForm(FormType.Sample, null)
                    {
                        MdiParent = this,
                        WindowState = FormWindowState.Maximized
                    };
                    sampleForm.Show();
                }
                else
                {
                    sampleForm.MdiParent = this;
                    sampleForm.WindowState = FormWindowState.Maximized;
                    sampleForm.Show();
                    sampleForm.Activate();
                }


            }
            catch (Exception ex)
            {
                Log.e($"OpenForm {ex}");

            }
            finally
            {
                Log.e("OpenForm Finish");
                CloseWaitForm();
                this.Text = "默认模板";
                Templatecolumn.name = "默认模板";
                for (int j = 1; j < sampleForm._control._view.Columns.Count; j++)
                {
                    sampleForm._control._view.Columns[j].Visible = false;
                }
                for (int j = 1; j < sampleForm._control._view.Columns.Count; j++)
                {
                    sampleForm._control._view.Columns[j].Visible = true;
                    sampleForm._control._view.Columns[j].VisibleIndex = j;
                }
            }
        }

        private TestStatistic teststatistic;
        /// <summary>
        /// 试验统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accordionControlElement44_Click(object sender, EventArgs e)
        {

            try
            {
                Log.i("进入试验统计模块", "试验统计");
                ShowWaitForm();

                if (teststatistic == null || teststatistic.IsDisposed)
                {
                    teststatistic = new TestStatistic(FormType.Test, FormSignIn.CurrentUser.Department.ToString())
                    {
                        MdiParent = this,
                        WindowState = FormWindowState.Maximized
                    };
                    teststatistic.Show();
                }
                else
                {
                    teststatistic.MdiParent = this;
                    teststatistic.WindowState = FormWindowState.Maximized;
                    teststatistic.Show();
                    teststatistic.Activate();
                }


            }
            catch (Exception ex)
            {
                Log.e($"OpenForm {ex}");

            }
            finally
            {
                Log.e("OpenForm Finish");
                CloseWaitForm();
                this.Text = "默认模板";
                Templatecolumn.name = "默认模板";
                for (int j = 1; j < teststatistic._control._view.Columns.Count; j++)
                {
                    teststatistic._control._view.Columns[j].Visible = false;
                }
                for (int j = 1; j < teststatistic._control._view.Columns.Count; j++)
                {
                    teststatistic._control._view.Columns[j].Visible = true;
                    teststatistic._control._view.Columns[j].VisibleIndex = j;
                }
            }
        }

        private ProjectPrice projectprice;

        /// <summary>
        /// 项目报价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accordionControlElement43_Click(object sender, EventArgs e)
        {
            try
            {
                Log.i("进入项目报价模块", "项目报价");
                ShowWaitForm();

                if (projectprice == null || projectprice.IsDisposed)
                {
                    projectprice = new ProjectPrice(FormType.Project, null)
                    {
                        MdiParent = this,
                        WindowState = FormWindowState.Maximized
                    };
                    projectprice.Show();
                }
                else
                {
                    projectprice.MdiParent = this;
                    projectprice.WindowState = FormWindowState.Maximized;
                    projectprice.Show();
                    projectprice.Activate();
                }

            }
            catch (Exception ex)
            {
                Log.e($"OpenForm {ex}");

            }
            finally
            {
                Log.e("OpenForm Finish");
                CloseWaitForm();

                this.Text = "默认模板";
                Templatecolumn.name = "默认模板";
                for (int j = 1; j < projectprice._control._view.Columns.Count; j++)
                {
                    projectprice._control._view.Columns[j].Visible = false;
                }
                for (int j = 1; j < projectprice._control._view.Columns.Count; j++)
                {
                    projectprice._control._view.Columns[j].Visible = true;
                    projectprice._control._view.Columns[j].VisibleIndex = j;
                }
            }
        }

        private FormLog formlog;


        public bool Authority;
        private void accordionControlElement1_Click(object sender, EventArgs e)
        {

            //Authority = Sql.AuthorityCheck2("系统维护", "用户管理");
            if (FormSignIn.CurrentUser.Department == "体系组" || FormSignIn.CurrentUser.Department == "系统维护")
            {
                formlog = new FormLog();
                formlog.ShowDialog();
            }
            else
            {
                MessageBox.Show("您没有权限查看系统日志，请联系管理员");
            }

        }

        private UserStructure Userstructure;
        /// <summary>
        /// 组织结构
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accordionControlElement3_Click(object sender, EventArgs e)
        {
            this.Text = "";
            if (Userstructure == null || Userstructure.IsDisposed)
            {
                Userstructure = new UserStructure { MdiParent = this, WindowState = FormWindowState.Maximized };
                Userstructure.Show();
            }
            else
            {
                Userstructure.MdiParent = this;
                Userstructure.WindowState = FormWindowState.Maximized;
                Userstructure.Show();
                Userstructure.Activate();
            }
        }

        //显示与隐藏
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (accordionControl1.Visible == false)
            {
                accordionControl1.Visible = true;
            }
            else
            {
                accordionControl1.Visible = false;
            }

        }

        private void backstageViewTabItem2_SelectedChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {
        }

        /// <summary>
        /// PVE系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accordionControlElement4_Click(object sender, EventArgs e)
        {
            try
            {
                DirectoryInfo info0 = new DirectoryInfo(Application.StartupPath);
                string debugpath = info0.Parent.FullName + "\\PVE测试管理系统\\PVE测试管理系统.exe";

                Process.Start(debugpath);
            }
            catch
            {
                MessageBox.Show("请将PVE测试管理系统安装包放到D盘根目录下,并将文件夹命名为'PVE测试管理系统'", "提示");
            }

        }

        #region 系统网址
        private void accordionControlElement6_Click(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 系统网址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SelectWebsite website = new SelectWebsite();
            website.freshwibesite += Website_freshwibesite;
            website.ShowDialog();
        }

        private void Website_freshwibesite()
        {

            WebsiteBind(网址链接);
        }

        public void WebsiteBind(AccordionControlElement name)
        {
            name.Elements.Clear();
            string sql = $"select distinct name,index1 from SystemWebsite order by index1";
            DataTable dt = SqlHelper.GetList(sql);
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                name.Elements.Add();
                name.Elements[i].Text = row[0].ToString();
                name.Elements[i].Style = ElementStyle.Item;

                name.Elements[i].Click += 系统网址_ElementClick;

                i++;



            }

        }
        public void 系统网址_ElementClick(object sender, EventArgs e)
        {
            //accordionControlElement42_Click(accordionControlElement42, e);
            string sql = $"select address from SystemWebsite where name ='{((AccordionControlElement)sender).Text}'";
            DataTable da = SqlHelper.GetList(sql);
            if (da.Rows.Count > 0)
            {
                try
                {

                    System.Diagnostics.Process.Start(da.Rows[0][0].ToString());


                }
                catch
                {
                    MessageBox.Show("网站地址有误");
                }
            }
            else
            {
                MessageBox.Show("网址不存在");
            }


        }

        #endregion



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否退出程序",
                 "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                e.Cancel = true;
            }
            //}

            ////将程序从任务栏移除显示

            //this.ShowInTaskbar = false;

            ////隐藏窗口

            //this.Visible = false;

            ////显示托盘图标

            //notifyIcon1.Visible = true;
            //e.Cancel = true;
        }


        public void judgeEquip()
        {


            string sql = $"select alert  from EquipmentTable where group1='{FormSignIn.CurrentUser.Department}'";
            DataTable data = SqlHelper.GetList(sql);
            Boolean bool1 = true;
            foreach (DataRow row in data.Rows)
            {
                if (row[0].ToString() == "是")
                {
                    baralert.Caption = "有设备快到有效期，请及时送检！";

                    baralert.ImageOptions.Image = Resources.红;

                }
            }





        }
        private int alertnum = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (alertnum == 0)
            {
                baralert.ImageOptions.Image = Resources.绿;
                baralert.Caption = "";
            }
            alertnum++;


            if (alertnum == 2)
            {
                //judgeEquip();
                alertnum = 0;
            }
        }

        private void accordionControl1_Click(object sender, EventArgs e)
        {
            //((AccordionControl)sender).BackColor = Color.Blue;
        }

        private void accordionControlWeight_Click(object sender, EventArgs e)
        {
            this.startThirdApp(ThirdAppName.WEIGHT_CLIENT);
        }

        /// <summary>
        /// 蒸发系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accordionControlElement6_Click_1(object sender, EventArgs e)
        {
            this.startThirdApp(ThirdAppName.EVAPORATION_SYSTEM);
        }

        private void startEvaporationSystemOld()
        {
            //try
            //{
            //    //string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //    //MessageBox.Show(path);
            //    DirectoryInfo info0 = new DirectoryInfo(Application.StartupPath);
            //    string debugpath = info0.Parent.FullName + "\\ORVR系统\\ORVR.exe";
            //    //string debugpath = path + "\\ORVR.lnk";
            //    //string realpath = getOriginPath(debugpath);
            //    Process.Start(debugpath);
            //}
            //catch
            //{
            //    MessageBox.Show("请将蒸发系统安装包放到D盘根目录下,并将文件夹命名为'ORVR系统'", "提示");
            //}
        }

        private void startThirdApp(ThirdAppName appName)
        {
            string appDescription = appName.GetDescription();
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string appPath = Path.Combine(appDirectory, ConfigStorage.Instance.ThirdAppRelativePathMap[appName]);
            if (string.IsNullOrEmpty(appPath) || !System.IO.File.Exists(appPath))
            {
                DialogResult result = MessageBox.Show("应用路径无效或文件不存在,请重新安装应用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            //启动应用
            StartApp(appPath);
        }

        private void startThirdAppBack(string appName)
        {
            string appPath = GetAppPathFromBinFile(appName);
            if (string.IsNullOrEmpty(appPath) || !System.IO.File.Exists(appPath))
            {
                if (!this.resetAppPath(appName, out appPath))
                {
                    return;
                }
            }

            //启动应用
            StartApp(appPath);
        }

        private bool resetAppPath(string appName, out string appPath)
        {
            appPath = XtraInputBox.Show("请输入应用的安装路径", "应用路径输入", "");
            if (string.IsNullOrEmpty(appPath) || !System.IO.File.Exists(appPath))
            {
                DialogResult result = MessageBox.Show("应用路径无效或文件不存在,需重新配置应用路径", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    return this.resetAppPath(appName, out appPath);
                }
                else
                {
                    return false;
                }
            }
            SaveAppPathToBinFile(appName, appPath);
            return true;
        }

        private void StartApp(string appPath)
        {
            try
            {
                Process.Start(appPath);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("启动应用时发生错误", ex);
            }
        }

        private string GetAppPathFromBinFile(string appName)
        {
            if (this.thirdAppCfgItemMap == null)
            {
                this.thirdAppCfgItemMap = FileCtr.ReadSerializable<string, ThirdAppCfgItem>(ConstHolder.THIRD_APP_CONFIG_FILE_NAME);
            }
            if (!this.thirdAppCfgItemMap.ContainsKey(appName))
            {
                return null;
            }

            return thirdAppCfgItemMap[appName].Path;
        }

        private void SaveAppPathToBinFile(string appName, string appPath)
        {
            ThirdAppCfgItem cfgItem = new ThirdAppCfgItem(appName, appPath);
            if (this.thirdAppCfgItemMap.ContainsKey(appName))
            {
                this.thirdAppCfgItemMap[appName] = cfgItem;
            }
            else
            {
                this.thirdAppCfgItemMap.Add(appName, cfgItem);
            }
            thirdAppCfgItemMap.WriteSerializable(ConstHolder.THIRD_APP_CONFIG_FILE_NAME);
        }

        // 显示成功消息  
        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 显示错误消息  
        private void ShowErrorMessage(string message, Exception ex = null)
        {
            string errorMessage = ex != null ? $"{message}: {ex.Message}" : message;
            MessageBox.Show(errorMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void tabItemConfig_Click(object sender, EventArgs e)
        {
            try
            {
                Log.e("OpenForm ConfigItemForm");
                ShowWaitForm();

                if (configItemForm == null || configItemForm.IsDisposed)
                {
                    configItemForm = new ConfigItemForm(FormType.ConfigItem, null)
                    {
                        MdiParent = this,
                        WindowState = FormWindowState.Maximized
                    };
                    configItemForm.Show();
                }
                else
                {
                    configItemForm.MdiParent = this;
                    configItemForm.WindowState = FormWindowState.Maximized;
                    configItemForm.Show();
                    configItemForm.Activate();
                }
            }
            catch (Exception ex)
            {
                Log.e($"OpenForm ConfigItemForm {ex}");
            }
            finally
            {
                Log.e("OpenForm ConfigItemForm Finish");
                CloseWaitForm();
            }
        }

        /// <summary>
        /// 根据kuai
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        //private string getOriginPath(string path)
        //{
        //    if (System.IO.File.Exists(path))
        //    {
        //        WshShell shell = new WshShell();
        //        IWshShortcut 当前快捷方式文件IWshShortcut类 = (IWshShortcut)shell.CreateShortcut(path);
        //        return 当前快捷方式文件IWshShortcut类.TargetPath;
        //    }
        //    else
        //    {
        //       return "";
        //    }

        //}
    }

    class FileDownloadResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>      
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>      
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>      
        public string FilePath { get; set; }

        public FileDownloadResult failed(string errorMessage)
        {
            this.IsSuccess = false;
            this.ErrorMessage = errorMessage;
            this.FilePath = "";

            return this;
        }

        public FileDownloadResult success(string filePath)
        {
            this.IsSuccess = true;
            this.ErrorMessage = "";
            this.FilePath = filePath;

            return this;
        }

    }
}
