namespace Project8.Editor
{
    partial class DrawBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            render = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)render).BeginInit();
            SuspendLayout();
            // 
            // render
            // 
            render.Dock = System.Windows.Forms.DockStyle.Fill;
            render.Location = new System.Drawing.Point(0, 0);
            render.Name = "render";
            render.Size = new System.Drawing.Size(160, 160);
            render.TabIndex = 0;
            render.TabStop = false;
            render.MouseDown += DrawBox_MouseDown;
            render.MouseLeave += DrawBox_MouseLeave;
            render.MouseMove += DrawBox_MouseMove;
            render.MouseUp += DrawBox_MouseUp;
            // 
            // DrawBox
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(render);
            Name = "DrawBox";
            Size = new System.Drawing.Size(160, 160);
            ((System.ComponentModel.ISupportInitialize)render).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox render;
    }
}
