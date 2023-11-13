using ExpertLib.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class FormSignIn : Form
    {
        #region DLL 操作

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        #endregion

        #region static 操作文件,获得用户信息

        public static Dictionary<string, User> RememberedUsers = new Dictionary<string, User>();
        public static User CurrentUser = new User();
        public static Dictionary<string,List<string>> UserDic = new Dictionary<string, List<string>>();

        public static List<string> Users
        {
            get
            {
                var value = new List<string>();
                foreach (var item in UserDic)
                {
                    if(item.Value[0].ToString()!="")
                        value.AddRange(item.Value);
                }

                return value;
            }
        }

        #endregion

        private DataControl sql = new DataControl();

        public FormSignIn()
        {
            InitializeComponent();
        }
        
        private void FormSignIn_Load(object sender, EventArgs e)
        {
            RememberedUsers = FileCtr.ReadSerializable<string, User>("data.bin");

            foreach (var user in RememberedUsers.Values)
            {
                if (user.Describe != "-()")
                    comboBoxEdit1.Properties.Items.Add(user.Describe);
            }

            if (comboBoxEdit1.Properties.Items.Count > 0)
            {
                comboBoxEdit1.SelectedIndex = comboBoxEdit1.Properties.Items.Count - 1;           
            }

            comboBoxEdit1.TextChanged += comboBoxEdit1_TextChanged;
        }
        
        public User GetUserInfo(string input, string pwd)
        {
            var user = new User();

            DataSet ds;
            using (var sqlConn = new SqlConnection())
            {
                sqlConn.ConnectionString = sql.strCon;
                sqlConn.Open();

                var selectCmd = new SqlCommand
                {
                    Connection = sqlConn,
                    CommandText =
                        "select * from UserTable " +
                        "where ( userName=@id or userID+'-'+userName+'('+firstLetter+')'=@id or userID=@id ) " +
                        "and password=@pwd"
                };

                selectCmd.Parameters.Add("@id", SqlDbType.Char, 100);
                selectCmd.Parameters.Add("@pwd", SqlDbType.Char, 100);
                selectCmd.Parameters["@id"].Value = input;
                selectCmd.Parameters["@pwd"].Value = pwd;

                var dataAdapter = new SqlDataAdapter {SelectCommand = selectCmd};
                ds = new DataSet();
                dataAdapter.Fill(ds);

                sqlConn.Close();
                sqlConn.Dispose();
                dataAdapter.Dispose();

            }

            var dt = ds.Tables[0];
            if (dt.Rows.Count <= 0) return user;
            user.Key = int.Parse(dt.Rows[0]["ID"].ToString());
            user.ID = dt.Rows[0]["userID"].ToString().Trim();
            user.Name = dt.Rows[0]["userName"].ToString();
            user.Letter = dt.Rows[0]["firstLetter"].ToString();
            user.Password = dt.Rows[0]["password"].ToString();
            user.Department = dt.Rows[0]["department"].ToString();
            user.Role = dt.Rows[0]["role"].ToString();

            return user;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 结果判定

            CurrentUser = GetUserInfo(comboBoxEdit1.Text, textEdit1.Text);
            var signIn = !string.IsNullOrEmpty(CurrentUser.ID);

            var dt = sql.ExecuteQuery("select department,userName from UserTable order by department").Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                var department = dr[0].ToString();
                var name = dr[1].ToString();

                if (UserDic.ContainsKey(department))
                    UserDic[department].Add(name);
                else
                    UserDic.Add(department, new List<string> {name});
            }

            #endregion

            #region 记住密码或者清除密码

            RememberedUsers.Clear();
            
            if(!signIn)//登录失败
            {
                RememberedUsers.Add(CurrentUser.ID, new User());
            }
            else
            {
                if (!checkEdit1.Checked)//如果没有单击记住密码的功能，则清除密码
                {
                    RememberedUsers.Add(CurrentUser.ID, new User());
                }
                else
                {
                    RememberedUsers.Remove(CurrentUser.ID);
                    RememberedUsers.Add(CurrentUser.ID, CurrentUser);
                }
            }
            RememberedUsers.Remove("");
            RememberedUsers.WriteSerializable("data.bin");

            #endregion

            #region 结果处理

            if (!signIn)
            {
                MessageBox.Show("账号或密码错误！", "登录失败",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                comboBoxEdit1.Text = "";
                textEdit1.Text = "";
                comboBoxEdit1.Focus();
            }
            else
            {
                DialogResult = DialogResult.OK;
            }

            #endregion
        }

        #region 用户输入体验

        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                button1_Click(sender, e);
            }
        }

        private void comboBoxEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                textEdit1.Focus();
            }
        }

        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                button1_Click(sender, e);
            }
        }

        private void FormSignIn_MouseDown(object sender, MouseEventArgs e)
        {
            //常量  
            int WM_SYSCOMMAND = 0x0112;

            //窗体移动  
            int SC_MOVE = 0xF010;
            int HTCAPTION = 0x0002;

            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        
        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        #region 联想输入密码

        /// <summary>
        /// 密码免输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textEdit1.Text = "";
            if (comboBoxEdit1.Text.Trim() != "")
            {
                string input = comboBoxEdit1.Text.Trim();
                foreach (User user in RememberedUsers.Values)
                {
                    if (user.Describe.Equals(input) || user.Name.Equals(input) || user.ID.Equals(input))
                    {
                        textEdit1.Text = user.Password;
                        checkEdit1.Checked = true;
                    }
                }
                //textEdit1.Focus();              
            }
        }
        
        private void comboBoxEdit1_TextChanged(object sender, EventArgs e)
        {
            string value = comboBoxEdit1.Text;
            string strsql = "select userID+'-'+userName+'('+firstLetter+')' from UserTable where userName like '%" + value + "%' or firstLetter like '%" + value + "%' or userID like '%" + value + "%' or userID+'-'+userName+'('+firstLetter+')'='" + value + "' ";

            DataTable dt = sql.ExecuteQuery(strsql).Tables[0];
            int count = dt.Rows.Count;
            if (count > 0)
                comboBoxEdit1.Properties.Items.Clear();
            //comboBoxEdit1.ShowPopup();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!Convert.IsDBNull(dt.Rows[i][0]))
                {
                    comboBoxEdit1.Properties.Items.Add(dt.Rows[i][0].ToString());
                    //if (i == 0)
                    //    comboBoxEdit1.Text = dt.Rows[i][0].ToString();
                }
                    
            }
            comboBoxEdit1.ShowPopup();            
            comboBoxEdit1.SelectionStart = comboBoxEdit1.Text.Length;
        }

        #endregion

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureEdit3_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
