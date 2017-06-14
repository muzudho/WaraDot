using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
                Form1 form1 = ((Form1)ParentForm);
                Graphics g = e.Graphics;
                // なるべくぼやけない補完方法
                g.InterpolationMode = InterpolationMode.NearestNeighbor;

                // 画像を、パネルに写すぜ☆（＾～＾）
                // 上の方には　ツールバーが被っているので、少し下の方に表示するぜ☆（＾～＾）
                g.DrawImage(form1.bitmap,
                    new RectangleF(10f, 50f, (float)(form1.config.scale * form1.config.width), (float)(form1.config.scale * form1.config.height))
                    //new RectangleF(0f, 0f, (float)form1.config.width, (float)form1.config.height)
                    );
            }
        }
    }
}
