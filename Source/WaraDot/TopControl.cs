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

        /// <summary>
        /// [保存]ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            // 自分で画像ファイルを開いているので、ロックがかかっていて保存に失敗することがある。
            ((Form1)ParentForm).bitmap.Save(Form1.IMAGE_FILE);
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
    }
}
