using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WaraDot.Algorithm.Sub
{
    /// <summary>
    /// バケツ（塗りつぶし）のようなカーソル移動。
    /// クリック地点から広がっていくように進む。
    /// </summary>
    public class BucketsLikeCursorIteration
    {
        public BucketsLikeCursorIteration(Form1 form1)
        {
            form1_cache = form1;
            currentPoints = new List<Point>();
            nextPoints = new List<Point>();
        }
        Form1 form1_cache;
        public List<Point> currentPoints;
        public List<Point> nextPoints;

        public void Clear()
        {
            currentPoints.Clear();//追加
            nextPoints.Clear();
            currentElementIndex = - 1;
        }
        public void Init(Point startImg) // Point startImg = form1_cache.ToImage(form1_cache.CursorRect.X, form1_cache.CursorRect.Y);
        {
            Clear();
            // スタート地点            
            nextPoints.Add(new Point(startImg.X, startImg.Y));
        }

        public bool IsFinished()
        {
            return nextPoints.Count < 1;
        }
        public int NextPointsCount
        {
            get
            {
                return nextPoints.Count;
            }
        }
        public bool HasNextPoints()
        {
            return 0 < NextPointsCount;
        }


        public void BeginIteration()
        {
            currentPoints.Clear();
            currentPoints.AddRange(nextPoints);
            nextPoints.Clear();
            currentElementIndex = -1; // 加算後、条件判定なので 0 ではなく -1 からスタート
        }
        public void EndIteration()
        {
            currentElementIndex--;
            // 残った分は次の機会に
            while (Iterate())
            {
                nextPoints.Add(new Point(currentPoints[currentElementIndex].X, currentPoints[currentElementIndex].Y));
            }
        }

        public int currentElementIndex;
        public bool Iterate()
        {
            currentElementIndex++;
            return currentElementIndex < currentPoints.Count;
        }
        /// <summary>
        /// 指している座標
        /// </summary>
        public Point Cursor
        {
            get
            {
                return currentPoints[currentElementIndex];
            }
        }
        public void OffsetCurrentPoint(int dx, int dy)
        {
            currentPoints[currentElementIndex] = new Point(Cursor.X + dx, Cursor.Y + dy);
        }

        /// <summary>
        /// 現在の座標を、次の座標としてマーキングします
        /// </summary>
        /// <param name="iPt"></param>
        public void MarkNextPoint()
        {
            nextPoints.Add(Cursor);
        }

        /// <summary>
        /// Pointクラスの Offset は効き目がないんじゃないか？
        /// </summary>
        /// <param name="iPt"></param>
        /// <returns></returns>
        public bool GoToNorth()
        {
            OffsetCurrentPoint(0, -1);
            return -1 < Cursor.Y;
        }
        public void BackFromNorth()
        {
            OffsetCurrentPoint(0, 1);
        }
        public bool GoToEast()
        {
            OffsetCurrentPoint(1, 0);
            return Cursor.X < Program.config.width;
        }
        public void BackFromEast()
        {
            OffsetCurrentPoint(-1, 0);
        }
        public bool GoToSouth()
        {
            OffsetCurrentPoint(0, 1);
            return Cursor.Y < Program.config.height;
        }
        public void BackFromSouth()
        {
            OffsetCurrentPoint(0, -1);
        }
        public bool GoToWest()
        {
            OffsetCurrentPoint(-1, 0);
            return -1 < Cursor.X;
        }
        public void BackFromWest()
        {
            OffsetCurrentPoint(+1, 0);
        }
    }
}
