using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskManager.controller
{
    /// <summary>
    /// 遮罩层
    /// </summary>
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public class MaskLayer : Control
    {
        /// <summary>
        /// 遮罩层
        /// </summary>
        public MaskLayer()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            CreateControl();
            Visible = false;

            this.Dock = DockStyle.Fill;
            this.Controls.Add(progressBar);
        }
        /// <summary>
        /// 进度条
        /// </summary>
        public ProgressBar progressBar = new ProgressBar();

        /// <summary>
        /// 设置进度条显示值
        /// </summary>
        /// <param name="value"></param>
        public void SetProgressBarValue(int value)
        {
            this.BeginInvoke(new Action(() =>
            {
                if (value <= progressBar.Maximum)
                {
                    progressBar.Value = value;
                }
            }));
        }
        private int _Alpha = 125;
        /// <summary>
        /// 透明度<para>范围：0~255（完全透明~完全不透明）</para><para>默认：125（半透明）</para>
        /// </summary>
        [Category("DemoUI"), Description("透明度\r\n范围：0~255（完全透明~完全不透明）\r\n默认：125（半透明）")]
        public int Alpha
        {
            get { return _Alpha; }
            set
            {
                if (value < 0) value = 0;
                if (value > 255) value = 255;
                _Alpha = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 是否处于显示状态
        /// </summary>
        [Category("LESLIE_UI"), Description("是否处于显示状态"), Browsable(false)]
        public bool IsShow { get; private set; } = true;



        /// <summary>
        /// OnPaint
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            SolidBrush BackColorBrush = new SolidBrush(Color.FromArgb(_Alpha, BackColor));
            e.Graphics.FillRectangle(BackColorBrush, e.ClipRectangle);
            BackColorBrush.Dispose();
        }
        /// <summary>
        /// 是否启用点击隐藏功能<para>默认：是</para>
        /// </summary>
        [Category("DemoUI"), Description("是否启用点击隐藏功能\r\n默认：否")]
        public bool EnabledClickHide { get; set; } = false;
        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (EnabledClickHide)
            {
                HideMask();
            }
        }

        /// <summary>
        /// 显示遮罩层
        /// </summary>
        public void ShowMask()
        {
            try
            {
                this.BeginInvoke(new Action(() =>
                {
                    IsShow = true;
                    SendKeys.Send("{Tab}");

                    BringToFront();
                    this.Visible = true;
                    this.BackColor = Color.Black;
                    Show();

                    int x = (int)(this.Width * 0.1);
                    int y = this.Height / 2;
                    this.progressBar.Location = new System.Drawing.Point(x, y);
                    this.progressBar.Name = "progressBar";
                    int w = (int)(this.Width * 0.8);
                    this.progressBar.Size = new System.Drawing.Size(w, 23);
                    this.progressBar.TabIndex = 2;
                }));
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 隐藏遮罩层
        /// </summary>
        public void HideMask()
        {
            try
            {
                this.BeginInvoke(new Action(() =>
                {
                    IsShow = false;
                    SendToBack();
                    Visible = false;
                    Hide();
                }));
            }
            catch (Exception)
            {
            }
        }
    }
}
