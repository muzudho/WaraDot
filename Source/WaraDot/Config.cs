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
        public Config()
        {
            layerOperation = new LayerOperation();
        }

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

        public string Dump()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("LOOKING_LAYER = ["); sb.Append(layerOperation.lookingLayer); sb.Append("] "); sb.Append(LayerOperation.GetImageFileName(layerOperation.lookingLayer)); sb.AppendLine();
            sb.Append("DRAWING_LAYER = [");sb.Append(layerOperation.drawingLayer); sb.Append("] "); sb.Append(LayerOperation.GetImageFileName(layerOperation.drawingLayer)); sb.AppendLine();

            sb.Append("LAYERS_VISIBLE.Length = "); sb.Append(layerOperation.layersVisible.Length); sb.AppendLine();

            int i = 1;
            foreach (bool layerVisible in layerOperation.layersVisible)
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


            // アルゴリズムが作業用として用いるレイヤー
            value = Program.lua["BACKGROUND_WORKING_LAYER"];
            Trace.WriteLine("BACKGROUND_WORKING_LAYER Type = " + value.GetType().Name);
            if (!(value is double)) { value = 0d; }
            config.layerOperation.backgroundWorkingLayer = (int)((double)value);

            // アルゴリズムが見る元のレイヤー
            value = Program.lua["LOOKING_LAYER"];
            Trace.WriteLine("LOOKING_LAYER Type = " + value.GetType().Name);
            if (!(value is double)) { value = 0d; }
            config.layerOperation.lookingLayer = (int)((double)value);

            // ドローイング・ツール、または  アルゴリズムが描く先のレイヤー
            value = Program.lua["DRAWING_LAYER"];
            Trace.WriteLine("DRAWING_LAYER Type = " + value.GetType().Name);
            if (!(value is double)) { value = 0d; }
            config.layerOperation.drawingLayer = (int)((double)value);

            // 描画対象レイヤー
            LuaTable luaTable = Program.lua.GetTable("LAYERS_VISIBLE");
            Trace.WriteLine("LAYERS_VISIBLE Count = " + luaTable.Values.Count);
            config.layerOperation.layersVisible = new bool[luaTable.Values.Count+1];
            int i = 1;
            foreach (KeyValuePair<object,object> entry in luaTable)
            {
                Trace.WriteLine("layerVisible value Type = " + entry.Value.GetType().Name);
                config.layerOperation.layersVisible[i] = (bool)entry.Value;
                i++;
            }
            config.layerOperation.layersBitmap = new Bitmap[i];

#if DEBUG
            // ダンプ
            Trace.WriteLine(config.Dump());
#endif

            return config;
        }

        /// <summary>
        /// レイヤー操作
        /// </summary>
        public LayerOperation layerOperation;
    }
}
