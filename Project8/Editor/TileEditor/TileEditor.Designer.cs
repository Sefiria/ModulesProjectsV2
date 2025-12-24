namespace Project8.Editor.TileEditor
{
    partial class TileEditor
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
            DGV = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)DGV).BeginInit();
            SuspendLayout();
            // 
            // DGV
            // 
            DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            DGV.Location = new System.Drawing.Point(0, 0);
            DGV.Margin = new System.Windows.Forms.Padding(4);
            DGV.Name = "DGV";
            DGV.Size = new System.Drawing.Size(772, 513);
            DGV.TabIndex = 0;
            // 
            // TileEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(772, 513);
            Controls.Add(DGV);
            Font = new System.Drawing.Font("Segoe UI", 12F);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "TileEditor";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "TileEditor";
            FormClosing += Form_FormClosing;
            Load += Form_Load;
            ((System.ComponentModel.ISupportInitialize)DGV).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView DGV;
    }
}