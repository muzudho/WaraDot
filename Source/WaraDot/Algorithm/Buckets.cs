using System.Collections.Generic;
using System.Drawing;
using WaraDot.Algorithm.Sub;

namespace WaraDot.Algorithm
{
    /// <summary>
    /// 1万ピクセルなら一瞬で塗りつぶせるが、
    /// 150x150=22500 ピクセルになるとスタックオーバーフローしてしまう。
    /// 
    /// - 読み先、書き先が分かれている
    /// - 読み先、書き先が同じでも使用可
    /// </summary>
    public class Buckets : IAlgorithm
    {
        /// <summary>
        /// アルゴリズム名
        /// </summary>
        public string Name { get { return "Buckets"; } }

        Form1 form1_cache;

        static Buckets instance;
        public static IAlgorithm Instance(Form1 form1)
        {
            if (null== instance)
            {
                instance = new Buckets(form1);
            }
            return instance;
        }
        Buckets(Form1 form1)
        {
            form1_cache = form1;
            markboard = new Markboard();
            bucketsLikeCursorIteration = new BucketsLikeCursorIteration(form1);
            timeManager = new TimeManager();
        }


        /// <summary>
        /// バケツ（塗りつぶし）のようなカーソル移動
        /// </summary>
        BucketsLikeCursorIteration bucketsLikeCursorIteration;

        Color color_cache;

        /// <summary>
        /// 加工した数
        /// </summary>
        int done;

        /// <summary>
        /// フラグが立っているところは編集しない
        /// </summary>
        Markboard markboard;

        /// <summary>
        /// 時間制御
        /// </summary>
        TimeManager timeManager;

        public void Init()
        {
            color_cache = Color.Transparent;
            done = 0; form1_cache.SyncDone(done);
            timeManager.Clear();

            // 加工前のビットマップを置いておき、これを元データとして見にいく
            Program.config.layerOperation.MemoryLayer();

            markboard.Init();
            bucketsLikeCursorIteration.Init(form1_cache.ToImage(form1_cache.CursorRect.X, form1_cache.CursorRect.Y));

            // マウス押下した地点の色
            color_cache = Program.config.layerOperation.GetBackgroundWorkingLayerPixel(bucketsLikeCursorIteration.nextPoints[0]);
        }
        /// <summary>
        /// 中断
        /// </summary>
        public void Stop()
        {

        }


        public bool IsFinished()
        {
            return bucketsLikeCursorIteration.IsFinished();
        }

        public void Tick()
        {
            if (IsFinished())
            {
                return;
            }

            bucketsLikeCursorIteration.BeginIteration();
            while (bucketsLikeCursorIteration.Iterate())
            {
                if(bucketsLikeCursorIteration.NextPointsCount < timeManager.countMax)
                {
                    DrawAndSearch();
                }
                else
                {
                    break;
                }
            }
            bucketsLikeCursorIteration.EndIteration();

            timeManager.IncleaseCapacity();
        }

        /// <summary>
        /// Step() から呼び出される
        /// </summary>
        /// <param name="imgX"></param>
        /// <param name="imgY"></param>
        void DrawAndSearch()
        {
            // 指定の升はとりあえずマークする
            markboard.Mark(bucketsLikeCursorIteration.Cursor);

            // 指定した地点の色
            Color color2 = Program.config.layerOperation.GetBackgroundWorkingLayerPixel(bucketsLikeCursorIteration.Cursor);

            if (color2.Equals( color_cache))//一致した場合
            {
                // 指定の地点をまず描画
                bool drawed = false;
                form1_cache.DrawDotByImage( bucketsLikeCursorIteration.Cursor, ref drawed);
                if (drawed)
                {
                    done++;
                    form1_cache.SyncDone(done);

                    // 上
                    if (bucketsLikeCursorIteration.GoToNorth() && markboard.Editable(bucketsLikeCursorIteration.Cursor))
                    {
                        bucketsLikeCursorIteration.MarkNextPoint();
                    }
                    bucketsLikeCursorIteration.BackFromNorth();
                    // 右
                    if (bucketsLikeCursorIteration.GoToEast() && markboard.Editable(bucketsLikeCursorIteration.Cursor))
                    {
                        bucketsLikeCursorIteration.MarkNextPoint();
                    }
                    bucketsLikeCursorIteration.BackFromEast();
                    // 下
                    if (bucketsLikeCursorIteration.GoToSouth() && markboard.Editable(bucketsLikeCursorIteration.Cursor))
                    {
                        bucketsLikeCursorIteration.MarkNextPoint();
                    }
                    bucketsLikeCursorIteration.BackFromSouth();
                    // 左
                    if (bucketsLikeCursorIteration.GoToWest() && markboard.Editable(bucketsLikeCursorIteration.Cursor))
                    {
                        bucketsLikeCursorIteration.MarkNextPoint();
                    }
                    bucketsLikeCursorIteration.BackFromWest();
                }
            }

        }

    }
}
