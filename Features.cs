using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DIP_Activity_2
{
    internal class Features
    {
        public static void ApplyBasicCopy(Bitmap sourceImage, PictureBox targetPictureBox)
        {
            Color pixel;
            Bitmap processed = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    pixel = sourceImage.GetPixel(i, j);
                    processed.SetPixel(i, j, pixel);
                }
            }
            targetPictureBox.Image = processed;
        }

        public static Bitmap ConvertToGrayscale(Bitmap sourceImage)
        {
            Bitmap processed = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < processed.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    Color pixel = sourceImage.GetPixel(i, j);
                    byte gray = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                    processed.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            return processed; // Return the processed image
        }

        private static object lockObj = new object();

        public static Bitmap InvertColors(Bitmap sourceImage)
        {
            Bitmap processed = new Bitmap(sourceImage.Width, sourceImage.Height);

            lock (lockObj)
            {
                for (int i = 0; i < sourceImage.Width; i++)
                {
                    for (int j = 0; j < sourceImage.Height; j++)
                    {
                        Color pixel = sourceImage.GetPixel(i, j);
                        Color invertedPixel = Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                        processed.SetPixel(i, j, invertedPixel);
                    }
                }
            }

            return processed;
        }

        public static Bitmap CalculateHistogram(Bitmap sourceImage)
        {
            int[] histogram = new int[256]; // Assuming 8-bit image

            // Calculate histogram for the source image
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    Color pixel = sourceImage.GetPixel(i, j);
                    int grayValue = (int)((pixel.R + pixel.G + pixel.B) / 3);
                    histogram[grayValue]++;
                }
            }

            // Normalize the histogram values for display
            int maxCount = histogram.Max();
            for (int i = 0; i < histogram.Length; i++)
            {
                histogram[i] = (int)((double)histogram[i] / maxCount * 100);
            }

            // Create a bitmap to draw the histogram
            Bitmap processed = new Bitmap(256, 100);
            using (Graphics g = Graphics.FromImage(processed))
            {
                for (int i = 0; i < histogram.Length; i++)
                {
                    g.DrawLine(Pens.Black, i, 100, i, 100 - histogram[i]);
                }
            }

            return processed;
        }


        public static Bitmap ApplySepiaFilter(Bitmap sourceImage)
        {
            Bitmap processed = new Bitmap(sourceImage.Width, sourceImage.Height);

            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    Color pixel = sourceImage.GetPixel(i, j);

                    int tr = (int)(0.393 * pixel.R + 0.769 * pixel.G + 0.189 * pixel.B);
                    int tg = (int)(0.349 * pixel.R + 0.686 * pixel.G + 0.168 * pixel.B);
                    int tb = (int)(0.272 * pixel.R + 0.534 * pixel.G + 0.131 * pixel.B);

                    tr = (tr > 255) ? 255 : (tr < 0) ? 0 : tr;
                    tg = (tg > 255) ? 255 : (tg < 0) ? 0 : tg;
                    tb = (tb > 255) ? 255 : (tb < 0) ? 0 : tb;

                    processed.SetPixel(i, j, Color.FromArgb(tr, tg, tb));
                }
            }

            return processed;
        }

        public static Bitmap SubtractImages(Bitmap imageA, Bitmap imageB)
        {
            if (imageA != null && imageB != null)
            {
                // Resize images if they are of different sizes
                if (imageA.Size != imageB.Size)
                {
                    // Resize imageA to match imageB dimensions
                    Bitmap resizedImageA = new Bitmap(imageB.Width, imageB.Height);
                    using (Graphics g = Graphics.FromImage(resizedImageA))
                    {
                        g.DrawImage(imageA, 0, 0, imageB.Width, imageB.Height);
                    }
                    imageA = resizedImageA;
                }

                // Rest of the subtraction logic remains unchanged
                Bitmap resultImage = new Bitmap(imageB.Width, imageB.Height);
                Color greenScreen = Color.FromArgb(0, 255, 0); // Green screen color
                int threshold = 100; // Adjust this threshold according to your images

                for (int x = 0; x < imageB.Width; x++)
                {
                    for (int y = 0; y < imageB.Height; y++)
                    {
                        Color pixelB = imageB.GetPixel(x, y);
                        Color pixelA = imageA.GetPixel(x, y);

                        // Check if the pixel in imageB is close to the green screen color
                        int greenDifference = Math.Abs(pixelB.G - greenScreen.G);
                        int blueDifference = Math.Abs(pixelB.B - greenScreen.B);

                        if (greenDifference < threshold && blueDifference < threshold)
                        {
                            // Use pixel from imageA as green screen replacement
                            resultImage.SetPixel(x, y, pixelA);
                        }
                        else
                        {
                            // Keep the pixel from imageB
                            resultImage.SetPixel(x, y, pixelB);
                        }
                    }
                }

                return resultImage;
            }
            else
            {
                throw new ArgumentException("Please load valid images.");
            }
        }

    }
}
