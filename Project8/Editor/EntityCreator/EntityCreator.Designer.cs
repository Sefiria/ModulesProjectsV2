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
            dgvAnims = new System.Windows.Forms.DataGridView();
            label7 = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            btRemoveEntity = new System.Windows.Forms.Button();
            btNewEntity = new System.Windows.Forms.Button();
            cbbEntities = new System.Windows.Forms.ComboBox();
            btSave = new System.Windows.Forms.Button();
            label3 = new System.Windows.Forms.Label();
            tbMetadataPath = new System.Windows.Forms.TextBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            cbEntityCanCollect = new System.Windows.Forms.CheckBox();
            btEntityValidateRenaming = new System.Windows.Forms.Button();
            tbEntityName = new System.Windows.Forms.TextBox();
            numEntityAnimationSpeed = new System.Windows.Forms.NumericUpDown();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            cbbEntityAlignment = new System.Windows.Forms.ComboBox();
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
            ((System.ComponentModel.ISupportInitialize)dgvAnims).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numEntityAnimationSpeed).BeginInit();
            SuspendLayout();
            // 
            // colorBuffer
            // 
            colorBuffer.Location = new System.Drawing.Point(288, 287);
            colorBuffer.Name = "colorBuffer";
            colorBuffer.Size = new System.Drawing.Size(26, 26);
            colorBuffer.TabIndex = 15;
            colorBuffer.TabStop = false;
            colorBuffer.MouseClick += color_MouseClick;
            // 
            // usedColor
            // 
            usedColor.Location = new System.Drawing.Point(96, 271);
            usedColor.Name = "usedColor";
            usedColor.Size = new System.Drawing.Size(58, 58);
            usedColor.TabIndex = 14;
            usedColor.TabStop = false;
            // 
            // color8
            // 
            color8.Location = new System.Drawing.Point(256, 303);
            color8.Name = "color8";
            color8.Size = new System.Drawing.Size(26, 26);
            color8.TabIndex = 6;
            color8.TabStop = false;
            color8.MouseClick += color_MouseClick;
            // 
            // color4
            // 
            color4.Location = new System.Drawing.Point(256, 271);
            color4.Name = "color4";
            color4.Size = new System.Drawing.Size(26, 26);
            color4.TabIndex = 7;
            color4.TabStop = false;
            color4.MouseClick += color_MouseClick;
            // 
            // color6
            // 
            color6.Location = new System.Drawing.Point(192, 303);
            color6.Name = "color6";
            color6.Size = new System.Drawing.Size(26, 26);
            color6.TabIndex = 8;
            color6.TabStop = false;
            color6.MouseClick += color_MouseClick;
            // 
            // color2
            // 
            color2.Location = new System.Drawing.Point(192, 271);
            color2.Name = "color2";
            color2.Size = new System.Drawing.Size(26, 26);
            color2.TabIndex = 9;
            color2.TabStop = false;
            color2.MouseClick += color_MouseClick;
            // 
            // color7
            // 
            color7.Location = new System.Drawing.Point(224, 303);
            color7.Name = "color7";
            color7.Size = new System.Drawing.Size(26, 26);
            color7.TabIndex = 10;
            color7.TabStop = false;
            color7.MouseClick += color_MouseClick;
            // 
            // color3
            // 
            color3.Location = new System.Drawing.Point(224, 271);
            color3.Name = "color3";
            color3.Size = new System.Drawing.Size(26, 26);
            color3.TabIndex = 11;
            color3.TabStop = false;
            color3.MouseClick += color_MouseClick;
            // 
            // color5
            // 
            color5.Location = new System.Drawing.Point(160, 303);
            color5.Name = "color5";
            color5.Size = new System.Drawing.Size(26, 26);
            color5.TabIndex = 12;
            color5.TabStop = false;
            color5.MouseClick += color_MouseClick;
            // 
            // color1
            // 
            color1.Location = new System.Drawing.Point(160, 271);
            color1.Name = "color1";
            color1.Size = new System.Drawing.Size(26, 26);
            color1.TabIndex = 13;
            color1.TabStop = false;
            color1.MouseClick += color_MouseClick;
            // 
            // FramesRender
            // 
            FramesRender.Location = new System.Drawing.Point(96, 415);
            FramesRender.Name = "FramesRender";
            FramesRender.Size = new System.Drawing.Size(218, 64);
            FramesRender.TabIndex = 16;
            FramesRender.TabStop = false;
            FramesRender.MouseClick += FramesRender_MouseClick;
            // 
            // numGap
            // 
            numGap.Location = new System.Drawing.Point(96, 358);
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
            groupGraphics.Controls.Add(dgvAnims);
            groupGraphics.Controls.Add(usedColor);
            groupGraphics.Controls.Add(numGap);
            groupGraphics.Controls.Add(color1);
            groupGraphics.Controls.Add(FramesRender);
            groupGraphics.Controls.Add(color5);
            groupGraphics.Controls.Add(label7);
            groupGraphics.Controls.Add(colorBuffer);
            groupGraphics.Controls.Add(color3);
            groupGraphics.Controls.Add(color7);
            groupGraphics.Controls.Add(color8);
            groupGraphics.Controls.Add(color2);
            groupGraphics.Controls.Add(color4);
            groupGraphics.Controls.Add(color6);
            groupGraphics.Location = new System.Drawing.Point(12, 12);
            groupGraphics.Name = "groupGraphics";
            groupGraphics.Size = new System.Drawing.Size(441, 723);
            groupGraphics.TabIndex = 18;
            groupGraphics.TabStop = false;
            groupGraphics.Text = "Graphics";
            // 
            // dgvAnims
            // 
            dgvAnims.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAnims.Location = new System.Drawing.Point(6, 43);
            dgvAnims.Name = "dgvAnims";
            dgvAnims.Size = new System.Drawing.Size(429, 218);
            dgvAnims.TabIndex = 18;
            dgvAnims.CellMouseClick += dgvAnims_CellMouseClick;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(68, 25);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(86, 15);
            label7.TabIndex = 1;
            label7.Text = "↓ Animations ↓";
            // 
            // groupBox1
            // 
            groupBox1.BackColor = System.Drawing.Color.LightGray;
            groupBox1.Controls.Add(btRemoveEntity);
            groupBox1.Controls.Add(btNewEntity);
            groupBox1.Controls.Add(cbbEntities);
            groupBox1.Controls.Add(btSave);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(tbMetadataPath);
            groupBox1.Location = new System.Drawing.Point(459, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(449, 200);
            groupBox1.TabIndex = 19;
            groupBox1.TabStop = false;
            groupBox1.Text = "Name / Path";
            // 
            // btRemoveEntity
            // 
            btRemoveEntity.BackColor = System.Drawing.Color.Pink;
            btRemoveEntity.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btRemoveEntity.Location = new System.Drawing.Point(170, 137);
            btRemoveEntity.Name = "btRemoveEntity";
            btRemoveEntity.Size = new System.Drawing.Size(75, 23);
            btRemoveEntity.TabIndex = 4;
            btRemoveEntity.Text = "Remove";
            btRemoveEntity.UseVisualStyleBackColor = false;
            btRemoveEntity.Click += btRemoveEntity_Click;
            // 
            // btNewEntity
            // 
            btNewEntity.BackColor = System.Drawing.Color.PaleGreen;
            btNewEntity.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btNewEntity.Location = new System.Drawing.Point(89, 137);
            btNewEntity.Name = "btNewEntity";
            btNewEntity.Size = new System.Drawing.Size(75, 23);
            btNewEntity.TabIndex = 4;
            btNewEntity.Text = "New Entity";
            btNewEntity.UseVisualStyleBackColor = false;
            btNewEntity.Click += btNewEntity_Click;
            // 
            // cbbEntities
            // 
            cbbEntities.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbbEntities.FormattingEnabled = true;
            cbbEntities.Location = new System.Drawing.Point(89, 108);
            cbbEntities.Name = "cbbEntities";
            cbbEntities.Size = new System.Drawing.Size(354, 23);
            cbbEntities.TabIndex = 3;
            cbbEntities.SelectedIndexChanged += cbbEntities_SelectedIndexChanged;
            // 
            // btSave
            // 
            btSave.BackColor = System.Drawing.Color.Pink;
            btSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btSave.Font = new System.Drawing.Font("Segoe UI", 12F);
            btSave.Location = new System.Drawing.Point(138, 13);
            btSave.Name = "btSave";
            btSave.Size = new System.Drawing.Size(171, 35);
            btSave.TabIndex = 1;
            btSave.Text = "Save all";
            btSave.UseVisualStyleBackColor = false;
            btSave.Click += btSave_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(6, 111);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(77, 15);
            label3.TabIndex = 1;
            label3.Text = "Entity edition";
            // 
            // tbMetadataPath
            // 
            tbMetadataPath.Location = new System.Drawing.Point(6, 57);
            tbMetadataPath.Name = "tbMetadataPath";
            tbMetadataPath.ReadOnly = true;
            tbMetadataPath.Size = new System.Drawing.Size(437, 23);
            tbMetadataPath.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = System.Drawing.Color.LightGray;
            groupBox2.Controls.Add(cbEntityCanCollect);
            groupBox2.Controls.Add(btEntityValidateRenaming);
            groupBox2.Controls.Add(tbEntityName);
            groupBox2.Controls.Add(numEntityAnimationSpeed);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(cbbEntityAlignment);
            groupBox2.Location = new System.Drawing.Point(459, 218);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(447, 517);
            groupBox2.TabIndex = 20;
            groupBox2.TabStop = false;
            groupBox2.Text = "Configure Entity";
            // 
            // cbEntityCanCollect
            // 
            cbEntityCanCollect.AutoSize = true;
            cbEntityCanCollect.Location = new System.Drawing.Point(275, 123);
            cbEntityCanCollect.Name = "cbEntityCanCollect";
            cbEntityCanCollect.Size = new System.Drawing.Size(15, 14);
            cbEntityCanCollect.TabIndex = 5;
            cbEntityCanCollect.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            cbEntityCanCollect.UseVisualStyleBackColor = true;
            // 
            // btEntityValidateRenaming
            // 
            btEntityValidateRenaming.BackColor = System.Drawing.Color.BlanchedAlmond;
            btEntityValidateRenaming.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            btEntityValidateRenaming.Location = new System.Drawing.Point(348, 36);
            btEntityValidateRenaming.Name = "btEntityValidateRenaming";
            btEntityValidateRenaming.Size = new System.Drawing.Size(93, 23);
            btEntityValidateRenaming.TabIndex = 4;
            btEntityValidateRenaming.Text = "Validate";
            btEntityValidateRenaming.UseVisualStyleBackColor = false;
            btEntityValidateRenaming.Click += btEntityValidateRenaming_Click;
            // 
            // tbEntityName
            // 
            tbEntityName.Location = new System.Drawing.Point(138, 36);
            tbEntityName.Name = "tbEntityName";
            tbEntityName.Size = new System.Drawing.Size(204, 23);
            tbEntityName.TabIndex = 3;
            // 
            // numEntityAnimationSpeed
            // 
            numEntityAnimationSpeed.Location = new System.Drawing.Point(138, 94);
            numEntityAnimationSpeed.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numEntityAnimationSpeed.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numEntityAnimationSpeed.Name = "numEntityAnimationSpeed";
            numEntityAnimationSpeed.Size = new System.Drawing.Size(303, 23);
            numEntityAnimationSpeed.TabIndex = 2;
            numEntityAnimationSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            numEntityAnimationSpeed.Value = new decimal(new int[] { 4, 0, 0, 0 });
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = System.Drawing.Color.DarkGray;
            label6.Location = new System.Drawing.Point(89, 123);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(182, 15);
            label6.TabIndex = 1;
            label6.Text = "-----------------------------------";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(18, 122);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(65, 15);
            label5.TabIndex = 1;
            label5.Text = "CanCollect";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(20, 97);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(95, 15);
            label2.TabIndex = 1;
            label2.Text = "AnimationSpeed";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(20, 40);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(39, 15);
            label4.TabIndex = 1;
            label4.Text = "Name";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(20, 68);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(63, 15);
            label1.TabIndex = 1;
            label1.Text = "Alignment";
            // 
            // cbbEntityAlignment
            // 
            cbbEntityAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cbbEntityAlignment.FormattingEnabled = true;
            cbbEntityAlignment.Location = new System.Drawing.Point(138, 65);
            cbbEntityAlignment.Name = "cbbEntityAlignment";
            cbbEntityAlignment.Size = new System.Drawing.Size(304, 23);
            cbbEntityAlignment.TabIndex = 0;
            // 
            // EntityCreator
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(918, 747);
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
            groupGraphics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAnims).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numEntityAnimationSpeed).EndInit();
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
        private System.Windows.Forms.TextBox tbMetadataPath;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbEntityAlignment;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.NumericUpDown numEntityAnimationSpeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbbEntities;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btRemoveEntity;
        private System.Windows.Forms.Button btNewEntity;
        private System.Windows.Forms.TextBox tbEntityName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btEntityValidateRenaming;
        private System.Windows.Forms.CheckBox cbEntityCanCollect;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dgvAnims;
    }
}