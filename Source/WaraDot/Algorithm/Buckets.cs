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
        Color color_cache;

        /// <summary>
        /// フラグが立っているところは編集しない
        /// </summary>
        Markboard markboard;

        /// <summary>
        /// バケツ（塗りつぶし）のようなカーソル移動
        /// </summary>
        BucketsLikeCursorIteration bucketsLikeCursorIteration;

        /// <summary>
        /// 時間制御
        /// </summary>
        TimeManager timeManager;

        /// <summary>
        /// 加工した数
        /// </summary>
        int done;

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
        public void Clear()
        {
            timeManager.Clear();
            bucketsLikeCursorIteration.Clear();
            markboard.Clear();
        }
        public void Init()
        {
            Clear();
            markboard.Init();
            bucketsLikeCursorIteration.Init(form1_cache.ToImage(form1_cache.CursorRect.X, form1_cache.CursorRect.Y));

            // マウス押下した地点の色
            color_cache = Program.config.LookingLayerBitmap.GetPixel(bucketsLikeCursorIteration.nextPoints[0].X, bucketsLikeCursorIteration.nextPoints[0].Y);
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
            markboard.Mark(bucketsLikeCursorIteration.CurrentPoint);

            // 指定した地点の色
            Color color2 = Program.config.GetLookingLayerPixel(bucketsLikeCursorIteration.CurrentPoint);

            if (color2.Equals( color_cache))//一致した場合
            {
                // 指定の地点をまず描画
                bool drawed = false;
                form1_cache.DrawDotByImage( bucketsLikeCursorIteration.CurrentPoint, ref drawed);
                if (drawed)
                {
                    done++;

                    // 上
                    if (bucketsLikeCursorIteration.GoToNorth() && markboard.Editable(bucketsLikeCursorIteration.CurrentPoint))
                    {
                        bucketsLikeCursorIteration.MarkNextPoint();
                    }
                    bucketsLikeCursorIteration.BackFromNorth();
                    // 右
                    if (bucketsLikeCursorIteration.GoToEast() && markboard.Editable(bucketsLikeCursorIteration.CurrentPoint))
                    {
                        bucketsLikeCursorIteration.MarkNextPoint();
                    }
                    bucketsLikeCursorIteration.BackFromEast();
                    // 下
                    if (bucketsLikeCursorIteration.GoToSouth() && markboard.Editable(bucketsLikeCursorIteration.CurrentPoint))
                    {
                        bucketsLikeCursorIteration.MarkNextPoint();
                    }
                    bucketsLikeCursorIteration.BackFromSouth();
                    // 左
                    if (bucketsLikeCursorIteration.GoToWest() && markboard.Editable(bucketsLikeCursorIteration.CurrentPoint))
                    {
                        bucketsLikeCursorIteration.MarkNextPoint();
                    }
                    bucketsLikeCursorIteration.BackFromWest();
                }
            }

        }

    }
}
