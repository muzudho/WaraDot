using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaraDot
{
    public partial class CenterControl : UserControl
    {
        public CenterControl()
        {
            InitializeComponent();
        }

        public void RefreshCanvas()
        {
            canvasPanel.Refresh();
        }

        private void canvasPanel_Paint(object sender, PaintEventArgs e)
        {
            if (ParentForm is Form1) // ビジュアル・エディターの初期化では、Form1 ではないぜ☆（＾～＾）
            {
                Graphics g = e.Graphics;

                Bitmap bitmap = ((Form1)ParentForm).bitmap;

                // 画像を、パネルに写すぜ☆（＾～＾）
                // 上の方には　ツールバーが被っているので、少し下の方に表示するぜ☆（＾～＾）
                g.DrawImage(bitmap, 10f, 50f);
            }
        }
    }
}
