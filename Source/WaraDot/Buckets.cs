using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

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
        const int countMax = 100;//10000;

        public static Buckets Build(int mouseX, int mouseY, Form1 form1)
        {
            Buckets obj = new Buckets( mouseX, mouseY, form1);
            return obj;
        }

        Buckets(int mouseX, int mouseY, Form1 form1)
        {
            form1_cache = form1;

            markboard_cache = new bool[form1_cache.config.width, form1_cache.config.height];

            // スタート地点
            Point imgPt = form1.ToImage(mouseX, mouseY);
            // マウス押下した地点の色
            color_cache = form1.bitmap.GetPixel(imgPt.X, imgPt.Y);

            currentPoints = new List<Point>();
            nextPoints = new List<Point>();
            nextPoints.Add(imgPt);
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

            if (0 < nextPoints.Count)
            {
                currentPoints.Clear();
                currentPoints.AddRange(nextPoints);
                nextPoints.Clear();
                for (int i = 0; i< currentPoints.Count; i++)
                {
                    DrawAndSearch(currentPoints[i].X, currentPoints[i].Y);
                }
            }
        }

        void DrawAndSearch(int imgX, int imgY)
        {
            // 指定の升はとりあえずマークする
            markboard_cache[imgX, imgY] = true;

            // 指定した地点の色
            Color color2 = form1_cache.bitmap.GetPixel(imgX, imgY);

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
                    if (imgX  < form1_cache.config.width && !markboard_cache[imgX, imgY] && nextPoints.Count < countMax)
                    {
                        nextPoints.Add(new Point(imgX, imgY));
                    }
                    imgX--;
                    //*/
                    //*
                    // 下
                    imgY++;
                    if (imgY  < form1_cache.config.height && !markboard_cache[imgX, imgY] && nextPoints.Count < countMax)
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
