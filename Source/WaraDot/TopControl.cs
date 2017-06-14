using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

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
        private void saveButton_Click(object sender, EventArgs e)
        {
            // 自分で画像ファイルを開いているので、ロックがかかっていて保存に失敗することがある。
            ((Form1)ParentForm).bitmap.Save(Form1.IMAGE_FILE);
        }

        /// <summary>
        /// [ノイズ]ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void noiseButton_Click(object sender, EventArgs e)
        {
            Bitmap img = ((Form1)ParentForm).bitmap;
            int r, g, b;
            Random rand = new Random(Environment.TickCount);

            // 全ピクセルにランダムに色を置いていくぜ☆（＾～＾）
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    r = rand.Next(256);
                    g = rand.Next(256);
                    b = rand.Next(256);
                    img.SetPixel(x, y, Color.FromArgb(r,g,b));
                }
            }

            ((Form1)ParentForm).RefreshCanvas();
        }
    }
}
