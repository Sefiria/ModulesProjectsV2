namespace Project8.Editor.EntityCreator
{
    partial class EntityCreator
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
            colorBuffer = new System.Windows.Forms.PictureBox();
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
            FramesRender = new System.Windows.Forms.PictureBox();
            numGap = new System.Windows.Forms.NumericUpDown();
            groupGraphics = new System.Windows.Forms.GroupBox();
            listAnimations = new System.Windows.Forms.ListBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            btSelectPath = new System.Windows.Forms.Button();
            textBox1 = new System.Windows.Forms.TextBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            label1 = new System.Windows.Forms.Label();
            cbbAlignment = new System.Windows.Forms.ComboBox();
            btSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)colorBuffer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)usedColor).BeginInit();
            ((System.ComponentModel.ISupportInitialize)color8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)color4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)color6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)color2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)color7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)color3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)color5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)color1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FramesRender).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numGap).BeginInit();
            groupGraphics.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // colorBuffer
            // 
            colorBuffer.Location = new System.Drawing.Point(192, 175);
            colorBuffer.Name = "colorBuffer";
            colorBuffer.Size = new System.Drawing.Size(26, 26);
            colorBuffer.TabIndex = 15;
            colorBuffer.TabStop = false;
            colorBuffer.MouseClick += color_MouseClick;
            // 
            // usedColor
            // 
            usedColor.Location = new System.Drawing.Point(0, 159);
            usedColor.Name = "usedColor";
            usedColor.Size = new System.Drawing.Size(58, 58);
            usedColor.TabIndex = 14;
            usedColor.TabStop = false;
            // 
            // color8
            // 
            color8.Location = new System.Drawing.Point(160, 191);
            color8.Name = "color8";
            color8.Size = new System.Drawing.Size(26, 26);
            color8.TabIndex = 6;
            color8.TabStop = false;
            color8.MouseClick += color_MouseClick;
            // 
            // color4
            // 
            color4.Location = new System.Drawing.Point(160, 159);
            color4.Name = "color4";
            color4.Size = new System.Drawing.Size(26, 26);
            color4.TabIndex = 7;
            color4.TabStop = false;
            color4.MouseClick += color_MouseClick;
            // 
            // color6
            // 
            color6.Location = new System.Drawing.Point(96, 191);
            color6.Name = "color6";
            color6.Size = new System.Drawing.Size(26, 26);
            color6.TabIndex = 8;
            color6.TabStop = false;
            color6.MouseClick += color_MouseClick;
            // 
            // color2
            // 
            color2.Location = new System.Drawing.Point(96, 159);
            color2.Name = "color2";
            color2.Size = new System.Drawing.Size(26, 26);
            color2.TabIndex = 9;
            color2.TabStop = false;
            color2.MouseClick += color_MouseClick;
            // 
            // color7
            // 
            color7.Location = new System.Drawing.Point(128, 191);
            color7.Name = "color7";
            color7.Size = new System.Drawing.Size(26, 26);
            color7.TabIndex = 10;
            color7.TabStop = false;
            color7.MouseClick += color_MouseClick;
            // 
            // color3
            // 
            color3.Location = new System.Drawing.Point(128, 159);
            color3.Name = "color3";
            color3.Size = new System.Drawing.Size(26, 26);
            color3.TabIndex = 11;
            color3.TabStop = false;
            color3.MouseClick += color_MouseClick;
            // 
            // color5
            // 
            color5.Location = new System.Drawing.Point(64, 191);
            color5.Name = "color5";
            color5.Size = new System.Drawing.Size(26, 26);
            color5.TabIndex = 12;
            color5.TabStop = false;
            color5.MouseClick += color_MouseClick;
            // 
            // color1
            // 
            color1.Location = new System.Drawing.Point(64, 159);
            color1.Name = "color1";
            color1.Size = new System.Drawing.Size(26, 26);
            color1.TabIndex = 13;
            color1.TabStop = false;
            color1.MouseClick += color_MouseClick;
            // 
            // FramesRender
            // 
            FramesRender.Location = new System.Drawing.Point(0, 303);
            FramesRender.Name = "FramesRender";
            FramesRender.Size = new System.Drawing.Size(218, 64);
            FramesRender.TabIndex = 16;
            FramesRender.TabStop = false;
            FramesRender.MouseClick += FramesRender_MouseClick;
            // 
            // numGap
            // 
            numGap.Location = new System.Drawing.Point(0, 246);
            numGap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            numGap.Name = "numGap";
            numGap.Size = new System.Drawing.Size(48, 23);
            numGap.TabIndex = 17;
            numGap.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numGap.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            numGap.Value = new decimal(new int[] { 40, 0, 0, 0 });
            numGap.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // groupGraphics
            // 
            groupGraphics.BackColor = System.Drawing.Color.LightGray;
            groupGraphics.Controls.Add(listAnimations);
            groupGraphics.Controls.Add(usedColor);
            groupGraphics.Controls.Add(numGap);
            groupGraphics.Controls.Add(color1);
            groupGraphics.Controls.Add(FramesRender);
            groupGraphics.Controls.Add(color5);
            groupGraphics.Controls.Add(colorBuffer);
            groupGraphics.Controls.Add(color3);
            groupGraphics.Controls.Add(color7);
            groupGraphics.Controls.Add(color8);
            groupGraphics.Controls.Add(color2);
            groupGraphics.Controls.Add(color4);
            groupGraphics.Controls.Add(color6);
            groupGraphics.Location = new System.Drawing.Point(12, 12);
            groupGraphics.Name = "groupGraphics";
            groupGraphics.Size = new System.Drawing.Size(222, 596);
            groupGraphics.TabIndex = 18;
            groupGraphics.TabStop = false;
            groupGraphics.Text = "Graphics";
            // 
            // listAnimations
            // 
            listAnimations.FormattingEnabled = true;
            listAnimations.ItemHeight = 15;
            listAnimations.Location = new System.Drawing.Point(6, 22);
            listAnimations.Name = "listAnimations";
            listAnimations.Size = new System.Drawing.Size(210, 124);
            listAnimations.TabIndex = 18;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = System.Drawing.Color.LightGray;
            groupBox1.Controls.Add(btSave);
            groupBox1.Controls.Add(btSelectPath);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Location = new System.Drawing.Point(240, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(449, 99);
            groupBox1.TabIndex = 19;
            groupBox1.TabStop = false;
            groupBox1.Text = "Name / Path";
            // 
            // btSelectPath
            // 
            btSelectPath.BackColor = System.Drawing.Color.PaleTurquoise;
            btSelectPath.Location = new System.Drawing.Point(6, 25);
            btSelectPath.Name = "btSelectPath";
            btSelectPath.Size = new System.Drawing.Size(75, 26);
            btSelectPath.TabIndex = 1;
            btSelectPath.Text = "Select...";
            btSelectPath.UseVisualStyleBackColor = false;
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(6, 57);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(437, 23);
            textBox1.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = System.Drawing.Color.LightGray;
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(cbbAlignment);
            groupBox2.Location = new System.Drawing.Point(240, 117);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(447, 491);
            groupBox2.TabIndex = 20;
            groupBox2.TabStop = false;
            groupBox2.Text = "Configure Entity";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 33);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(63, 15);
            label1.TabIndex = 1;
            label1.Text = "Alignment";
            // 
            // cbbAlignment
            // 
            cbbAlignment.FormattingEnabled = true;
            cbbAlignment.Location = new System.Drawing.Point(75, 30);
            cbbAlignment.Name = "cbbAlignment";
            cbbAlignment.Size = new System.Drawing.Size(121, 23);
            cbbAlignment.TabIndex = 0;
            // 
            // btSave
            // 
            btSave.BackColor = System.Drawing.Color.Pink;
            btSave.Location = new System.Drawing.Point(87, 25);
            btSave.Name = "btSave";
            btSave.Size = new System.Drawing.Size(75, 26);
            btSave.TabIndex = 1;
            btSave.Text = "Save";
            btSave.UseVisualStyleBackColor = false;
            btSave.Click += btSave_Click;
            // 
            // EntityCreator
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(699, 620);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(groupGraphics);
            Name = "EntityCreator";
            Text = "EntityCreator";
            Load += EntityCreator_Load;
            ((System.ComponentModel.ISupportInitialize)colorBuffer).EndInit();
            ((System.ComponentModel.ISupportInitialize)usedColor).EndInit();
            ((System.ComponentModel.ISupportInitialize)color8).EndInit();
            ((System.ComponentModel.ISupportInitialize)color4).EndInit();
            ((System.ComponentModel.ISupportInitialize)color6).EndInit();
            ((System.ComponentModel.ISupportInitialize)color2).EndInit();
            ((System.ComponentModel.ISupportInitialize)color7).EndInit();
            ((System.ComponentModel.ISupportInitialize)color3).EndInit();
            ((System.ComponentModel.ISupportInitialize)color5).EndInit();
            ((System.ComponentModel.ISupportInitialize)color1).EndInit();
            ((System.ComponentModel.ISupportInitialize)FramesRender).EndInit();
            ((System.ComponentModel.ISupportInitialize)numGap).EndInit();
            groupGraphics.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox colorBuffer;
        private System.Windows.Forms.PictureBox usedColor;
        private System.Windows.Forms.PictureBox color8;
        private System.Windows.Forms.PictureBox color4;
        private System.Windows.Forms.PictureBox color6;
        private System.Windows.Forms.PictureBox color2;
        private System.Windows.Forms.PictureBox color7;
        private System.Windows.Forms.PictureBox color3;
        private System.Windows.Forms.PictureBox color5;
        private System.Windows.Forms.PictureBox color1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox FramesRender;
        private System.Windows.Forms.NumericUpDown numGap;
        private System.Windows.Forms.GroupBox groupGraphics;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btSelectPath;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbAlignment;
        private System.Windows.Forms.ListBox listAnimations;
        private System.Windows.Forms.Button btSave;
    }
}