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
            this.centerControl1 = new WaraDot.CenterUserControl();
            this.topPanel = new System.Windows.Forms.Panel();
            this.topUserControl1 = new WaraDot.TopUserControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.アルゴリズムToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ドットイーターToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ブラッカイズToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ドットアベレージToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.半透明の透明化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AlgorithmNoiseCancelerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AlgorithmEraseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.選択範囲ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectionAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectionCancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.作り直し操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RemakeNoiseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RemakeWhitePaperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.topPanel.Controls.Add(this.topUserControl1);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 24);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(591, 38);
            this.topPanel.TabIndex = 0;
            // 
            // topUserControl1
            // 
            this.topUserControl1.BackColor = System.Drawing.SystemColors.Control;
            this.topUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topUserControl1.Location = new System.Drawing.Point(0, 0);
            this.topUserControl1.Name = "topUserControl1";
            this.topUserControl1.Size = new System.Drawing.Size(591, 38);
            this.topUserControl1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 16;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.アルゴリズムToolStripMenuItem,
            this.選択範囲ToolStripMenuItem,
            this.作り直し操作ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(591, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // アルゴリズムToolStripMenuItem
            // 
            this.アルゴリズムToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ドットイーターToolStripMenuItem,
            this.ブラッカイズToolStripMenuItem,
            this.ドットアベレージToolStripMenuItem,
            this.半透明の透明化ToolStripMenuItem,
            this.AlgorithmNoiseCancelerToolStripMenuItem,
            this.AlgorithmEraseAllToolStripMenuItem});
            this.アルゴリズムToolStripMenuItem.Name = "アルゴリズムToolStripMenuItem";
            this.アルゴリズムToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.アルゴリズムToolStripMenuItem.Text = "アルゴリズム";
            // 
            // ドットイーターToolStripMenuItem
            // 
            this.ドットイーターToolStripMenuItem.Name = "ドットイーターToolStripMenuItem";
            this.ドットイーターToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.ドットイーターToolStripMenuItem.Text = "1ドットイーター";
            this.ドットイーターToolStripMenuItem.Click += new System.EventHandler(this.Algorithm1DotEaterToolStripMenuItem_Click);
            // 
            // ブラッカイズToolStripMenuItem
            // 
            this.ブラッカイズToolStripMenuItem.Name = "ブラッカイズToolStripMenuItem";
            this.ブラッカイズToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.ブラッカイズToolStripMenuItem.Text = "ドット・ブラッカイズ";
            this.ブラッカイズToolStripMenuItem.Click += new System.EventHandler(this.AlgorithmBlackizeToolStripMenuItem_Click);
            // 
            // ドットアベレージToolStripMenuItem
            // 
            this.ドットアベレージToolStripMenuItem.Name = "ドットアベレージToolStripMenuItem";
            this.ドットアベレージToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.ドットアベレージToolStripMenuItem.Text = "ドット・アベレージ";
            this.ドットアベレージToolStripMenuItem.Click += new System.EventHandler(this.AlgorithmDotAverageToolStripMenuItem_Click);
            // 
            // 半透明の透明化ToolStripMenuItem
            // 
            this.半透明の透明化ToolStripMenuItem.Name = "半透明の透明化ToolStripMenuItem";
            this.半透明の透明化ToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.半透明の透明化ToolStripMenuItem.Text = "半透明の透明化";
            this.半透明の透明化ToolStripMenuItem.Click += new System.EventHandler(this.DotTransparentClearToolStripMenuItem_Click);
            // 
            // AlgorithmNoiseCancelerToolStripMenuItem
            // 
            this.AlgorithmNoiseCancelerToolStripMenuItem.Name = "AlgorithmNoiseCancelerToolStripMenuItem";
            this.AlgorithmNoiseCancelerToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.AlgorithmNoiseCancelerToolStripMenuItem.Text = "ノイズキャンセラー";
            this.AlgorithmNoiseCancelerToolStripMenuItem.Click += new System.EventHandler(this.AlgorithmNoiseCancelerToolStripMenuItem_Click);
            // 
            // AlgorithmEraseAllToolStripMenuItem
            // 
            this.AlgorithmEraseAllToolStripMenuItem.Name = "AlgorithmEraseAllToolStripMenuItem";
            this.AlgorithmEraseAllToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.AlgorithmEraseAllToolStripMenuItem.Text = "イレーズ・オール";
            this.AlgorithmEraseAllToolStripMenuItem.Click += new System.EventHandler(this.AlgorithmEraseAllToolStripMenuItem_Click);
            // 
            // 選択範囲ToolStripMenuItem
            // 
            this.選択範囲ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectionAllToolStripMenuItem,
            this.SelectionCancelToolStripMenuItem});
            this.選択範囲ToolStripMenuItem.Name = "選択範囲ToolStripMenuItem";
            this.選択範囲ToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.選択範囲ToolStripMenuItem.Text = "選択範囲";
            // 
            // SelectionAllToolStripMenuItem
            // 
            this.SelectionAllToolStripMenuItem.Name = "SelectionAllToolStripMenuItem";
            this.SelectionAllToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.SelectionAllToolStripMenuItem.Text = "全選択";
            this.SelectionAllToolStripMenuItem.Click += new System.EventHandler(this.SelectionAllToolStripMenuItem_Click);
            // 
            // SelectionCancelToolStripMenuItem
            // 
            this.SelectionCancelToolStripMenuItem.Name = "SelectionCancelToolStripMenuItem";
            this.SelectionCancelToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.SelectionCancelToolStripMenuItem.Text = "解除";
            this.SelectionCancelToolStripMenuItem.Click += new System.EventHandler(this.SelectionCancelToolStripMenuItem_Click);
            // 
            // 作り直し操作ToolStripMenuItem
            // 
            this.作り直し操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RemakeNoiseToolStripMenuItem,
            this.RemakeWhitePaperToolStripMenuItem});
            this.作り直し操作ToolStripMenuItem.Name = "作り直し操作ToolStripMenuItem";
            this.作り直し操作ToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.作り直し操作ToolStripMenuItem.Text = "作り直し操作";
            // 
            // RemakeNoiseToolStripMenuItem
            // 
            this.RemakeNoiseToolStripMenuItem.Name = "RemakeNoiseToolStripMenuItem";
            this.RemakeNoiseToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.RemakeNoiseToolStripMenuItem.Text = "ノイズ";
            this.RemakeNoiseToolStripMenuItem.Click += new System.EventHandler(this.RemakeNoiseToolStripMenuItem_Click);
            // 
            // RemakeWhitePaperToolStripMenuItem
            // 
            this.RemakeWhitePaperToolStripMenuItem.Name = "RemakeWhitePaperToolStripMenuItem";
            this.RemakeWhitePaperToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.RemakeWhitePaperToolStripMenuItem.Text = "白紙";
            this.RemakeWhitePaperToolStripMenuItem.Click += new System.EventHandler(this.RemakeWhitePaperToolStripMenuItem_Click);
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
        private TopUserControl topUserControl1;
        private CenterUserControl centerControl1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem アルゴリズムToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ドットイーターToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ブラッカイズToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ドットアベレージToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 半透明の透明化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 選択範囲ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SelectionCancelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SelectionAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AlgorithmNoiseCancelerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AlgorithmEraseAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 作り直し操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RemakeNoiseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RemakeWhitePaperToolStripMenuItem;
    }
}

