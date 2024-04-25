namespace TaskManager.编辑数据
{
    partial class StructureDialog
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Section = new ExpertLib.Controls.TitleCombox();
            this.Office = new ExpertLib.Controls.TitleCombox();
            this.Experimentsite = new ExpertLib.Controls.TitleCombox();
            this.Group1 = new ExpertLib.Controls.TitleCombox();
            this.Locationnumber = new ExpertLib.Controls.TitleCombox();
            this.Username = new ExpertLib.Controls.TitleCombox();
            this.Unit = new ExpertLib.Controls.TitleCombox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(640, 223);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(163, 49);
            this.button1.TabIndex = 1;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(855, 223);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(164, 49);
            this.button2.TabIndex = 2;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.Section, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Office, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Experimentsite, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.Group1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.Locationnumber, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.Username, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.Unit, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1052, 197);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // Section
            // 
            this.Section.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Section.LabelWidth = 100;
            this.Section.Location = new System.Drawing.Point(530, 5);
            this.Section.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Section.Name = "Section";
            this.Section.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Section.Size = new System.Drawing.Size(489, 38);
            this.Section.TabIndex = 1;
            this.Section.Title = "部门";
            // 
            // Office
            // 
            this.Office.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Office.LabelWidth = 100;
            this.Office.Location = new System.Drawing.Point(4, 53);
            this.Office.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Office.Name = "Office";
            this.Office.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Office.Size = new System.Drawing.Size(489, 38);
            this.Office.TabIndex = 2;
            this.Office.Title = "科室";
            // 
            // Experimentsite
            // 
            this.Experimentsite.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Experimentsite.LabelWidth = 100;
            this.Experimentsite.Location = new System.Drawing.Point(530, 53);
            this.Experimentsite.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Experimentsite.Name = "Experimentsite";
            this.Experimentsite.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Experimentsite.Size = new System.Drawing.Size(489, 38);
            this.Experimentsite.TabIndex = 3;
            this.Experimentsite.Title = "试验地点";
            // 
            // Group1
            // 
            this.Group1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Group1.LabelWidth = 100;
            this.Group1.Location = new System.Drawing.Point(4, 101);
            this.Group1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Group1.Name = "Group1";
            this.Group1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Group1.Size = new System.Drawing.Size(489, 38);
            this.Group1.TabIndex = 4;
            this.Group1.Title = "组别";
            // 
            // Locationnumber
            // 
            this.Locationnumber.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Locationnumber.LabelWidth = 100;
            this.Locationnumber.Location = new System.Drawing.Point(530, 101);
            this.Locationnumber.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Locationnumber.Name = "Locationnumber";
            this.Locationnumber.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Locationnumber.Size = new System.Drawing.Size(489, 38);
            this.Locationnumber.TabIndex = 5;
            this.Locationnumber.Title = "定位编号";
            // 
            // Username
            // 
            this.Username.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Username.ForeColor = System.Drawing.Color.Red;
            this.Username.LabelWidth = 100;
            this.Username.Location = new System.Drawing.Point(4, 149);
            this.Username.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Username.Name = "Username";
            this.Username.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Username.Size = new System.Drawing.Size(489, 41);
            this.Username.TabIndex = 6;
            this.Username.Title = "权限账号";
            // 
            // Unit
            // 
            this.Unit.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Unit.LabelWidth = 100;
            this.Unit.Location = new System.Drawing.Point(5, 5);
            this.Unit.Margin = new System.Windows.Forms.Padding(5);
            this.Unit.Name = "Unit";
            this.Unit.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Unit.Size = new System.Drawing.Size(488, 38);
            this.Unit.TabIndex = 7;
            this.Unit.Title = "所属单位";
            // 
            // StructureDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 294);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "StructureDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新增信息（选择权限账号可自动关联信息）";
            this.Load += new System.EventHandler(this.StructureDialog_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ExpertLib.Controls.TitleCombox Section;
        private ExpertLib.Controls.TitleCombox Office;
        private ExpertLib.Controls.TitleCombox Experimentsite;
        private ExpertLib.Controls.TitleCombox Group1;
        private ExpertLib.Controls.TitleCombox Locationnumber;
        private ExpertLib.Controls.TitleCombox Username;
        private ExpertLib.Controls.TitleCombox Unit;
    }
}