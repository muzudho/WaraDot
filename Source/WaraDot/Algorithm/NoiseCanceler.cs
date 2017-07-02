using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        TextLikeCursorIteration textlike;

        /// <summary>
        /// バケツ（塗りつぶし）のようなカーソル移動
        /// </summary>
        BucketsLikeCursorIteration bucketslike;

        /// <summary>
        /// 時間制御
        /// </summary>
        TimeManager timeManager;

        /// <summary>
        /// 加工した数
        /// </summary>
        int done;

        List<Point> selection;
        Color startColor;

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
            textlike = new TextLikeCursorIteration();
            bucketslike = new BucketsLikeCursorIteration(form1);
            selection = new List<Point>();
        }
        public void Clear()
        {
            // 加工前のビットマップを置いておき、これを元データとして見にいく
            Program.config.layerOperation.MemoryLayer();

            done = 0;
            form1_cache.SyncDone(done);

            timeManager.Clear();
            markboard.Clear();
            bucketslike.Clear();
            selection.Clear();
        }
        public void Init()
        {
            Clear();
            markboard.Init();
            // テキスト読みの開始位置
            textlike.Init();
            form1_cache.SyncPos(textlike.cursor);
            // バケツ読みの開始位置
            bucketslike.Init(textlike.cursor);
            startColor = Program.config.layerOperation.GetBackgroundWorkingLayerPixel(textlike.cursor);
        }

        public bool IsFinished()
        {
            return textlike.IsFinished();
        }

        public void Tick()
        {
            if (IsFinished())
            {
                return;
            }

            bool nextText = false;

            if (Color.Transparent == startColor)
            {
                // 透明の色は処理しない
                nextText = true;
                goto gt_nextText;
            }

            // バケツスキャン
            bucketslike.BeginIteration();
            while (bucketslike.Iterate())
            {
                if (bucketslike.NextPointsCount < timeManager.countMax)
                {
                    DrawAndSearch();
                }
                else
                {
                    break;
                }
            }
            bucketslike.EndIteration();


            if (!bucketslike.HasNextPoints())
            {
                // バケツスキャンが終わった時
                if (1< selection.Count && selection.Count< 13)
                {
                    // 2ドット以上、13ドット未満はノイズと判定
                    Trace.WriteLine("selection.Count=" + selection.Count + " ノイズと判定");

                    // ノイズは透明化
                    foreach (Point pt in selection)
                    {
                        form1_cache.DrawingColor = Color.Transparent;
                        bool drawed = false;
                        form1_cache.DrawDotByImage(pt, ref drawed);
                        if (drawed) { done++; form1_cache.SyncDone(done); };
                    }
                }
                else
                {
                    Trace.WriteLine("selectionPoints.Count=" + selection.Count);
                }

                nextText = true;
            }

            // テキスト読み走査
            gt_nextText:
            if (nextText)
            {
                selection.Clear();

                // カーソルを進める
                textlike.GoToNext();
                form1_cache.SyncPos(textlike.cursor);

                // 境界判定
                if (!IsFinished())
                {
                    startColor = Program.config.layerOperation.GetBackgroundWorkingLayerPixel(textlike.cursor);

                    // バケツの開始位置を変更
                    bucketslike.Init(textlike.cursor);
                }
            }

            timeManager.IncleaseCapacity();
        }

        public bool IsFinishedScan()
        {
            return textlike.IsFinished();
        }

        /// <summary>
        /// 再帰
        /// </summary>
        /// <param name="imgX"></param>
        /// <param name="imgY"></param>
        void DrawAndSearch()
        {

            // 指定した地点の色
            Color color2 = Program.config.layerOperation.GetBackgroundWorkingLayerPixel(bucketslike.Cursor);

            if (ColorUtility.IsSimilarColor( startColor, color2 ))
            {
                // 指定の色と似ている色の升
                // 選択対象
                selection.Add(bucketslike.Cursor);
                // とりあえずマークする
                markboard.Mark(bucketslike.Cursor);

                // 上
                if (bucketslike.GoToNorth() && markboard.Editable(bucketslike.Cursor))
                {
                    bucketslike.MarkNextPoint();
                }
                bucketslike.BackFromNorth();
                // 右
                if (bucketslike.GoToEast() && markboard.Editable(bucketslike.Cursor))
                {
                    bucketslike.MarkNextPoint();
                }
                bucketslike.BackFromEast();
                // 下
                if (bucketslike.GoToSouth() && markboard.Editable(bucketslike.Cursor))
                {
                    bucketslike.MarkNextPoint();
                }
                bucketslike.BackFromSouth();
                // 左
                if (bucketslike.GoToWest() && markboard.Editable(bucketslike.Cursor))
                {
                    bucketslike.MarkNextPoint();
                }
                bucketslike.BackFromWest();
            }
        }

    }
}
