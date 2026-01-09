namespace Project8.Editor.TileCreator
{
    partial class TileCreator
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
            cbbMode = new System.Windows.Forms.ComboBox();
            tbName = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            tbFileNameA = new System.Windows.Forms.TextBox();
            numID = new System.Windows.Forms.NumericUpDown();
            label4 = new System.Windows.Forms.Label();
            tbCharacteristics = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            toolTip1 = new System.Windows.Forms.ToolTip(components);
            label6 = new System.Windows.Forms.Label();
            numMultiTileID = new System.Windows.Forms.NumericUpDown();
            Render = new System.Windows.Forms.PictureBox();
            panel_props = new System.Windows.Forms.Panel();
            radFileNameC = new System.Windows.Forms.RadioButton();
            radFileNameB = new System.Windows.Forms.RadioButton();
            radFileNameA = new System.Windows.Forms.RadioButton();
            tbFileNameC = new System.Windows.Forms.TextBox();
            tbFileNameB = new System.Windows.Forms.TextBox();
            btSave = new System.Windows.Forms.Button();
            btLoad = new System.Windows.Forms.Button();
            colorDialog1 = new System.Windows.Forms.ColorDialog();
            panel_render = new System.Windows.Forms.Panel();
            btManage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)numID).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMultiTileID).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Render).BeginInit();
            panel_props.SuspendLayout();
            panel_render.SuspendLayout();
            SuspendLayout();
            // 
            // cbbMode
            // 
            cbbMode.FormattingEnabled = true;
            cbbMode.Location = new System.Drawing.Point(128, 50);
            cbbMode.Margin = new System.Windows.Forms.Padding(4);
            cbbMode.Name = "cbbMode";
            cbbMode.Size = new System.Drawing.Size(154, 29);
            cbbMode.TabIndex = 0;
            // 
            // tbName
            // 
            tbName.Location = new System.Drawing.Point(128, 86);
            tbName.Name = "tbName";
            tbName.Size = new System.Drawing.Size(155, 29);
            tbName.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(69, 53);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(50, 21);
            label1.TabIndex = 2;
            label1.Text = "Mode";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(69, 89);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(52, 21);
            label2.TabIndex = 2;
            label2.Text = "Name";
            // 
            // tbFileNameA
            // 
            tbFileNameA.Location = new System.Drawing.Point(127, 121);
            tbFileNameA.Name = "tbFileNameA";
            tbFileNameA.Size = new System.Drawing.Size(333, 29);
            tbFileNameA.TabIndex = 1;
            // 
            // numID
            // 
            numID.Location = new System.Drawing.Point(128, 226);
            numID.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numID.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            numID.Name = "numID";
            numID.Size = new System.Drawing.Size(156, 29);
            numID.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(95, 228);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(25, 21);
            label4.TabIndex = 2;
            label4.Text = "ID";
            // 
            // tbCharacteristics
            // 
            tbCharacteristics.Location = new System.Drawing.Point(129, 261);
            tbCharacteristics.Name = "tbCharacteristics";
            tbCharacteristics.Size = new System.Drawing.Size(155, 29);
            tbCharacteristics.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(9, 264);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(111, 21);
            label5.TabIndex = 2;
            label5.Text = "Characteristics";
            toolTip1.SetToolTip(label5, "s: solid");
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(33, 298);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(89, 21);
            label6.TabIndex = 2;
            label6.Text = "MultiTile ID";
            // 
            // numMultiTileID
            // 
            numMultiTileID.Location = new System.Drawing.Point(128, 296);
            numMultiTileID.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numMultiTileID.Name = "numMultiTileID";
            numMultiTileID.Size = new System.Drawing.Size(156, 29);
            numMultiTileID.TabIndex = 3;
            // 
            // Render
            // 
            Render.Dock = System.Windows.Forms.DockStyle.Fill;
            Render.Location = new System.Drawing.Point(0, 0);
            Render.Name = "Render";
            Render.Size = new System.Drawing.Size(706, 610);
            Render.TabIndex = 4;
            Render.TabStop = false;
            // 
            // panel_props
            // 
            panel_props.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            panel_props.Controls.Add(radFileNameC);
            panel_props.Controls.Add(radFileNameB);
            panel_props.Controls.Add(radFileNameA);
            panel_props.Controls.Add(tbFileNameC);
            panel_props.Controls.Add(tbFileNameB);
            panel_props.Controls.Add(btSave);
            panel_props.Controls.Add(btLoad);
            panel_props.Controls.Add(cbbMode);
            panel_props.Controls.Add(tbName);
            panel_props.Controls.Add(numMultiTileID);
            panel_props.Controls.Add(tbFileNameA);
            panel_props.Controls.Add(label6);
            panel_props.Controls.Add(tbCharacteristics);
            panel_props.Controls.Add(numID);
            panel_props.Controls.Add(label1);
            panel_props.Controls.Add(label4);
            panel_props.Controls.Add(label2);
            panel_props.Controls.Add(label5);
            panel_props.Location = new System.Drawing.Point(724, 64);
            panel_props.Name = "panel_props";
            panel_props.Size = new System.Drawing.Size(467, 558);
            panel_props.TabIndex = 5;
            // 
            // radFileNameC
            // 
            radFileNameC.AutoSize = true;
            radFileNameC.Location = new System.Drawing.Point(18, 191);
            radFileNameC.Name = "radFileNameC";
            radFileNameC.Size = new System.Drawing.Size(104, 25);
            radFileNameC.TabIndex = 9;
            radFileNameC.TabStop = true;
            radFileNameC.Text = "FileNameC";
            radFileNameC.UseVisualStyleBackColor = true;
            radFileNameC.CheckedChanged += radFileName_CheckedChanged;
            // 
            // radFileNameB
            // 
            radFileNameB.AutoSize = true;
            radFileNameB.Location = new System.Drawing.Point(18, 156);
            radFileNameB.Name = "radFileNameB";
            radFileNameB.Size = new System.Drawing.Size(103, 25);
            radFileNameB.TabIndex = 9;
            radFileNameB.TabStop = true;
            radFileNameB.Text = "FileNameB";
            radFileNameB.UseVisualStyleBackColor = true;
            radFileNameB.CheckedChanged += radFileName_CheckedChanged;
            // 
            // radFileNameA
            // 
            radFileNameA.AutoSize = true;
            radFileNameA.Checked = true;
            radFileNameA.Location = new System.Drawing.Point(18, 122);
            radFileNameA.Name = "radFileNameA";
            radFileNameA.Size = new System.Drawing.Size(104, 25);
            radFileNameA.TabIndex = 9;
            radFileNameA.TabStop = true;
            radFileNameA.Text = "FileNameA";
            radFileNameA.UseVisualStyleBackColor = true;
            radFileNameA.CheckedChanged += radFileName_CheckedChanged;
            // 
            // tbFileNameC
            // 
            tbFileNameC.Location = new System.Drawing.Point(128, 191);
            tbFileNameC.Name = "tbFileNameC";
            tbFileNameC.Size = new System.Drawing.Size(333, 29);
            tbFileNameC.TabIndex = 7;
            // 
            // tbFileNameB
            // 
            tbFileNameB.Location = new System.Drawing.Point(127, 156);
            tbFileNameB.Name = "tbFileNameB";
            tbFileNameB.Size = new System.Drawing.Size(333, 29);
            tbFileNameB.TabIndex = 5;
            // 
            // btSave
            // 
            btSave.Location = new System.Drawing.Point(163, 7);
            btSave.Name = "btSave";
            btSave.Size = new System.Drawing.Size(98, 33);
            btSave.TabIndex = 4;
            btSave.Text = "Save";
            btSave.UseVisualStyleBackColor = true;
            btSave.Click += btSave_Click;
            // 
            // btLoad
            // 
            btLoad.Location = new System.Drawing.Point(32, 7);
            btLoad.Name = "btLoad";
            btLoad.Size = new System.Drawing.Size(98, 33);
            btLoad.TabIndex = 4;
            btLoad.Text = "Load";
            btLoad.UseVisualStyleBackColor = true;
            btLoad.Click += btLoad_Click;
            // 
            // panel_render
            // 
            panel_render.Controls.Add(Render);
            panel_render.Location = new System.Drawing.Point(12, 12);
            panel_render.Name = "panel_render";
            panel_render.Size = new System.Drawing.Size(706, 610);
            panel_render.TabIndex = 7;
            // 
            // btManage
            // 
            btManage.Location = new System.Drawing.Point(724, 12);
            btManage.Name = "btManage";
            btManage.Size = new System.Drawing.Size(467, 46);
            btManage.TabIndex = 9;
            btManage.Text = "Manage";
            btManage.UseVisualStyleBackColor = true;
            btManage.Click += btManage_Click;
            // 
            // TileCreator
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1203, 634);
            Controls.Add(btManage);
            Controls.Add(panel_render);
            Controls.Add(panel_props);
            Font = new System.Drawing.Font("Segoe UI", 12F);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "TileCreator";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "TileCreator";
            Load += TileCreator_Load;
            ((System.ComponentModel.ISupportInitialize)numID).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMultiTileID).EndInit();
            ((System.ComponentModel.ISupportInitialize)Render).EndInit();
            panel_props.ResumeLayout(false);
            panel_props.PerformLayout();
            panel_render.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ComboBox cbbMode;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbFileNameA;
        private System.Windows.Forms.NumericUpDown numID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbCharacteristics;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numMultiTileID;
        private System.Windows.Forms.PictureBox Render;
        private System.Windows.Forms.Panel panel_props;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel panel_render;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btLoad;
        private System.Windows.Forms.TextBox tbFileNameC;
        private System.Windows.Forms.TextBox tbFileNameB;
        private System.Windows.Forms.Button btManage;
        private System.Windows.Forms.RadioButton radFileNameC;
        private System.Windows.Forms.RadioButton radFileNameB;
        private System.Windows.Forms.RadioButton radFileNameA;
    }
}