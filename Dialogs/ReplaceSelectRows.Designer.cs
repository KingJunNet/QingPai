namespace TaskManager.Dialogs
{
    partial class ReplaceSelectRows
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReplaceSelectRows));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Oldcontent = new System.Windows.Forms.TextBox();
            this.Newcontent = new System.Windows.Forms.TextBox();
            this.replace = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.filedName = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.filedName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "查找内容";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "替换为";
            // 
            // Oldcontent
            // 
            this.Oldcontent.Location = new System.Drawing.Point(170, 86);
            this.Oldcontent.Name = "Oldcontent";
            this.Oldcontent.Size = new System.Drawing.Size(274, 25);
            this.Oldcontent.TabIndex = 2;
            // 
            // Newcontent
            // 
            this.Newcontent.Location = new System.Drawing.Point(170, 136);
            this.Newcontent.Name = "Newcontent";
            this.Newcontent.Size = new System.Drawing.Size(274, 25);
            this.Newcontent.TabIndex = 3;
            // 
            // replace
            // 
            this.replace.Location = new System.Drawing.Point(436, 213);
            this.replace.Name = "replace";
            this.replace.Size = new System.Drawing.Size(105, 39);
            this.replace.TabIndex = 4;
            this.replace.Text = "替换";
            this.replace.UseVisualStyleBackColor = true;
            this.replace.Click += new System.EventHandler(this.replace_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(73, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "替换列";
            // 
            // filedName
            // 
            this.filedName.Location = new System.Drawing.Point(170, 35);
            this.filedName.Name = "filedName";
            this.filedName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.filedName.Size = new System.Drawing.Size(274, 24);
            this.filedName.TabIndex = 6;
            // 
            // ReplaceSelectRows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 264);
            this.Controls.Add(this.filedName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.replace);
            this.Controls.Add(this.Newcontent);
            this.Controls.Add(this.Oldcontent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReplaceSelectRows";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批量替换(替换当前筛选后表格数据)";
            this.Load += new System.EventHandler(this.ReplaceSelectRows_Load);
            ((System.ComponentModel.ISupportInitialize)(this.filedName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Oldcontent;
        private System.Windows.Forms.TextBox Newcontent;
        private System.Windows.Forms.Button replace;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.ComboBoxEdit filedName;
    }
}