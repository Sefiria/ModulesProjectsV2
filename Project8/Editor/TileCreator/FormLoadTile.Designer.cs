namespace Project8.Editor.TileCreator
{
    partial class FormLoadTile
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
            imgListTiles = new System.Windows.Forms.ImageList(components);
            lv = new System.Windows.Forms.ListView();
            btSelect = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // imgListTiles
            // 
            imgListTiles.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imgListTiles.ImageSize = new System.Drawing.Size(16, 16);
            imgListTiles.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lv
            // 
            lv.Location = new System.Drawing.Point(12, 48);
            lv.Name = "lv";
            lv.Size = new System.Drawing.Size(472, 390);
            lv.TabIndex = 0;
            lv.UseCompatibleStateImageBehavior = false;
            lv.MouseDoubleClick += lv_MouseDoubleClick;
            // 
            // btSelect
            // 
            btSelect.Location = new System.Drawing.Point(183, 12);
            btSelect.Name = "btSelect";
            btSelect.Size = new System.Drawing.Size(133, 23);
            btSelect.TabIndex = 1;
            btSelect.Text = "Sélectionner";
            btSelect.UseVisualStyleBackColor = true;
            btSelect.Click += btSelect_Click;
            // 
            // FormLoadTile
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(495, 450);
            Controls.Add(btSelect);
            Controls.Add(lv);
            Name = "FormLoadTile";
            Text = "FormLoadTile";
            Load += LoadForm;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ImageList imgListTiles;
        private System.Windows.Forms.ListView lv;
        private System.Windows.Forms.Button btSelect;
    }
}