using System;
using System.Drawing;
using System.Windows.Forms;

namespace WaraDot
{
    public partial class TopControl : UserControl
    {
        public TopControl()
        {
            InitializeComponent();
        }

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
            // 自分で画像ファイルを開いているので、ロックがかかっていて保存に失敗することがある。
            ((Form1)ParentForm).bitmap.Save(Form1.IMAGE_FILE);

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
            Bitmap img = ((Form1)ParentForm).bitmap;
            int r, g, b;

            // 全ピクセルにランダムに色を置いていくぜ☆（＾～＾）
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    r = Form1.rand.Next(256);
                    g = Form1.rand.Next(256);
                    b = Form1.rand.Next(256);
                    img.SetPixel(x, y, Color.FromArgb(r,g,b));
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

        private void colorTextBox_TextChanged(object sender, EventArgs e)
        {
            // 参考:「HTMLカラーの色名表記と16進表記を相互に変換するには？」http://www.atmarkit.co.jp/fdotnet/dotnettips/239colorconv/colorconv.html
            try
            {
                colorButton.BackColor = ColorTranslator.FromHtml(colorTextBox.Text);
            }catch(Exception)
            {
                // 書式エラーなどは無視
            }
        }

        /// <summary>
        /// [白紙]ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearButton_Click(object sender, EventArgs e)
        {
            Bitmap img = ((Form1)ParentForm).bitmap;

            // 全ピクセルにランダムに色を置いていくぜ☆（＾～＾）
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    img.SetPixel(x, y, Color.White);
                }
            }

            ((Form1)ParentForm).RefreshCanvas();
        }
    }
}
