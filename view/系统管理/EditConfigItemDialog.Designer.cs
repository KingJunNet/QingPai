using ExpertLib.Controls;

namespace TaskManager
{
    partial class EditConfigItemDialog
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.titleComboxName = new ExpertLib.Controls.TitleCombox();
            this.titleComboxValue = new ExpertLib.Controls.TitleCombox();
            this.titleComboxModule = new ExpertLib.Controls.TitleCombox();
            this.titleComboxRegistrant = new ExpertLib.Controls.TitleCombox();
            this.titleMutiComboxGroup = new ExpertLib.Controls.TitleMutiCombox();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.label7);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxName);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxValue);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxModule);
            this.flowLayoutPanel1.Controls.Add(this.titleComboxRegistrant);
            this.flowLayoutPanel1.Controls.Add(this.titleMutiComboxGroup);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1058, 334);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 15);
            this.label1.TabIndex = 60;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 15);
            this.label2.TabIndex = 61;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(15, 12);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 12, 3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(1022, 30);
            this.label7.TabIndex = 105;
            this.label7.Text = "备选项信息";
            // 
            // titleComboxName
            // 
            this.titleComboxName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxName.LabelWidth = 100;
            this.titleComboxName.Location = new System.Drawing.Point(5, 47);
            this.titleComboxName.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxName.Name = "titleComboxName";
            this.titleComboxName.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxName.Size = new System.Drawing.Size(505, 41);
            this.titleComboxName.TabIndex = 1;
            this.titleComboxName.Title = "选项类别";
            this.titleComboxName.ViewModels = null;
            // 
            // titleComboxValue
            // 
            this.titleComboxValue.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxValue.LabelWidth = 100;
            this.titleComboxValue.Location = new System.Drawing.Point(520, 47);
            this.titleComboxValue.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxValue.Name = "titleComboxValue";
            this.titleComboxValue.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxValue.Size = new System.Drawing.Size(505, 41);
            this.titleComboxValue.TabIndex = 2;
            this.titleComboxValue.Title = "选项名称";
            this.titleComboxValue.ViewModels = null;
            // 
            // titleComboxModule
            // 
            this.titleComboxModule.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxModule.LabelWidth = 100;
            this.titleComboxModule.Location = new System.Drawing.Point(5, 98);
            this.titleComboxModule.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxModule.Name = "titleComboxModule";
            this.titleComboxModule.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxModule.Size = new System.Drawing.Size(505, 41);
            this.titleComboxModule.TabIndex = 4;
            this.titleComboxModule.Title = "模块";
            this.titleComboxModule.ViewModels = null;
            // 
            // titleComboxRegistrant
            // 
            this.titleComboxRegistrant.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleComboxRegistrant.LabelWidth = 100;
            this.titleComboxRegistrant.Location = new System.Drawing.Point(520, 98);
            this.titleComboxRegistrant.Margin = new System.Windows.Forms.Padding(5);
            this.titleComboxRegistrant.Name = "titleComboxRegistrant";
            this.titleComboxRegistrant.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.titleComboxRegistrant.Size = new System.Drawing.Size(505, 41);
            this.titleComboxRegistrant.TabIndex = 5;
            this.titleComboxRegistrant.Title = "登记人";
            this.titleComboxRegistrant.ViewModels = null;
            // 
            // titleMutiComboxGroup
            // 
            this.titleMutiComboxGroup.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleMutiComboxGroup.LabelWidth = 100;
            this.titleMutiComboxGroup.Location = new System.Drawing.Point(4, 148);
            this.titleMutiComboxGroup.Margin = new System.Windows.Forms.Padding(4);
            this.titleMutiComboxGroup.Name = "titleMutiComboxGroup";
            this.titleMutiComboxGroup.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.titleMutiComboxGroup.Size = new System.Drawing.Size(506, 116);
            this.titleMutiComboxGroup.TabIndex = 106;
            this.titleMutiComboxGroup.Title = "组别";
            this.titleMutiComboxGroup.ViewModels = null;
            // 
            // EditConfigItemDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.ClientSize = new System.Drawing.Size(1078, 400);
            this.Controls.Add(this.flowLayoutPanel1);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "EditConfigItemDialog";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Load += new System.EventHandler(this.EditEquipmentUsageRecordDialog_Load);
            this.Controls.SetChildIndex(this.flowLayoutPanel1, 0);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private TitleCombox titleComboxName;
        private TitleCombox titleComboxValue;
        private TitleCombox titleComboxModule;
        private TitleCombox titleComboxRegistrant;
        private System.Windows.Forms.Label label7;
        private TitleMutiCombox titleMutiComboxGroup;
    }
}