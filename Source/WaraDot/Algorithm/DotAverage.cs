using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System;
using WaraDot.Algorithm.Sub;

namespace WaraDot.Algorithm
{
    /// <summary>
    /// ドット・アベレージ
    /// 
    /// 現在地と、４方向の色を見て、色を平均化します。
    /// 透明は無視します
    /// 
    /// - 読み先、書き先が分かれている
    /// - 読み先、書き先が同じでも使用可
    /// </summary>
    public class DotAverage : IAlgorithm
    {
        /// <summary>
        /// アルゴリズム名
        /// </summary>
        public string Name { get { return "DotAverage"; } }

        Form1 form1_cache;

        /// <summary>
        /// フラグが立っているところは編集しない
        /// </summary>
        Markboard markboard;

        /// <summary>
        /// 選択範囲の左上隅から右端へ、改行して左端から右端へ、といった順でカーソル移動
        /// </summary>
        TextLikeCursorIteration textLikeCursorIteration;

        /// <summary>
        /// 時間制御
        /// </summary>
        TimeManager timeManager;

        /// <summary>
        /// 加工前のビットマップ
        /// </summary>
        Bitmap beforeDrawingBitmap;

        /// <summary>
        /// 加工した数
        /// </summary>
        int done;

        static DotAverage instance;
        public static IAlgorithm Instance(Form1 form1)
        {
            if(null== instance)
            {
                instance = new DotAverage(form1);
            }
            return instance;
        }
        DotAverage(Form1 form1)
        {
            form1_cache = form1;
            timeManager = new TimeManager();
            markboard = new Markboard();
            textLikeCursorIteration = new TextLikeCursorIteration();
        }
        public void Clear()
        {
            timeManager.Clear();
            markboard.Clear();
            beforeDrawingBitmap = new Bitmap(Program.config.DrawingLayerBitmap);
            done = 0;
        }
        public void Init()
        {
            Clear();
            markboard.Init();
            textLikeCursorIteration.Init();
        }

        public bool IsFinished()
        {
            return textLikeCursorIteration.IsFinished();
        }

        public void Tick()
        {
            if (IsFinished())
            {
                return;
            }

            Trace.WriteLine("cur(" + textLikeCursorIteration.cursor.X + ", " + textLikeCursorIteration.cursor.Y + ") img(" + Program.config.width + ", " + Program.config.height + ") done="+done);

            timeManager.BeginIteration();
            while (timeManager.Iterate())
            {
                if (!IsFinished())
                {
                    DrawAndSearch();
                }
            }
            timeManager.EndIteration();

            timeManager.IncleaseCapacity();
        }

        /// <summary>
        /// Step() から呼び出される
        /// </summary>
        /// <param name="imgX"></param>
        /// <param name="imgY"></param>
        void DrawAndSearch()
        {
            // 指定した地点の色
            Color color2 = beforeDrawingBitmap.GetPixel(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y);

            if (255!=color2.A)
            {
                // 透明セルは無視
                goto gt_Next;
            }

            // 指定した地点の四方の色
            Color north = Color.Transparent;
            {
                if (textLikeCursorIteration.GoToNorth())
                {
                    north = beforeDrawingBitmap.GetPixel(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y);
                }
                textLikeCursorIteration.BackFromNorth();
            }
            Color east = Color.Transparent;
            {
                if (textLikeCursorIteration.GoToEast())
                {
                    east = beforeDrawingBitmap.GetPixel(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y);
                }
                textLikeCursorIteration.BackFromEast();
            }
            Color south = Color.Transparent;
            {
                if (textLikeCursorIteration.GoToSouth())
                {
                    south = beforeDrawingBitmap.GetPixel(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y);
                }
                textLikeCursorIteration.BackFromSouth();
            }
            Color west = Color.Transparent;
            {
                if (textLikeCursorIteration.GoToWest())
                {
                    west = beforeDrawingBitmap.GetPixel(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y);
                }
                textLikeCursorIteration.BackFromWest();
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
            form1_cache.DrawingColor = Color.FromArgb(average, average, average);

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
                if (markboard.Editable(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y))
                {
                    int next = color2.R + (0 < color2.R - average ? -near : near);
                    if (next < 1) { next = 0; } else if (255 < next) { next = 255; }
                    form1_cache.DrawingColor = Color.FromArgb(next, next, next);
                    bool drawed = false;
                    form1_cache.DrawDotByImage(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y, ref drawed);
                    if (drawed) { done++; };
                }
            }

            if (north.A == 255)
            {
                if (textLikeCursorIteration.GoToNorth())
                {
                    if (markboard.Editable(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y))
                    {
                        int next = north.R + (0 < north.R - average ? -near : near);
                        if (next < 1) { next = 0; } else if (255 < next) { next = 255; }
                        form1_cache.DrawingColor = Color.FromArgb(next, next, next);
                        bool drawed = false;
                        form1_cache.DrawDotByImage(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y, ref drawed);
                        if (drawed) { done++; };
                    }
                }
                textLikeCursorIteration.BackFromNorth();
            }

            if (east.A == 255)
            {
                if (textLikeCursorIteration.GoToEast())
                {
                    if (markboard.Editable(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y))
                    {
                        int next = east.R + (0 < east.R - average ? -near : near);
                        if (next < 1) { next = 0; } else if (255 < next) { next = 255; }
                        form1_cache.DrawingColor = Color.FromArgb(next, next, next);
                        bool drawed = false;
                        form1_cache.DrawDotByImage(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y, ref drawed);
                        if (drawed) { done++; };
                    }
                }
                textLikeCursorIteration.BackFromEast();
            }

            if (south.A == 255)
            {
                if (textLikeCursorIteration.GoToSouth())
                {
                    if (markboard.Editable(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y))
                    {
                        int next = south.R + (0 < south.R - average ? -near : near);
                        if (next < 1) { next = 0; } else if (255 < next) { next = 255; }
                        form1_cache.DrawingColor = Color.FromArgb(next, next, next);
                        bool drawed = false;
                        form1_cache.DrawDotByImage(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y, ref drawed);
                        if (drawed) { done++; };
                    }
                }
                textLikeCursorIteration.BackFromSouth();
            }

            if (west.A == 255)
            {
                if (textLikeCursorIteration.GoToWest())
                {
                    if (markboard.Editable(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y))
                    {
                        int next = west.R + (0 < west.R - average ? -near : near);
                        if (next < 1) { next = 0; } else if (255 < next) { next = 255; }
                        form1_cache.DrawingColor = Color.FromArgb(next, next, next);
                        bool drawed = false;
                        form1_cache.DrawDotByImage(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y, ref drawed);
                        if (drawed) { done++; };
                    }
                }
                textLikeCursorIteration.BackFromWest();
            }

            gt_Next:
            // 次の地点
            textLikeCursorIteration.GoToNext();
            form1_cache.SyncPos(textLikeCursorIteration.cursor);
        }

    }
}
