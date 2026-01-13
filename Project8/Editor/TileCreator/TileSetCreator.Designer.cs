namespace Project8.Editor.TileSetCreator
{
    partial class TileSetCreator
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
            panel_colors = new System.Windows.Forms.Panel();
            colorBuffer = new System.Windows.Forms.PictureBox();
            btHelp = new System.Windows.Forms.Button();
            groupAffichage = new System.Windows.Forms.GroupBox();
            radTransparent = new System.Windows.Forms.RadioButton();
            radMultiTile = new System.Windows.Forms.RadioButton();
            radAtutotile = new System.Windows.Forms.RadioButton();
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
            r_nq = new System.Windows.Forms.PictureBox();
            r_d = new System.Windows.Forms.PictureBox();
            r_sd = new System.Windows.Forms.PictureBox();
            r_v = new System.Windows.Forms.PictureBox();
            r_nz = new System.Windows.Forms.PictureBox();
            r_q = new System.Windows.Forms.PictureBox();
            r_zd = new System.Windows.Forms.PictureBox();
            r_f = new System.Windows.Forms.PictureBox();
            r_nd = new System.Windows.Forms.PictureBox();
            r_s = new System.Windows.Forms.PictureBox();
            r_sq = new System.Windows.Forms.PictureBox();
            r_h = new System.Windows.Forms.PictureBox();
            r_ns = new System.Windows.Forms.PictureBox();
            r_z = new System.Windows.Forms.PictureBox();
            r_zq = new System.Windows.Forms.PictureBox();
            r_a = new System.Windows.Forms.PictureBox();
            btLoad = new System.Windows.Forms.Button();
            btSave = new System.Windows.Forms.Button();
            panel_props = new System.Windows.Forms.Panel();
            tv = new System.Windows.Forms.TreeView();
            btNew = new System.Windows.Forms.Button();
            panel_colors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)colorBuffer).BeginInit();
            groupAffichage.SuspendLayout();
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
            ((System.ComponentModel.ISupportInitialize)r_nq).BeginInit();
            ((System.ComponentModel.ISupportInitialize)r_d).BeginInit();
            ((System.ComponentModel.ISupportInitialize)r_sd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)r_v).BeginInit();
            ((System.ComponentModel.ISupportInitialize)r_nz).BeginInit();
            ((System.ComponentModel.ISupportInitialize)r_q).BeginInit();
            ((System.ComponentModel.ISupportInitialize)r_zd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)r_f).BeginInit();
            ((System.ComponentModel.ISupportInitialize)r_nd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)r_s).BeginInit();
            ((System.ComponentModel.ISupportInitialize)r_sq).BeginInit();
            ((System.ComponentModel.ISupportInitialize)r_h).BeginInit();
            ((System.ComponentModel.ISupportInitialize)r_ns).BeginInit();
            ((System.ComponentModel.ISupportInitialize)r_z).BeginInit();
            ((System.ComponentModel.ISupportInitialize)r_zq).BeginInit();
            ((System.ComponentModel.ISupportInitialize)r_a).BeginInit();
            panel_props.SuspendLayout();
            SuspendLayout();
            // 
            // panel_colors
            // 
            panel_colors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panel_colors.Controls.Add(colorBuffer);
            panel_colors.Controls.Add(btHelp);
            panel_colors.Controls.Add(groupAffichage);
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
            panel_colors.Size = new System.Drawing.Size(664, 104);
            panel_colors.TabIndex = 6;
            // 
            // colorBuffer
            // 
            colorBuffer.Location = new System.Drawing.Point(209, 37);
            colorBuffer.Name = "colorBuffer";
            colorBuffer.Size = new System.Drawing.Size(26, 26);
            colorBuffer.TabIndex = 5;
            colorBuffer.TabStop = false;
            colorBuffer.MouseClick += color_MouseClick;
            // 
            // btHelp
            // 
            btHelp.BackColor = System.Drawing.Color.MintCream;
            btHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btHelp.Location = new System.Drawing.Point(345, 22);
            btHelp.Name = "btHelp";
            btHelp.Size = new System.Drawing.Size(87, 41);
            btHelp.TabIndex = 4;
            btHelp.Text = "Help";
            btHelp.UseVisualStyleBackColor = false;
            btHelp.Click += btHelp_Click;
            // 
            // groupAffichage
            // 
            groupAffichage.Controls.Add(radTransparent);
            groupAffichage.Controls.Add(radMultiTile);
            groupAffichage.Controls.Add(radAtutotile);
            groupAffichage.Location = new System.Drawing.Point(457, 4);
            groupAffichage.Name = "groupAffichage";
            groupAffichage.Size = new System.Drawing.Size(200, 100);
            groupAffichage.TabIndex = 3;
            groupAffichage.TabStop = false;
            groupAffichage.Text = "Affichage";
            // 
            // radTransparent
            // 
            radTransparent.AutoSize = true;
            radTransparent.Location = new System.Drawing.Point(23, 18);
            radTransparent.Name = "radTransparent";
            radTransparent.Size = new System.Drawing.Size(110, 25);
            radTransparent.TabIndex = 2;
            radTransparent.Text = "Transparent";
            radTransparent.UseVisualStyleBackColor = true;
            // 
            // radMultiTile
            // 
            radMultiTile.AutoSize = true;
            radMultiTile.Location = new System.Drawing.Point(23, 59);
            radMultiTile.Name = "radMultiTile";
            radMultiTile.Size = new System.Drawing.Size(88, 25);
            radMultiTile.TabIndex = 2;
            radMultiTile.TabStop = true;
            radMultiTile.Text = "MultiTile";
            radMultiTile.UseVisualStyleBackColor = true;
            // 
            // radAtutotile
            // 
            radAtutotile.AutoSize = true;
            radAtutotile.Checked = true;
            radAtutotile.Location = new System.Drawing.Point(23, 38);
            radAtutotile.Name = "radAtutotile";
            radAtutotile.Size = new System.Drawing.Size(82, 25);
            radAtutotile.TabIndex = 2;
            radAtutotile.TabStop = true;
            radAtutotile.Text = "Autotile";
            radAtutotile.UseVisualStyleBackColor = true;
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
            panel_render.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            panel_render.Controls.Add(r_nq);
            panel_render.Controls.Add(r_d);
            panel_render.Controls.Add(r_sd);
            panel_render.Controls.Add(r_v);
            panel_render.Controls.Add(r_nz);
            panel_render.Controls.Add(r_q);
            panel_render.Controls.Add(r_zd);
            panel_render.Controls.Add(r_f);
            panel_render.Controls.Add(r_nd);
            panel_render.Controls.Add(r_s);
            panel_render.Controls.Add(r_sq);
            panel_render.Controls.Add(r_h);
            panel_render.Controls.Add(r_ns);
            panel_render.Controls.Add(r_z);
            panel_render.Controls.Add(r_zq);
            panel_render.Controls.Add(r_a);
            panel_render.Location = new System.Drawing.Point(12, 124);
            panel_render.Name = "panel_render";
            panel_render.Size = new System.Drawing.Size(664, 663);
            panel_render.TabIndex = 7;
            // 
            // r_nq
            // 
            r_nq.BackColor = System.Drawing.Color.White;
            r_nq.Location = new System.Drawing.Point(501, 501);
            r_nq.Name = "r_nq";
            r_nq.Size = new System.Drawing.Size(160, 160);
            r_nq.TabIndex = 0;
            r_nq.TabStop = false;
            r_nq.MouseDown += Render_MouseDown;
            r_nq.MouseLeave += Render_MouseLeave;
            r_nq.MouseMove += Render_MouseMove;
            r_nq.MouseUp += Render_MouseUp;
            // 
            // r_d
            // 
            r_d.BackColor = System.Drawing.Color.White;
            r_d.Location = new System.Drawing.Point(501, 169);
            r_d.Name = "r_d";
            r_d.Size = new System.Drawing.Size(160, 160);
            r_d.TabIndex = 0;
            r_d.TabStop = false;
            r_d.MouseDown += Render_MouseDown;
            r_d.MouseLeave += Render_MouseLeave;
            r_d.MouseMove += Render_MouseMove;
            r_d.MouseUp += Render_MouseUp;
            // 
            // r_sd
            // 
            r_sd.BackColor = System.Drawing.Color.White;
            r_sd.Location = new System.Drawing.Point(501, 335);
            r_sd.Name = "r_sd";
            r_sd.Size = new System.Drawing.Size(160, 160);
            r_sd.TabIndex = 0;
            r_sd.TabStop = false;
            r_sd.MouseDown += Render_MouseDown;
            r_sd.MouseLeave += Render_MouseLeave;
            r_sd.MouseMove += Render_MouseMove;
            r_sd.MouseUp += Render_MouseUp;
            // 
            // r_v
            // 
            r_v.BackColor = System.Drawing.Color.White;
            r_v.Location = new System.Drawing.Point(501, 3);
            r_v.Name = "r_v";
            r_v.Size = new System.Drawing.Size(160, 160);
            r_v.TabIndex = 0;
            r_v.TabStop = false;
            r_v.MouseDown += Render_MouseDown;
            r_v.MouseLeave += Render_MouseLeave;
            r_v.MouseMove += Render_MouseMove;
            r_v.MouseUp += Render_MouseUp;
            // 
            // r_nz
            // 
            r_nz.BackColor = System.Drawing.Color.White;
            r_nz.Location = new System.Drawing.Point(169, 501);
            r_nz.Name = "r_nz";
            r_nz.Size = new System.Drawing.Size(160, 160);
            r_nz.TabIndex = 0;
            r_nz.TabStop = false;
            r_nz.MouseDown += Render_MouseDown;
            r_nz.MouseLeave += Render_MouseLeave;
            r_nz.MouseMove += Render_MouseMove;
            r_nz.MouseUp += Render_MouseUp;
            // 
            // r_q
            // 
            r_q.BackColor = System.Drawing.Color.White;
            r_q.Location = new System.Drawing.Point(169, 169);
            r_q.Name = "r_q";
            r_q.Size = new System.Drawing.Size(160, 160);
            r_q.TabIndex = 0;
            r_q.TabStop = false;
            r_q.MouseDown += Render_MouseDown;
            r_q.MouseLeave += Render_MouseLeave;
            r_q.MouseMove += Render_MouseMove;
            r_q.MouseUp += Render_MouseUp;
            // 
            // r_zd
            // 
            r_zd.BackColor = System.Drawing.Color.White;
            r_zd.Location = new System.Drawing.Point(169, 335);
            r_zd.Name = "r_zd";
            r_zd.Size = new System.Drawing.Size(160, 160);
            r_zd.TabIndex = 0;
            r_zd.TabStop = false;
            r_zd.MouseDown += Render_MouseDown;
            r_zd.MouseLeave += Render_MouseLeave;
            r_zd.MouseMove += Render_MouseMove;
            r_zd.MouseUp += Render_MouseUp;
            // 
            // r_f
            // 
            r_f.BackColor = System.Drawing.Color.White;
            r_f.Location = new System.Drawing.Point(169, 3);
            r_f.Name = "r_f";
            r_f.Size = new System.Drawing.Size(160, 160);
            r_f.TabIndex = 0;
            r_f.TabStop = false;
            r_f.MouseDown += Render_MouseDown;
            r_f.MouseLeave += Render_MouseLeave;
            r_f.MouseMove += Render_MouseMove;
            r_f.MouseUp += Render_MouseUp;
            // 
            // r_nd
            // 
            r_nd.BackColor = System.Drawing.Color.White;
            r_nd.Location = new System.Drawing.Point(335, 501);
            r_nd.Name = "r_nd";
            r_nd.Size = new System.Drawing.Size(160, 160);
            r_nd.TabIndex = 0;
            r_nd.TabStop = false;
            r_nd.MouseDown += Render_MouseDown;
            r_nd.MouseLeave += Render_MouseLeave;
            r_nd.MouseMove += Render_MouseMove;
            r_nd.MouseUp += Render_MouseUp;
            // 
            // r_s
            // 
            r_s.BackColor = System.Drawing.Color.White;
            r_s.Location = new System.Drawing.Point(335, 169);
            r_s.Name = "r_s";
            r_s.Size = new System.Drawing.Size(160, 160);
            r_s.TabIndex = 0;
            r_s.TabStop = false;
            r_s.MouseDown += Render_MouseDown;
            r_s.MouseLeave += Render_MouseLeave;
            r_s.MouseMove += Render_MouseMove;
            r_s.MouseUp += Render_MouseUp;
            // 
            // r_sq
            // 
            r_sq.BackColor = System.Drawing.Color.White;
            r_sq.Location = new System.Drawing.Point(335, 335);
            r_sq.Name = "r_sq";
            r_sq.Size = new System.Drawing.Size(160, 160);
            r_sq.TabIndex = 0;
            r_sq.TabStop = false;
            r_sq.MouseDown += Render_MouseDown;
            r_sq.MouseLeave += Render_MouseLeave;
            r_sq.MouseMove += Render_MouseMove;
            r_sq.MouseUp += Render_MouseUp;
            // 
            // r_h
            // 
            r_h.BackColor = System.Drawing.Color.White;
            r_h.Location = new System.Drawing.Point(335, 3);
            r_h.Name = "r_h";
            r_h.Size = new System.Drawing.Size(160, 160);
            r_h.TabIndex = 0;
            r_h.TabStop = false;
            r_h.MouseDown += Render_MouseDown;
            r_h.MouseLeave += Render_MouseLeave;
            r_h.MouseMove += Render_MouseMove;
            r_h.MouseUp += Render_MouseUp;
            // 
            // r_ns
            // 
            r_ns.BackColor = System.Drawing.Color.White;
            r_ns.Location = new System.Drawing.Point(3, 501);
            r_ns.Name = "r_ns";
            r_ns.Size = new System.Drawing.Size(160, 160);
            r_ns.TabIndex = 0;
            r_ns.TabStop = false;
            r_ns.MouseDown += Render_MouseDown;
            r_ns.MouseLeave += Render_MouseLeave;
            r_ns.MouseMove += Render_MouseMove;
            r_ns.MouseUp += Render_MouseUp;
            // 
            // r_z
            // 
            r_z.BackColor = System.Drawing.Color.White;
            r_z.Location = new System.Drawing.Point(3, 169);
            r_z.Name = "r_z";
            r_z.Size = new System.Drawing.Size(160, 160);
            r_z.TabIndex = 0;
            r_z.TabStop = false;
            r_z.MouseDown += Render_MouseDown;
            r_z.MouseLeave += Render_MouseLeave;
            r_z.MouseMove += Render_MouseMove;
            r_z.MouseUp += Render_MouseUp;
            // 
            // r_zq
            // 
            r_zq.BackColor = System.Drawing.Color.White;
            r_zq.Location = new System.Drawing.Point(3, 335);
            r_zq.Name = "r_zq";
            r_zq.Size = new System.Drawing.Size(160, 160);
            r_zq.TabIndex = 0;
            r_zq.TabStop = false;
            r_zq.MouseDown += Render_MouseDown;
            r_zq.MouseLeave += Render_MouseLeave;
            r_zq.MouseMove += Render_MouseMove;
            r_zq.MouseUp += Render_MouseUp;
            // 
            // r_a
            // 
            r_a.BackColor = System.Drawing.Color.White;
            r_a.Location = new System.Drawing.Point(3, 3);
            r_a.Name = "r_a";
            r_a.Size = new System.Drawing.Size(160, 160);
            r_a.TabIndex = 0;
            r_a.TabStop = false;
            r_a.MouseDown += Render_MouseDown;
            r_a.MouseLeave += Render_MouseLeave;
            r_a.MouseMove += Render_MouseMove;
            r_a.MouseUp += Render_MouseUp;
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
            panel_props.Location = new System.Drawing.Point(682, 12);
            panel_props.Name = "panel_props";
            panel_props.Size = new System.Drawing.Size(290, 775);
            panel_props.TabIndex = 5;
            // 
            // tv
            // 
            tv.Location = new System.Drawing.Point(3, 42);
            tv.Name = "tv";
            tv.Size = new System.Drawing.Size(277, 726);
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
            ClientSize = new System.Drawing.Size(979, 797);
            Controls.Add(panel_render);
            Controls.Add(panel_colors);
            Controls.Add(panel_props);
            Font = new System.Drawing.Font("Segoe UI", 12F);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "TileSetCreator";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "TileSetCreator";
            Load += TileCreator_Load;
            panel_colors.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)colorBuffer).EndInit();
            groupAffichage.ResumeLayout(false);
            groupAffichage.PerformLayout();
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
            ((System.ComponentModel.ISupportInitialize)r_nq).EndInit();
            ((System.ComponentModel.ISupportInitialize)r_d).EndInit();
            ((System.ComponentModel.ISupportInitialize)r_sd).EndInit();
            ((System.ComponentModel.ISupportInitialize)r_v).EndInit();
            ((System.ComponentModel.ISupportInitialize)r_nz).EndInit();
            ((System.ComponentModel.ISupportInitialize)r_q).EndInit();
            ((System.ComponentModel.ISupportInitialize)r_zd).EndInit();
            ((System.ComponentModel.ISupportInitialize)r_f).EndInit();
            ((System.ComponentModel.ISupportInitialize)r_nd).EndInit();
            ((System.ComponentModel.ISupportInitialize)r_s).EndInit();
            ((System.ComponentModel.ISupportInitialize)r_sq).EndInit();
            ((System.ComponentModel.ISupportInitialize)r_h).EndInit();
            ((System.ComponentModel.ISupportInitialize)r_ns).EndInit();
            ((System.ComponentModel.ISupportInitialize)r_z).EndInit();
            ((System.ComponentModel.ISupportInitialize)r_zq).EndInit();
            ((System.ComponentModel.ISupportInitialize)r_a).EndInit();
            panel_props.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
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
        private System.Windows.Forms.PictureBox r_a;
        private System.Windows.Forms.PictureBox r_nq;
        private System.Windows.Forms.PictureBox r_d;
        private System.Windows.Forms.PictureBox r_sd;
        private System.Windows.Forms.PictureBox r_v;
        private System.Windows.Forms.PictureBox r_nz;
        private System.Windows.Forms.PictureBox r_q;
        private System.Windows.Forms.PictureBox r_zd;
        private System.Windows.Forms.PictureBox r_f;
        private System.Windows.Forms.PictureBox r_nd;
        private System.Windows.Forms.PictureBox r_s;
        private System.Windows.Forms.PictureBox r_sq;
        private System.Windows.Forms.PictureBox r_h;
        private System.Windows.Forms.PictureBox r_ns;
        private System.Windows.Forms.PictureBox r_z;
        private System.Windows.Forms.PictureBox r_zq;
        private System.Windows.Forms.GroupBox groupAffichage;
        private System.Windows.Forms.RadioButton radTransparent;
        private System.Windows.Forms.RadioButton radMultiTile;
        private System.Windows.Forms.RadioButton radAtutotile;
        private System.Windows.Forms.Button btHelp;
        private System.Windows.Forms.PictureBox colorBuffer;
    }
}