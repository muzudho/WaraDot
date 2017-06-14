using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WaraDot
{
    public class Buckets
    {
        static Form1 form1_cache;
        static Color color_cache;

        static bool[,] markboard_cache;

        public static void FillStart(int mouseX, int mouseY, Form1 form1)
        {
            //MessageBox.Show("塗りつぶしたい");

            form1_cache = form1;
            markboard_cache = new bool[form1_cache.config.width, form1_cache.config.height];

            Point imgPt = form1.ToImage(mouseX, mouseY);
            // マウス押下した地点の色
            color_cache = form1.bitmap.GetPixel(imgPt.X, imgPt.Y);

            int count = 0;
            PaintLoop(imgPt.X, imgPt.Y, ref count);
        }

        const int countMax = 10000;
        static void PaintLoop(int imgX, int imgY, ref int count)
        {
            // 指定した地点の色
            Color color2 = form1_cache.bitmap.GetPixel(imgX, imgY);

            if (color2.Equals( color_cache))//一致した場合
            {
                // 指定の地点をまず描画
                bool drawed = false;
                form1_cache.DrawDotByImage(imgX, imgY, ref drawed);
                markboard_cache[imgX, imgY] = true;
                count++;

                if (drawed)
                {
                    //*
                    // 上
                    imgY--;
                    if (-1< imgY && !markboard_cache[imgX, imgY] && count < countMax)
                    {
                        PaintLoop(imgX, imgY, ref count);
                    }
                    imgY++;
                    //*/
                    //*
                    // 右
                    imgX++;
                    if (imgX  < form1_cache.config.width && !markboard_cache[imgX, imgY] && count < countMax)
                    {
                        PaintLoop(imgX, imgY, ref count);
                    }
                    imgX--;
                    //*/
                    //*
                    // 下
                    imgY++;
                    if (imgY  < form1_cache.config.height && !markboard_cache[imgX, imgY] && count < countMax)
                    {
                        PaintLoop(imgX, imgY, ref count);
                    }
                    imgY--;
                    //*/
                    //*
                    // 左
                    imgX--;
                    if (-1 < imgX && !markboard_cache[imgX, imgY] && count < countMax)
                    {
                        PaintLoop(imgX, imgY, ref count);
                    }
                    imgX++;
                    //*/
                }
            }

        }

    }
}
