using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleMinecraftLauncher
{
    public partial class RippleButton : UserControl
    {
        [Description("Ripple button text"), Category("Appearance")]
        public new string Text
        {
            get => text;
            set
            {
                Invalidate();
                text = value;
            }
        }

        [Description("Enables or disables button"), Category("Appearance")]
        public new bool Enabled
        {
            get => enabled;
            set
            {
                Invalidate();
                enabled = value;
            }
        }


        [Description("Ripple color of button (rippling circle shows after click)"), Category("Appearance")]
        public Color RippleColor
        {
            get => rippleColor;
            set => rippleColor = value;
        }

        [Description("Button highlight color"), Category("Appearance")]
        public Color HighlightColor
        {
            get => highlightColor;
            set => highlightColor = value;
        }

        public new event EventHandler OnClick;

        private Color rippleColor = Color.FromArgb(255, 180, 240, 255);
        private Color highlightColor = Color.FromArgb(255, 140, 200, 225);

        private Color disabledRippleColor = Color.FromArgb(140, 255, 150, 150);
        private Color disabledHighlightColor = Color.FromArgb(140, 150, 56, 56);
        private Color currentBgColor;

        private string text = "button";
        private bool hovering = false;
        private bool rippleChanged = false;
        private bool enabled = true;
        private double bgOpacity = 0f;
        private double bgStep = 0.00003;
        private float rippleOpacity = 0.0f;
        private float rippleRadius = 0.0f;
        private float rippleMaximum = 600.0f;
        private float rippleStep = 0.3f;
        private const int rippleInitialAlpha = 180;
        private Point lastClick = new Point(0, 0);

        public RippleButton()
        {
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
            backgroundWorker1.RunWorkerAsync();

            currentBgColor = highlightColor;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Color bgColor = new Color();
            int bgAlpha = (int)(bgOpacity * 255);
            if (enabled)
                bgColor = Color.FromArgb(bgAlpha, highlightColor.R, highlightColor.G, highlightColor.B);
            else
                bgColor = Color.FromArgb(bgAlpha, disabledHighlightColor.R, disabledHighlightColor.G, disabledHighlightColor.B);

            e.Graphics.FillRectangle(new SolidBrush(bgColor), ClientRectangle);

            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(text, Font, new SolidBrush(Color.White), ClientRectangle, format);

            if (rippleChanged)
            {
                Color myRippleColor = new Color();
                int rippleAlpha = Constants.Clamp((int)(rippleInitialAlpha - rippleOpacity * 255), 0, 255);
                if (enabled)
                    myRippleColor = Color.FromArgb(rippleAlpha, rippleColor.R, rippleColor.G, rippleColor.B);
                else
                    myRippleColor = Color.FromArgb(rippleAlpha, disabledRippleColor.R, disabledRippleColor.G, disabledRippleColor.B);

                e.Graphics.FillEllipse(new SolidBrush(myRippleColor), lastClick.X-rippleRadius/2, lastClick.Y-rippleRadius / 2, rippleRadius, rippleRadius);
                rippleRadius += rippleStep;
                rippleOpacity = rippleRadius / rippleMaximum;

                if (rippleRadius >= rippleMaximum)
                {
                    rippleChanged = false;
                    rippleRadius = 0;
                    rippleOpacity = 0;
                    if (enabled)
                        OnClick?.Invoke(this, null);
                }
            }
        }

        private void RippleButton_MouseClick(object sender, MouseEventArgs e)
        {
            
            lastClick = new Point(e.X, e.Y);
            rippleChanged = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                bgOpacity = Constants.Clamp(bgOpacity + (hovering ? bgStep : -bgStep), 0, 1.0f);

                if (rippleChanged | bgOpacity>0)
                {
                    Invalidate();
                }
            }
        }

        private void RippleButton_MouseEnter(object sender, EventArgs e)
        {
            hovering = true;
        }

        private void RippleButton_MouseLeave(object sender, EventArgs e)
        {
            hovering = false;
        }
    }
}
