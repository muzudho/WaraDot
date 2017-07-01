using NLua;
using System;
using System.Windows.Forms;

namespace WaraDot
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        /// <summary>
        /// Luaファイル名
        /// </summary>
        public const string LUA_FILE = "Work/WaraDot.lua";

        /// <summary>
        /// 静的コンストラクター
        /// </summary>
        static Program()
        {
            #region Luaの初期設定
            lua = new Lua();
            // 初期化
            lua.LoadCLRPackage();
            #endregion
        }
        /// <summary>
        /// Luaスクリプト・ファイルを１個しか使わないのなら、インスタンス１つで十分。
        /// </summary>
        public static Lua lua;

        /// <summary>
        /// 設定
        /// </summary>
        public static Config config;

        /// <summary>
        /// バケツ
        /// 未使用時はヌル
        /// </summary>
        public static Buckets buckets;
        /// <summary>
        /// ワンドット・イーター
        /// 未使用時はヌル
        /// </summary>
        public static OneDotEater oneDotEater;
        /// <summary>
        /// ドット・ブラッカイズ
        /// 未使用時はヌル
        /// </summary>
        public static DotBlackize dotBlackize;
        /// <summary>
        /// ドット・アベレージ
        /// 未使用時はヌル
        /// </summary>
        public static DotAverage dotAverage;
        /// <summary>
        /// 未使用時はヌル
        /// </summary>
        public static DotTransparentClear dotTransparentClear;

    }
}
