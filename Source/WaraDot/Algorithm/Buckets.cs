using System.Collections.Generic;
using System.Drawing;

namespace WaraDot.Algorithm
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

        /// <summary>
        /// フラグが立っているところは編集しない
        /// </summary>
        Markboard markboard;

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

            markboard = new Markboard();
            markboard.Init();

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
            markboard.Mark(imgX, imgY);

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
                    if (-1< imgY && markboard.Editable(imgX, imgY) && nextPoints.Count < countMax)
                    {
                        nextPoints.Add( new Point(imgX, imgY));
                    }
                    imgY++;
                    //*/
                    //*
                    // 右
                    imgX++;
                    if (imgX  < Program.config.width && markboard.Editable(imgX, imgY) && nextPoints.Count < countMax)
                    {
                        nextPoints.Add(new Point(imgX, imgY));
                    }
                    imgX--;
                    //*/
                    //*
                    // 下
                    imgY++;
                    if (imgY  < Program.config.height && markboard.Editable(imgX, imgY) && nextPoints.Count < countMax)
                    {
                        nextPoints.Add(new Point(imgX, imgY));
                    }
                    imgY--;
                    //*/
                    //*
                    // 左
                    imgX--;
                    if (-1 < imgX && markboard.Editable(imgX, imgY) && nextPoints.Count < countMax)
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
