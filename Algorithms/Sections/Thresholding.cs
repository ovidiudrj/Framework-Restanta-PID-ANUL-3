using System;
using Emgu.CV;
using Emgu.CV.Structure;
using Algorithms.Utilities;

namespace Algorithms.Sections
{
    public class Thresholding
    {
        #region MinErr Threshold
        public static byte MinErrThreshold(Image<Gray, byte> inputImage)
        {
            int[] H = Utils.ComputeHistogram(inputImage);

            int n = inputImage.Width * inputImage.Height;
            double[] p = new double[256];
            for (int k = 0; k <= 255; ++k)
            {
                p[k] = (double)H[k] / n;
            }

            byte T = 0;
            double minErr = 1000;

            for (int t = 0; t <= 255; ++t)
            {
                double P1 = 0;
                for (int k = 0; k <= t; ++k)
                {
                    P1 += p[k];
                }

                double P2 = 1.0 - P1;

                if (P1 <= 0 || P2 <= 0)
                    continue;

                double sumMu1 = 0;
                for (int k = 0; k <= t; ++k)
                {
                    sumMu1 += k * p[k];
                }
                double mu1 = sumMu1 / P1;

                double sumMu2 = 0;
                for (int k = t + 1; k <= 255; ++k)
                {
                    sumMu2 += k * p[k];
                }
                double mu2 = sumMu2 / P2;

                double sumSigma1 = 0;
                for (int k = 0; k <= t; ++k)
                {
                    sumSigma1 += (k - mu1) * (k - mu1) * p[k];
                }
                double s1 = sumSigma1 / P1;

                double sumSigma2 = 0;
                for (int k = t + 1; k <= 255; ++k)
                {
                    sumSigma2 += (k - mu2) * (k - mu2) * p[k];
                }
                double s2 = sumSigma2 / P2;

                const double min = 1e-10;
                if (s1 < min) s1 = min;
                if (s2 < min) s2 = min;

                double Err = 1.0 + P1 * Math.Log(s1) + P2 * Math.Log(s2)
                                 - 2.0 * P1 * Math.Log(P1) - 2.0 * P2 * Math.Log(P2);

                if (Err < minErr)
                {
                    minErr = Err;
                    T = (byte)t;
                }
            }
            return T;
        }
        #endregion
    }
}