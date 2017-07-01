using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System;

namespace WaraDot.Algorithm
{
    /// <summary>
    /// ドット・トランスペアレント・クリアー
    /// 
    /// 半透明は、透明にしてしまいます
    /// </summary>
    public class DotTransparentClear
    {
        Form1 form1_cache;
        Point currentPoint;
        /// <summary>
        /// 見てると飽きてくるんで、だんだん増やしていく。
        /// </summary>
        int countMax = 100;
        /// <summary>
        /// 増分。こいつも増やしていく。
        /// </summary>
        int countMaxStep = 10;
        /// <summary>
        /// 増やし過ぎると処理時間が追いつかなくなる？
        /// </summary>
        const int COUNT_MAX_LIMIT = 10000;

        /// <summary>
        /// 加工前のビットマップ
        /// </summary>
        Bitmap beforeBitmap;

        /// <summary>
        /// 加工した数
        /// </summary>
        int done;

        public static DotTransparentClear Build(Form1 form1)
        {
            DotTransparentClear obj = new DotTransparentClear(form1);
            return obj;
        }

        DotTransparentClear(Form1 form1)
        {
            form1_cache = form1;

            beforeBitmap = new Bitmap(Program.config.GetDrawingLayerBitmap());
            done = 0;

            // スタート地点
            currentPoint = new Point();
        }

        public bool IsFinished()
        {
            return currentPoint.X  == Program.config.width &&
                currentPoint.Y == Program.config.height;
        }

        public void Step()
        {
            if (IsFinished())
            {
                return;
            }

            Trace.WriteLine("cur(" + currentPoint.X + ", " + currentPoint.Y + ") img(" + Program.config.width + ", " + Program.config.height + ") done="+done);

            for (int i = 0; i < countMax; i++)
            {
                if (!IsFinished())
                {
                    DrawAndSearch();
                }
            }

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

        /// <summary>
        /// Step() から呼び出される
        /// </summary>
        /// <param name="imgX"></param>
        /// <param name="imgY"></param>
        void DrawAndSearch()
        {
            // 指定した地点の色
            Color color2 = beforeBitmap.GetPixel(currentPoint.X, currentPoint.Y);

            if (color2.A<255)
            {
                // 半透明セルは透明化
                form1_cache.Color = Color.Transparent;
                bool drawed = false;
                form1_cache.DrawDotByImage(currentPoint.X, currentPoint.Y, ref drawed);
                if (drawed) { done++; };
            }

            // 次の地点
            if (currentPoint.X + 1 != Program.config.width)
            {
                currentPoint.X++;
            }
            else if (currentPoint.Y + 1 != Program.config.height)
            {
                currentPoint.X=0;
                currentPoint.Y++;
            }
            else
            {
                // 終了
                currentPoint.X = Program.config.width;
                currentPoint.Y = Program.config.height;
            }
            form1_cache.SyncPos(currentPoint);
        }

    }
}
