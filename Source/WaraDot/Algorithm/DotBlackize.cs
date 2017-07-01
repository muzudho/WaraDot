using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace WaraDot.Algorithm
{
    /// <summary>
    /// ドット・ブラッカイズ
    /// 
    /// ４方向の色を見て、透明を覗いて現在地点の色が一番白に近いとき、
    /// 自分より黒に近い隣の色を　もう少し黒くして、自分のはもっと白に近づけます。
    /// 
    /// 完全白だった場合は透明にします。
    /// </summary>
    public class DotBlackize
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
        /// 効き目の強さ
        /// </summary>
        int multiplier = 2;//8;//
        /// <summary>
        /// 似ている色の許容範囲
        /// </summary>
        int sameRange = 16;
        /// <summary>
        /// 加工した数
        /// </summary>
        int done;

        public static DotBlackize Build(Form1 form1)
        {
            DotBlackize obj = new DotBlackize(form1);
            return obj;
        }

        DotBlackize(Form1 form1)
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

        void DrawWhiteout(int value)
        {
            // 指定した地点の色
            Color color2 = beforeBitmap.GetPixel(currentPoint.X, currentPoint.Y);

            //int next = color2.R + 2*value;//ホワイトアウトの方が強く。
            int next = color2.R + value;
            if (255 < next)
            {
                // 完全白は消します
                form1_cache.Color = Color.Transparent;
                bool drawed = false;
                form1_cache.DrawDotByImage(currentPoint.X, currentPoint.Y, ref drawed);
                if (drawed) { done++; };
            }
            else
            {
                form1_cache.Color = Color.FromArgb(next, next, next);
                bool drawed = false;
                form1_cache.DrawDotByImage(currentPoint.X, currentPoint.Y, ref drawed);
                if (drawed) { done++; };
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

            // 隣接する透明色の数
            int transparentCount = 0;
            if (north.A != 255) { transparentCount++; }
            if (east.A != 255) { transparentCount++; }
            if (south.A != 255) { transparentCount++; }
            if (west.A != 255) { transparentCount++; }

            // 現在地点が一番白に近いか判定。グレースケールの前提なので、赤だけ比較。
            // 隣接する、自分より黒に近いセルの数
            int blackerCount = 0;
            int sameColorCount = 0;
            if (north.A == 255) {
                if (north.R < color2.R - sameRange) { blackerCount++; }
                if (color2.R - sameRange <= north.R && north.R <= color2.R) { sameColorCount++; }
            }
            if (east.A == 255) {
                if (east.R < color2.R - sameRange) { blackerCount++; }
                if (color2.R - sameRange <= east.R && east.R <= color2.R) { sameColorCount++; }
            }
            if (south.A == 255) {
                if (south.R < color2.R - sameRange) { blackerCount++; }
                if (color2.R - sameRange <= south.R && south.R <= color2.R) { sameColorCount++; }
            }
            if (west.A == 255) {
                if (west.R < color2.R - sameRange) { blackerCount++; }
                if (color2.R - sameRange <= west.R && west.R <= color2.R) { sameColorCount++; }
            }


            // 透明の数＋自分より黒いセルの数＝４　なら、自分が一番白いと判定
            if (transparentCount + blackerCount == 4)
            {
                //Trace.WriteLine("ブラッカイズ！");

                // 現在地点を、より白に近づけます
                DrawWhiteout(multiplier * blackerCount);

                //*
                // 周囲を黒に近づけます
                if (north.A == 255 && north.R < color2.R)
                {
                    currentPoint.Y--;
                    int next = north.R + multiplier*1;
                    if (255 < next) { next = 255; }
                    form1_cache.Color = Color.FromArgb(next, next, next);
                    bool drawed = false;
                    form1_cache.DrawDotByImage(currentPoint.X, currentPoint.Y, ref drawed);
                    if (drawed) { done++; };
                    currentPoint.Y++;
                }

                if (east.A == 255 && east.R < color2.R)
                {
                    currentPoint.X++;
                    int next = east.R + multiplier * 1;
                    if (255 < next) { next = 255; }
                    form1_cache.Color = Color.FromArgb(next, next, next);
                    bool drawed = false;
                    form1_cache.DrawDotByImage(currentPoint.X, currentPoint.Y, ref drawed);
                    if (drawed) { done++; };
                    currentPoint.X--;
                }

                if (south.A == 255 && south.R < color2.R)
                {
                    currentPoint.Y++;
                    int next = south.R + multiplier * 1;
                    if (255 < next) { next = 255; }
                    form1_cache.Color = Color.FromArgb(next, next, next);
                    bool drawed = false;
                    form1_cache.DrawDotByImage(currentPoint.X, currentPoint.Y, ref drawed);
                    if (drawed) { done++; };
                    currentPoint.Y--;
                }

                if (west.A == 255 && west.R < color2.R)
                {
                    currentPoint.X--;
                    int next = west.R + multiplier * 1;
                    if (255 < next) { next = 255; }
                    form1_cache.Color = Color.FromArgb(next, next, next);
                    bool drawed = false;
                    form1_cache.DrawDotByImage(currentPoint.X, currentPoint.Y, ref drawed);
                    if (drawed) { done++; };
                    currentPoint.X++;
                }
                //*/
            }
            // 透明の数が３、（自分より黒いセルの数が０、または同色のセルの数が１）　なら、自分がはみ出た色と判定
            else if (transparentCount==3 && (blackerCount==0||sameColorCount==1))
            {
                // 現在地点を、より白に近づけます
                DrawWhiteout(multiplier * transparentCount);
            }

            gt_Next:
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
