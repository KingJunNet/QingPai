using LabSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManager.主界面
{
    public partial class updateVIN : Form
    {
        public updateVIN()
        {
            InitializeComponent();
        }

        private string vin;
        public updateVIN(string vin)
        {
            
            InitializeComponent();
            this.vin = vin;
            textBox2.Text = vin;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = $"update TestStatistic set Carvin ={textBox1.Text} where Carvin='{vin}'";
            string sql2 = $"update SampleTable set VIN ={textBox1.Text} where VIN='{vin}'";
            SqlHelper.ExecuteNonquery(sql, CommandType.Text);
            SqlHelper.ExecuteNonquery(sql2, CommandType.Text);
            MessageBox.Show("更新成功");
        }

        private void updateVIN_Load(object sender, EventArgs e)
        {

        }
    }
}
