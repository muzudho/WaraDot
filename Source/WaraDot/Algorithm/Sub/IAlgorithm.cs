using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaraDot.Algorithm.Sub
{
    public interface IAlgorithm
    {
        /// <summary>
        /// アルゴリズム名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 開始できる状態にする
        /// </summary>
        void Init();

        /// <summary>
        /// 終了判定
        /// </summary>
        /// <returns></returns>
        bool IsFinished();

        /// <summary>
        /// タイマーで繰り返し実行される
        /// </summary>
        void Tick();

        /// <summary>
        /// 中断
        /// </summary>
        void Stop();
    }
}
