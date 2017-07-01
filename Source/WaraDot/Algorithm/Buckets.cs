using System.Collections.Generic;
using System.Drawing;

namespace WaraDot
{
    /// <summary>
    /// 1万ピクセルなら一瞬で塗りつぶせるが、
    /// 150x150=22500 ピクセルになるとスタックオーバーフローしてしまう。
    /// 
    /// 塗りつぶしアルゴリズムを視覚化できないだろうか？
    /// 
    /// </summary>
    public class Buckets
    {
        Form1 form1_cache;
        Color color_cache;
        bool[,] markboard_cache;
        List<Point> currentPoints;
        List<Point> nextPoints;
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

        public static Buckets Build(int mouseX, int mouseY, Form1 form1)
        {
            Buckets obj = new Buckets( mouseX, mouseY, form1);
            return obj;
        }

        Buckets(int mouseX, int mouseY, Form1 form1)
        {
            form1_cache = form1;

            markboard_cache = new bool[Program.config.width, Program.config.height];

            // 選択範囲の外は編集しないようにする
            for (int y=0; y< Program.config.height; y++)
            {
                for (int x = 0; x < Program.config.width; x++)
                {
                    if (!Common.selectionImg.Contains(x, y))
                    {
                        markboard_cache[x, y] = true;
                    }
                }
            }

            // スタート地点
            Point imgPt = form1.ToImage(mouseX, mouseY);
            // マウス押下した地点の色
            color_cache = Program.config.GetDrawingLayerBitmap().GetPixel(imgPt.X, imgPt.Y);

            currentPoints = new List<Point>();
            nextPoints = new List<Point>
            {
                imgPt
            };
        }

        public bool IsFinished()
        {
            return nextPoints.Count < 1;
        }

        public void Step()
        {
            if (IsFinished())
            {
                return;
            }

            currentPoints.Clear();
            currentPoints.AddRange(nextPoints);
            nextPoints.Clear();
            for (int i = 0; i< currentPoints.Count; i++)
            {
                DrawAndSearch(currentPoints[i].X, currentPoints[i].Y);
            }

            if (countMax < COUNT_MAX_LIMIT)
            {
                countMax += countMaxStep;
                countMaxStep++;
                if (COUNT_MAX_LIMIT<countMax)
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
        void DrawAndSearch(int imgX, int imgY)
        {
            // 指定の升はとりあえずマークする
            markboard_cache[imgX, imgY] = true;

            // 指定した地点の色
            Color color2 = Program.config.GetDrawingLayerBitmap().GetPixel(imgX, imgY);

            if (color2.Equals( color_cache))//一致した場合
            {
                // 指定の地点をまず描画
                bool drawed = false;
                form1_cache.DrawDotByImage(imgX, imgY, ref drawed);

                if (drawed)
                {
                    //bool worked_unUse = false;
                    //*
                    // 上
                    imgY--;
                    if (-1< imgY && !markboard_cache[imgX, imgY] && nextPoints.Count < countMax)
                    {
                        nextPoints.Add( new Point(imgX, imgY));
                    }
                    imgY++;
                    //*/
                    //*
                    // 右
                    imgX++;
                    if (imgX  < Program.config.width && !markboard_cache[imgX, imgY] && nextPoints.Count < countMax)
                    {
                        nextPoints.Add(new Point(imgX, imgY));
                    }
                    imgX--;
                    //*/
                    //*
                    // 下
                    imgY++;
                    if (imgY  < Program.config.height && !markboard_cache[imgX, imgY] && nextPoints.Count < countMax)
                    {
                        nextPoints.Add(new Point(imgX, imgY));
                    }
                    imgY--;
                    //*/
                    //*
                    // 左
                    imgX--;
                    if (-1 < imgX && !markboard_cache[imgX, imgY] && nextPoints.Count < countMax)
                    {
                        nextPoints.Add(new Point(imgX, imgY));
                    }
                    imgX++;
                    //*/
                }
            }

        }

    }
}
