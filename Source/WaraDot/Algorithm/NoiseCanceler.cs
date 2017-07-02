using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System;

namespace WaraDot.Algorithm
{
    /// <summary>
    /// ノイズキャンセラー
    /// 
    /// 12ドット以下の透明色以外の色のかたまりは、透明に置換します
    /// </summary>
    public class NoiseCanceler : IAlgorithm
    {
        /// <summary>
        /// アルゴリズム名
        /// </summary>
        public string Name { get { return "NoiseCanceler"; } }

        Form1 form1_cache;

        /// <summary>
        /// フラグが立っているところは編集しない
        /// </summary>
        Markboard markboard;

        Point currentPoint;
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

        /// <summary>
        /// 加工前のビットマップ
        /// </summary>
        Bitmap beforeBitmap;

        /// <summary>
        /// 加工した数
        /// </summary>
        int done;

        static NoiseCanceler instance;
        public static NoiseCanceler Instance(Form1 form1)
        {
            if (null == instance)
            {
                instance = new NoiseCanceler(form1);
            }
            return instance;
        }
        NoiseCanceler(Form1 form1)
        {
            form1_cache = form1;
            markboard = new Markboard();
            countPoints = new List<Point>();
            currentPoints = new List<Point>();
            nextPoints = new List<Point>();
        }
        public void Clear()
        {
            beforeBitmap = new Bitmap(Program.config.GetDrawingLayerBitmap());
            done = 0;
            markboard.Clear();
        }
        public void Init()
        {
            markboard.Init();
            // スタート地点
            currentPoint = new Point(Program.selectionImg.X, Program.selectionImg.Y);
        }

        public bool IsFinished()
        {
            return currentPoint.X == Program.config.width &&
                currentPoint.Y == Program.config.height;
        }

        public void Step()
        {
            if (IsFinished())
            {
                return;
            }

            // バケツスキャン
            currentPoints.Clear();
            currentPoints.AddRange(nextPoints);
            nextPoints.Clear();
            for (int i = 0; i < currentPoints.Count; i++)
            {
                Scan(currentPoints[i].X, currentPoints[i].Y);
            }

            if (nextPoints.Count < 1)
            {
                // セレクション
                for (int i = 0; i < countMax; i++)
                {
                    if (!IsFinished())
                    {
                        DrawAndSearch();
                    }
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
        /// 数えた数
        /// </summary>
        List<Point> countPoints;


        /// <summary>
        /// Step() から呼び出される
        /// </summary>
        void DrawAndSearch()
        {
            if (markboard.Editable(currentPoint.X, currentPoint.Y))
            {
                // 指定した地点の色
                Color color2 = beforeBitmap.GetPixel(currentPoint.X, currentPoint.Y);

                if (255 == color2.A)
                {
                    // スタート地点
                    currentPoints.Clear();
                    nextPoints.Clear();
                    Point imgPt = new Point(currentPoint.X, currentPoint.Y);
                    //Point imgPt = form1_cache.ToImage(currentPoint.X, currentPoint.Y);
                    nextPoints.Add(imgPt);
                    countPoints.Clear();
                    Scan(imgPt.X, imgPt.Y);

                    if (countPoints.Count<13)
                    {
                        foreach (Point pt in countPoints)
                        {
                            // ノイズは透明化
                            form1_cache.Color = Color.Transparent;
                            bool drawed = false;
                            form1_cache.DrawDotByImage(pt.X, pt.Y, ref drawed);
                            if (drawed) { done++; };
                        }
                    }
                    countPoints.Clear();
                }
            }



            // 次の地点
            if (currentPoint.X + 1 < Program.selectionImg.X + Program.selectionImg.Width)// Program.config.width
            {
                currentPoint.X++;
            }
            else if (currentPoint.Y + 1 < Program.selectionImg.Y + Program.selectionImg.Height)// Program.config.height
            {
                currentPoint.X = Program.selectionImg.X;// 0;
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


        public bool IsFinishedScan()
        {
            return nextPoints.Count < 1;
        }

        /// <summary>
        /// 再帰
        /// </summary>
        /// <param name="imgX"></param>
        /// <param name="imgY"></param>
        void Scan(int imgX, int imgY)
        {
            // 指定の升はとりあえずマークする
            markboard.Mark(imgX, imgY);
            countPoints.Add(new Point(imgX, imgY));

            // 指定した地点の色
            Color color2 = Program.config.GetDrawingLayerBitmap().GetPixel(imgX, imgY);

            if (Color.Transparent != color2)//透明でない場合
            {
                // 上
                imgY--;
                if (-1 < imgY && markboard.Editable(imgX, imgY) && nextPoints.Count < countMax)
                {
                    nextPoints.Add(new Point(imgX, imgY));
                }
                imgY++;
                // 右
                imgX++;
                if (imgX < Program.config.width && markboard.Editable(imgX, imgY) && nextPoints.Count < countMax)
                {
                    nextPoints.Add(new Point(imgX, imgY));
                }
                imgX--;
                // 下
                imgY++;
                if (imgY < Program.config.height && markboard.Editable(imgX, imgY) && nextPoints.Count < countMax)
                {
                    nextPoints.Add(new Point(imgX, imgY));
                }
                imgY--;
                // 左
                imgX--;
                if (-1 < imgX && markboard.Editable(imgX, imgY) && nextPoints.Count < countMax)
                {
                    nextPoints.Add(new Point(imgX, imgY));
                }
                imgX++;
            }
        }

    }
}
