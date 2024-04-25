using ExpertLib.Controls;

namespace TaskManager
{
    partial class EditEquipmentUsageRecordDialog
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
            this.label4 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.titleComboxEquipmentCode = new ExpertLib.Controls.TitleCombox();
            this.titleComboxEquipmentName = new ExpertLib.Controls.TitleCombox();
            this.titleComboxEquipmentType = new ExpertLib.Controls.TitleCombox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.titleComboxDepartment = new ExpertLib.Controls.TitleCombox();
            this.titleComboxLocationNo = new ExpertLib.Controls.TitleCombox();
            this.titleComboxItem = new ExpertLib.Controls.TitleCombox();
            this.titleComboxItemCarVin = new ExpertLib.Controls.TitleCombox();
            this.titleComboxSampleModel = new ExpertLib.Controls.TitleCombox();
            this.titleComboxProducer = new ExpertLib.Controls.TitleCombox();
            this.label5 = new System.Windows.Forms.Label();
            this.titleComboxUsePerson = new ExpertLib.Controls.TitleCombox();
            this.titleComboxUseTime = new ExpertLib.Controls.TitleEditor.DateEdit();
            this.titleComboxPurpose = new ExpertLib.Controls.TitleCombox();
            this.titleComboxPreUseState = new ExpertLib.Controls.TitleCombox();
            this.titleComboxUseState = new ExpertLib.Controls.TitleCombox();
            this.titleComboxPostUseState = new ExpertLib.Controls.TitleCombox();
            this.titleComboxPostUseProblem = new ExpertLib.Controls.TitleCombox();
            this.titleComboxRemark = new ExpertLib.Controls.TitleCombox();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(1022, 30);
            this.label4.TabIndex = 45;
            this.label4.Text = "设备信息";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.label4);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxEquipmentCode);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxEquipmentName);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxEquipmentType);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.label7);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxDepartment);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxLocationNo);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxItem);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxItemCarVin);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxSampleModel);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxProducer);
            this.flowLayoutPanel1.Controls.Add(this.label5);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxUsePerson);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxUseTime);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxPurpose);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxPreUseState);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxUseState);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxPostUseState);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxPostUseProblem);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxRemark);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1064, 479);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // titleComboxEquipmentCode
            // 
            this.titleComboxEquipmentCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxEquipmentCode.LabelWidth = 100;
            this.titleComboxEquipmentCode.Location = new System.Drawing.Point(5, 35);
            this.titleComboxEquipmentCode.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxEquipmentCode.Name = "titleComboxEquipmentCode";
            this.titleComboxEquipmentCode.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxEquipmentCode.Size = new System.Drawing.Size(505, 41);
            this.titleComboxEquipmentCode.TabIndex = 0;
            this.titleComboxEquipmentCode.Title = "设备编码";
            this.titleComboxEquipmentCode.ViewModels = null;
            // 
            // titleComboxEquipmentName
            // 
            this.titleComboxEquipmentName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxEquipmentName.LabelWidth = 100;
            this.titleComboxEquipmentName.Location = new System.Drawing.Point(520, 35);
            this.titleComboxEquipmentName.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxEquipmentName.Name = "titleComboxEquipmentName";
            this.titleComboxEquipmentName.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxEquipmentName.Size = new System.Drawing.Size(505, 41);
            this.titleComboxEquipmentName.TabIndex = 1;
            this.titleComboxEquipmentName.Title = "设备名称";
            this.titleComboxEquipmentName.ViewModels = null;
            // 
            // titleComboxEquipmentType
            // 
            this.titleComboxEquipmentType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxEquipmentType.LabelWidth = 100;
            this.titleComboxEquipmentType.Location = new System.Drawing.Point(5, 86);
            this.titleComboxEquipmentType.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxEquipmentType.Name = "titleComboxEquipmentType";
            this.titleComboxEquipmentType.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxEquipmentType.Size = new System.Drawing.Size(505, 41);
            this.titleComboxEquipmentType.TabIndex = 2;
            this.titleComboxEquipmentType.Title = "设备型号";
            this.titleComboxEquipmentType.ViewModels = null;
            this.titleComboxEquipmentType.Load += new System.EventHandler(this.titleComboxLocationNo_Load);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(518, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 15);
            this.label1.TabIndex = 60;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(524, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 15);
            this.label2.TabIndex = 61;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(3, 144);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(1022, 30);
            this.label7.TabIndex = 105;
            this.label7.Text = "项目信息";
            // 
            // titleComboxDepartment
            // 
            this.titleComboxDepartment.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxDepartment.LabelWidth = 100;
            this.titleComboxDepartment.Location = new System.Drawing.Point(5, 179);
            this.titleComboxDepartment.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxDepartment.Name = "titleComboxDepartment";
            this.titleComboxDepartment.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxDepartment.Size = new System.Drawing.Size(505, 41);
            this.titleComboxDepartment.TabIndex = 10;
            this.titleComboxDepartment.Title = "组别";
            this.titleComboxDepartment.ViewModels = null;
            // 
            // titleComboxLocationNo
            // 
            this.titleComboxLocationNo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxLocationNo.LabelWidth = 100;
            this.titleComboxLocationNo.Location = new System.Drawing.Point(520, 179);
            this.titleComboxLocationNo.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxLocationNo.Name = "titleComboxLocationNo";
            this.titleComboxLocationNo.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxLocationNo.Size = new System.Drawing.Size(505, 41);
            this.titleComboxLocationNo.TabIndex = 11;
            this.titleComboxLocationNo.Title = "定位编号";
            this.titleComboxLocationNo.ViewModels = null;
            // 
            // titleComboxItem
            // 
            this.titleComboxItem.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxItem.LabelWidth = 100;
            this.titleComboxItem.Location = new System.Drawing.Point(5, 230);
            this.titleComboxItem.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxItem.Name = "titleComboxItem";
            this.titleComboxItem.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxItem.Size = new System.Drawing.Size(505, 41);
            this.titleComboxItem.TabIndex = 12;
            this.titleComboxItem.Title = "项目";
            this.titleComboxItem.UseWaitCursor = true;
            this.titleComboxItem.ViewModels = null;
            // 
            // titleComboxItemCarVin
            // 
            this.titleComboxItemCarVin.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxItemCarVin.LabelWidth = 100;
            this.titleComboxItemCarVin.Location = new System.Drawing.Point(520, 230);
            this.titleComboxItemCarVin.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxItemCarVin.Name = "titleComboxItemCarVin";
            this.titleComboxItemCarVin.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxItemCarVin.Size = new System.Drawing.Size(505, 41);
            this.titleComboxItemCarVin.TabIndex = 13;
            this.titleComboxItemCarVin.Title = "VIN";
            this.titleComboxItemCarVin.ViewModels = null;
            // 
            // titleComboxSampleModel
            // 
            this.titleComboxSampleModel.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxSampleModel.LabelWidth = 100;
            this.titleComboxSampleModel.Location = new System.Drawing.Point(5, 281);
            this.titleComboxSampleModel.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxSampleModel.Name = "titleComboxSampleModel";
            this.titleComboxSampleModel.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxSampleModel.Size = new System.Drawing.Size(505, 41);
            this.titleComboxSampleModel.TabIndex = 14;
            this.titleComboxSampleModel.Title = "车辆型号";
            this.titleComboxSampleModel.ViewModels = null;
            // 
            // titleComboxProducer
            // 
            this.titleComboxProducer.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxProducer.LabelWidth = 100;
            this.titleComboxProducer.Location = new System.Drawing.Point(520, 281);
            this.titleComboxProducer.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxProducer.Name = "titleComboxProducer";
            this.titleComboxProducer.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxProducer.Size = new System.Drawing.Size(505, 41);
            this.titleComboxProducer.TabIndex = 15;
            this.titleComboxProducer.Title = "生产厂家";
            this.titleComboxProducer.ViewModels = null;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(3, 339);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1022, 30);
            this.label5.TabIndex = 114;
            this.label5.Text = "使用情况";
            // 
            // titleComboxUsePerson
            // 
            this.titleComboxUsePerson.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxUsePerson.LabelWidth = 100;
            this.titleComboxUsePerson.Location = new System.Drawing.Point(5, 374);
            this.titleComboxUsePerson.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxUsePerson.Name = "titleComboxUsePerson";
            this.titleComboxUsePerson.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxUsePerson.Size = new System.Drawing.Size(505, 41);
            this.titleComboxUsePerson.TabIndex = 3;
            this.titleComboxUsePerson.Title = "使用人";
            this.titleComboxUsePerson.ViewModels = null;
            // 
            // titleComboxUseTime
            // 
            this.titleComboxUseTime.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxUseTime.LabelWidth = 100;
            this.titleComboxUseTime.Location = new System.Drawing.Point(519, 374);
            this.titleComboxUseTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.titleComboxUseTime.Name = "titleComboxUseTime";
            this.titleComboxUseTime.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxUseTime.Size = new System.Drawing.Size(507, 41);
            this.titleComboxUseTime.TabIndex = 4;
            this.titleComboxUseTime.Title = "使用时间";
            // 
            // titleComboxPurpose
            // 
            this.titleComboxPurpose.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxPurpose.LabelWidth = 100;
            this.titleComboxPurpose.Location = new System.Drawing.Point(5, 425);
            this.titleComboxPurpose.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxPurpose.Name = "titleComboxPurpose";
            this.titleComboxPurpose.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxPurpose.Size = new System.Drawing.Size(505, 41);
            this.titleComboxPurpose.TabIndex = 5;
            this.titleComboxPurpose.Title = "设备用途";
            this.titleComboxPurpose.ViewModels = null;
            // 
            // titleComboxPreUseState
            // 
            this.titleComboxPreUseState.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxPreUseState.LabelWidth = 100;
            this.titleComboxPreUseState.Location = new System.Drawing.Point(520, 425);
            this.titleComboxPreUseState.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxPreUseState.Name = "titleComboxPreUseState";
            this.titleComboxPreUseState.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxPreUseState.Size = new System.Drawing.Size(505, 41);
            this.titleComboxPreUseState.TabIndex = 6;
            this.titleComboxPreUseState.Title = "使用前状态";
            this.titleComboxPreUseState.ViewModels = null;
            // 
            // titleComboxUseState
            // 
            this.titleComboxUseState.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxUseState.LabelWidth = 100;
            this.titleComboxUseState.Location = new System.Drawing.Point(5, 476);
            this.titleComboxUseState.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxUseState.Name = "titleComboxUseState";
            this.titleComboxUseState.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxUseState.Size = new System.Drawing.Size(505, 41);
            this.titleComboxUseState.TabIndex = 7;
            this.titleComboxUseState.Title = "使用状况";
            this.titleComboxUseState.ViewModels = null;
            // 
            // titleComboxPostUseState
            // 
            this.titleComboxPostUseState.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxPostUseState.LabelWidth = 100;
            this.titleComboxPostUseState.Location = new System.Drawing.Point(520, 476);
            this.titleComboxPostUseState.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxPostUseState.Name = "titleComboxPostUseState";
            this.titleComboxPostUseState.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxPostUseState.Size = new System.Drawing.Size(505, 41);
            this.titleComboxPostUseState.TabIndex = 8;
            this.titleComboxPostUseState.Title = "使用后状态";
            this.titleComboxPostUseState.ViewModels = null;
            // 
            // titleComboxPostUseProblem
            // 
            this.titleComboxPostUseProblem.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxPostUseProblem.LabelWidth = 100;
            this.titleComboxPostUseProblem.Location = new System.Drawing.Point(5, 527);
            this.titleComboxPostUseProblem.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxPostUseProblem.Name = "titleComboxPostUseProblem";
            this.titleComboxPostUseProblem.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxPostUseProblem.Size = new System.Drawing.Size(505, 41);
            this.titleComboxPostUseProblem.TabIndex = 9;
            this.titleComboxPostUseProblem.Title = "使用后问题";
            this.titleComboxPostUseProblem.ViewModels = null;
            // 
            // titleComboxRemark
            // 
            this.titleComboxRemark.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxRemark.LabelWidth = 100;
            this.titleComboxRemark.Location = new System.Drawing.Point(520, 527);
            this.titleComboxRemark.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxRemark.Name = "titleComboxRemark";
            this.titleComboxRemark.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxRemark.Size = new System.Drawing.Size(505, 41);
            this.titleComboxRemark.TabIndex = 16;
            this.titleComboxRemark.Title = "备注";
            this.titleComboxRemark.ViewModels = null;
            // 
            // EditEquipmentUsageRecordDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.ClientSize = new System.Drawing.Size(1084, 545);
            this.Controls.Add(this.flowLayoutPanel1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "EditEquipmentUsageRecordDialog";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Load += new System.EventHandler(this.EditEquipmentUsageRecordDialog_Load);
            this.Controls.SetChildIndex(this.flowLayoutPanel1, 0);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private TitleCombox titleComboxEquipmentName;
        private TitleCombox titleComboxEquipmentCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private TitleCombox titleComboxEquipmentType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private TitleCombox titleComboxDepartment;
        private TitleCombox titleComboxLocationNo;
        private TitleCombox titleComboxItem;
        private TitleCombox titleComboxItemCarVin;
        private TitleCombox titleComboxSampleModel;
        private TitleCombox titleComboxProducer;
        private System.Windows.Forms.Label label5;
        private TitleCombox titleComboxUsePerson;
        private ExpertLib.Controls.TitleEditor.DateEdit titleComboxUseTime;
        private TitleCombox titleComboxPurpose;
        private TitleCombox titleComboxPreUseState;
        private TitleCombox titleComboxUseState;
        private TitleCombox titleComboxPostUseState;
        private TitleCombox titleComboxPostUseProblem;
        private TitleCombox titleComboxRemark;
    }
}