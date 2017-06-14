using NLua;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WaraDot
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 出力画像ファイル名
        /// </summary>
        public const string IMAGE_FILE = "WaraDot.png";

        /// <summary>
        /// Luaファイル名
        /// </summary>
        public const string LUA_FILE = "WaraDot.lua";

        /// <summary>
        /// 描画中の画像
        /// </summary>
        public Bitmap bitmap;

        /// <summary>
        /// Luaスクリプトを使う準備。
        /// </summary>
        private static Lua lua;


        public void RefreshCanvas()
        {
            ((CenterControl)centerPanel1.Controls["centerControl1"]).RefreshCanvas();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region Luaの設定
            lua = new Lua();
            // 初期化
            lua.LoadCLRPackage();

            // ファイルの読み込み
            lua.DoFile(LUA_FILE);

            // Lua「debugOut("あー☆")」
            // ↓
            // C#「Console.WriteLine("あー☆")」
            // lua.RegisterFunction("debugOut", typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));

            // Lua「random(0,100)」
            // ↓
            // C#「System.Random(0,100)」
            // lua.RegisterFunction("random", typeof(System).GetMethod("Random", new Type[] { typeof(float), typeof(float) }));

            // init関数実行
            // lua.GetFunction("init").Call();

            // double型 か null か、はたまた想定外の型か
            var width = lua["WIDTH"];
            if (!(width is double)) { width = 100d; }
            var height = lua["HEIGHT"];
            if (!(height is double)) { height = 100d; }

            #endregion

            bitmap = new Bitmap((int)((double)width), (int)((double)height));

        }
    }
}
