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

        public void OnReloadConfig()
        {
            Form1 form1 = ((Form1)ParentForm);
            pointerPenWidth = 3f;

            if(null!= form1.config)
            {
                pointerRect = new Rectangle(
                    form1.config.ox,
                    form1.config.oy,
                    (int)(form1.config.scale + pointerPenWidth / 2),
                    (int)(form1.config.scale + pointerPenWidth / 2)
                    );
                pointerPen = new Pen(Color.Orange, pointerPenWidth);
                target = new RectangleF(form1.config.ox, form1.config.oy, (float)(form1.config.scale * form1.config.width), (float)(form1.config.scale * form1.config.height));
            }
            else
            {
                pointerRect = new Rectangle(0, 0, 100, 100);
                pointerPen = new Pen(Color.Black, pointerPenWidth);
                target = new RectangleF(100, 100, 100, 100);
            }
        }

        public void RefreshCanvas()
        {
            Refresh();
        }

        /// <summary>
        /// ペンが指しているピクセル
        /// </summary>
        Rectangle pointerRect;
        Pen pointerPen;
        float pointerPenWidth;

        /// <summary>
        /// 描画先の矩形
        /// </summary>
        RectangleF target;

        private void CenterControl_Load(object sender, EventArgs e)
        {
            if (ParentForm is Form1 form1) // ビジュアル・エディターの初期化では、Form1 ではないぜ☆（＾～＾）
            {
                OnReloadConfig();
            }
        }

        private void CenterControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (ParentForm is Form1 form1) // ビジュアル・エディターの初期化では、Form1 ではないぜ☆（＾～＾）
            {
                int pointerCellX = (int)((int)((e.X - form1.config.ox) / form1.config.scale) * form1.config.scale + form1.config.ox);
                int pointerCellY = (int)((int)((e.Y - form1.config.oy) / form1.config.scale) * form1.config.scale + form1.config.oy);

                if (pointerCellX != pointerRect.X || pointerCellY != pointerRect.Y)
                {
                    // リペイント回数が多いとちらつくので、枠を再描画する必要のあるときだけリペイントする
                    pointerRect.X = (int)(pointerCellX);
                    pointerRect.Y = (int)(pointerCellY);
                    RefreshCanvas();
                }
            }
        }

        private void CenterControl_Paint(object sender, PaintEventArgs e)
        {
            if (ParentForm is Form1 form1) // ビジュアル・エディターの初期化では、Form1 ではないぜ☆（＾～＾）
            {
                Graphics g = e.Graphics;
                // なるべくぼやけない補完方法
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                // 左上の１行目、１列目が　半ドット　になって、右下に半ドット背景画像が描かれてしまうのを防ぐ☆（＞＿＜）
                // 出典: 「C# に挑戦 」http://d.hatena.ne.jp/umonist/20060902/p1
                g.PixelOffsetMode = PixelOffsetMode.Half;

                // 画像を、パネルに写すぜ☆（＾～＾）
                // 上の方には　ツールバーが被っているので、少し下の方に表示しろだぜ☆（＾～＾）
                g.DrawImage(form1.bitmap, target);

                // 指しているピクセルを枠で囲む。等倍のとき邪魔だが……☆（＾～＾）
                g.DrawRectangle(pointerPen, pointerRect);
            }
        }
    }
}
