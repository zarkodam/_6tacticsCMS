using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace JukeBox.Utilities
{
    public static class ScrollPrepare
    {
        private static Canvas elementWrapper = new Canvas() { Width = 220, Background = Brushes.Transparent };
        enum Scroll { NewAndPopular, Music, Search, TopList, AlbumProfile, SongsOnQueue };

        private static void TopCanvas(string orderNumb = "")
        {
            Canvas topCanvas = new Canvas() { Width = 220, Height = 46};
            if (orderNumb != "")
            {
                topCanvas.Height = 46;
                topCanvas.Background = Brushes.Transparent;
                Rectangle topCanvasBg = new Rectangle() { Width = 220, Height = 20, Fill = new SolidColorBrush(Color.FromRgb(35, 35, 35)) };
                Canvas.SetTop(topCanvasBg, 26);
                topCanvas.Children.Add(topCanvasBg);
                Label orderNumber = new Label() { Content = orderNumb, FontFamily = new FontFamily("Dosis"), FontWeight = FontWeights.Light, Foreground = Brushes.White, FontSize = 35 };
                topCanvas.Children.Add(orderNumber);
            }
            else
            {
                topCanvas.Height = 20;
                topCanvas.Background = new SolidColorBrush(Color.FromRgb(35, 35, 35));
            }
                
            
            elementWrapper.Children.Add(topCanvas);
        }

        private static void SetImage(double yPos = 21)
        {
            Canvas imageWrapper = new Canvas() { Width = 220, Height = 220, Background = Brushes.Black };
            Image albumArtist = new Image();
            albumArtist.Source = ImageLoader.LoadFromFile(@"L:\jukeboxMusic\Bonobo\DaysToCome 2006\AlbumArtistBig.jpg");
            albumArtist.Width = 218;
            albumArtist.Height = 218;
            imageWrapper.Children.Add(albumArtist);

            //for time
            Canvas timeWrapper = new Canvas() { Width = 47, Height = 25, Background = Brushes.Black };
            Label time = new Label() { Content = "00:00", FontFamily = new FontFamily("Arial"), FontWeight = FontWeights.Light, Foreground = Brushes.White, FontSize = 15 };
            imageWrapper.Children.Add(timeWrapper);
            Canvas.SetRight(timeWrapper, 0);
            Canvas.SetTop(timeWrapper, 0);
            timeWrapper.Children.Add(time);

            elementWrapper.Children.Add(imageWrapper);
            Canvas.SetRight(albumArtist, 1);
            Canvas.SetTop(imageWrapper, yPos);
        }

        private static void SetHeadline(string elementHeadline, string elementSubheading = "", double yPos = 240)
        {
            Canvas textWrapper = new Canvas() { Width = 220, Height = 64, Background = new SolidColorBrush(Color.FromRgb(45, 45, 45)) };
            Label headline = new Label() { Content = elementHeadline, FontFamily = new FontFamily("Dosis"), FontWeight = FontWeights.SemiBold, Foreground = Brushes.White, FontSize = 16 };
            textWrapper.Children.Add(headline);
            Canvas.SetTop(headline, 8);
            Canvas.SetLeft(headline, 6);

            if (elementHeadline != "")
            {
                Label subheading = new Label() { Content = elementSubheading, FontFamily = new FontFamily("Dosis"), FontWeight = FontWeights.Light, Foreground = Brushes.White, FontSize = 15 };
                textWrapper.Children.Add(subheading);
                Canvas.SetTop(subheading, 29);
                Canvas.SetLeft(subheading, 6);
            }

            elementWrapper.Children.Add(textWrapper);
            Canvas.SetTop(textWrapper, yPos);
        }

        private static void SetLineOnTheBottom(double yPos = 302)
        {
            Canvas lineWrapper = new Canvas() { Width = 220, Height = 35, Background = Brushes.Transparent };
            Rectangle line = new Rectangle() { Width = 200, Height = 1, Fill = Brushes.Cornsilk };

            lineWrapper.Children.Add(line);
            Canvas.SetLeft(line, 10);
            Canvas.SetTop(line, 17);

            elementWrapper.Children.Add(lineWrapper);
            Canvas.SetTop(lineWrapper, yPos);
        }

        public static void NewAndPopular(Canvas scrollContentCanvas)
        {
            TopCanvas();
            SetImage();
            SetHeadline("Bonobo", "Days To Come...");
            SetLineOnTheBottom();

            scrollContentCanvas.Children.Add(elementWrapper);
            elementWrapper.Height = 337;
        }

        public static void Search(Canvas scrollContentCanvas)
        {
            TopCanvas();
            SetImage();
            SetHeadline("Bonobo", "Days To Come...");
            SetLineOnTheBottom();

            scrollContentCanvas.Children.Add(elementWrapper);
            elementWrapper.Height = 337;
        }

        public static void Music(Canvas scrollContentCanvas)
        {
            TopCanvas();
            SetImage();
            SetHeadline("Bonobo");
            SetLineOnTheBottom();

            scrollContentCanvas.Children.Add(elementWrapper);
            elementWrapper.Height = 337;
        }


        public static void TopList(Canvas scrollContentCanvas)
        {
            TopCanvas("01");
            SetImage(47);
            SetHeadline("Bonobo", "Days To Come...");

            SetLineOnTheBottom();
            scrollContentCanvas.Children.Add(elementWrapper);
            elementWrapper.Height = 337;
        }

        public static void SongsOnQueue(Canvas scrollContentCanvas)
        {
            //Rectangle topLineRect = new Rectangle() { Width = 60, Height = 20, Fill = Brushes.Cornsilk };
        }
    }
}
