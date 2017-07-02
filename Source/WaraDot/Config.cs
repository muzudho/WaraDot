using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NLua;
using System.IO;
using System.Drawing;

namespace WaraDot
{
    public class Config
    {
        /// <summary>
        /// 画像の左端
        /// </summary>
        public int ox;

        /// <summary>
        /// 画像の上端
        /// </summary>
        public int oy;

        /// <summary>
        /// 画像の横幅
        /// </summary>
        public int width;

        /// <summary>
        /// 画像の縦幅
        /// </summary>
        public int height;

        /// <summary>
        /// 画像の拡大率
        /// </summary>
        public double scale;

        /// <summary>
        /// 描画対象レイヤー
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
        public static string GetImageFile(int layer)
        {
            return string.Format("Work/WaraLayer{0}.png", layer);
        }

        public string Dump()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DRAWING_LAYER = ");sb.Append(drawingLayer);sb.AppendLine();
            sb.Append("読込む画像ファイル = "); sb.Append(GetImageFile(drawingLayer)); sb.AppendLine();


            sb.Append("LAYERS_VISIBLE.Length = "); sb.Append(layersVisible.Length); sb.AppendLine();

            int i = 1;
            foreach (bool layerVisible in layersVisible)
            {
                sb.Append("LAYERS_VISIBLE["); sb.Append(i); sb.Append("] = "); sb.Append(layerVisible); sb.AppendLine();
                i++;
            }
            return sb.ToString();
        }

        public static Config ReloadLua(Form1 form1)
        {
            Config config = new Config();
            // ファイルの読み込み
            Program.lua.DoFile(Program.LUA_FILE);

            // Lua「debugOut("あー☆")」
            // ↓
            // C#「Console.WriteLine("あー☆")」
            // lua.RegisterFunction("debugOut", typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));

            // Lua「random(0,100)」
            // ↓
            // C#「System.Random(0,100)」
            // lua.RegisterFunction("random", typeof(System).GetMethod("Random", new Type[] { typeof(float), typeof(float) }));

            // init関数実行
            // lua.GetFunction("init").Call();

            // double型 か null か、はたまた想定外の型か
            // 画像の左端
            var value = Program.lua["OX"];
            if (!(value is double)) { value = 100d; }
            config.ox = (int)((double)value);

            // 画像の上端
            value = Program.lua["OY"];
            if (!(value is double)) { value = 100d; }
            config.oy = (int)((double)value);

            // 画像の横幅(300以下推奨)
            value = Program.lua["WIDTH"];
            if (!(value is double)) { value = 100d; }
            config.width = (int)((double)value);

            // 画像の縦幅(300以下推奨)
            value = Program.lua["HEIGHT"];
            if (!(value is double)) { value = 100d; }
            config.height = (int)((double)value);

            // 画像の倍率
            value = Program.lua["SCALE"];
            if (!(value is double)) { value = 1d; }
            config.scale = (double)value;

            // 描画対象レイヤー
            value = Program.lua["DRAWING_LAYER"];
            Trace.WriteLine("DRAWING_LAYER Type = " + value.GetType().Name);
            if (!(value is double)) { value = 0d; }
            config.drawingLayer = (int)((double)value);

            // 描画対象レイヤー
            LuaTable luaTable = Program.lua.GetTable("LAYERS_VISIBLE");
            Trace.WriteLine("LAYERS_VISIBLE Count = " + luaTable.Values.Count);
            config.layersVisible = new bool[luaTable.Values.Count+1];
            int i = 1;
            foreach (KeyValuePair<object,object> entry in luaTable)
            {
                Trace.WriteLine("layerVisible value Type = " + entry.Value.GetType().Name);
                config.layersVisible[i] = (bool)entry.Value;
                i++;
            }
            config.layersBitmap = new Bitmap[i];

#if DEBUG
            // ダンプ
            Trace.WriteLine(config.Dump());
#endif

            return config;
        }

        /// <summary>
        /// 描画中のレイヤー画像
        /// </summary>
        public Bitmap[] layersBitmap;
        public Bitmap GetDrawingLayerBitmap()
        {
            return layersBitmap[drawingLayer];
        }

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
            for (int i = 1; i < layersBitmap.Length; i++)
            {
                if (File.Exists(Config.GetImageFile(drawingLayer)))
                {
                    //// 画像をそのまま読込むと、形式が分からないので、Bitmapインスタンスに移し替える。
                    //// 出典: 「簡単な画像処理と読み込み・保存（C#）」 http://qiita.com/Toshi332/items/2749690489730f32e63d
                    layersBitmap[i] = new Bitmap(CreateImage(Config.GetImageFile(i)));
                }
                else
                {
                    layersBitmap[i] = new Bitmap(width, height);
                }
            }
        }
    }
}
