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
        /// <summary>
        /// 指している点
        /// </summary>
        public Point cursor;

        public void Init()
        {
            // スタート地点
            cursor = new Point(Program.selectionImg.X, Program.selectionImg.Y);
        }

        public bool IsFinished()
        {
            return Program.config.width <= cursor.X &&
                Program.config.height <= cursor.Y;
        }

        public void GoToNext()
        {
            // 次の地点
            if (cursor.X + 1 < Program.selectionImg.X + Program.selectionImg.Width)// Program.config.width
            {
                cursor.X++;
            }
            else if (cursor.Y + 1 < Program.selectionImg.Y + Program.selectionImg.Height)// Program.config.height
            {
                cursor.X = Program.selectionImg.X;// 0;
                cursor.Y++;
            }
            else
            {
                // 終了
                cursor.X = Program.config.width;
                cursor.Y = Program.config.height;
            }
        }
        public bool GoToNorth()
        {
            cursor.Y--;
            return -1 < cursor.Y;
        }
        public void BackFromNorth()
        {
            cursor.Y++;
        }
        public bool GoToEast()
        {
            cursor.X++;
            return cursor.X < Program.config.width;
        }
        public void BackFromEast()
        {
            cursor.X--;
        }
        public bool GoToSouth()
        {
            cursor.Y++;
            return cursor.Y < Program.config.height;
        }
        public void BackFromSouth()
        {
            cursor.Y--;
        }
        public bool GoToWest()
        {
            cursor.X--;
            return -1 < cursor.X;
        }
        public void BackFromWest()
        {
            cursor.X++;
        }
    }
}
