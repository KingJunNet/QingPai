﻿namespace ExpertLib.Controls
{
    partial class TitleTextBox
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
            this.textBoxEx1 = new ExpertLib.Controls.TextBoxEx();
            this.SuspendLayout();
            // 
            // textBoxEx1
            // 
            this.textBoxEx1.DecLength = 2;
            this.textBoxEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxEx1.InputType = ExpertLib.Controls.TextInputType.NotControl;
            this.textBoxEx1.Location = new System.Drawing.Point(126, 2);
            this.textBoxEx1.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxEx1.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.textBoxEx1.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.textBoxEx1.MyRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.textBoxEx1.Name = "textBoxEx1";
            this.textBoxEx1.OldText = null;
            this.textBoxEx1.PromptColor = System.Drawing.Color.Gray;
            this.textBoxEx1.PromptFont = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBoxEx1.PromptText = "";
            this.textBoxEx1.RegexPattern = "";
            this.textBoxEx1.Size = new System.Drawing.Size(402, 31);
            this.textBoxEx1.TabIndex = 1;
            this.textBoxEx1.TextChanged += new System.EventHandler(this.textBoxEx1_TextChanged);
            // 
            // TitleTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.Controls.Add(this.textBoxEx1);
            this.LabelWidth = 122;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "TitleTextBox";
            this.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Size = new System.Drawing.Size(532, 42);
            this.Title = "文本";
            this.Controls.SetChildIndex(this.textBoxEx1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.TextBoxEx textBoxEx1;
    }
}
