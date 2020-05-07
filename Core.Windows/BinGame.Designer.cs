namespace Core.Windows
{
    partial class BinGame
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
            this.settinggroup = new System.Windows.Forms.GroupBox();
            this.groupbwrow = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // settinggroup
            // 
            this.settinggroup.Dock = System.Windows.Forms.DockStyle.Left;
            this.settinggroup.Location = new System.Drawing.Point(0, 0);
            this.settinggroup.Name = "settinggroup";
            this.settinggroup.Size = new System.Drawing.Size(362, 730);
            this.settinggroup.TabIndex = 0;
            this.settinggroup.TabStop = false;
            this.settinggroup.Text = "设置";
            // 
            // groupbwrow
            // 
            this.groupbwrow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupbwrow.Location = new System.Drawing.Point(362, 0);
            this.groupbwrow.Name = "groupbwrow";
            this.groupbwrow.Size = new System.Drawing.Size(839, 730);
            this.groupbwrow.TabIndex = 1;
            this.groupbwrow.TabStop = false;
            this.groupbwrow.Text = "chrome";
            // 
            // BinGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1201, 730);
            this.Controls.Add(this.groupbwrow);
            this.Controls.Add(this.settinggroup);
            this.Name = "BinGame";
            this.Text = "BinGame";
            this.Load += new System.EventHandler(this.BinGame_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox settinggroup;
        private System.Windows.Forms.GroupBox groupbwrow;
    }
}