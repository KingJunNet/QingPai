using ExpertLib.Controls;


namespace TaskManager
{
    partial class CreateTaskForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.titleComboxGroup = new ExpertLib.Controls.TitleCombox();
            this.titleComboxArea = new ExpertLib.Controls.TitleCombox();
            this.titleComboxLocationNo = new ExpertLib.Controls.TitleCombox();
            this.titleComboxUser = new ExpertLib.Controls.TitleCombox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.titleComboxVin = new ExpertLib.Controls.TitleCombox();
            this.titleComboxSampleType = new ExpertLib.Controls.TitleCombox();
            this.titleComboxCarType = new ExpertLib.Controls.TitleCombox();
            this.titleComboxCarModel = new ExpertLib.Controls.TitleCombox();
            this.titleComboxProducer = new ExpertLib.Controls.TitleCombox();
            this.titleComboxEngineType = new ExpertLib.Controls.TitleCombox();
            this.titleComboxEngineModel = new ExpertLib.Controls.TitleCombox();
            this.titleComboxEngineProducer = new ExpertLib.Controls.TitleCombox();
            this.titleComboxYNDirect = new ExpertLib.Controls.TitleCombox();
            this.titleComboxTransType = new ExpertLib.Controls.TitleCombox();
            this.titleComboxDriverType = new ExpertLib.Controls.TitleCombox();
            this.titleComboxFuelType = new ExpertLib.Controls.TitleCombox();
            this.titleComboxRoz = new ExpertLib.Controls.TitleCombox();
            this.label7 = new System.Windows.Forms.Label();
            this.titleComboxItemType = new ExpertLib.Controls.TitleCombox();
            this.titleComboxItemBrief = new ExpertLib.Controls.TitleCombox();
            this.titleComboxBeginTime = new ExpertLib.Controls.TitleEditor.DateEdit();
            this.titleComboxStandard = new ExpertLib.Controls.TitleCombox();
            this.titleComboxItemRemark = new ExpertLib.Controls.TitleCombox();
            this.titleComboxTaskCode = new ExpertLib.Controls.TitleCombox();
            this.titleComboxTaskCodeRemark = new ExpertLib.Controls.TitleCombox();
            this.titleComboxSecurityLevel = new ExpertLib.Controls.TitleCombox();
            this.label6 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.titleComboxEquipment = new ExpertLib.Controls.TitleCombox();
            this.listViewUsingEquipment = new System.Windows.Forms.ListView();
            this.listColumnEuipCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listColumnEuipName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listColumnEuipType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listColumnGroup = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listColumnRemove = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddEquipment = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSaveItemTask = new System.Windows.Forms.Button();
            this.btnCancelItemTask = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(3, 146);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1281, 30);
            this.label5.TabIndex = 46;
            this.label5.Text = "样品信息";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(1281, 30);
            this.label4.TabIndex = 45;
            this.label4.Text = "用户信息";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.label4);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxGroup);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxArea);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxLocationNo);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxUser);
            this.flowLayoutPanel1.Controls.Add(this.label5);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxVin);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxSampleType);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxCarType);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxCarModel);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxProducer);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxEngineType);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxEngineModel);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxEngineProducer);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxYNDirect);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxTransType);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxDriverType);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxFuelType);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxRoz);
            this.flowLayoutPanel1.Controls.Add(this.label7);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxItemType);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxItemBrief);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxBeginTime);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxStandard);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxItemRemark);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxTaskCode);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxTaskCodeRemark);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxSecurityLevel);
            this.flowLayoutPanel1.Controls.Add(this.label6);
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel1);
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel2);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1321, 933);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // titleComboxGroup
            // 
            this.titleComboxGroup.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxGroup.LabelWidth = 100;
            this.titleComboxGroup.Location = new System.Drawing.Point(6, 36);
            this.titleComboxGroup.Margin = new System.Windows.Forms.Padding(6);
            this.titleComboxGroup.Name = "titleComboxGroup";
            this.titleComboxGroup.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxGroup.Size = new System.Drawing.Size(420, 41);
            this.titleComboxGroup.TabIndex = 0;
            this.titleComboxGroup.Title = "组别";
            this.titleComboxGroup.ViewModels = null;
            // 
            // titleComboxArea
            // 
            this.titleComboxArea.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxArea.LabelWidth = 100;
            this.titleComboxArea.Location = new System.Drawing.Point(436, 35);
            this.titleComboxArea.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.titleComboxArea.Name = "titleComboxArea";
            this.titleComboxArea.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxArea.Size = new System.Drawing.Size(420, 41);
            this.titleComboxArea.TabIndex = 1;
            this.titleComboxArea.Title = "试验地点";
            this.titleComboxArea.ViewModels = null;
            // 
            // titleComboxLocationNo
            // 
            this.titleComboxLocationNo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxLocationNo.LabelWidth = 100;
            this.titleComboxLocationNo.Location = new System.Drawing.Point(864, 35);
            this.titleComboxLocationNo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.titleComboxLocationNo.Name = "titleComboxLocationNo";
            this.titleComboxLocationNo.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxLocationNo.Size = new System.Drawing.Size(420, 41);
            this.titleComboxLocationNo.TabIndex = 2;
            this.titleComboxLocationNo.Title = "定位编号";
            this.titleComboxLocationNo.ViewModels = null;
            // 
            // titleComboxUser
            // 
            this.titleComboxUser.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxUser.LabelWidth = 100;
            this.titleComboxUser.Location = new System.Drawing.Point(4, 88);
            this.titleComboxUser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.titleComboxUser.Name = "titleComboxUser";
            this.titleComboxUser.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxUser.Size = new System.Drawing.Size(420, 41);
            this.titleComboxUser.TabIndex = 3;
            this.titleComboxUser.Title = "登记人";
            this.titleComboxUser.ViewModels = null;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1290, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 15);
            this.label1.TabIndex = 60;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1296, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 15);
            this.label2.TabIndex = 61;
            // 
            // titleComboxVin
            // 
            this.titleComboxVin.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxVin.LabelWidth = 100;
            this.titleComboxVin.Location = new System.Drawing.Point(4, 180);
            this.titleComboxVin.Margin = new System.Windows.Forms.Padding(4);
            this.titleComboxVin.Name = "titleComboxVin";
            this.titleComboxVin.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxVin.Size = new System.Drawing.Size(420, 41);
            this.titleComboxVin.TabIndex = 64;
            this.titleComboxVin.Title = "VIN";
            this.titleComboxVin.ViewModels = null;
            // 
            // titleComboxSampleType
            // 
            this.titleComboxSampleType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxSampleType.LabelWidth = 100;
            this.titleComboxSampleType.Location = new System.Drawing.Point(432, 181);
            this.titleComboxSampleType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.titleComboxSampleType.Name = "titleComboxSampleType";
            this.titleComboxSampleType.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxSampleType.Size = new System.Drawing.Size(420, 41);
            this.titleComboxSampleType.TabIndex = 86;
            this.titleComboxSampleType.Title = "样本类型";
            this.titleComboxSampleType.ViewModels = null;
            // 
            // titleComboxCarType
            // 
            this.titleComboxCarType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxCarType.LabelWidth = 100;
            this.titleComboxCarType.Location = new System.Drawing.Point(862, 182);
            this.titleComboxCarType.Margin = new System.Windows.Forms.Padding(6);
            this.titleComboxCarType.Name = "titleComboxCarType";
            this.titleComboxCarType.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxCarType.Size = new System.Drawing.Size(420, 41);
            this.titleComboxCarType.TabIndex = 87;
            this.titleComboxCarType.Title = "车辆类型";
            this.titleComboxCarType.ViewModels = null;
            // 
            // titleComboxCarModel
            // 
            this.titleComboxCarModel.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxCarModel.LabelWidth = 100;
            this.titleComboxCarModel.Location = new System.Drawing.Point(6, 235);
            this.titleComboxCarModel.Margin = new System.Windows.Forms.Padding(6);
            this.titleComboxCarModel.Name = "titleComboxCarModel";
            this.titleComboxCarModel.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxCarModel.Size = new System.Drawing.Size(420, 41);
            this.titleComboxCarModel.TabIndex = 88;
            this.titleComboxCarModel.Title = "车辆型号";
            this.titleComboxCarModel.ViewModels = null;
            // 
            // titleComboxProducer
            // 
            this.titleComboxProducer.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxProducer.LabelWidth = 100;
            this.titleComboxProducer.Location = new System.Drawing.Point(436, 234);
            this.titleComboxProducer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.titleComboxProducer.Name = "titleComboxProducer";
            this.titleComboxProducer.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxProducer.Size = new System.Drawing.Size(420, 41);
            this.titleComboxProducer.TabIndex = 89;
            this.titleComboxProducer.Title = "生产厂家";
            this.titleComboxProducer.ViewModels = null;
            // 
            // titleComboxEngineType
            // 
            this.titleComboxEngineType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxEngineType.LabelWidth = 100;
            this.titleComboxEngineType.Location = new System.Drawing.Point(866, 235);
            this.titleComboxEngineType.Margin = new System.Windows.Forms.Padding(6);
            this.titleComboxEngineType.Name = "titleComboxEngineType";
            this.titleComboxEngineType.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxEngineType.Size = new System.Drawing.Size(420, 41);
            this.titleComboxEngineType.TabIndex = 92;
            this.titleComboxEngineType.Title = "动力类型";
            this.titleComboxEngineType.ViewModels = null;
            // 
            // titleComboxEngineModel
            // 
            this.titleComboxEngineModel.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxEngineModel.LabelWidth = 100;
            this.titleComboxEngineModel.Location = new System.Drawing.Point(6, 288);
            this.titleComboxEngineModel.Margin = new System.Windows.Forms.Padding(6);
            this.titleComboxEngineModel.Name = "titleComboxEngineModel";
            this.titleComboxEngineModel.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxEngineModel.Size = new System.Drawing.Size(420, 41);
            this.titleComboxEngineModel.TabIndex = 93;
            this.titleComboxEngineModel.Title = "发动机型号";
            this.titleComboxEngineModel.ViewModels = null;
            // 
            // titleComboxEngineProducer
            // 
            this.titleComboxEngineProducer.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxEngineProducer.LabelWidth = 100;
            this.titleComboxEngineProducer.Location = new System.Drawing.Point(436, 287);
            this.titleComboxEngineProducer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.titleComboxEngineProducer.Name = "titleComboxEngineProducer";
            this.titleComboxEngineProducer.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxEngineProducer.Size = new System.Drawing.Size(420, 41);
            this.titleComboxEngineProducer.TabIndex = 94;
            this.titleComboxEngineProducer.Title = "发动机厂家";
            this.titleComboxEngineProducer.ViewModels = null;
            // 
            // titleComboxYNDirect
            // 
            this.titleComboxYNDirect.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxYNDirect.LabelWidth = 100;
            this.titleComboxYNDirect.Location = new System.Drawing.Point(864, 287);
            this.titleComboxYNDirect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.titleComboxYNDirect.Name = "titleComboxYNDirect";
            this.titleComboxYNDirect.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxYNDirect.Size = new System.Drawing.Size(420, 41);
            this.titleComboxYNDirect.TabIndex = 95;
            this.titleComboxYNDirect.Title = "是否直喷";
            this.titleComboxYNDirect.ViewModels = null;
            // 
            // titleComboxTransType
            // 
            this.titleComboxTransType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxTransType.LabelWidth = 100;
            this.titleComboxTransType.Location = new System.Drawing.Point(6, 341);
            this.titleComboxTransType.Margin = new System.Windows.Forms.Padding(6);
            this.titleComboxTransType.Name = "titleComboxTransType";
            this.titleComboxTransType.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxTransType.Size = new System.Drawing.Size(420, 41);
            this.titleComboxTransType.TabIndex = 96;
            this.titleComboxTransType.Title = "变速器形式";
            this.titleComboxTransType.ViewModels = null;
            // 
            // titleComboxDriverType
            // 
            this.titleComboxDriverType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxDriverType.LabelWidth = 100;
            this.titleComboxDriverType.Location = new System.Drawing.Point(438, 341);
            this.titleComboxDriverType.Margin = new System.Windows.Forms.Padding(6);
            this.titleComboxDriverType.Name = "titleComboxDriverType";
            this.titleComboxDriverType.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxDriverType.Size = new System.Drawing.Size(420, 41);
            this.titleComboxDriverType.TabIndex = 97;
            this.titleComboxDriverType.Title = "驱动形式";
            this.titleComboxDriverType.ViewModels = null;
            // 
            // titleComboxFuelType
            // 
            this.titleComboxFuelType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxFuelType.LabelWidth = 100;
            this.titleComboxFuelType.Location = new System.Drawing.Point(868, 340);
            this.titleComboxFuelType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.titleComboxFuelType.Name = "titleComboxFuelType";
            this.titleComboxFuelType.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxFuelType.Size = new System.Drawing.Size(420, 41);
            this.titleComboxFuelType.TabIndex = 90;
            this.titleComboxFuelType.Title = "燃油种类";
            this.titleComboxFuelType.ViewModels = null;
            // 
            // titleComboxRoz
            // 
            this.titleComboxRoz.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxRoz.LabelWidth = 100;
            this.titleComboxRoz.Location = new System.Drawing.Point(5, 393);
            this.titleComboxRoz.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxRoz.Name = "titleComboxRoz";
            this.titleComboxRoz.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxRoz.Size = new System.Drawing.Size(420, 41);
            this.titleComboxRoz.TabIndex = 91;
            this.titleComboxRoz.Title = "燃油标号";
            this.titleComboxRoz.ViewModels = null;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(3, 451);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(1278, 30);
            this.label7.TabIndex = 105;
            this.label7.Text = "项目信息";
            // 
            // titleComboxItemType
            // 
            this.titleComboxItemType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxItemType.LabelWidth = 100;
            this.titleComboxItemType.Location = new System.Drawing.Point(5, 486);
            this.titleComboxItemType.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxItemType.Name = "titleComboxItemType";
            this.titleComboxItemType.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxItemType.Size = new System.Drawing.Size(420, 41);
            this.titleComboxItemType.TabIndex = 101;
            this.titleComboxItemType.Title = "项目类型";
            this.titleComboxItemType.ViewModels = null;
            // 
            // titleComboxItemBrief
            // 
            this.titleComboxItemBrief.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxItemBrief.LabelWidth = 100;
            this.titleComboxItemBrief.Location = new System.Drawing.Point(435, 486);
            this.titleComboxItemBrief.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxItemBrief.Name = "titleComboxItemBrief";
            this.titleComboxItemBrief.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxItemBrief.Size = new System.Drawing.Size(420, 41);
            this.titleComboxItemBrief.TabIndex = 102;
            this.titleComboxItemBrief.Title = "项目简称";
            this.titleComboxItemBrief.ViewModels = null;
            // 
            // titleComboxBeginTime
            // 
            this.titleComboxBeginTime.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxBeginTime.LabelWidth = 100;
            this.titleComboxBeginTime.Location = new System.Drawing.Point(865, 486);
            this.titleComboxBeginTime.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxBeginTime.Name = "titleComboxBeginTime";
            this.titleComboxBeginTime.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxBeginTime.Size = new System.Drawing.Size(438, 41);
            this.titleComboxBeginTime.TabIndex = 103;
            this.titleComboxBeginTime.Title = "实验开始时间";
            this.titleComboxBeginTime.Load += new System.EventHandler(this.titleCombox39_Load);
            // 
            // titleComboxStandard
            // 
            this.titleComboxStandard.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxStandard.LabelWidth = 100;
            this.titleComboxStandard.Location = new System.Drawing.Point(5, 537);
            this.titleComboxStandard.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxStandard.Name = "titleComboxStandard";
            this.titleComboxStandard.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxStandard.Size = new System.Drawing.Size(420, 41);
            this.titleComboxStandard.TabIndex = 104;
            this.titleComboxStandard.Title = "标准阶段";
            this.titleComboxStandard.ViewModels = null;
            // 
            // titleComboxItemRemark
            // 
            this.titleComboxItemRemark.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxItemRemark.LabelWidth = 100;
            this.titleComboxItemRemark.Location = new System.Drawing.Point(434, 536);
            this.titleComboxItemRemark.Margin = new System.Windows.Forms.Padding(4);
            this.titleComboxItemRemark.Name = "titleComboxItemRemark";
            this.titleComboxItemRemark.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxItemRemark.Size = new System.Drawing.Size(420, 41);
            this.titleComboxItemRemark.TabIndex = 106;
            this.titleComboxItemRemark.Title = "项目备注";
            this.titleComboxItemRemark.ViewModels = null;
            // 
            // titleComboxTaskCode
            // 
            this.titleComboxTaskCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxTaskCode.LabelWidth = 100;
            this.titleComboxTaskCode.Location = new System.Drawing.Point(862, 537);
            this.titleComboxTaskCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.titleComboxTaskCode.Name = "titleComboxTaskCode";
            this.titleComboxTaskCode.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxTaskCode.Size = new System.Drawing.Size(420, 41);
            this.titleComboxTaskCode.TabIndex = 107;
            this.titleComboxTaskCode.Title = "任务单号";
            this.titleComboxTaskCode.ViewModels = null;
            // 
            // titleComboxTaskCodeRemark
            // 
            this.titleComboxTaskCodeRemark.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxTaskCodeRemark.LabelWidth = 100;
            this.titleComboxTaskCodeRemark.Location = new System.Drawing.Point(5, 588);
            this.titleComboxTaskCodeRemark.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxTaskCodeRemark.Name = "titleComboxTaskCodeRemark";
            this.titleComboxTaskCodeRemark.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxTaskCodeRemark.Size = new System.Drawing.Size(420, 41);
            this.titleComboxTaskCodeRemark.TabIndex = 108;
            this.titleComboxTaskCodeRemark.Title = "任务单号备注";
            this.titleComboxTaskCodeRemark.ViewModels = null;
            // 
            // titleComboxSecurityLevel
            // 
            this.titleComboxSecurityLevel.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxSecurityLevel.LabelWidth = 100;
            this.titleComboxSecurityLevel.Location = new System.Drawing.Point(434, 587);
            this.titleComboxSecurityLevel.Margin = new System.Windows.Forms.Padding(4);
            this.titleComboxSecurityLevel.Name = "titleComboxSecurityLevel";
            this.titleComboxSecurityLevel.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleComboxSecurityLevel.Size = new System.Drawing.Size(420, 41);
            this.titleComboxSecurityLevel.TabIndex = 109;
            this.titleComboxSecurityLevel.Title = "保密级别";
            this.titleComboxSecurityLevel.ViewModels = null;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(3, 646);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(1281, 30);
            this.label6.TabIndex = 110;
            this.label6.Text = "使用设备";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.914546F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 519F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 544F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 113F));
            this.tableLayoutPanel1.Controls.Add(this.titleComboxEquipment, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.listViewUsingEquipment, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAddEquipment, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 679);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1279, 148);
            this.tableLayoutPanel1.TabIndex = 111;
            // 
            // titleComboxEquipment
            // 
            this.titleComboxEquipment.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxEquipment.LabelWidth = 50;
            this.titleComboxEquipment.Location = new System.Drawing.Point(627, 5);
            this.titleComboxEquipment.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxEquipment.Name = "titleComboxEquipment";
            this.titleComboxEquipment.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxEquipment.Size = new System.Drawing.Size(534, 41);
            this.titleComboxEquipment.TabIndex = 18;
            this.titleComboxEquipment.Title = "设备";
            this.titleComboxEquipment.ViewModels = null;
            // 
            // listViewUsingEquipment
            // 
            this.listViewUsingEquipment.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.listColumnEuipCode,
            this.listColumnEuipName,
            this.listColumnEuipType,
            this.listColumnGroup,
            this.listColumnRemove});
            this.listViewUsingEquipment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewUsingEquipment.FullRowSelect = true;
            this.listViewUsingEquipment.HideSelection = false;
            this.listViewUsingEquipment.Location = new System.Drawing.Point(106, 3);
            this.listViewUsingEquipment.Name = "listViewUsingEquipment";
            this.listViewUsingEquipment.Size = new System.Drawing.Size(513, 142);
            this.listViewUsingEquipment.TabIndex = 20;
            this.listViewUsingEquipment.UseCompatibleStateImageBehavior = false;
            this.listViewUsingEquipment.View = System.Windows.Forms.View.Details;
            this.listViewUsingEquipment.SelectedIndexChanged += new System.EventHandler(this.listViewUsingEquipment_SelectedIndexChanged);
            // 
            // listColumnEuipCode
            // 
            this.listColumnEuipCode.Text = "编号";
            this.listColumnEuipCode.Width = 112;
            // 
            // listColumnEuipName
            // 
            this.listColumnEuipName.Text = "名称";
            this.listColumnEuipName.Width = 112;
            // 
            // listColumnEuipType
            // 
            this.listColumnEuipType.Text = "型号";
            this.listColumnEuipType.Width = 112;
            // 
            // listColumnGroup
            // 
            this.listColumnGroup.Text = "组别";
            this.listColumnGroup.Width = 112;
            // 
            // listColumnRemove
            // 
            this.listColumnRemove.Text = "";
            // 
            // btnAddEquipment
            // 
            this.btnAddEquipment.Location = new System.Drawing.Point(1169, 3);
            this.btnAddEquipment.Name = "btnAddEquipment";
            this.btnAddEquipment.Size = new System.Drawing.Size(107, 34);
            this.btnAddEquipment.TabIndex = 19;
            this.btnAddEquipment.Text = "添加";
            this.btnAddEquipment.UseVisualStyleBackColor = true;
            this.btnAddEquipment.Click += new System.EventHandler(this.btnAddEquipment_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 195F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 163F));
            this.tableLayoutPanel2.Controls.Add(this.btnSaveItemTask, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCancelItemTask, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 833);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1281, 64);
            this.tableLayoutPanel2.TabIndex = 112;
            // 
            // btnSaveItemTask
            // 
            this.btnSaveItemTask.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveItemTask.Location = new System.Drawing.Point(1121, 3);
            this.btnSaveItemTask.Name = "btnSaveItemTask";
            this.btnSaveItemTask.Size = new System.Drawing.Size(157, 47);
            this.btnSaveItemTask.TabIndex = 0;
            this.btnSaveItemTask.Text = "保存";
            this.btnSaveItemTask.UseVisualStyleBackColor = true;
            this.btnSaveItemTask.Click += new System.EventHandler(this.btnSaveItemTask_Click);
            // 
            // btnCancelItemTask
            // 
            this.btnCancelItemTask.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancelItemTask.Location = new System.Drawing.Point(926, 3);
            this.btnCancelItemTask.Name = "btnCancelItemTask";
            this.btnCancelItemTask.Size = new System.Drawing.Size(156, 47);
            this.btnCancelItemTask.TabIndex = 1;
            this.btnCancelItemTask.Text = "取消";
            this.btnCancelItemTask.UseVisualStyleBackColor = true;
            this.btnCancelItemTask.Click += new System.EventHandler(this.btnCancelItemTask_Click_1);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(3, 912);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1281, 10);
            this.label3.TabIndex = 113;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // CreateTaskForm
            // 
            this.ClientSize = new System.Drawing.Size(1341, 953);
            this.Controls.Add(this.flowLayoutPanel1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "CreateTaskForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Load += new System.EventHandler(this.CreateTaskForm_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label5;
        private TitleCombox titleComboxUser;
        private TitleCombox titleComboxArea;
        private TitleCombox titleComboxGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private TitleCombox titleComboxLocationNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private TitleCombox titleComboxVin;
        private TitleCombox titleComboxSampleType;
        private TitleCombox titleComboxCarType;
        private TitleCombox titleComboxCarModel;
        private TitleCombox titleComboxProducer;
        private TitleCombox titleComboxEngineType;
        private TitleCombox titleComboxEngineModel;
        private TitleCombox titleComboxEngineProducer;
        private TitleCombox titleComboxYNDirect;
        private TitleCombox titleComboxTransType;
        private TitleCombox titleComboxDriverType;
        private TitleCombox titleComboxFuelType;
        private TitleCombox titleComboxRoz;
        private System.Windows.Forms.Label label7;
        private TitleCombox titleComboxItemType;
        private TitleCombox titleComboxItemBrief;
        private ExpertLib.Controls.TitleEditor.DateEdit titleComboxBeginTime;
        private TitleCombox titleComboxStandard;
        private TitleCombox titleComboxItemRemark;
        private TitleCombox titleComboxTaskCode;
        private TitleCombox titleComboxTaskCodeRemark;
        private TitleCombox titleComboxSecurityLevel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private TitleCombox titleComboxEquipment;
        private System.Windows.Forms.ListView listViewUsingEquipment;
        private System.Windows.Forms.ColumnHeader listColumnEuipCode;
        private System.Windows.Forms.ColumnHeader listColumnEuipName;
        private System.Windows.Forms.ColumnHeader listColumnEuipType;
        private System.Windows.Forms.ColumnHeader listColumnGroup;
        private System.Windows.Forms.ColumnHeader listColumnRemove;
        private System.Windows.Forms.Button btnAddEquipment;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnSaveItemTask;
        private System.Windows.Forms.Button btnCancelItemTask;
        private System.Windows.Forms.Label label3;
    }
}