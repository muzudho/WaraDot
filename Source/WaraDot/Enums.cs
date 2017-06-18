using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaraDot
{
    /// <summary>
    /// 操作している者
    /// </summary>
    public enum OperatorType
    {
        /// <summary>
        /// 人間が操作中
        /// </summary>
        Human,
        /// <summary>
        /// アルゴリズムが実行されているのを再生中
        /// </summary>
        Computer
    }

    /// <summary>
    /// 描画ツール
    /// </summary>
    public enum Tools
    {
        /// <summary>
        /// フリーハンド線
        /// </summary>
        FreeHandLine,

        /// <summary>
        /// 塗りつぶし
        /// </summary>
        Buckets,

        /// <summary>
        /// 消しゴム
        /// </summary>
        Eraser
    }
}
