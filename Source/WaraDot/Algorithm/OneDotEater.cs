using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace WaraDot.Algorithm
{
    /// <summary>
    /// 1ドットイーター
    /// 
    /// 1ドット浮いていれば、周りの色で置換します
    /// </summary>
    public class OneDotEater
    {
        Form1 form1_cache;

        /// <summary>
        /// フラグが立っているところは編集しない
        /// </summary>
        Markboard markboard;

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

        int done;

        public static OneDotEater Build(Form1 form1)
        {
            OneDotEater obj = new OneDotEater(form1);
            return obj;
        }

        OneDotEater(Form1 form1)
        {
            form1_cache = form1;

            markboard = new Markboard();
            markboard.Init();
            // スタート地点
            currentPoint = new Point(Common.selectionImg.X, Common.selectionImg.Y);

            form1_cache.SyncPos(currentPoint);
            done = 0;
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
            Color color2 = Program.config.GetDrawingLayerBitmap().GetPixel(currentPoint.X, currentPoint.Y);

            // 指定した地点の四方の色
            Color north = Color.Transparent;
            {
                currentPoint.Y--;
                if (-1 < currentPoint.Y)
                {
                    north = Program.config.GetDrawingLayerBitmap().GetPixel(currentPoint.X, currentPoint.Y);
                }
                currentPoint.Y++;
            }
            Color east = Color.Transparent;
            {
                currentPoint.X++;
                if (currentPoint.X < Program.config.width)
                {
                    east = Program.config.GetDrawingLayerBitmap().GetPixel(currentPoint.X, currentPoint.Y);
                }
                currentPoint.X--;
            }
            Color south = Color.Transparent;
            {
                currentPoint.Y++;
                if (currentPoint.Y < Program.config.height)
                {
                    south = Program.config.GetDrawingLayerBitmap().GetPixel(currentPoint.X, currentPoint.Y);
                }
                currentPoint.Y--;
            }
            Color west = Color.Transparent;
            {
                currentPoint.X--;
                if (-1 < currentPoint.X)
                {
                    west = Program.config.GetDrawingLayerBitmap().GetPixel(currentPoint.X, currentPoint.Y);
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
                if (markboard.Editable(currentPoint.X, currentPoint.Y))
                {
                    Trace.WriteLine("イート！");

                    // 指定の地点を、周りの色で塗ります
                    form1_cache.Color = aroundColor;
                    bool drawed = false;
                    form1_cache.DrawDotByImage(currentPoint.X, currentPoint.Y, ref drawed);
                    if (drawed) { done++; }
                }

            }

            // 次の地点
            if (currentPoint.X + 1 < Common.selectionImg.X + Common.selectionImg.Width)// Program.config.width
            {
                currentPoint.X++;
            }
            else if (currentPoint.Y + 1 < Common.selectionImg.Y + Common.selectionImg.Height)// Program.config.height
            {
                currentPoint.X = Common.selectionImg.X;// 0;
                currentPoint.Y++;
            }
            else
            {
                // 終了
                currentPoint.X = Program.config.width;
                currentPoint.Y = Program.config.height;
            }

            /*
            // 次の地点
            if (currentPoint.X + 1 != Program.config.width)
            {
                currentPoint.X++;
            }
            else if (currentPoint.Y + 1 != Program.config.height)
            {
                currentPoint.X = 0;
                currentPoint.Y++;
            }
            else
            {
                // 終了
                currentPoint.X = Program.config.width;
                currentPoint.Y = Program.config.height;
            }
            */
            form1_cache.SyncPos(currentPoint);
        }

    }
}
