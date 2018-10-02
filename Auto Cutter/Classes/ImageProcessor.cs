using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Auto_Image_Croper.Classes
{
    class ImageProcessor
    {
        public string path;
        public Bitmap _bt = null;
        public Color _BackgroundColor = Color.White;
        public int Tolarate = 0;
        public bool Debug = false;

        public ImageProcessor(string FilePath)
        {
            path = FilePath;
            string p = Path.GetExtension(path).ToLower();
            if (p == ".jpg" || p == ".png")
            {
                try
                {
                    FileStream fs = new FileStream(FilePath, FileMode.Open);
                    _bt = new Bitmap(fs);
                    fs.Close();
                    fs.Dispose();
                } catch (FileNotFoundException e)
                {
                    //Log
                }
            }
        }

        public Bitmap getBitmap()
        {
            if( _bt != null)
            {
                return _bt;
            }
            else
            {
                return null;
            }
        }

        public CropArea FindCropArea()
        {
            if(_bt == null)
            {
                throw new System.InvalidOperationException("Bitmap not loaded");
            }

            bool wrongPixel = false;
            int imageHeight = _bt.Height;
            int imageWidth = _bt.Width;

            

            int R = _BackgroundColor.R;
            int G = _BackgroundColor.G;
            int B = _BackgroundColor.B;

            int RL = R - Tolarate < 0 ? 0 : R - Tolarate;
            int GL = G - Tolarate < 0 ? 0 : G - Tolarate;
            int BL = B- Tolarate < 0 ? 0 : B - Tolarate;

            int RH = R + Tolarate >= 255 ? 255 : R + Tolarate;
            int GH = G + Tolarate >= 255 ? 255 : G + Tolarate;
            int BH = B + Tolarate >= 255 ? 255 : B + Tolarate;

            Color pick;
            int TR, TG, TB;
            List<int> BlankRows = new List<int>();

            //Row
            for (int y = 0; y < imageHeight; y++)
            {
                wrongPixel = false;
                for(int i = 0; i< imageWidth; i++)
                {
                    pick = _bt.GetPixel(i, y);
                    TR = pick.R;
                    TG = pick.G;
                    TB = pick.B;
                                                          
                    if (TR > RH || TR < RL)
                    {
                        wrongPixel = true;
                        break;
                    }
                    else if(TG > GH || TG < GL)
                    {
                        wrongPixel = true;
                        break;
                    }
                    else if(TB > BH || TB < BL)
                    {
                        wrongPixel = true;
                        break;
                    }
                    if (Debug)
                    {
                        _bt.SetPixel(i, y, Color.Red);
                    }
                }
                if (! wrongPixel)
                {
                    BlankRows.Add(y);
                }
            }

            //Find the Biggest Gap
            int FirstPoint = 0;
            int SecondPoint = 0;
            int Distance = 0;
            int count = BlankRows.Count - 1;
            int tmpDistance;
            for(int x = 0; x < count; x++)
            {
                tmpDistance = BlankRows[x + 1] - BlankRows[x];
                if ( tmpDistance > Distance)
                {
                    Distance = tmpDistance;
                    FirstPoint = BlankRows[x];
                    SecondPoint = BlankRows[x +  1];
                }
            }

            CropArea C = new CropArea();
            C.A = new Point(0,FirstPoint);
            C.B = new Point(imageWidth, Distance);
            return C;
        }

        public void DrawBorder()
        {
            using(Graphics gr = Graphics.FromImage(_bt))
            {
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                
                CropArea C = this.FindCropArea();
                Rectangle rect = new Rectangle(C.A.X, C.A.Y, C.B.X , C.B.Y);
                
                //gr.FillEllipse(Brushes.LightBlue, rect);
                using (Pen thick_pen = new Pen(Color.Blue, 10))
                {
                    gr.DrawRectangle(thick_pen, rect);
                }
            }
        }

        public void crop()
        {

        }
        

        public void save()
        {
            string name = Path.GetFileNameWithoutExtension(path);
            name = name + "_crop";
            string newFile = Path.GetDirectoryName(path) + "\\" + name + Path.GetExtension(path);

            try
            {
                using (FileStream fss = new FileStream(newFile, FileMode.CreateNew))
                {
                    _bt.Save(fss, ImageFormat.Jpeg);
                    fss.Close();
                }
            }catch(System.IO.IOException e)
            {
                //Silently Ignore
            }
            
        }
               
    }
    class CropArea
    {
        public Point A;
        public Point B;
    }
}
