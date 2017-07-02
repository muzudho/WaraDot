using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaraDot.Algorithm
{
    public interface IAlgorithm
    {
        /// <summary>
        /// アルゴリズム名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 終了判定
        /// </summary>
        /// <returns></returns>
        bool IsFinished();

        /// <summary>
        /// タイマーで繰り返し実行される
        /// </summary>
        void Step();
    }
}
