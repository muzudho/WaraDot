using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaraDot
{
    public partial class Form1 : Form
    {
        public const string IMAGE_FILE = "WaraDot.png";

        /// <summary>
        /// 描画中の画像
        /// </summary>
        public Bitmap bitmap;

        public void RefreshCanvas()
        {
            ((CenterControl)centerPanel1.Controls["centerControl1"]).RefreshCanvas();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bitmap = new Bitmap(100, 100);

        }
    }
}
