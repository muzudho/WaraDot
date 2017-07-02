using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace WaraDot
{
    /// <summary>
    /// レイヤー操作
    /// </summary>
    public class LayerOperation
    {
        /// <summary>
        /// アルゴリズムが作業用として用いるレイヤー
        /// </summary>
        public int backgroundWorkingLayer;

        /// <summary>
        /// アルゴリズムが見る元のレイヤー
        /// </summary>
        public int lookingLayer;

        /// <summary>
        /// ドローイング・ツール、または  アルゴリズムが描く先のレイヤー
        /// </summary>
        public int drawingLayer;

        /// <summary>
        /// 描画対象レイヤー
        /// </summary>
        public bool[] layersVisible;
        public int GetLayersCount()
        {
            return layersVisible.Length;
        }

        /// <summary>
        /// 出力画像ファイル名
        /// </summary>
        public static string GetImageFileName(int iLayer)
        {
            return string.Format("Work/WaraLayer{0}.png", iLayer);
        }

        /// <summary>
        /// 描画中のレイヤー画像
        /// </summary>
        public Bitmap[] layersBitmap;
        /// <summary>
        /// アルゴリズムが作業用として用いるレイヤー
        /// </summary>
        public Bitmap BackgroundWorkingLayerBitmap { get { return layersBitmap[backgroundWorkingLayer]; } set { layersBitmap[backgroundWorkingLayer] = value; } }
        public Color GetBackgroundWorkingLayerPixel(Point pt) { return BackgroundWorkingLayerBitmap.GetPixel(pt.X, pt.Y); }
        /// <summary>
        /// アルゴリズムが見る元のレイヤー
        /// </summary>
        public Bitmap LookingLayerBitmap { get { return layersBitmap[lookingLayer]; } }
        public Color GetLookingLayerPixel(Point pt) { return LookingLayerBitmap.GetPixel(pt.X, pt.Y); }
        /// <summary>
        /// ドローイング・ツール、または  アルゴリズムが描く先のレイヤー
        /// </summary>
        public Bitmap DrawingLayerBitmap { get { return layersBitmap[drawingLayer]; } }

        /// <summary>
        /// 指定したファイルをロックせずに、System.Drawing.Imageを作成する。
        /// 出典: 「表示中の画像ファイルが削除できない問題の解決法」http://dobon.net/vb/dotnet/graphics/drawpicture2.html
        /// </summary>
        /// <param name="filename">作成元のファイルのパス</param>
        /// <returns>作成したSystem.Drawing.Image。</returns>
        public static System.Drawing.Image CreateImage(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(
                filename,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
            fs.Close();
            return img;
        }

        /// <summary>
        /// レイヤー画像の読込
        /// </summary>
        public void ReloadLayerImages()
        {
            for (int iLayer = 1; iLayer < layersBitmap.Length; iLayer++)
            {
                // if (File.Exists(Config.GetImageFile(drawingLayer)))
                if (File.Exists(GetImageFileName(iLayer)))
                {
                    //// 画像をそのまま読込むと、形式が分からないので、Bitmapインスタンスに移し替える。
                    //// 出典: 「簡単な画像処理と読み込み・保存（C#）」 http://qiita.com/Toshi332/items/2749690489730f32e63d
                    layersBitmap[iLayer] = new Bitmap(CreateImage(GetImageFileName(iLayer)));
                }
                else
                {
                    layersBitmap[iLayer] = new Bitmap(Program.config.width, Program.config.height);
                }
            }
        }

        /// <summary>
        /// 見にいくレイヤー画像を、作業用レイヤーにコピーしておく
        /// </summary>
        public void MemoryLayer()
        {
            BackgroundWorkingLayerBitmap = new Bitmap(LookingLayerBitmap);
        }
        /// <summary>
        /// 描画する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void DrawPixel(int x, int y, Color color)
        {
            DrawingLayerBitmap.SetPixel(x, y, color);
        }
    }
}
