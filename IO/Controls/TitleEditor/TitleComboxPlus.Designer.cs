namespace ExpertLib.Controls
{
    partial class TitleComboxPlus
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox1 = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBox1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.CausesValidation = false;
            this.comboBox1.EditValue = "";
            this.comboBox1.Location = new System.Drawing.Point(103, 2);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.comboBox1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.Properties.Appearance.Options.UseFont = true;
            this.comboBox1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBox1.Properties.NullText = "";
            this.comboBox1.Properties.NullValuePrompt = "";
            this.comboBox1.Properties.NullValuePromptShowForEmptyValue = true;
            this.comboBox1.Size = new System.Drawing.Size(329, 25);
            this.comboBox1.TabIndex = 3;
            // 
            // TitleComboxPlus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.Controls.Add(this.comboBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TitleComboxPlus";
            this.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Size = new System.Drawing.Size(435, 34);
            this.Load += new System.EventHandler(this.TitleComboxPlus_Load);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.comboBox1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.ComboBoxEdit comboBox1;
    }
}
