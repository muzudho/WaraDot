using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System;

namespace WaraDot.Algorithm
{
    /// <summary>
    /// ドット・アベレージ
    /// 
    /// 現在地と、４方向の色を見て、色を平均化します。
    /// 透明は無視します
    /// </summary>
    public class DotAverage
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

        /// <summary>
        /// 加工前のビットマップ
        /// </summary>
        Bitmap beforeBitmap;

        /// <summary>
        /// 加工した数
        /// </summary>
        int done;

        public static DotAverage Build(Form1 form1)
        {
            DotAverage obj = new DotAverage(form1);
            return obj;
        }

        DotAverage(Form1 form1)
        {
            form1_cache = form1;

            markboard = new Markboard();
            markboard.Init();

            beforeBitmap = new Bitmap(Program.config.GetDrawingLayerBitmap());
            done = 0;

            // スタート地点
            currentPoint = new Point(Common.selectionImg.X, Common.selectionImg.Y);
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

            if (255!=color2.A)
            {
                // 透明セルは無視
                goto gt_Next;
            }

            // 指定した地点の四方の色
            Color north = Color.Transparent;
            {
                currentPoint.Y--;
                if (-1 < currentPoint.Y)
                {
                    north = beforeBitmap.GetPixel(currentPoint.X, currentPoint.Y);
                }
                currentPoint.Y++;
            }
            Color east = Color.Transparent;
            {
                currentPoint.X++;
                if (currentPoint.X < Program.config.width)
                {
                    east = beforeBitmap.GetPixel(currentPoint.X, currentPoint.Y);
                }
                currentPoint.X--;
            }
            Color south = Color.Transparent;
            {
                currentPoint.Y++;
                if (currentPoint.Y < Program.config.height)
                {
                    south = beforeBitmap.GetPixel(currentPoint.X, currentPoint.Y);
                }
                currentPoint.Y--;
            }
            Color west = Color.Transparent;
            {
                currentPoint.X--;
                if (-1 < currentPoint.X)
                {
                    west = beforeBitmap.GetPixel(currentPoint.X, currentPoint.Y);
                }
                currentPoint.X++;
            }

            // 透明は無視して、平均の色をもとめます
            int total = color2.R;
            int count = 1;
            if (north.A == 255) { total += north.R; count++; }
            if (east.A == 255) { total += east.R; count++; }
            if (south.A == 255) { total += south.R; count++; }
            if (west.A == 255) { total += west.R; count++; }

            // 平均を求める
            int average = (int)((float)total/count);
            form1_cache.Color = Color.FromArgb(average, average, average);

            // 平均に一番近い値
            int near=int.MaxValue;
            near = Math.Min(near, Math.Abs(color2.R-average));
            near = Math.Min(near, Math.Abs(north.R - average));
            near = Math.Min(near, Math.Abs(east.R - average));
            near = Math.Min(near, Math.Abs(south.R - average));
            near = Math.Min(near, Math.Abs(west.R - average));
            if (near < 1) { near = 1; }

            // 5つのセルの色をセット

            // 現在地
            {
                if (markboard.Editable(currentPoint.X, currentPoint.Y))
                {
                    int next = color2.R + (0 < color2.R - average ? -near : near);
                    if (next < 1) { next = 0; } else if (255 < next) { next = 255; }
                    form1_cache.Color = Color.FromArgb(next, next, next);
                    bool drawed = false;
                    form1_cache.DrawDotByImage(currentPoint.X, currentPoint.Y, ref drawed);
                    if (drawed) { done++; };
                }
            }

            if (north.A == 255)
            {
                currentPoint.Y--;
                if (markboard.Editable(currentPoint.X, currentPoint.Y))
                {
                    int next = north.R + (0 < north.R - average ? -near : near);
                    if (next < 1) { next = 0; } else if (255 < next) { next = 255; }
                    form1_cache.Color = Color.FromArgb(next, next, next);
                    bool drawed = false;
                    form1_cache.DrawDotByImage(currentPoint.X, currentPoint.Y, ref drawed);
                    if (drawed) { done++; };
                }
                currentPoint.Y++;
            }

            if (east.A == 255)
            {
                currentPoint.X++;
                if (markboard.Editable(currentPoint.X, currentPoint.Y))
                {
                    int next = east.R + (0 < east.R - average ? -near : near);
                    if (next < 1) { next = 0; } else if (255 < next) { next = 255; }
                    form1_cache.Color = Color.FromArgb(next, next, next);
                    bool drawed = false;
                    form1_cache.DrawDotByImage(currentPoint.X, currentPoint.Y, ref drawed);
                    if (drawed) { done++; };
                }
                currentPoint.X--;
            }

            if (south.A == 255)
            {
                currentPoint.Y++;
                if (markboard.Editable(currentPoint.X, currentPoint.Y))
                {
                    int next = south.R + (0 < south.R - average ? -near : near);
                    if (next < 1) { next = 0; } else if (255 < next) { next = 255; }
                    form1_cache.Color = Color.FromArgb(next, next, next);
                    bool drawed = false;
                    form1_cache.DrawDotByImage(currentPoint.X, currentPoint.Y, ref drawed);
                    if (drawed) { done++; };
                }
                currentPoint.Y--;
            }

            if (west.A == 255)
            {
                currentPoint.X--;
                if (markboard.Editable(currentPoint.X, currentPoint.Y))
                {
                    int next = west.R + (0 < west.R - average ? -near : near);
                    if (next < 1) { next = 0; } else if (255 < next) { next = 255; }
                    form1_cache.Color = Color.FromArgb(next, next, next);
                    bool drawed = false;
                    form1_cache.DrawDotByImage(currentPoint.X, currentPoint.Y, ref drawed);
                    if (drawed) { done++; };
                }
                currentPoint.X++;
            }

            gt_Next:
            // 次の地点
            if (currentPoint.X + 1 < Common.selectionImg.X+Common.selectionImg.Width)// Program.config.width
            {
                currentPoint.X++;
            }
            else if (currentPoint.Y + 1 < Common.selectionImg.Y+Common.selectionImg.Height)// Program.config.height
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
            form1_cache.SyncPos(currentPoint);
        }

    }
}
