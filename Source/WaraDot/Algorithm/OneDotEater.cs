using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using WaraDot.Algorithm.Sub;

namespace WaraDot.Algorithm
{
    /// <summary>
    /// 1ドットイーター
    /// 
    /// 1ドット浮いていれば、周りの色で置換します
    /// 
    /// - 読み先、書き先が分かれている
    /// - 読み先、書き先が同じでも使用可
    /// </summary>
    public class OneDotEater : IAlgorithm
    {
        /// <summary>
        /// アルゴリズム名
        /// </summary>
        public string Name { get { return "OneDotEater"; } }

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

        int done;

        static OneDotEater instance;
        public static IAlgorithm Instance(Form1 form1)
        {
            if (null == instance)
            {
                instance = new OneDotEater(form1);
            }
            return instance;
        }
        OneDotEater(Form1 form1)
        {
            form1_cache = form1;
            markboard = new Markboard();
            textLikeCursorIteration = new TextLikeCursorIteration();
        }
        public void Clear()
        {
            markboard.Clear();
            done = 0;
            form1_cache.SyncDone(done);
        }
        public void Init()
        {
            Clear();
            markboard.Init();
            textLikeCursorIteration.Init();
            form1_cache.SyncPos(textLikeCursorIteration.cursor);
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

            Trace.WriteLine("cur(" + textLikeCursorIteration.cursor.X + ", " + textLikeCursorIteration.cursor.Y + ") img(" + Program.config.width + ", " + Program.config.height + ") done="+done);

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
            // 指定した地点の色
            Color color2 = Program.config.layerOperation.GetLookingLayerPixel(textLikeCursorIteration.cursor);

            // 指定した地点の四方の色
            Color north = Color.Transparent;
            {
                if (textLikeCursorIteration.GoToNorth())
                {
                    north = Program.config.layerOperation.GetLookingLayerPixel(textLikeCursorIteration.cursor);
                }
                textLikeCursorIteration.BackFromNorth();
            }
            Color east = Color.Transparent;
            {
                if (textLikeCursorIteration.GoToEast())
                {
                    east = Program.config.layerOperation.GetLookingLayerPixel(textLikeCursorIteration.cursor);
                }
                textLikeCursorIteration.BackFromEast();
            }
            Color south = Color.Transparent;
            {
                if (textLikeCursorIteration.GoToSouth())
                {
                    south = Program.config.layerOperation.GetLookingLayerPixel(textLikeCursorIteration.cursor);
                }
                textLikeCursorIteration.BackFromSouth();
            }
            Color west = Color.Transparent;
            {
                if (textLikeCursorIteration.GoToWest())
                {
                    west = Program.config.layerOperation.GetLookingLayerPixel(textLikeCursorIteration.cursor);
                }
                textLikeCursorIteration.BackFromEast();
            }

            Color aroundColor = Color.Transparent;
            if (Color.Transparent != north) { aroundColor = north; }
            else if (Color.Transparent != east) { aroundColor = east; }
            else if (Color.Transparent != south) { aroundColor = south; }
            else if (Color.Transparent != west) { aroundColor = west; }

            if (Color.Transparent==aroundColor)
            {
                Trace.WriteLine("一致なし");
            }

            // 四方の色が全て同じで、現在地点が違う色の場合
            if (
                (Color.Transparent == north || aroundColor == north) &&
                (Color.Transparent == east || aroundColor == east) &&
                (Color.Transparent == south || aroundColor == south) &&
                (Color.Transparent == west || aroundColor == west) &&
                aroundColor != color2
                )
            {
                if (markboard.Editable(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y))
                {
                    Trace.WriteLine("イート！");

                    // 指定の地点を、周りの色で塗ります
                    form1_cache.DrawingColor = aroundColor;
                    bool drawed = false;
                    form1_cache.DrawDotByImage(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y, ref drawed);
                    if (drawed) { done++; form1_cache.SyncDone(done); }
                }

            }

            // 次の地点
            textLikeCursorIteration.GoToNext();

            form1_cache.SyncPos(textLikeCursorIteration.cursor);
        }

    }
}
