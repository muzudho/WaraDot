using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaraDot.Algorithm
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

        public void Init()
        {
            markboard = new bool[Program.config.width, Program.config.height];

            // 選択範囲の外は編集しないようにする
            for (int y = 0; y < Program.config.height; y++)
            {
                for (int x = 0; x < Program.config.width; x++)
                {
                    if (!Common.selectionImg.Contains(x, y))
                    {
                        markboard[x, y] = true;
                    }
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
    }
}
