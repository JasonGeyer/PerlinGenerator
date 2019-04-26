using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawingProg
{
    class PerlinDrawer
    {
        //draws the perlin noise when initialzed, defaulting at 100
        public PerlinDrawer(Form targetForm, int height = 100, int width = 100)
        {
            DrawPerlinNoise(targetForm, height, width);
        }

        //Gets the noise and draws
        private void DrawPerlinNoise(Form targetForm, int height, int width)
        {
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

            int xPos = 0;
            int yPos = 0;
            for (int i = 0; i < Noise.Length; i++)
            {
                Color col = Color.FromArgb(255, NoiseColor[i], NoiseColor[i], NoiseColor[i]);
                DrawDot(targetForm, xPos, yPos, col);

                xPos++;
                if (xPos >= width)
                {
                    xPos = 0;
                    yPos++;
                }
            }
        }

        //converts noise (0-1 range) into a 0-255 int to represent color.
        private int[] GetColorFromNoise(double[] Noise)
        {
            int[] ColorFromNoise = new int[Noise.Length];
            for (int i = 0; i < Noise.Length; i++)
            {
                ColorFromNoise[i] = System.Convert.ToInt32(Noise[i] * 255);
            }
            return ColorFromNoise;
        }

        // Creates a greater contrast of the values, since perlin results tend to float around 0.49-.051
        private double[] NormalizeNoise(double[] Noise)
        {
            double[] NormNoise = new double[Noise.Length];
            double max = Noise.Max();
            double min = Noise.Min();
            for (int i = 0; i < Noise.Length; i++)
            {
                NormNoise[i] = (Noise[i] - min) / (max - min);
            }

            return NormNoise;
        }

        //Gets height * width perlin values in a 1d array
        private double[] GetPerlinNoise(int startX, int startY, int width, int height, double scale)
        {
            double[] result = new double[width * height];

            int i = 0;
            for (int y = startY; y < height; y++)
            {
                for (int x = startX; x < width; x++)
                {
                    result[i] = Perlin.perlin((startX + x + .01) * scale, (startY + y + .01) * scale, 0);
                    i++;
                }
            }
            return result;
        }

        //draws a filled rectangle 4 pixels at the x,y position with the input color
        private void DrawDot(Form target, int x, int y, Color color)
        {
            Graphics graphicsObj;
            graphicsObj =target.CreateGraphics();
            SolidBrush myBrush = new System.Drawing.SolidBrush(color);
            graphicsObj.FillRectangle(myBrush, x * 4, y * 4, 4, 4);
        }

        //anything between top and bottom will be increased to a value of 1. anything else will be 0.
        private double[] Threshold(double[] Noise, double bottom, double top)
        {
            for (int i = 0; i < Noise.Length; i++)
            {
                if (Noise[i] > bottom && Noise[i] < top) Noise[i] = 1;
                else
                    Noise[i] = 0;
            }

            return Noise;
        }

        //decreases the noise value by yScale in a gradient along the y axis of a generated perlin noise array
        private double[] ApplyGradient(double[] Noise, int height, int width, double yScale)
        {
            int xPos = 0;
            int yPos = 0;

            for (int i = 0; i < Noise.Length; i++)
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
