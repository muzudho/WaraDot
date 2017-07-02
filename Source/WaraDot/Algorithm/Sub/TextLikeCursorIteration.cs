using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WaraDot.Algorithm.Sub
{
    /// <summary>
    /// 選択範囲の左上隅から右端へ、改行して左端から右端へ、といった順でカーソル移動
    /// </summary>
    public class TextLikeCursorIteration
    {
        public Point currentPoint;

        public void Init()
        {
            // スタート地点
            currentPoint = new Point(Program.selectionImg.X, Program.selectionImg.Y);
        }

        public bool IsFinished()
        {
            return currentPoint.X == Program.config.width &&
                currentPoint.Y == Program.config.height;
        }

        public void GoToNext()
        {
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
        }
        public bool GoToNorth()
        {
            currentPoint.Y--;
            return -1 < currentPoint.Y;
        }
        public void BackFromNorth()
        {
            currentPoint.Y++;
        }
        public bool GoToEast()
        {
            currentPoint.X++;
            return currentPoint.X < Program.config.width;
        }
        public void BackFromEast()
        {
            currentPoint.X--;
        }
        public bool GoToSouth()
        {
            currentPoint.Y++;
            return currentPoint.Y < Program.config.height;
        }
        public void BackFromSouth()
        {
            currentPoint.Y--;
        }
        public bool GoToWest()
        {
            currentPoint.X--;
            return -1 < currentPoint.X;
        }
        public void BackFromWest()
        {
            currentPoint.X++;
        }
    }
}
