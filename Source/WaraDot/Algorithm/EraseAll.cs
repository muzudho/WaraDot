using System.Drawing;
using WaraDot.Algorithm.Sub;

namespace WaraDot.Algorithm
{
    /// <summary>
    /// イレーズ・オール
    /// 
    /// 透明で塗りつぶします
    /// 
    /// - 読み先、書き先が分かれている
    /// - 読み先、書き先が同じでも使用可
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

        /// <summary>
        /// 選択範囲の左上隅から右端へ、改行して左端から右端へ、といった順でカーソル移動
        /// </summary>
        TextLikeCursorIteration textLikeCursorIteration;

        /// <summary>
        /// 時間制御
        /// </summary>
        TimeManager timeManager;

        /// <summary>
        /// 加工した数
        /// </summary>
        int done;

        static EraseAll instance;
        public static IAlgorithm Instance(Form1 form1)
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
            timeManager = new TimeManager();
            markboard = new Markboard();
            textLikeCursorIteration = new TextLikeCursorIteration();
        }
        public void Init()
        {
            timeManager.Clear();

            // 加工前のビットマップを置いておき、これを元データとして見にいく
            Program.config.layerOperation.MemoryLayer();

            done = 0;
            form1_cache.SyncDone(done);

            markboard.Init();
            textLikeCursorIteration.Init();
            form1_cache.SyncPos(textLikeCursorIteration.cursor);
        }
        /// <summary>
        /// 中断
        /// </summary>
        public void Stop()
        {

        }

        public bool IsFinished()
        {
            return textLikeCursorIteration.IsFinished();
        }

        public void Tick()
        {
            if (IsFinished())
            {
                return;
            }

            timeManager.BeginIteration();
            while (timeManager.Iterate())
            {
                if (!IsFinished())
                {
                    DrawAndSearch();
                }
            }
            timeManager.EndIteration();

            timeManager.IncleaseCapacity();
        }

        /// <summary>
        /// Step() から呼び出される
        /// </summary>
        /// <param name="imgX"></param>
        /// <param name="imgY"></param>
        void DrawAndSearch()
        {
            if (markboard.Editable(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y))
            {
                // 指定した地点の色
                Color color2 = Program.config.layerOperation.GetBackgroundWorkingLayerPixel(textLikeCursorIteration.cursor);

                if (Color.Transparent != color2)
                {
                    // 透明化
                    form1_cache.DrawingColor = Color.Transparent;
                    bool drawed = false;
                    form1_cache.DrawDotByImage(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y, ref drawed);
                    if (drawed) { done++; };
                }
            }

            // 次の地点
            textLikeCursorIteration.GoToNext();
            form1_cache.SyncPos(textLikeCursorIteration.cursor);
        }

    }
}
