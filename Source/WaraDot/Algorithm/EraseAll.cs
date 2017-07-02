using System.Drawing;

namespace WaraDot.Algorithm
{
    /// <summary>
    /// イレーズ・オール
    /// 
    /// 透明で塗りつぶします
    /// </summary>
    public class EraseAll : IAlgorithm
    {
        /// <summary>
        /// アルゴリズム名
        /// </summary>
        public string Name { get { return "EraseAll"; } }

        Form1 form1_cache;

        /// <summary>
        /// フラグが立っているところは編集しない
        /// </summary>
        Markboard markboard;

        Point currentPoint;
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

        static EraseAll instance;
        public static EraseAll Instance(Form1 form1)
        {
            if (null == instance)
            {
                instance = new EraseAll(form1);
            }
            return instance;
        }
        EraseAll(Form1 form1)
        {
            form1_cache = form1;
            markboard = new Markboard();
        }
        public void Clear()
        {
            markboard.Clear();
            beforeBitmap = new Bitmap(Program.config.GetDrawingLayerBitmap());
            done = 0;
        }
        public void Init()
        {
            markboard.Init();
            // スタート地点
            currentPoint = new Point(Program.selectionImg.X, Program.selectionImg.Y);
        }

        public bool IsFinished()
        {
            return currentPoint.X  == Program.config.width &&
                currentPoint.Y == Program.config.height;
        }

        public void Step()
        {
            if (IsFinished())
            {
                return;
            }

            for (int i = 0; i < countMax; i++)
            {
                if (!IsFinished())
                {
                    DrawAndSearch();
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
        /// Step() から呼び出される
        /// </summary>
        /// <param name="imgX"></param>
        /// <param name="imgY"></param>
        void DrawAndSearch()
        {
            if (markboard.Editable(currentPoint.X, currentPoint.Y))
            {
                // 指定した地点の色
                Color color2 = beforeBitmap.GetPixel(currentPoint.X, currentPoint.Y);

                if (Color.Transparent != color2)
                {
                    // 透明化
                    form1_cache.Color = Color.Transparent;
                    bool drawed = false;
                    form1_cache.DrawDotByImage(currentPoint.X, currentPoint.Y, ref drawed);
                    if (drawed) { done++; };
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

    }
}
