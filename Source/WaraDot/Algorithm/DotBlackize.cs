using System.Diagnostics;
using System.Drawing;
using WaraDot.Algorithm.Sub;

namespace WaraDot.Algorithm
{
    /// <summary>
    /// ドット・ブラッカイズ
    /// 
    /// ４方向の色を見て、透明を覗いて現在地点の色が一番白に近いとき、
    /// 自分より黒に近い隣の色を　もう少し黒くして、自分のはもっと白に近づけます。
    /// 
    /// 完全白だった場合は透明にします。
    /// 
    /// - 読み先、書き先が分かれているが、同じであることを想定
    /// - 読み先、書き先が同じでも使用可
    /// </summary>
    public class DotBlackize : IAlgorithm
    {
        /// <summary>
        /// アルゴリズム名
        /// </summary>
        public string Name { get { return "DotBlackize"; } }

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
        /// 効き目の強さ
        /// </summary>
        int multiplier = 2;//8;//
        /// <summary>
        /// 似ている色の許容範囲
        /// </summary>
        int sameRange = 16;
        /// <summary>
        /// 加工した数
        /// </summary>
        int done;

        static DotBlackize instance;
        public static IAlgorithm Instance(Form1 form1)
        {
            if (null == instance)
            {
                instance = new DotBlackize(form1);
            }
            return instance;
        }

        DotBlackize(Form1 form1)
        {
            form1_cache = form1;
            timeManager = new TimeManager();
            markboard = new Markboard();
            textLikeCursorIteration = new TextLikeCursorIteration();
        }
        public void Clear()
        {
            timeManager.Clear();
            markboard.Clear();

            // 加工前のビットマップを置いておき、これを元データとして見にいく
            Program.config.layerOperation.MemoryLayer();

            done = 0;
            form1_cache.SyncDone(done);
        }
        public void Init()
        {
            Clear();
            markboard.Init();
            textLikeCursorIteration.Init();
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

        void DrawWhiteout(int value)
        {
            // 指定した地点の色
            Color color2 = Program.config.layerOperation.GetBackgroundWorkingLayerPixel(textLikeCursorIteration.cursor);

            //int next = color2.R + 2*value;//ホワイトアウトの方が強く。
            int next = color2.R + value;
            if (255 < next)
            {
                // 完全白は消します
                form1_cache.DrawingColor = Color.Transparent;
                bool drawed = false;
                form1_cache.DrawDotByImage(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y, ref drawed);
                if (drawed) { done++; form1_cache.SyncDone(done); };
            }
            else
            {
                form1_cache.DrawingColor = Color.FromArgb(next, next, next);
                bool drawed = false;
                form1_cache.DrawDotByImage(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y, ref drawed);
                if (drawed) { done++; form1_cache.SyncDone(done); };
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
            Color color2 = Program.config.layerOperation.GetBackgroundWorkingLayerPixel(textLikeCursorIteration.cursor);

            if (255!=color2.A)
            {
                // 透明セルは無視
                goto gt_Next;
            }

            // 指定した地点の四方の色
            Color north = Color.Transparent;
            {
                if (textLikeCursorIteration.GoToNorth())
                {
                    north = Program.config.layerOperation.GetBackgroundWorkingLayerPixel(textLikeCursorIteration.cursor);
                }
                textLikeCursorIteration.BackFromNorth();
            }
            Color east = Color.Transparent;
            {
                if (textLikeCursorIteration.GoToEast())
                {
                    east = Program.config.layerOperation.GetBackgroundWorkingLayerPixel(textLikeCursorIteration.cursor);
                }
                textLikeCursorIteration.BackFromEast();
            }
            Color south = Color.Transparent;
            {
                if (textLikeCursorIteration.GoToSouth())
                {
                    south = Program.config.layerOperation.GetBackgroundWorkingLayerPixel(textLikeCursorIteration.cursor);
                }
                textLikeCursorIteration.BackFromSouth();
            }
            Color west = Color.Transparent;
            {
                if (textLikeCursorIteration.GoToWest())
                {
                    west = Program.config.layerOperation.GetBackgroundWorkingLayerPixel(textLikeCursorIteration.cursor);
                }
                textLikeCursorIteration.BackFromWest();
            }

            // 隣接する透明色の数
            int transparentCount = 0;
            if (north.A != 255) { transparentCount++; }
            if (east.A != 255) { transparentCount++; }
            if (south.A != 255) { transparentCount++; }
            if (west.A != 255) { transparentCount++; }

            // 現在地点が一番白に近いか判定。グレースケールの前提なので、赤だけ比較。
            // 隣接する、自分より黒に近いセルの数
            int blackerCount = 0;
            int sameColorCount = 0;
            if (north.A == 255) {
                if (north.R < color2.R - sameRange) { blackerCount++; }
                if (color2.R - sameRange <= north.R && north.R <= color2.R) { sameColorCount++; }
            }
            if (east.A == 255) {
                if (east.R < color2.R - sameRange) { blackerCount++; }
                if (color2.R - sameRange <= east.R && east.R <= color2.R) { sameColorCount++; }
            }
            if (south.A == 255) {
                if (south.R < color2.R - sameRange) { blackerCount++; }
                if (color2.R - sameRange <= south.R && south.R <= color2.R) { sameColorCount++; }
            }
            if (west.A == 255) {
                if (west.R < color2.R - sameRange) { blackerCount++; }
                if (color2.R - sameRange <= west.R && west.R <= color2.R) { sameColorCount++; }
            }


            // 透明の数＋自分より黒いセルの数＝４　なら、自分が一番白いと判定
            if (transparentCount + blackerCount == 4)
            {
                //Trace.WriteLine("ブラッカイズ！");

                if (markboard.Editable(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y))
                {
                    // 現在地点を、より白に近づけます
                    DrawWhiteout(multiplier * blackerCount);
                }

                //*
                // 周囲を黒に近づけます
                if (north.A == 255 && north.R < color2.R)
                {
                    if (textLikeCursorIteration.GoToNorth())
                    {
                        if (markboard.Editable(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y))
                        {
                            int next = north.R + multiplier * 1;
                            if (255 < next) { next = 255; }
                            form1_cache.DrawingColor = Color.FromArgb(next, next, next);
                            bool drawed = false;
                            form1_cache.DrawDotByImage(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y, ref drawed);
                            if (drawed) { done++; form1_cache.SyncDone(done); };
                        }
                    }
                    textLikeCursorIteration.BackFromNorth();
                }

                if (east.A == 255 && east.R < color2.R)
                {
                    if (textLikeCursorIteration.GoToEast())
                    {
                        if (markboard.Editable(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y))
                        {
                            int next = east.R + multiplier * 1;
                            if (255 < next) { next = 255; }
                            form1_cache.DrawingColor = Color.FromArgb(next, next, next);
                            bool drawed = false;
                            form1_cache.DrawDotByImage(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y, ref drawed);
                            if (drawed) { done++; form1_cache.SyncDone(done); };
                        }
                    }
                    textLikeCursorIteration.BackFromEast();
                }

                if (south.A == 255 && south.R < color2.R)
                {
                    if (textLikeCursorIteration.GoToSouth())
                    {
                        if (markboard.Editable(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y))
                        {
                            int next = south.R + multiplier * 1;
                            if (255 < next) { next = 255; }
                            form1_cache.DrawingColor = Color.FromArgb(next, next, next);
                            bool drawed = false;
                            form1_cache.DrawDotByImage(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y, ref drawed);
                            if (drawed) { done++; form1_cache.SyncDone(done); };
                        }
                    }
                    textLikeCursorIteration.BackFromSouth();
                }

                if (west.A == 255 && west.R < color2.R)
                {
                    if (textLikeCursorIteration.GoToWest())
                    {
                        if (markboard.Editable(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y))
                        {
                            int next = west.R + multiplier * 1;
                            if (255 < next) { next = 255; }
                            form1_cache.DrawingColor = Color.FromArgb(next, next, next);
                            bool drawed = false;
                            form1_cache.DrawDotByImage(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y, ref drawed);
                            if (drawed) { done++; form1_cache.SyncDone(done); };
                        }
                    }
                    textLikeCursorIteration.BackFromWest();
                }
                //*/
            }
            // 透明の数が３、（自分より黒いセルの数が０、または同色のセルの数が１）　なら、自分がはみ出た色と判定
            else if (transparentCount==3 && (blackerCount==0||sameColorCount==1))
            {
                if (markboard.Editable(textLikeCursorIteration.cursor.X, textLikeCursorIteration.cursor.Y))
                {
                    // 現在地点を、より白に近づけます
                    DrawWhiteout(multiplier * transparentCount);
                }
            }

            gt_Next:
            // 次の地点
            textLikeCursorIteration.GoToNext();
            form1_cache.SyncPos(textLikeCursorIteration.cursor);
        }

    }
}
