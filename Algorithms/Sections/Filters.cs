using Emgu.CV;
using Emgu.CV.Structure;
using System;

namespace Algorithms.Sections
{
    public class Filters
    {
        public static Image<Bgr, byte> SobelRGB(Image<Bgr, byte> inputImage)
        {
            int width = inputImage.Width;
            int height = inputImage.Height;
            var outputImage = new Image<Bgr, byte>(width, height);

            int[,] S_x = new int[3, 3]
            {
                { -1, 0, 1 },
                { -2, 0, 2 },
                { -1, 0, 1 }
            };

            int[,] S_y = new int[3, 3]
            {
                { -1, -2, -1 },
                {  0,  0,  0 },
                {  1,  2,  1 }
            };

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        double sum_X = 0;
                        double sum_Y = 0;

                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                byte val = inputImage.Data[y + i, x + j, c];
                                sum_X += val * S_x[i + 1, j + 1];
                                sum_Y += val * S_y[i + 1, j + 1];
                            }
                        }

                        double magnitude = Math.Sqrt(sum_X * sum_X + sum_Y * sum_Y);

                        if (magnitude > 255) magnitude = 255;
                        if (magnitude < 0) magnitude = 0;

                        outputImage.Data[y, x, c] = (byte)magnitude;
                    }
                }
            }

            return outputImage;
        }
    }
}