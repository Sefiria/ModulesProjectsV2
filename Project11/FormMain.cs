using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Project11
{
    public partial class FormMain : Form
    {
        public static FormMain Instance { get; private set; }
        public static Graphics Graphics;

        Timer timer = new Timer();
        Bitmap Image;
        Button close;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool ReleaseCapture();
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public FormMain()
        {
            InitializeComponent();
            InitializeHeader();
        }
        private void InitializeHeader()
        {
            FormBorderStyle = FormBorderStyle.None;
            var header = new Panel()
            {
                Height = 30,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(66, 131, 209)
            };
            Controls.Add(header);
            header.MouseDown += (s, e) => {
                if (e.Button == MouseButtons.Left)
                    SendMessage(Handle, 0xA1, 0x2, 0);
            };
            close = new Button
            {
                Text = "✕",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Width = 40,
                Height = 30,
                Dock = DockStyle.Right
            };
            close.FlatAppearance.BorderSize = 0;
            close.Click += (s, e) => Close();
            close.MouseEnter += (s, e) => close.BackColor = Color.FromArgb(170, header.BackColor.G - 25, header.BackColor.B - 25);
            close.MouseLeave += (s, e) => close.BackColor = Color.Transparent;
            //close.PreviewKeyDown += (s, e) =>
            //{
            //    if (e.KeyCode == Keys.Escape)
            //        Close();
            //};
            header.Controls.Add(close);
            header.MouseDown += (s, e) => {
                if (e.Button == MouseButtons.Left)
                    SendMessage(Handle, 0xA1, 0x2, 0);
            };
            var title = new Label
            {
                Text = "S-CLI",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, header.Height / 2 - 5, 0, 0)
            };
            header.Controls.Add(title);

            void Drag(object s, System.Windows.Forms.MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, 0xA1, 0x2, 0);
                }
            }
            header.MouseDown += Drag;
            title.MouseDown += Drag;
            close.MouseDown += (s, e) => {
                if (e.Button == MouseButtons.Left && e.X < 0)
                    SendMessage(Handle, 0xA1, 0x2, 0);
            };
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Space && close.Focused)
                return true;
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            Image = new Bitmap(Render.Width, Render.Height);
            Graphics = Graphics.FromImage(Image);
            Instance = this;
            GV.Initialize();
            Init();
            timer.Interval = 10;
            timer.Tick += DrawFrame;
            timer.Start();
        }

        private void DrawFrame(object sender, EventArgs e)
        {
            Update();
            GV.Update();

            Image = new Bitmap(Render.Width, Render.Height);
            Graphics = Graphics.FromImage(Image);
            Draw();
            Render.Image = Image;
        }
    }

}


