using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto_Cutter
{
    public partial class Form1 : Form
    {
        Bitmap b; 

        public Form1()
        {
            InitializeComponent();
        }
        List<int> gap;


        private void btn_Click(object sender, EventArgs e)
        {
            b = new Bitmap(@"C:\Users\Rowshan\Desktop\Test\image2.png");
            pic.Image = b;
            System.GC.Collect();

        }

        public void check(int T = 0)
        {
            gap = new List<int>();

            Color White = Color.White;
            Color Red = Color.Red;

            int R = 255;
            int G = 255;
            int B = 255;
            
            int RL = R - T;
            int GL = R - T;
            int BL = R - T;

            // Draw line to screen.

            for (int y = 0; y < b.Height; y++)
            {
                bool foundWhiteLine = true;
                for (int x = 0; x < b.Width; x++)
                {

                    Color pixel = b.GetPixel(x, y);

                    int TR = pixel.R;
                    int TG = pixel.G;
                    int TB = pixel.B;

                    bool BR = false;
                    bool BG = false;
                    bool BB = false;

                    if (TR <= R && TR >= RL) BR = true;
                    if (TG <= G && TG >= GL) BG = true;
                    if (TB <= B && TB >= BL) BB = true;

                    if (BR && BG && BB)
                    {
                        //b.SetPixel(x, y, Red);
                    }
                    else
                    {
                        foundWhiteLine = false;
                        break;
                    }

                }

                if (foundWhiteLine)
                {
                    gap.Add(y);
                    //for (int z = 0; z < b.Width; z++)
                    //{
                    //    b.SetPixel(z, y, Red);
                    //}
                }

            }
            fill();
          pic.Refresh();


        }


        private void fill()
        {
            int f;
            int s;
            int max = 0;
            int tm = 0;
            int index = 0;


            for(int x = 0; x < gap.Count-1; x++)
            {
                f = gap[x];
                s = gap[x+1];
                tm = s - f;
               if(tm > max)
                {
                    max = tm;
                    index = x;
                }
            }

            index = gap[index];

            for (int q = index; q < index + max; q++)
            {
                for (int p = 0; p < b.Width; p++)
                {
                    b.SetPixel(p, q, Color.Red);
                }
            }

        }

        private void nUP_ValueChanged(object sender, EventArgs e)
        {
            check((int)nUP.Value);
        }
    }
}
