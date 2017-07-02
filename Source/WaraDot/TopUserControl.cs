using System;
using System.Drawing;
using System.Windows.Forms;

namespace WaraDot
{
    public partial class TopUserControl : UserControl
    {
        public TopUserControl()
        {
            InitializeComponent();

            toolComboBox.SelectedIndex = 0;
        }

        #region ツールボックス
        private void ToolComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (toolComboBox.SelectedIndex)
            {
                case 1: tools = Tools.Buckets; break;
                case 2: tools = Tools.Eraser; break;
                case 3: tools = Tools.Selection; break;
                default://thru
                case 0: tools = Tools.FreeHandLine; break;
            }
        }
        public Tools tools;
        #endregion

        #region 操作している者
        public void SyncOperatorType(OperatorType value)
        {
            switch (value)
            {
                case OperatorType.Computer:
                    {
                        operatorTypeLabel.Text = "COM";
                        operatorTypeLabel.BackColor = Color.Red;
                    }
                    break;
                default:
                case OperatorType.Human:
                    {
                        operatorTypeLabel.Text = "人間";
                        operatorTypeLabel.BackColor = SystemColors.Control;
                    }
                    break;
            }
        }
        #endregion

        #region 保存フラグ
        public void SyncEditing(bool value)
        {
            if (value)
            {
                saveButton.BackColor = Color.LightGreen;
            }
            else
            {
                saveButton.BackColor = Color.LightGray;
            }
        }
        #endregion

        #region ペン色
        public void SyncColor(Color color)
        {
            colorButton.BackColor = color;

            // 参考:「HTMLカラーの色名表記と16進表記を相互に変換するには？」http://www.atmarkit.co.jp/fdotnet/dotnettips/239colorconv/colorconv.html
            colorTextBox.Text = ColorTranslator.ToHtml(color);
            alphaTextBox.Text = color.A.ToString();
        }
        public Color GetColor()
        {
            return colorButton.BackColor;
        }
        #endregion

        /// <summary>
        /// 画像を既定のファイルに保存します
        /// </summary>
        public void Save()
        {
            for (int iLayer = 1; iLayer < Program.config.layersBitmap.Length; iLayer++)
            {
                // 自分で画像ファイルを開いているので、ロックがかかっていて保存に失敗することがある。
                Program.config.layersBitmap[iLayer].Save(Config.GetImageFileName(iLayer));
            }

            #region 保存フラグ
            ((Form1)ParentForm).Editing = false;
            #endregion
        }

        /// <summary>
        /// [保存]ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            Save();
        }

        /// <summary>
        /// [ノイズ]ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoiseButton_Click(object sender, EventArgs e)
        {
            int r, g, b;

            // 全ピクセルにランダムに色を置いていくぜ☆（＾～＾）
            for (int y = 0; y < Program.config.LookingLayerBitmap.Height; y++)
            {
                for (int x = 0; x < Program.config.LookingLayerBitmap.Width; x++)
                {
                    r = Form1.rand.Next(256);
                    g = Form1.rand.Next(256);
                    b = Form1.rand.Next(256);
                    Program.config.DrawingLayerBitmap.SetPixel(x, y, Color.FromArgb(r,g,b));
                }
            }

            ((Form1)ParentForm).RefreshCanvas();
        }

        private void ReloadConfigButton_Click(object sender, EventArgs e)
        {
            Form1 form1 = (Form1)ParentForm;
            form1.ReloadConfig();
            form1.RefreshCanvas();
        }

        void OnColorTextChanged()
        {
            // 参考:「HTMLカラーの色名表記と16進表記を相互に変換するには？」http://www.atmarkit.co.jp/fdotnet/dotnettips/239colorconv/colorconv.html
            try
            {
                Color c = ColorTranslator.FromHtml(colorTextBox.Text);

                if (!int.TryParse(alphaTextBox.Text, out int alpha))
                {
                    alpha = 255;
                }

                colorButton.BackColor = Color.FromArgb(alpha, c.R, c.G, c.B);
            }
            catch (Exception)
            {
                // 書式エラーなどは無視
            }
        }

        private void ColorTextBox_TextChanged(object sender, EventArgs e)
        {
            OnColorTextChanged();
        }

        private void AlphaTextBox_TextChanged(object sender, EventArgs e)
        {
            OnColorTextChanged();
        }

        /// <summary>
        /// [白紙]ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearButton_Click(object sender, EventArgs e)
        {
            // 全ピクセルにランダムに色を置いていくぜ☆（＾～＾）
            for (int y = 0; y < Program.config.LookingLayerBitmap.Height; y++)
            {
                for (int x = 0; x < Program.config.LookingLayerBitmap.Width; x++)
                {
                    Program.config.DrawingLayerBitmap.SetPixel(x, y, Color.White);
                }
            }

            ((Form1)ParentForm).RefreshCanvas();
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            Form1 form1 = ((Form1)ParentForm);
            // ランダム色打ち
            form1.RandomColor();
        }
    }
}
