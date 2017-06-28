using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// バケツ
        /// 未使用時はヌル
        /// </summary>
        public static Buckets buckets;
        /// <summary>
        /// ワンドット・イーター
        /// 未使用時はヌル
        /// </summary>
        public static OneDotEater oneDotEater;

    }
}
