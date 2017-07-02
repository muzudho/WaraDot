using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System;
using WaraDot.Algorithm.Sub;

namespace WaraDot.Algorithm
{
    /// <summary>
    /// ノイズキャンセラー
    /// 
    /// 12ドット以下の透明色以外の色のかたまりは、透明に置換します
    /// 
    /// - 読み先、書き先が分かれている
    /// - 読み先、書き先が同じでも使用可
    /// </summary>
    public class NoiseCanceler : IAlgorithm
    {
        /// <summary>
        /// アルゴリズム名
        /// </summary>
        public string Name { get { return "NoiseCanceler"; } }

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
        /// バケツ（塗りつぶし）のようなカーソル移動
        /// </summary>
        BucketsLikeCursorIteration bucketsLikeCursorIteration;

        /// <summary>
        /// 時間制御
        /// </summary>
        TimeManager timeManager;

        /// <summary>
        /// 加工前のビットマップ
        /// </summary>
        Bitmap beforeDrawingBitmap;

        /// <summary>
        /// 加工した数
        /// </summary>
        int done;

        static NoiseCanceler instance;
        public static IAlgorithm Instance(Form1 form1)
        {
            if (null == instance)
            {
                instance = new NoiseCanceler(form1);
            }
            return instance;
        }
        NoiseCanceler(Form1 form1)
        {
            form1_cache = form1;
            timeManager = new TimeManager();
            markboard = new Markboard();
            textLikeCursorIteration = new TextLikeCursorIteration();
            bucketsLikeCursorIteration = new BucketsLikeCursorIteration(form1);
        }
        public void Clear()
        {
            beforeDrawingBitmap = new Bitmap(Program.config.DrawingLayerBitmap);
            done = 0;
            timeManager.Clear();
            markboard.Clear();
            bucketsLikeCursorIteration.Clear();
        }
        public void Init()
        {
            Clear();
            markboard.Init();
            // テキスト読みの開始位置
            textLikeCursorIteration.Init();
            form1_cache.SyncPos(textLikeCursorIteration.currentPoint);
            // バケツ読みの開始位置
            bucketsLikeCursorIteration.Init(textLikeCursorIteration.currentPoint);
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

            // バケツスキャン
            bucketsLikeCursorIteration.BeginIteration();
            while (bucketsLikeCursorIteration.Iterate())
            {
                if (bucketsLikeCursorIteration.NextPointsCount < timeManager.countMax)
                {
                    DrawAndSearch();
                }
                else
                {
                    break;
                }
            }
            bucketsLikeCursorIteration.EndIteration();


            // テキスト読み走査
            if (!bucketsLikeCursorIteration.HasNextPoints())
            {
                if (!IsFinished())
                {
                    // 次の地点
                    textLikeCursorIteration.GoToNext();
                    form1_cache.SyncPos(textLikeCursorIteration.currentPoint);

                    // バケツの開始位置を変更
                    bucketsLikeCursorIteration.Init(textLikeCursorIteration.currentPoint);
                }
            }

            timeManager.IncleaseCapacity();
        }

        public bool IsFinishedScan()
        {
            return textLikeCursorIteration.IsFinished();
        }

        /// <summary>
        /// 再帰
        /// </summary>
        /// <param name="imgX"></param>
        /// <param name="imgY"></param>
        void DrawAndSearch()
        {
            // 指定した地点の色
            Color color2 = Program.config.GetLookingLayerPixel(bucketsLikeCursorIteration.CurrentPoint);

            if (true)//Color.Transparent != color2)//透明でない場合
            {
                // 指定の升
                {
                    // 指定の升はとりあえずマークする
                    markboard.Mark(bucketsLikeCursorIteration.CurrentPoint);

                    // ノイズは透明化
                    form1_cache.Color = Color.Red;//  Color.Transparent;
                    bool drawed = false;
                    form1_cache.DrawDotByImage(bucketsLikeCursorIteration.CurrentPoint, ref drawed);
                    if (drawed) { done++; };
                }

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
