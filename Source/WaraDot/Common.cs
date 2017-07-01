using System.Drawing;

namespace WaraDot
{
    public static class Common
    {
        static Common()
        {
            selectionImg = Rectangle.Empty;
        }

        /// <summary>
        /// 選択範囲。無ければエンプティ
        /// </summary>
        public static Rectangle selectionImg;

    }
}
