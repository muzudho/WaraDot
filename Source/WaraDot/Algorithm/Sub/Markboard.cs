using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WaraDot.Algorithm.Sub
{
    /// <summary>
    /// フラグが立っているところは編集しない
    /// </summary>
    public class Markboard
    {
        /// <summary>
        /// フラグが立っているところは編集しない
        /// </summary>
        bool[,] markboard;

        public Markboard()
        {
            markboard = new bool[Program.config.width, Program.config.height];
        }
        public void Init()
        {
            // 選択範囲の外は編集しないようにする
            for (int y = 0; y < Program.config.height; y++)
            {
                for (int x = 0; x < Program.config.width; x++)
                {
                    // 選択範囲なら偽、選択範囲外なら真
                    markboard[x, y] = !Program.selectionImg.Contains(x, y);
                }
            }
        }

        /// <summary>
        /// 編集不能にする
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Mark(int x, int y)
        {
            markboard[x, y] = true;
        }
        public void Mark(Point pt)
        {
            Mark(pt.X, pt.Y);
        }

        /// <summary>
        /// 編集可能
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Editable(int x, int y)
        {
            return !markboard[x, y];
        }
        /// <summary>
        /// 編集可能
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool Editable(Point pt)
        {
            return Editable(pt.X, pt.Y);
        }
    }
}
