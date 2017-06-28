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
            this.components = new System.ComponentModel.Container();
            this.centerControl1 = new WaraDot.CenterControl();
            this.topPanel = new System.Windows.Forms.Panel();
            this.topControl1 = new WaraDot.TopControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.アルゴリズムToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ドットイーターToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topPanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // centerControl1
            // 
            this.centerControl1.BackColor = System.Drawing.SystemColors.Control;
            this.centerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centerControl1.Location = new System.Drawing.Point(0, 62);
            this.centerControl1.Name = "centerControl1";
            this.centerControl1.Size = new System.Drawing.Size(591, 407);
            this.centerControl1.TabIndex = 0;
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.SystemColors.Control;
            this.topPanel.Controls.Add(this.topControl1);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 24);
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
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.アルゴリズムToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(591, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // アルゴリズムToolStripMenuItem
            // 
            this.アルゴリズムToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ドットイーターToolStripMenuItem});
            this.アルゴリズムToolStripMenuItem.Name = "アルゴリズムToolStripMenuItem";
            this.アルゴリズムToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.アルゴリズムToolStripMenuItem.Text = "アルゴリズム";
            // 
            // ドットイーターToolStripMenuItem
            // 
            this.ドットイーターToolStripMenuItem.Name = "ドットイーターToolStripMenuItem";
            this.ドットイーターToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ドットイーターToolStripMenuItem.Text = "1ドットイーター";
            this.ドットイーターToolStripMenuItem.Click += new System.EventHandler(this.Algorithm1DotEaterToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 469);
            this.Controls.Add(this.centerControl1);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "わらドット";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.topPanel.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel topPanel;
        private TopControl topControl1;
        private CenterControl centerControl1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem アルゴリズムToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ドットイーターToolStripMenuItem;
    }
}

