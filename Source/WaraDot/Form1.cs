﻿using NLua;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using WaraDot.Algorithm;
using System.Diagnostics;

namespace WaraDot
{
    public partial class Form1 : Form
    {
        static Form1()
        {
            rand = new Random(Environment.TickCount);
            drawingColor = SystemColors.Window;
        }

        /// <summary>
        /// 乱数
        /// </summary>
        public static Random rand;

        #region 操作している者
        /// <summary>
        /// 操作している者
        /// </summary>
        public OperatorType OperatorType
        {
            get
            {
                return operatorType;
            }
            set
            {
                operatorType = value;

                TopUserControl topUserControl1 = (TopUserControl)topPanel.Controls["topUserControl1"];
                topUserControl1.SyncOperatorType(value);
            }
        }
        OperatorType operatorType;
        #endregion

        #region 保存フラグ
        /// <summary>
        /// 編集した内容を、まだ保存していないなら真
        /// </summary>
        public bool Editing
        {
            get
            {
                return editing;
            }
            set
            {
                editing = value;

                TopUserControl topUserControl1 = (TopUserControl)topPanel.Controls["topUserControl1"];
                topUserControl1.SyncEditing(editing);
            }
        }
        bool editing;
        #endregion

        #region ペン色、ツールボックス
        static Color drawingColor;
        public Color DrawingColor
        {
            get
            {
                return drawingColor;
            }
            set
            {
                drawingColor = value;
                TopUserControl topUserControl1 = (TopUserControl)topPanel.Controls["topUserControl1"];
                topUserControl1.SyncColor(value);
            }
        }
        public void RandomColor()
        {
            // ランダム色打ち
            DrawingColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }

        public Tools GetTool()
        {
            TopUserControl topUserControl1 = (TopUserControl)topPanel.Controls["topUserControl1"];
            return topUserControl1.tools;
        }
        #endregion

        public void RefreshCanvas()
        {
            centerControl1.RefreshCanvas();
        }

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 設定再読込
        /// </summary>
        public void ReloadConfig()
        {
            Program.config = Config.ReloadLua(this);
            Program.config.layerOperation.ReloadLayerImages();
            centerControl1.OnReloadConfig();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Luaファイル読取
            ReloadConfig();

            #region 画像の読み込み、または新規作成
            Program.config.layerOperation.ReloadLayerImages();
            #endregion

            #region 保存フラグ
            // 初回のテキストボックスの内容変更は、未保存とは扱わない
            Editing = false;
            topUserControl1.SyncEditing(Editing);
            #endregion

            OperatorType = OperatorType.Human;

            // タイマーの実行
            timer1.Start();
        }

        /// <summary>
        /// [Ctrl]キーを押しているなら真
        /// </summary>
        public bool pressingCtrl;
        /// <summary>
        /// マウスの左ボタンを押しているなら真
        /// </summary>
        public bool pressingMouseLeft;

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Modifiers & Keys.Control) == Keys.Control)
            {
                if ((e.KeyData & Keys.S) == Keys.S)
                {
                    TopUserControl topUserControl1 = (TopUserControl)topPanel.Controls["topUserControl1"];
                    topUserControl1.Save();

                    // ビープ音を鳴らないようにする
                    e.SuppressKeyPress = true;
                }

                pressingCtrl = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                pressingCtrl = false;
            }
        }

        #region 描画
        public Point ToImage(int mouseX, int mouseY)
        {
            return centerControl1.ToImage(mouseX, mouseY);
        }

        /// <summary>
        /// 点を打ちます
        /// 出典:「線を描く」http://dobon.net/vb/dotnet/graphics/drawline.html
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        /// <param name="form1"></param>
        public void DrawDotByImage(int imgX, int imgY, ref bool drawed)
        {
            centerControl1.DrawDotByImage(imgX, imgY, this, ref drawed);
        }
        public void DrawDotByImage(Point pt, ref bool drawed)
        {
            DrawDotByImage(pt.X, pt.Y, ref drawed);
        }
        #endregion

        #region コンピューターの自動実行

        private void Timer1_Tick(object sender, EventArgs e)
        {
            // 現在使用中のアルゴリズム
            if (null != Program.currentAlgorithm)
            {
                if (Program.isCancelAlgorithm)
                {
                    Trace.WriteLine("中断を呼び出し");
                    Program.currentAlgorithm.Stop();
                    Program.currentAlgorithm = null;
                    Program.isCancelAlgorithm = false;
                }
                else if (Program.currentAlgorithm.IsFinished())
                {
                    Program.currentAlgorithm = null;
                    OperatorType = OperatorType.Human;
                }
                else
                {
                    Program.currentAlgorithm.Tick();
                    RefreshCanvas();
                }
            }
        }
        #endregion

        public void SyncPos(Point imgPt)
        {
            centerControl1.SyncPos(imgPt.X, imgPt.Y);
        }

        /// <summary>
        /// ペン先の位置
        /// </summary>
        /// <returns></returns>
        public Rectangle CursorRect
        {
            get
            {
                return centerControl1.cursorRect;
            }
        }

        /// <summary>
        /// [アルゴリズム] - [1ドットイーター]
        /// 1ドットの点を消したい
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Algorithm1DotEaterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.currentAlgorithm = OneDotEater.Instance(this);
            Program.currentAlgorithm.Init();
            OperatorType = OperatorType.Computer;
        }

        /// <summary>
        /// [アルゴリズム] - [ドット・ブラッカイズ]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlgorithmBlackizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.currentAlgorithm = DotBlackize.Instance(this);
            Program.currentAlgorithm.Init();
            OperatorType = OperatorType.Computer;
        }

        /// <summary>
        /// [アルゴリズム] - [ドット・アベレージ]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlgorithmDotAverageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.currentAlgorithm = DotAverage.Instance(this);
            Program.currentAlgorithm.Init();
            OperatorType = OperatorType.Computer;
        }

        /// <summary>
        /// [アルゴリズム] - [半透明の透明化]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DotTransparentClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.currentAlgorithm = DotTransparentClear.Instance(this);
            Program.currentAlgorithm.Init();
            OperatorType = OperatorType.Computer;
        }

        /// <summary>
        /// [選択範囲] - [解除]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectionCancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            centerControl1.ClearSelection();
            centerControl1.Refresh();
        }

        /// <summary>
        /// 全選択
        /// </summary>
        public void DoSelectionAll()
        {
            centerControl1.DoSelectionAll();
            centerControl1.Refresh();
        }

        /// <summary>
        /// [選択範囲] - [全選択]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectionAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoSelectionAll();
        }

        /// <summary>
        /// [アルゴリズム] - [ノイズキャンセラー]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlgorithmNoiseCancelerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.currentAlgorithm = NoiseCanceler.Instance(this);
            Program.currentAlgorithm.Init();
            OperatorType = OperatorType.Computer;
        }

        /// <summary>
        /// [アルゴリズム] - [イレーズ・オール]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlgorithmEraseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.currentAlgorithm = EraseAll.Instance(this);
            Program.currentAlgorithm.Init();
            OperatorType = OperatorType.Computer;
        }

        /// <summary>
        /// [作り直し操作] - [ノイズ]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemakeNoiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int r, g, b;

            // 全ピクセルにランダムに色を置いていくぜ☆（＾～＾）
            for (int y = 0; y < Program.config.layerOperation.DrawingLayerBitmap.Height; y++)
            {
                for (int x = 0; x < Program.config.layerOperation.DrawingLayerBitmap.Width; x++)
                {
                    r = Form1.rand.Next(256);
                    g = Form1.rand.Next(256);
                    b = Form1.rand.Next(256);
                    Program.config.layerOperation.DrawPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            RefreshCanvas();
        }

        /// <summary>
        /// [作り直し操作] - [白紙]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemakeWhitePaperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 全ピクセルにランダムに色を置いていくぜ☆（＾～＾）
            for (int y = 0; y < Program.config.layerOperation.DrawingLayerBitmap.Height; y++)
            {
                for (int x = 0; x < Program.config.layerOperation.DrawingLayerBitmap.Width; x++)
                {
                    Program.config.layerOperation.DrawPixel(x, y, Color.White);
                }
            }

            RefreshCanvas();
        }

        public void SyncDone(int done)
        {
            topUserControl1.SyncDone(done);
        }

        /// <summary>
        /// [アルゴリズム] - [中断]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AlgorithmBreakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Trace.WriteLine("中断を要求");
            Program.isCancelAlgorithm = true;
        }
    }
}
