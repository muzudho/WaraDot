using NLua;
using System;
using System.Windows.Forms;
using WaraDot.Algorithm;
using System.Drawing;
using System.Collections.Generic;

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

            selectionImg = Rectangle.Empty;

            currentAlgorithm = null;
            //algorithms = new Dictionary<string, IAlgorithm>()
            //{
            //    //{ "Buckets", Buckets.Build() }
            //};
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
        /// 選択範囲。無ければエンプティ
        /// </summary>
        public static Rectangle selectionImg;

        /// <summary>
        /// 現在実行中のアルゴリズム
        /// </summary>
        public static IAlgorithm currentAlgorithm;
        ///// <summary>
        ///// アルゴリズム一覧
        ///// </summary>
        //public static Dictionary<string,IAlgorithm> algorithms;
    }
}
