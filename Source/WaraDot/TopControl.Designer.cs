﻿namespace WaraDot
{
    partial class TopControl
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.saveButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.noiseButton = new System.Windows.Forms.Button();
            this.reloadConfigButton = new System.Windows.Forms.Button();
            this.colorButton = new System.Windows.Forms.Button();
            this.colorTextBox = new System.Windows.Forms.TextBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(3, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "保存";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.saveButton);
            this.flowLayoutPanel1.Controls.Add(this.noiseButton);
            this.flowLayoutPanel1.Controls.Add(this.reloadConfigButton);
            this.flowLayoutPanel1.Controls.Add(this.colorButton);
            this.flowLayoutPanel1.Controls.Add(this.colorTextBox);
            this.flowLayoutPanel1.Controls.Add(this.clearButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(460, 150);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // noiseButton
            // 
            this.noiseButton.Location = new System.Drawing.Point(84, 3);
            this.noiseButton.Name = "noiseButton";
            this.noiseButton.Size = new System.Drawing.Size(75, 23);
            this.noiseButton.TabIndex = 1;
            this.noiseButton.Text = "ノイズ";
            this.noiseButton.UseVisualStyleBackColor = true;
            this.noiseButton.Click += new System.EventHandler(this.NoiseButton_Click);
            // 
            // reloadConfigButton
            // 
            this.reloadConfigButton.Location = new System.Drawing.Point(165, 3);
            this.reloadConfigButton.Name = "reloadConfigButton";
            this.reloadConfigButton.Size = new System.Drawing.Size(75, 23);
            this.reloadConfigButton.TabIndex = 2;
            this.reloadConfigButton.Text = "設定再読込";
            this.reloadConfigButton.UseVisualStyleBackColor = true;
            this.reloadConfigButton.Click += new System.EventHandler(this.ReloadConfigButton_Click);
            // 
            // colorButton
            // 
            this.colorButton.Location = new System.Drawing.Point(246, 3);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(23, 23);
            this.colorButton.TabIndex = 3;
            this.colorButton.UseVisualStyleBackColor = true;
            // 
            // colorTextBox
            // 
            this.colorTextBox.Location = new System.Drawing.Point(275, 3);
            this.colorTextBox.Name = "colorTextBox";
            this.colorTextBox.Size = new System.Drawing.Size(59, 19);
            this.colorTextBox.TabIndex = 4;
            this.colorTextBox.TextChanged += new System.EventHandler(this.colorTextBox_TextChanged);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(340, 3);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(44, 23);
            this.clearButton.TabIndex = 5;
            this.clearButton.Text = "白紙";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // TopControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "TopControl";
            this.Size = new System.Drawing.Size(460, 150);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button noiseButton;
        private System.Windows.Forms.Button reloadConfigButton;
        private System.Windows.Forms.Button colorButton;
        private System.Windows.Forms.TextBox colorTextBox;
        private System.Windows.Forms.Button clearButton;
    }
}
