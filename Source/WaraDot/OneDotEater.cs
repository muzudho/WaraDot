using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace WaraDot
{
    /// <summary>
    /// 1ドットイーター
    /// </summary>
    public class OneDotEater
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

        public static OneDotEater Build(Form1 form1)
        {
            OneDotEater obj = new OneDotEater(form1);
            return obj;
        }

        OneDotEater(Form1 form1)
        {
            form1_cache = form1;

            // スタート地点
            currentPoint = new Point();
        }

        public bool IsFinished()
        {
            return currentPoint.X  == form1_cache.config.width &&
                currentPoint.Y == form1_cache.config.height;
        }

        public void Step()
        {
            if (IsFinished())
            {
                return;
            }

            Trace.WriteLine("cur(" + currentPoint.X + ", " + currentPoint.Y + ") img(" + form1_cache.config.width + ", " + form1_cache.config.height + ")");

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
            Color color2 = form1_cache.config.GetDrawingLayerBitmap().GetPixel(currentPoint.X, currentPoint.Y);

            // 指定した地点の四方の色
            Color north = Color.Transparent;
            {
                currentPoint.Y--;
                if (-1 < currentPoint.Y)
                {
                    north = form1_cache.config.GetDrawingLayerBitmap().GetPixel(currentPoint.X, currentPoint.Y);
                }
                currentPoint.Y++;
            }
            Color east = Color.Transparent;
            {
                currentPoint.X++;
                if (currentPoint.X < form1_cache.config.width)
                {
                    east = form1_cache.config.GetDrawingLayerBitmap().GetPixel(currentPoint.X, currentPoint.Y);
                }
                currentPoint.X--;
            }
            Color south = Color.Transparent;
            {
                currentPoint.Y++;
                if (currentPoint.Y < form1_cache.config.height)
                {
                    south = form1_cache.config.GetDrawingLayerBitmap().GetPixel(currentPoint.X, currentPoint.Y);
                }
                currentPoint.Y--;
            }
            Color west = Color.Transparent;
            {
                currentPoint.X--;
                if (-1 < currentPoint.X)
                {
                    west = form1_cache.config.GetDrawingLayerBitmap().GetPixel(currentPoint.X, currentPoint.Y);
                }
                currentPoint.X++;
            }

            Color aroundColor = Color.Transparent;
            if (Color.Transparent != north) { aroundColor = north; }
            else if (Color.Transparent != east) { aroundColor = east; }
            else if (Color.Transparent != south) { aroundColor = south; }
            else if (Color.Transparent != west) { aroundColor = west; }

            if (Color.Transparent==aroundColor)
            {
                Trace.WriteLine("一致なし");
            }

            // 四方の色が全て同じで、現在地点が違う色の場合
            if (
                (Color.Transparent == north || aroundColor == north) &&
                (Color.Transparent == east || aroundColor == east) &&
                (Color.Transparent == south || aroundColor == south) &&
                (Color.Transparent == west || aroundColor == west) &&
                aroundColor != color2
                )
            {
                Trace.WriteLine("イート！");

                // 指定の地点を、周りの色で塗ります
                form1_cache.Color = aroundColor;
                bool drawed = false;
                form1_cache.DrawDotByImage(currentPoint.X, currentPoint.Y, ref drawed);
            }

            // 次の地点
            if (currentPoint.X + 1 != form1_cache.config.width)
            {
                currentPoint.X++;
            }
            else if (currentPoint.Y + 1 != form1_cache.config.height)
            {
                currentPoint.X=0;
                currentPoint.Y++;
            }
            else
            {
                // 終了
                currentPoint.X = form1_cache.config.width;
                currentPoint.Y = form1_cache.config.height;
            }
        }

    }
}
