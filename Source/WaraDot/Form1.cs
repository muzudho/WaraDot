using NLua;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace WaraDot
{
    public partial class Form1 : Form
    {
        static Form1()
        {
            rand = new Random(Environment.TickCount);
        }

        /// <summary>
        /// 乱数
        /// </summary>
        public static Random rand;

        /// <summary>
        /// 出力画像ファイル名
        /// </summary>
        public const string IMAGE_FILE = "Work/WaraDot.png";

        /// <summary>
        /// Luaファイル名
        /// </summary>
        public const string LUA_FILE = "Work/WaraDot.lua";

        /// <summary>
        /// 描画中の画像
        /// </summary>
        public Bitmap bitmap;

        /// <summary>
        /// Luaスクリプトを使う準備。
        /// </summary>
        public static Lua lua;
        /// <summary>
        /// 設定
        /// </summary>
        public Config config;

        #region 保存フラグ
        /// <summary>
        /// 編集した内容を、まだ保存していないなら真
        /// </summary>
        public bool Editing
        {
            get
            {
                return editing;
            }
            set
            {
                editing = value;

                TopControl topControl1 = (TopControl)topPanel.Controls["topControl1"];
                topControl1.SyncEditing(editing);
            }
        }
        bool editing;
        #endregion

        #region ペン色、ツールボックス
        public Color Color
        {
            get
            {
                TopControl topControl1 = (TopControl)topPanel.Controls["topControl1"];
                return topControl1.GetColor();
            }
            set
            {
                TopControl topControl1 = (TopControl)topPanel.Controls["topControl1"];
                topControl1.SyncColor(value);
            }
        }

        public Tools GetTools()
        {
            TopControl topControl1 = (TopControl)topPanel.Controls["topControl1"];
            return topControl1.tools;
        }
        #endregion

        public void RefreshCanvas()
        {
            centerControl1.RefreshCanvas();
        }

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 指定したファイルをロックせずに、System.Drawing.Imageを作成する。
        /// 出典: 「表示中の画像ファイルが削除できない問題の解決法」http://dobon.net/vb/dotnet/graphics/drawpicture2.html
        /// </summary>
        /// <param name="filename">作成元のファイルのパス</param>
        /// <returns>作成したSystem.Drawing.Image。</returns>
        public static System.Drawing.Image CreateImage(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(
                filename,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
            fs.Close();
            return img;
        }

        public void ReloadConfig()
        {
            config = Config.ReloadLua(this);
            centerControl1.OnReloadConfig();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region Luaの初期設定
            lua = new Lua();
            // 初期化
            lua.LoadCLRPackage();

            ReloadConfig();

            #endregion

            #region 画像の読み込み、または新規作成
            if (File.Exists(IMAGE_FILE))
            {
                //// 画像をそのまま読込むと、形式が分からないので、Bitmapインスタンスに移し替える。
                //// 出典: 「簡単な画像処理と読み込み・保存（C#）」 http://qiita.com/Toshi332/items/2749690489730f32e63d
                bitmap = new Bitmap(CreateImage(IMAGE_FILE));
            }
            else
            {
                bitmap = new Bitmap(config.width, config.height);
            }
            #endregion

            #region 保存フラグ
            // 初回のテキストボックスの内容変更は、未保存とは扱わない
            Editing = false;
            topControl1.SyncEditing(Editing);
            #endregion

        }

        /// <summary>
        /// [Ctrl]キーを押しているなら真
        /// </summary>
        public bool pressingCtrl;
        /// <summary>
        /// マウスの左ボタンを押しているなら真
        /// </summary>
        public bool pressingMouseLeft;

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Modifiers & Keys.Control) == Keys.Control)
            {
                if ((e.KeyData & Keys.S) == Keys.S)
                {
                    TopControl topUserControl1 = (TopControl)topPanel.Controls["topControl1"];
                    topUserControl1.Save();

                    // ビープ音を鳴らないようにする
                    e.SuppressKeyPress = true;
                }

                pressingCtrl = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                pressingCtrl = false;
            }
        }

        #region 描画
        public Point ToImage(int mouseX, int mouseY)
        {
            return centerControl1.ToImage(mouseX, mouseY, this);
        }

        ///// <summary>
        ///// 点を打ちます
        ///// 出典:「線を描く」http://dobon.net/vb/dotnet/graphics/drawline.html
        ///// </summary>
        ///// <param name="mouseX"></param>
        ///// <param name="mouseY"></param>
        ///// <param name="form1"></param>
        //public void DrawDotByMouse(int mouseX, int mouseY, ref bool drawed)
        //{
        //    centerControl1.DrawDotByMouse(mouseX, mouseY, this, ref drawed);
        //}
        /// <summary>
        /// 点を打ちます
        /// 出典:「線を描く」http://dobon.net/vb/dotnet/graphics/drawline.html
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        /// <param name="form1"></param>
        public void DrawDotByImage(int imgX, int imgY, ref bool drawed)
        {
            centerControl1.DrawDotByImage(imgX, imgY, this, ref drawed);
        }
        #endregion

    }
}
