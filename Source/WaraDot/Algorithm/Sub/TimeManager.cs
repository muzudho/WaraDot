using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaraDot.Algorithm.Sub
{
    public class TimeManager
    {
        public void Clear()
        {
            current = -1;
            countMax = 100;
            countMaxStep = 10;
            COUNT_MAX_LIMIT = 10000;
        }

        public void IncleaseCapacity()
        {
            if (countMax < COUNT_MAX_LIMIT)
            {
                countMax += countMaxStep;
                countMaxStep++;
                if (COUNT_MAX_LIMIT < countMax)
                {
                    countMax = COUNT_MAX_LIMIT;
                }
            }
        }

        public int current;
        /// <summary>
        /// Tick１回で描く上限
        /// 長々と見てると飽きてくるんで、だんだん増やしてスピードアップさせる
        /// </summary>
        public int countMax;
        /// <summary>
        /// 増分。こいつも増やしていく。
        /// </summary>
        public int countMaxStep;
        /// <summary>
        /// 増やし過ぎると処理時間が追いつかなくなる？
        /// </summary>
        public int COUNT_MAX_LIMIT = 10000;

        public void BeginIteration()
        {
            current = -1;
        }
        public bool Iterate()
        {
            current++;
            return current< countMax;
        }
        public void EndIteration()
        {
            current = -1;
        }
    }
}
