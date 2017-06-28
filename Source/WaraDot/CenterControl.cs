using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WaraDot
{
    public partial class CenterControl : UserControl
    {
        public CenterControl()
        {
            InitializeComponent();

            // マウス・ホイールイベントの追加
            MouseWheel += new MouseEventHandler(this.CenterControl_MouseWheel);
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
            previousMouse = new Point();
        }

        public void RefreshCanvas()
        {
            Refresh();
        }

        /// <summary>
        /// 前のマウスカーソル位置
        /// </summary>
        Point previousMouse;

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
        public Point ToImage(int mouseX, int mouseY, Form1 form1)
        {
            workImageLocation.X = (int)((mouseX - target.X) / form1.config.scale);
            workImageLocation.Y = (int)((mouseY - target.Y) / form1.config.scale);
            return workImageLocation;
        }

        /// <summary>
        /// 使いまわす変数
        /// </summary>
        Point workWindowLocation;
        Point ToWindow(int x, int y, Form1 form1)
        {
            workWindowLocation.X = (int)(x * form1.config.scale + target.X);
            workWindowLocation.Y = (int)(y * form1.config.scale + target.Y);
            return workWindowLocation;
        }

        bool InImage(Point imgPt, Bitmap bitmap)
        {
            return InImage(imgPt.X, imgPt.Y, bitmap);
        }
        bool InImage(int x, int y, Bitmap bitmap)
        {
            return -1 < x && x < bitmap.Width && -1 < y && y < bitmap.Height;
        }

        private void CenterControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ParentForm is Form1 form1) // ビジュアル・エディターの初期化では、Form1 ではないぜ☆（＾～＾）
            {
                // 上に転がせば 3、下に転がせば -3 ぐらい。
                float volume = e.Delta * SystemInformation.MouseWheelScrollLines / 120;

                if (0 < volume)
                {
                    form1.config.scale += 1;
                }
                else
                {
                    form1.config.scale -= 1;
                }

                target.Width = (float)(form1.config.scale * form1.config.width);
                target.Height = (float)(form1.config.scale * form1.config.height);

                pointerRect.Width = (int)(form1.config.scale + pointerPenWidth / 2);
                pointerRect.Height = (int)(form1.config.scale + pointerPenWidth / 2);

                // いったん画像の座標に変える
                Point pt = ToImage(e.X, e.Y, form1);
                // 画像のサイズ内を指しているかチェック
                if (InImage(pt, form1.config.GetDrawingLayerBitmap()))
                {
                    // 画面の座標に戻す
                    pt = ToWindow(pt.X, pt.Y, form1);

                    pointerRect.X = pt.X;
                    pointerRect.Y = pt.Y;
                }

                RefreshCanvas();
            }
        }

        /// <summary>
        /// 点を打ちます
        /// 出典:「線を描く」http://dobon.net/vb/dotnet/graphics/drawline.html
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        /// <param name="form1"></param>
        public void DrawDotByMouse(int mouseX, int mouseY, Form1 form1, ref bool drawed)
        {
            #region 色塗り
            // いったん画像の座標に変える
            Point imgPt = ToImage(mouseX, mouseY, form1);

            // 画像のサイズ内を指しているかチェック
            if (InImage(imgPt, form1.config.GetDrawingLayerBitmap()))
            {
                // 指定色打ち
                form1.config.GetDrawingLayerBitmap().SetPixel(imgPt.X, imgPt.Y, form1.Color);

                #region 保存フラグ
                ((Form1)ParentForm).Editing = true;
                #endregion
                drawed = true;
            }
            #endregion
        }
        /// <summary>
        /// 点を打ちます
        /// 出典:「線を描く」http://dobon.net/vb/dotnet/graphics/drawline.html
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        /// <param name="form1"></param>
        public void DrawDotByImage(int imgX, int imgY, Form1 form1, ref bool drawed)
        {
            // 画像のサイズ内を指しているかチェック
            if (InImage(imgX, imgY, form1.config.GetDrawingLayerBitmap()))
            {
                // 指定色打ち
                form1.config.GetDrawingLayerBitmap().SetPixel(imgX, imgY, form1.Color);

                #region 保存フラグ
                ((Form1)ParentForm).Editing = true;
                #endregion
                drawed = true;
            }
        }

        /// <summary>
        /// 線を引きます
        /// 出典:「線を描く」http://dobon.net/vb/dotnet/graphics/drawline.html
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        /// <param name="form1"></param>
        void DrawLine(int mouseX, int mouseY, Form1 form1)
        {
            #region 色塗り
            // いったん画像の座標に変える
            Point imgPt1 = ToImage(previousMouse.X, previousMouse.Y, form1);
            Point imgPt2 = ToImage(mouseX, mouseY, form1);

            Graphics g = Graphics.FromImage(form1.config.GetDrawingLayerBitmap());
            Pen pen = new Pen(form1.Color);
            g.DrawLine(pen, imgPt1, imgPt2);
            g.Dispose();

            #region 保存フラグ
            ((Form1)ParentForm).Editing = true;
            #endregion
            #endregion
        }

        /// <summary>
        /// 線状に消します
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        /// <param name="form1"></param>
        void EraseLine(int mouseX, int mouseY, Form1 form1)
        {
            #region 消しゴム
            Bitmap bitmap = form1.config.GetDrawingLayerBitmap();

            // いったん画像の座標に変える
            Point imgPt1 = ToImage(previousMouse.X, previousMouse.Y, form1);
            Point imgPt2 = ToImage(mouseX, mouseY, form1);

            // ベクトル
            int width = imgPt2.X - imgPt1.X;
            int height = imgPt2.Y - imgPt1.Y;
            // 辺が長い方をメインに考える
            if (Math.Abs(height) < Math.Abs(width))
            {
                // 傾き(<1)
                if (0 != width)//0除算防止
                {
                    float katamuki = (float)height / (float)width;

                    // xを走査
                    for (int x = 0; x < Math.Abs(width); x++)
                    {
                        // yを算出
                        int y;
                        if (-1 < width)
                        {
                            y = (int)(imgPt1.Y + katamuki * x);
                        }
                        else
                        {
                            y = (int)(imgPt2.Y + katamuki * x);
                        }

                        // 色を除去
                        int dstX = x + Math.Min(imgPt1.X, imgPt2.X);
                        bitmap.SetPixel(dstX, y, Color.Transparent);
                        #region 保存フラグ
                        ((Form1)ParentForm).Editing = true;
                        #endregion
                    }
                }
            }
            else
            {

                // 傾き(<1)
                if (0 != height)//0除算防止
                {
                    float katamuki = (float)width / (float)height;

                    // yを走査
                    for (int y = 0; y < Math.Abs(height); y++)
                    {
                        // xを算出
                        int x;
                        if (-1 < height)
                        {
                            x = (int)(imgPt1.X + katamuki * y);
                        }
                        else
                        {
                            x = (int)(imgPt2.X + katamuki * y);
                        }

                        // 色を除去
                        int dstY = y + Math.Min(imgPt1.Y, imgPt2.Y);
                        bitmap.SetPixel(x, dstY, Color.Transparent);
                        #region 保存フラグ
                        ((Form1)ParentForm).Editing = true;
                        #endregion
                    }
                }
            }

            #endregion
        }

        private void CenterControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (ParentForm is Form1 form1) // ビジュアル・エディターの初期化では、Form1 ではないぜ☆（＾～＾）
            {
                bool refresh = false;
                bool pixelMove = false;

                #region ポインター
                {
                    // いったん画像の座標に変える
                    Point pt = ToImage(e.X, e.Y, form1);

                    // 画像のサイズ内を指しているかチェック
                    if (InImage(pt, form1.config.GetDrawingLayerBitmap()))
                    {
                        // 画面の座標に戻す
                        pt = ToWindow(pt.X, pt.Y, form1);

                        if (pt.X != pointerRect.X || pt.Y != pointerRect.Y)
                        {
                            // リペイント回数が多いとちらつくので、枠を再描画する必要のあるときだけリペイントする
                            pointerRect.X = pt.X;
                            pointerRect.Y = pt.Y;
                            refresh = true;
                            pixelMove = true;
                        }
                    }
                }
                #endregion

                #region 描画
                if (form1.pressingMouseLeft && pixelMove)
                {
                    if (form1.pressingCtrl)
                    {
                        // キャンバスずらし中かもしれない
                    }
                    else
                    {
                        // 色塗り
                        switch (form1.GetTool())
                        {
                            case Tools.Eraser:
                                {
                                    EraseLine(e.X, e.Y, form1);
                                    refresh = true;
                                }
                                break;
                            default:
                            case Tools.FreeHandLine:
                                {
                                    DrawLine(e.X, e.Y, form1);
                                    refresh = true;
                                }
                                break;
                        }
                    }
                }
                #endregion

                #region キャンバスずらし
                if (form1.pressingCtrl)
                {
                    // 差
                    int dx = e.X - previousMouse.X;
                    int dy = e.Y - previousMouse.Y;

                    target.Offset(dx, dy);
                    refresh = true;
                }
                previousMouse.X = e.X;
                previousMouse.Y = e.Y;
                #endregion

                if (refresh)
                {
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

                for (int i=1; i<form1.config.GetLayersCount(); i++)
                {
                    if (form1.config.layersVisible[i])
                    {
                        // 画像を、パネルに写すぜ☆（＾～＾）
                        // 上の方には　ツールバーが被っているので、少し下の方に表示しろだぜ☆（＾～＾）
                        g.DrawImage(form1.config.layersBitmap[i], target);
                    }
                }

                // 指しているピクセルを枠で囲む。等倍のとき邪魔だが……☆（＾～＾）
                g.DrawRectangle(pointerPen, pointerRect);
            }
        }

        private void CenterControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (ParentForm is Form1 form1) // ビジュアル・エディターの初期化では、Form1 ではないぜ☆（＾～＾）
            {
                if (MouseButtons.Left == e.Button)
                {
                    form1.pressingMouseLeft = true;

                    switch (form1.GetTool())
                    {
                        case Tools.FreeHandLine:
                            {
                                // マウスの左ボタンを押下した直後も描画したい
                                bool drawed = false;
                                DrawDotByMouse(e.X, e.Y, form1, ref drawed);
                                if (drawed) { RefreshCanvas(); }
                            }
                            break;
                        case Tools.Buckets:
                            {
                                // 塗りつぶしたい
                                Program.buckets = Buckets.Build(e.X, e.Y, form1);
                                form1.OperatorType = OperatorType.Computer;
                            }
                            break;
                    }
                }
                else if (MouseButtons.Right == e.Button)
                {
                    // スポイト
                    Point pt = ToImage(e.X, e.Y, form1);
                    form1.Color = form1.config.GetDrawingLayerBitmap().GetPixel(pt.X, pt.Y);
                }
            }
        }

        private void CenterControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (ParentForm is Form1 form1) // ビジュアル・エディターの初期化では、Form1 ではないぜ☆（＾～＾）
            {
                if (MouseButtons.Left == e.Button)
                {
                    form1.pressingMouseLeft = false;

                    // マウスの左ボタンを放した直後にも描画したい。
                    bool drawed = false;
                    DrawDotByMouse(e.X, e.Y, form1, ref drawed);
                    if (drawed) { RefreshCanvas(); }
                }
            }
        }
    }
}
