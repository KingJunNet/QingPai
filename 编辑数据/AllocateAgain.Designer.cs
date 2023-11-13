using ExpertLib.Controls;

namespace TaskManager
{
    partial class AllocateAgain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AllocateAgain));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.comboxDepartment = new ExpertLib.Controls.TitleCombox();
            this.button1 = new System.Windows.Forms.Button();
            this.starttime = new ExpertLib.Controls.TitleCombox();
            this.ExperimentalSite = new ExpertLib.Controls.TitleCombox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.65683F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.65683F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.65683F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.65683F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.72086F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.65182F));
            this.tableLayoutPanel1.Controls.Add(this.ExperimentalSite, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboxDepartment, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button1, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.starttime, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(7, 7);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1196, 89);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // comboxDepartment
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.comboxDepartment, 2);
            this.comboxDepartment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboxDepartment.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboxDepartment.LabelWidth = 100;
            this.comboxDepartment.Location = new System.Drawing.Point(4, 4);
            this.comboxDepartment.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.comboxDepartment.Name = "comboxDepartment";
            this.comboxDepartment.Padding = new System.Windows.Forms.Padding(2);
            this.comboxDepartment.Size = new System.Drawing.Size(390, 22);
            this.comboxDepartment.TabIndex = 14;
            this.comboxDepartment.Tag = "department";
            this.comboxDepartment.Title = "部门";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(963, 53);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(229, 33);
            this.button1.TabIndex = 28;
            this.button1.Text = "试验再分配";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // starttime
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.starttime, 2);
            this.starttime.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.starttime.LabelWidth = 120;
            this.starttime.Location = new System.Drawing.Point(800, 4);
            this.starttime.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.starttime.Name = "starttime";
            this.starttime.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.starttime.Size = new System.Drawing.Size(392, 22);
            this.starttime.TabIndex = 29;
            this.starttime.Title = "试验开始时间";
            // 
            // ExperimentalSite
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ExperimentalSite, 2);
            this.ExperimentalSite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExperimentalSite.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ExperimentalSite.LabelWidth = 100;
            this.ExperimentalSite.Location = new System.Drawing.Point(402, 4);
            this.ExperimentalSite.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ExperimentalSite.Name = "ExperimentalSite";
            this.ExperimentalSite.Padding = new System.Windows.Forms.Padding(2);
            this.ExperimentalSite.Size = new System.Drawing.Size(390, 22);
            this.ExperimentalSite.TabIndex = 15;
            this.ExperimentalSite.Tag = "ExperimentalSite";
            this.ExperimentalSite.Title = "试验地点";
            // 
            // AllocateAgain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1210, 103);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AllocateAgain";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "试验再分配";
            this.Load += new System.EventHandler(this.AllocateTaskDialog_Load);
            this.Shown += new System.EventHandler(this.AllocateTaskDialog_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private TitleCombox comboxDepartment;
        private System.Windows.Forms.Button button1;
        private TitleCombox starttime;
        private TitleCombox ExperimentalSite;
    }
}