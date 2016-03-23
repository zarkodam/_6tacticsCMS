using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace _6tactics.Utilities.Common
{
    public class ImageUtility
    {

        public byte[] GetThumbnailAsBinary(string imageFile)
        {
            try
            {
                byte[] result;
                using (Image thumbnail = new Bitmap(160, 59))
                {
                    using (var source = new Bitmap(imageFile))
                    {
                        using (Graphics graphic = Graphics.FromImage(thumbnail))
                        {
                            graphic.SmoothingMode = SmoothingMode.HighQuality;
                            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            graphic.DrawImage(source, 0, 0, 160, 59);
                        }
                    }
                    using (var ms = new MemoryStream())
                    {
                        result = ms.ToArray();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return null;
            }
        }

        private void CalculateDimensionsByPercentage()
        {
            //...
        }

        public void SaveThumbnailOnDisk(string sourcePath, string destinationPath, int outputWidth, int outputHeight, ImageFormat outputFormat)
        {
            try
            {
                using (Image thumbnail = new Bitmap(outputWidth, outputHeight))
                {
                    using (var source = new Bitmap(sourcePath))
                    {
                        using (Graphics graphic = Graphics.FromImage(thumbnail))
                        {
                            graphic.SmoothingMode = SmoothingMode.HighQuality;
                            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            graphic.DrawImage(source, 0, 0, outputWidth, outputHeight);
                        }
                    }

                    thumbnail.Save(destinationPath, outputFormat);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

    }
}
