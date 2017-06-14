namespace WaraDot
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.centerPanel1 = new System.Windows.Forms.Panel();
            this.centerControl1 = new WaraDot.CenterControl();
            this.topPanel = new System.Windows.Forms.Panel();
            this.topControl1 = new WaraDot.TopControl();
            this.centerPanel1.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // centerPanel1
            // 
            this.centerPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.centerPanel1.Controls.Add(this.centerControl1);
            this.centerPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centerPanel1.Location = new System.Drawing.Point(0, 0);
            this.centerPanel1.Name = "centerPanel1";
            this.centerPanel1.Size = new System.Drawing.Size(591, 469);
            this.centerPanel1.TabIndex = 0;
            // 
            // centerControl1
            // 
            this.centerControl1.BackColor = System.Drawing.SystemColors.Control;
            this.centerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centerControl1.Location = new System.Drawing.Point(0, 0);
            this.centerControl1.Name = "centerControl1";
            this.centerControl1.Size = new System.Drawing.Size(591, 469);
            this.centerControl1.TabIndex = 0;
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.SystemColors.Control;
            this.topPanel.Controls.Add(this.topControl1);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(591, 38);
            this.topPanel.TabIndex = 0;
            // 
            // topControl1
            // 
            this.topControl1.BackColor = System.Drawing.SystemColors.Control;
            this.topControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topControl1.Location = new System.Drawing.Point(0, 0);
            this.topControl1.Name = "topControl1";
            this.topControl1.Size = new System.Drawing.Size(591, 38);
            this.topControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 469);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.centerPanel1);
            this.Name = "Form1";
            this.Text = "わらドット";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.centerPanel1.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel centerPanel1;
        private System.Windows.Forms.Panel topPanel;
        private TopControl topControl1;
        private CenterControl centerControl1;
    }
}

