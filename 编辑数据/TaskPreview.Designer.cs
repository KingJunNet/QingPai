namespace TaskManager
{
    partial class TaskPreview
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.department = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ExperimentalSite = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LocationNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Registrant = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CarType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ItemBrief = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TestStartDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SampleModel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Producer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VIN = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.YNDirect = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PowerType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TransmissionType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EngineModel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EngineProduct = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StandardStage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FuelType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FuelLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ProjectPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.department,
            this.ExperimentalSite,
            this.LocationNumber,
            this.Registrant,
            this.CarType,
            this.ItemBrief,
            this.TestStartDate,
            this.SampleModel,
            this.Producer,
            this.VIN,
            this.YNDirect,
            this.PowerType,
            this.TransmissionType,
            this.EngineModel,
            this.EngineProduct,
            this.StandardStage,
            this.FuelType,
            this.FuelLabel,
            this.ProjectPrice});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1608, 397);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // ID
            // 
            this.ID.Text = "ID";
            // 
            // department
            // 
            this.department.Text = "部门";
            // 
            // ExperimentalSite
            // 
            this.ExperimentalSite.Text = "试验地点";
            this.ExperimentalSite.Width = 88;
            // 
            // LocationNumber
            // 
            this.LocationNumber.Text = "定位编号";
            this.LocationNumber.Width = 106;
            // 
            // Registrant
            // 
            this.Registrant.Text = "登记人";
            this.Registrant.Width = 80;
            // 
            // CarType
            // 
            this.CarType.Text = "车辆类型";
            this.CarType.Width = 95;
            // 
            // ItemBrief
            // 
            this.ItemBrief.Text = "项目简称";
            this.ItemBrief.Width = 144;
            // 
            // TestStartDate
            // 
            this.TestStartDate.Text = "试验开始时间";
            this.TestStartDate.Width = 120;
            // 
            // SampleModel
            // 
            this.SampleModel.Text = "样品型号";
            this.SampleModel.Width = 100;
            // 
            // Producer
            // 
            this.Producer.Text = "生产厂家";
            this.Producer.Width = 100;
            // 
            // VIN
            // 
            this.VIN.Text = "VIN";
            this.VIN.Width = 95;
            // 
            // YNDirect
            // 
            this.YNDirect.Text = "是否直喷";
            this.YNDirect.Width = 104;
            // 
            // PowerType
            // 
            this.PowerType.Text = "动力类型";
            this.PowerType.Width = 39;
            // 
            // TransmissionType
            // 
            this.TransmissionType.Text = "变速器形式";
            this.TransmissionType.Width = 67;
            // 
            // EngineModel
            // 
            this.EngineModel.Text = "发动机型号";
            this.EngineModel.Width = 43;
            // 
            // EngineProduct
            // 
            this.EngineProduct.Text = "发动机生产厂家";
            this.EngineProduct.Width = 64;
            // 
            // StandardStage
            // 
            this.StandardStage.Text = "标准阶段";
            this.StandardStage.Width = 32;
            // 
            // FuelType
            // 
            this.FuelType.Text = "燃油种类";
            // 
            // FuelLabel
            // 
            this.FuelLabel.Text = "燃油标号";
            this.FuelLabel.Width = 35;
            // 
            // ProjectPrice
            // 
            this.ProjectPrice.Text = "项目单价";
            this.ProjectPrice.Width = 92;
            // 
            // TaskPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listView1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "TaskPreview";
            this.Size = new System.Drawing.Size(1608, 397);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader department;
        private System.Windows.Forms.ColumnHeader CarType;
        private System.Windows.Forms.ColumnHeader LocationNumber;
        private System.Windows.Forms.ColumnHeader ItemBrief;
        private System.Windows.Forms.ColumnHeader Registrant;
        private System.Windows.Forms.ColumnHeader VIN;
        private System.Windows.Forms.ColumnHeader ExperimentalSite;
        private System.Windows.Forms.ColumnHeader FuelLabel;
        private System.Windows.Forms.ColumnHeader TestStartDate;
        private System.Windows.Forms.ColumnHeader SampleModel;
        private System.Windows.Forms.ColumnHeader Producer;
        private System.Windows.Forms.ColumnHeader YNDirect;
        private System.Windows.Forms.ColumnHeader TransmissionType;
        private System.Windows.Forms.ColumnHeader EngineModel;
        private System.Windows.Forms.ColumnHeader EngineProduct;
        private System.Windows.Forms.ColumnHeader PowerType;
        private System.Windows.Forms.ColumnHeader StandardStage;
        private System.Windows.Forms.ColumnHeader ProjectPrice;
        private System.Windows.Forms.ColumnHeader FuelType;
    }
}
