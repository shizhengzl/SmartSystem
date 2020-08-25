namespace Core.Windows
{
    partial class Generators
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Generators));
            this.tabpan = new System.Windows.Forms.TabControl();
            this.tabDatabase = new System.Windows.Forms.TabPage();
            this.panDatabase = new System.Windows.Forms.Panel();
            this.treeViewDatabase = new System.Windows.Forms.TreeView();
            this.toolStripDatabase = new System.Windows.Forms.ToolStrip();
            this.databaseAdd = new System.Windows.Forms.ToolStripButton();
            this.tabSetting = new System.Windows.Forms.TabPage();
            this.tabControls = new System.Windows.Forms.TabControl();
            this.tabpan.SuspendLayout();
            this.tabDatabase.SuspendLayout();
            this.panDatabase.SuspendLayout();
            this.toolStripDatabase.SuspendLayout();
            this.tabSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabpan
            // 
            this.tabpan.Controls.Add(this.tabDatabase);
            this.tabpan.Controls.Add(this.tabSetting);
            this.tabpan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabpan.Location = new System.Drawing.Point(0, 0);
            this.tabpan.Name = "tabpan";
            this.tabpan.SelectedIndex = 0;
            this.tabpan.Size = new System.Drawing.Size(1278, 760);
            this.tabpan.TabIndex = 0;
            // 
            // tabDatabase
            // 
            this.tabDatabase.Controls.Add(this.panDatabase);
            this.tabDatabase.Location = new System.Drawing.Point(4, 22);
            this.tabDatabase.Name = "tabDatabase";
            this.tabDatabase.Padding = new System.Windows.Forms.Padding(3);
            this.tabDatabase.Size = new System.Drawing.Size(1270, 734);
            this.tabDatabase.TabIndex = 0;
            this.tabDatabase.Text = "数据库设置";
            this.tabDatabase.UseVisualStyleBackColor = true;
            // 
            // panDatabase
            // 
            this.panDatabase.Controls.Add(this.treeViewDatabase);
            this.panDatabase.Controls.Add(this.toolStripDatabase);
            this.panDatabase.Dock = System.Windows.Forms.DockStyle.Left;
            this.panDatabase.Location = new System.Drawing.Point(3, 3);
            this.panDatabase.Name = "panDatabase";
            this.panDatabase.Size = new System.Drawing.Size(374, 728);
            this.panDatabase.TabIndex = 0;
            // 
            // treeViewDatabase
            // 
            this.treeViewDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewDatabase.Location = new System.Drawing.Point(0, 25);
            this.treeViewDatabase.Name = "treeViewDatabase";
            this.treeViewDatabase.Size = new System.Drawing.Size(374, 703);
            this.treeViewDatabase.TabIndex = 1;
            // 
            // toolStripDatabase
            // 
            this.toolStripDatabase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.databaseAdd});
            this.toolStripDatabase.Location = new System.Drawing.Point(0, 0);
            this.toolStripDatabase.Name = "toolStripDatabase";
            this.toolStripDatabase.Size = new System.Drawing.Size(374, 25);
            this.toolStripDatabase.TabIndex = 0;
            this.toolStripDatabase.Text = "toolStrip1";
            // 
            // databaseAdd
            // 
            this.databaseAdd.Image = ((System.Drawing.Image)(resources.GetObject("databaseAdd.Image")));
            this.databaseAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.databaseAdd.Name = "databaseAdd";
            this.databaseAdd.Size = new System.Drawing.Size(76, 22);
            this.databaseAdd.Text = "新增链接";
            // 
            // tabSetting
            // 
            this.tabSetting.Controls.Add(this.tabControls);
            this.tabSetting.Location = new System.Drawing.Point(4, 22);
            this.tabSetting.Name = "tabSetting";
            this.tabSetting.Padding = new System.Windows.Forms.Padding(3);
            this.tabSetting.Size = new System.Drawing.Size(1270, 734);
            this.tabSetting.TabIndex = 1;
            this.tabSetting.Text = "系统设置";
            this.tabSetting.UseVisualStyleBackColor = true;
            // 
            // tabControls
            // 
            this.tabControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControls.Location = new System.Drawing.Point(3, 3);
            this.tabControls.Name = "tabControls";
            this.tabControls.SelectedIndex = 0;
            this.tabControls.Size = new System.Drawing.Size(1264, 728);
            this.tabControls.TabIndex = 0;
            // 
            // Generators
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1278, 760);
            this.Controls.Add(this.tabpan);
            this.Name = "Generators";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "代码生成器";
            this.tabpan.ResumeLayout(false);
            this.tabDatabase.ResumeLayout(false);
            this.panDatabase.ResumeLayout(false);
            this.panDatabase.PerformLayout();
            this.toolStripDatabase.ResumeLayout(false);
            this.toolStripDatabase.PerformLayout();
            this.tabSetting.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabpan;
        private System.Windows.Forms.TabPage tabDatabase;
        private System.Windows.Forms.Panel panDatabase;
        private System.Windows.Forms.TabPage tabSetting;
        private System.Windows.Forms.ToolStrip toolStripDatabase;
        private System.Windows.Forms.ToolStripButton databaseAdd;
        private System.Windows.Forms.TreeView treeViewDatabase;
        private System.Windows.Forms.TabControl tabControls;
    }
}

