using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleMinecraftLauncher
{
    
    public partial class RippleButton : UserControl
    {
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Ripple button text"), Category("Appearance")]
        public new string Text
        {
            get => text;
            set
            {
                text = value;
            }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Enables or disables button"), Category("Appearance")]
        public new bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
            }
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Ripple color of button (rippling circle shows after click)"), Category("Appearance")]
        public Color RippleColor
        {
            get => rippleColor;
            set => rippleColor = value;
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Button background color"), Category("Appearance")]
        public new Color BackColor
        {
            get => backColor;
            set => backColor = value;
        }

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Button highlight color"), Category("Appearance")]
        public Color HoverColor
        {
            get => hoverColor;
            set => hoverColor = value;
        }

        public new event EventHandler OnClick;

        private Color backColor = Color.FromArgb(255, 50, 88, 98);
        private Color hoverColor = Color.FromArgb(255, 65, 103, 113);
        private Color rippleColor = Color.FromArgb(150, 25, 45, 45);
        private string text = "button";
        private Point lastClick = new Point(0, 0);
        private StringFormat LabelStringFormat;
        private bool enabled = true;

        public RippleButton()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint |
              ControlStyles.ResizeRedraw, true);
            DoubleBuffered = true;
            LabelStringFormat = new StringFormat();
            LabelStringFormat.LineAlignment = StringAlignment.Center;
            LabelStringFormat.Alignment = StringAlignment.Center;
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.Clear(backColor);

            g.DrawString(text, Font, new SolidBrush(ForeColor), ClientRectangle, LabelStringFormat);

            Refresh();

        }

    }
}
