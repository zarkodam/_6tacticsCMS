using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace JukeBox.Utilities
{
    public static class ImageLoader
    {
        public static BitmapImage LoadFromPath(string imagePath)
         {
            string smallImageLocation = imagePath;
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(smallImageLocation);
            myBitmapImage.EndInit();
            return myBitmapImage;
         }
    }
}
