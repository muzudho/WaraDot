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
        /// Luaファイル名
        /// </summary>
        public const string LUA_FILE = "Work/WaraDot.lua";

        /// <summary>
        /// Luaスクリプトを使う準備。
        /// </summary>
        public static Lua lua;
        /// <summary>
        /// 設定
        /// </summary>
        public Config config;

        #region 操作している者
        /// <summary>
        /// 操作している者
        /// </summary>
        public OperatorType OperatorType
        {
            get
            {
                return operatorType;
            }
            set
            {
                operatorType = value;

                TopControl topControl1 = (TopControl)topPanel.Controls["topControl1"];
                topControl1.SyncOperatorType(value);
            }
        }
        OperatorType operatorType;
        #endregion

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

        public Tools GetTool()
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
        /// 設定再読込
        /// </summary>
        public void ReloadConfig()
        {
            config = Config.ReloadLua(this);
            config.ReloadLayerImages();
            centerControl1.OnReloadConfig();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region Luaの初期設定
            lua = new Lua();
            // 初期化
            lua.LoadCLRPackage();

            // 設定読取
            ReloadConfig();

            #endregion

            #region 画像の読み込み、または新規作成
            config.ReloadLayerImages();
            #endregion

            #region 保存フラグ
            // 初回のテキストボックスの内容変更は、未保存とは扱わない
            Editing = false;
            topControl1.SyncEditing(Editing);
            #endregion

            OperatorType = OperatorType.Human;

            // タイマーの実行
            timer1.Start();
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

        #region コンピューターの自動実行

        private void Timer1_Tick(object sender, EventArgs e)
        {
            // バケツ
            if (null!= Program.buckets)
            {
                if (Program.buckets.IsFinished())
                {
                    Program.buckets = null;
                    OperatorType = OperatorType.Human;
                }
                else
                {
                    Program.buckets.Step();
                    RefreshCanvas();
                }
            }

            // ワンドット・イーター
            if (null != Program.oneDotEater)
            {
                if (Program.oneDotEater.IsFinished())
                {
                    Program.oneDotEater = null;
                    OperatorType = OperatorType.Human;
                }
                else
                {
                    Program.oneDotEater.Step();
                    RefreshCanvas();
                }
            }
        }
        #endregion

        /// <summary>
        /// [アルゴリズム] - [1ドットイーター]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Algorithm1DotEaterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1ドットの点を消したい
            Program.oneDotEater = OneDotEater.Build(this);
            OperatorType = OperatorType.Computer;
        }
    }
}
