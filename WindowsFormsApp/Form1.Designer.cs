namespace WindowsFormsApp
{
    partial class Form1
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
            this.txtInput = new System.Windows.Forms.RichTextBox();
            this.txtoutput = new System.Windows.Forms.RichTextBox();
            this.btnexec = new System.Windows.Forms.Button();
            this.txtsnippet = new System.Windows.Forms.TextBox();
            this.btnsql = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(47, 12);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(1060, 262);
            this.txtInput.TabIndex = 0;
            this.txtInput.Text = "";
            // 
            // txtoutput
            // 
            this.txtoutput.Location = new System.Drawing.Point(47, 362);
            this.txtoutput.Name = "txtoutput";
            this.txtoutput.Size = new System.Drawing.Size(1149, 317);
            this.txtoutput.TabIndex = 1;
            this.txtoutput.Text = "";
            // 
            // btnexec
            // 
            this.btnexec.Location = new System.Drawing.Point(868, 316);
            this.btnexec.Name = "btnexec";
            this.btnexec.Size = new System.Drawing.Size(136, 23);
            this.btnexec.TabIndex = 2;
            this.btnexec.Text = "生成Param参数";
            this.btnexec.UseVisualStyleBackColor = true;
            this.btnexec.Click += new System.EventHandler(this.btnexec_Click);
            // 
            // txtsnippet
            // 
            this.txtsnippet.Location = new System.Drawing.Point(732, 280);
            this.txtsnippet.Name = "txtsnippet";
            this.txtsnippet.Size = new System.Drawing.Size(375, 21);
            this.txtsnippet.TabIndex = 3;
            this.txtsnippet.Text = " model.@ColumnName.ToStr()";
            // 
            // btnsql
            // 
            this.btnsql.Location = new System.Drawing.Point(143, 296);
            this.btnsql.Name = "btnsql";
            this.btnsql.Size = new System.Drawing.Size(136, 23);
            this.btnsql.TabIndex = 4;
            this.btnsql.Text = "分析SQL";
            this.btnsql.UseVisualStyleBackColor = true;
            this.btnsql.Click += new System.EventHandler(this.btnsql_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1251, 716);
            this.Controls.Add(this.btnsql);
            this.Controls.Add(this.txtsnippet);
            this.Controls.Add(this.btnexec);
            this.Controls.Add(this.txtoutput);
            this.Controls.Add(this.txtInput);
            this.Name = "Form1";
            this.Text = "FormDemo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtInput;
        private System.Windows.Forms.RichTextBox txtoutput;
        private System.Windows.Forms.Button btnexec;
        private System.Windows.Forms.TextBox txtsnippet;
        private System.Windows.Forms.Button btnsql;
    }
}

