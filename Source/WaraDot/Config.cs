﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static Config ReloadLua(Form1 form1)
        {
            Config config = new Config();
            // ファイルの読み込み
            Form1.lua.DoFile(Form1.LUA_FILE);

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
            var value = Form1.lua["OX"];
            if (!(value is double)) { value = 100d; }
            config.ox = (int)((double)value);

            value = Form1.lua["OY"];
            if (!(value is double)) { value = 100d; }
            config.oy = (int)((double)value);

            value = Form1.lua["WIDTH"];
            if (!(value is double)) { value = 100d; }
            config.width = (int)((double)value);

            value = Form1.lua["HEIGHT"];
            if (!(value is double)) { value = 100d; }
            config.height = (int)((double)value);

            value = Form1.lua["SCALE"];
            if (!(value is double)) { value = 1d; }
            config.scale = (double)value;

            return config;
        }
    }
}