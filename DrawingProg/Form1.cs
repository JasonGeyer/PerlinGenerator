using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawingProg
{
    public partial class Form1 : Form
    {
        bool donePainting = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            int height = 100;
            int width = 100;
            double[] Noise = new double[width * height];
            double[] Noise0 = GetPerlinNoise(0, 0, width, height, 1);
            double[] Noise1 = GetPerlinNoise(0, 0, width, height, .5);
            double[] Noise2 = GetPerlinNoise(0, 0, width, height, .25);
            double[] Noise3 = GetPerlinNoise(0, 0, width, height, .125);
            double[] Noise4 = GetPerlinNoise(0, 0, width, height, .0625);

            for (int i = 0; i < Noise0.Length; i++)
            {
                Noise[i] = (Noise0[i] + Noise1[i] + Noise2[i] + Noise3[i] + Noise4[i]) / 5.0;
            }


            Noise = ApplyGradient(Noise, height, width, .007);

            Noise = NormalizeNoise(Noise);

            Noise = Threshold(Noise, .00, .1);

            int[] NoiseColor = GetColorFromNoise(Noise);

            if (!donePainting)
            {
                int xPos = 0;
                int yPos = 0;
                for (int i = 0; i < Noise.Length; i++)
                {
                    Color col = Color.FromArgb(255, NoiseColor[i],NoiseColor[i],NoiseColor[i]);
                    DrawDot(xPos, yPos, col);

                    xPos++;
                    if(xPos >= width)
                    {
                        xPos = 0;
                        yPos++;
                    }
                }
            }
        }

        private int[] GetColorFromNoise(double[] Noise)
        {
            int[] ColorFromNoise = new int[Noise.Length];
            for(int i = 0; i < Noise.Length; i++)
            {
                ColorFromNoise[i] = System.Convert.ToInt32(Noise[i] * 255);
            }
            return ColorFromNoise;
        }

        /* Creates a greater contrast of the values, since perlin results tend to float around 0.49-.051
         * 
         */
        private double[] NormalizeNoise (double[] Noise)
        {
            double[] NormNoise = new double[Noise.Length];
            double max = Noise.Max();
            double min = Noise.Min();
            for(int i = 0; i < Noise.Length; i++)
            {
                NormNoise[i] = (Noise[i] - min) / (max - min);
            }

            return NormNoise;
        }

        private double[] GetPerlinNoise(int startX, int startY, int width, int height, double scale)
        {
            double[] result = new double[width*height];

            int i = 0;
            for (int y = startY; y < height; y++)
            {
                for (int x = startX; x < width; x++ )
                {
                    result[i] = Perlin.perlin((startX+x+.01)*scale, (startY+y+.01)*scale, 0);
                    i++;
                }
            }
            return result;
        }

        private void DrawDot(int x, int y, Color color)
        {
            System.Drawing.Graphics graphicsObj;
            graphicsObj = this.CreateGraphics();
            SolidBrush myBrush = new System.Drawing.SolidBrush(color);
            graphicsObj.FillRectangle(myBrush, x * 4, y * 4, 4, 4);
        }

        private double[] Threshold(double[] Noise, double bottom, double top)
        {
            for(int i = 0; i < Noise.Length; i++)
            {
                if (Noise[i] > bottom && Noise[i] < top) Noise[i] = 1;
                else
                    Noise[i] = 0;
            }

            return Noise;
        }

        private double[] ApplyGradient(double[] Noise, int height, int width, double yScale)
        {
            int xPos = 0;
            int yPos = 0;

            for(int i = 0; i < Noise.Length; i++)
            {
                Noise[i] -= yScale * yPos;

                xPos++;
                if (xPos >= width)
                {
                    xPos = 0;
                    yPos++;
                }
            }

            return Noise;
        }
    }
}
