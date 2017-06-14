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

            workImageLocation = new Point();
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

        /// <summary>
        /// 使いまわす変数
        /// </summary>
        Point workImageLocation;
        Point ToImage(int x, int y, Form1 form1)
        {
            workImageLocation.X = (int)((x - form1.config.ox) / form1.config.scale);
            workImageLocation.Y = (int)((y - form1.config.oy) / form1.config.scale);
            return workImageLocation;
        }

        /// <summary>
        /// 使いまわす変数
        /// </summary>
        Point workWindowLocation;
        Point ToWindow(int x, int y, Form1 form1)
        {
            workWindowLocation.X = (int)(x * form1.config.scale + form1.config.ox);
            workWindowLocation.Y = (int)(y * form1.config.scale + form1.config.oy);
            return workWindowLocation;
        }

        bool InImage(Point pt, Bitmap bitmap)
        {
            return -1 < pt.X && pt.X < bitmap.Width && -1 < pt.Y && pt.Y < bitmap.Height;
        }

        private void CenterControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (ParentForm is Form1 form1) // ビジュアル・エディターの初期化では、Form1 ではないぜ☆（＾～＾）
            {
                // いったん画像の座標に変える
                Point pt = ToImage(e.X, e.Y, form1);

                // 画像のサイズ内を指しているかチェック
                if (InImage( pt, form1.bitmap))
                {
                    // 画面の座標に戻す
                    pt = ToWindow(pt.X, pt.Y, form1);

                    if (pt.X != pointerRect.X || pt.Y != pointerRect.Y)
                    {
                        // リペイント回数が多いとちらつくので、枠を再描画する必要のあるときだけリペイントする
                        pointerRect.X = pt.X;
                        pointerRect.Y = pt.Y;
                        RefreshCanvas();
                    }
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

        private void CenterControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (ParentForm is Form1 form1) // ビジュアル・エディターの初期化では、Form1 ではないぜ☆（＾～＾）
            {
                // いったん画像の座標に変える
                Point pt = ToImage(e.X, e.Y, form1);

                // 画像のサイズ内を指しているかチェック
                if (InImage(pt, form1.bitmap))
                {
                    int r = Form1.rand.Next(256);
                    int g = Form1.rand.Next(256);
                    int b = Form1.rand.Next(256);

                    form1.bitmap.SetPixel(pt.X, pt.Y, Color.FromArgb(r, g, b));
                    RefreshCanvas();
                }

            }
        }
    }
}
