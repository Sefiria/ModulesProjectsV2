namespace Project8.Editor.TileSetCreator
{
    partial class TileSetCreatorOLD
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
            components = new System.ComponentModel.Container();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            Render = new System.Windows.Forms.PictureBox();
            panel_colors = new System.Windows.Forms.Panel();
            usedColor = new System.Windows.Forms.PictureBox();
            color8 = new System.Windows.Forms.PictureBox();
            color4 = new System.Windows.Forms.PictureBox();
            color6 = new System.Windows.Forms.PictureBox();
            color2 = new System.Windows.Forms.PictureBox();
            color7 = new System.Windows.Forms.PictureBox();
            color3 = new System.Windows.Forms.PictureBox();
            color5 = new System.Windows.Forms.PictureBox();
            color1 = new System.Windows.Forms.PictureBox();
            colorDialog1 = new System.Windows.Forms.ColorDialog();
            panel_render = new System.Windows.Forms.Panel();
            btLoad = new System.Windows.Forms.Button();
            btSave = new System.Windows.Forms.Button();
            panel_props = new System.Windows.Forms.Panel();
            tv = new System.Windows.Forms.TreeView();
            btNew = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)Render).BeginInit();
            panel_colors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)usedColor).BeginInit();
            ((System.ComponentModel.ISupportInitialize)color8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)color4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)color6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)color2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)color7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)color3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)color5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)color1).BeginInit();
            panel_render.SuspendLayout();
            panel_props.SuspendLayout();
            SuspendLayout();
            // 
            // Render
            // 
            Render.BackColor = System.Drawing.SystemColors.Control;
            Render.Dock = System.Windows.Forms.DockStyle.Fill;
            Render.Location = new System.Drawing.Point(0, 0);
            Render.Name = "Render";
            Render.Size = new System.Drawing.Size(706, 498);
            Render.TabIndex = 4;
            Render.TabStop = false;
            Render.MouseDown += Render_MouseDown;
            Render.MouseLeave += Render_MouseLeave;
            Render.MouseMove += Render_MouseMove;
            Render.MouseUp += Render_MouseUp;
            // 
            // panel_colors
            // 
            panel_colors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panel_colors.Controls.Add(usedColor);
            panel_colors.Controls.Add(color8);
            panel_colors.Controls.Add(color4);
            panel_colors.Controls.Add(color6);
            panel_colors.Controls.Add(color2);
            panel_colors.Controls.Add(color7);
            panel_colors.Controls.Add(color3);
            panel_colors.Controls.Add(color5);
            panel_colors.Controls.Add(color1);
            panel_colors.Location = new System.Drawing.Point(12, 12);
            panel_colors.Name = "panel_colors";
            panel_colors.Size = new System.Drawing.Size(706, 104);
            panel_colors.TabIndex = 6;
            // 
            // usedColor
            // 
            usedColor.Location = new System.Drawing.Point(17, 21);
            usedColor.Name = "usedColor";
            usedColor.Size = new System.Drawing.Size(58, 58);
            usedColor.TabIndex = 1;
            usedColor.TabStop = false;
            // 
            // color8
            // 
            color8.Location = new System.Drawing.Point(177, 53);
            color8.Name = "color8";
            color8.Size = new System.Drawing.Size(26, 26);
            color8.TabIndex = 0;
            color8.TabStop = false;
            color8.MouseClick += color_MouseClick;
            // 
            // color4
            // 
            color4.Location = new System.Drawing.Point(177, 21);
            color4.Name = "color4";
            color4.Size = new System.Drawing.Size(26, 26);
            color4.TabIndex = 0;
            color4.TabStop = false;
            color4.MouseClick += color_MouseClick;
            // 
            // color6
            // 
            color6.Location = new System.Drawing.Point(113, 53);
            color6.Name = "color6";
            color6.Size = new System.Drawing.Size(26, 26);
            color6.TabIndex = 0;
            color6.TabStop = false;
            color6.MouseClick += color_MouseClick;
            // 
            // color2
            // 
            color2.Location = new System.Drawing.Point(113, 21);
            color2.Name = "color2";
            color2.Size = new System.Drawing.Size(26, 26);
            color2.TabIndex = 0;
            color2.TabStop = false;
            color2.MouseClick += color_MouseClick;
            // 
            // color7
            // 
            color7.Location = new System.Drawing.Point(145, 53);
            color7.Name = "color7";
            color7.Size = new System.Drawing.Size(26, 26);
            color7.TabIndex = 0;
            color7.TabStop = false;
            color7.MouseClick += color_MouseClick;
            // 
            // color3
            // 
            color3.Location = new System.Drawing.Point(145, 21);
            color3.Name = "color3";
            color3.Size = new System.Drawing.Size(26, 26);
            color3.TabIndex = 0;
            color3.TabStop = false;
            color3.MouseClick += color_MouseClick;
            // 
            // color5
            // 
            color5.Location = new System.Drawing.Point(81, 53);
            color5.Name = "color5";
            color5.Size = new System.Drawing.Size(26, 26);
            color5.TabIndex = 0;
            color5.TabStop = false;
            color5.MouseClick += color_MouseClick;
            // 
            // color1
            // 
            color1.Location = new System.Drawing.Point(81, 21);
            color1.Name = "color1";
            color1.Size = new System.Drawing.Size(26, 26);
            color1.TabIndex = 0;
            color1.TabStop = false;
            color1.MouseClick += color_MouseClick;
            // 
            // panel_render
            // 
            panel_render.Controls.Add(Render);
            panel_render.Location = new System.Drawing.Point(12, 124);
            panel_render.Name = "panel_render";
            panel_render.Size = new System.Drawing.Size(706, 498);
            panel_render.TabIndex = 7;
            // 
            // btLoad
            // 
            btLoad.Location = new System.Drawing.Point(94, 3);
            btLoad.Name = "btLoad";
            btLoad.Size = new System.Drawing.Size(95, 33);
            btLoad.TabIndex = 4;
            btLoad.Text = "Load";
            btLoad.UseVisualStyleBackColor = true;
            btLoad.Click += btLoad_Click;
            // 
            // btSave
            // 
            btSave.Location = new System.Drawing.Point(195, 3);
            btSave.Name = "btSave";
            btSave.Size = new System.Drawing.Size(85, 33);
            btSave.TabIndex = 4;
            btSave.Text = "Save";
            btSave.UseVisualStyleBackColor = true;
            btSave.Click += btSave_Click;
            // 
            // panel_props
            // 
            panel_props.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panel_props.Controls.Add(tv);
            panel_props.Controls.Add(btSave);
            panel_props.Controls.Add(btNew);
            panel_props.Controls.Add(btLoad);
            panel_props.Location = new System.Drawing.Point(724, 12);
            panel_props.Name = "panel_props";
            panel_props.Size = new System.Drawing.Size(290, 610);
            panel_props.TabIndex = 5;
            // 
            // tv
            // 
            tv.Location = new System.Drawing.Point(3, 42);
            tv.Name = "tv";
            tv.Size = new System.Drawing.Size(277, 561);
            tv.TabIndex = 5;
            // 
            // btNew
            // 
            btNew.Location = new System.Drawing.Point(3, 3);
            btNew.Name = "btNew";
            btNew.Size = new System.Drawing.Size(85, 33);
            btNew.TabIndex = 4;
            btNew.Text = "New";
            btNew.UseVisualStyleBackColor = true;
            btNew.Click += btNew_Click;
            // 
            // TileSetCreator
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1026, 634);
            Controls.Add(panel_render);
            Controls.Add(panel_colors);
            Controls.Add(panel_props);
            Font = new System.Drawing.Font("Segoe UI", 12F);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "TileSetCreator";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "TileSetCreator";
            Load += TileCreator_Load;
            ((System.ComponentModel.ISupportInitialize)Render).EndInit();
            panel_colors.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)usedColor).EndInit();
            ((System.ComponentModel.ISupportInitialize)color8).EndInit();
            ((System.ComponentModel.ISupportInitialize)color4).EndInit();
            ((System.ComponentModel.ISupportInitialize)color6).EndInit();
            ((System.ComponentModel.ISupportInitialize)color2).EndInit();
            ((System.ComponentModel.ISupportInitialize)color7).EndInit();
            ((System.ComponentModel.ISupportInitialize)color3).EndInit();
            ((System.ComponentModel.ISupportInitialize)color5).EndInit();
            ((System.ComponentModel.ISupportInitialize)color1).EndInit();
            panel_render.ResumeLayout(false);
            panel_props.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox Render;
        private System.Windows.Forms.Panel panel_colors;
        private System.Windows.Forms.PictureBox color1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.PictureBox color8;
        private System.Windows.Forms.PictureBox color4;
        private System.Windows.Forms.PictureBox color6;
        private System.Windows.Forms.PictureBox color2;
        private System.Windows.Forms.PictureBox color7;
        private System.Windows.Forms.PictureBox color3;
        private System.Windows.Forms.PictureBox color5;
        private System.Windows.Forms.PictureBox usedColor;
        private System.Windows.Forms.Panel panel_render;
        private System.Windows.Forms.Button btLoad;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Panel panel_props;
        private System.Windows.Forms.Button btNew;
        private System.Windows.Forms.TreeView tv;
    }
}