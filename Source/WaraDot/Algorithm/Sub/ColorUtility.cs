using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WaraDot.Algorithm.Sub
{
    public static class ColorUtility
    {
        /// <summary>
        /// 似ている色なら真
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsSimilarColor(Color a, Color b)
        {
            return Math.Abs(b.R - a.R) < 32 && Math.Abs(b.G - a.G) < 32 && Math.Abs(b.B - a.B) < 32;
        }
    }
}
