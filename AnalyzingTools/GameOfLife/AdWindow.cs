using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace GameOfLife
{
    class AdWindow : Window
    {
        private readonly DispatcherTimer adTimer;
        private readonly List<BitmapImage> images;
        private readonly ImageBrush myBrush;
        private int imgNmb;     // the number of the image currently shown
        private string link;    // the URL where the currently shown ad leads to
        
    
        public AdWindow(Window owner)
        {
            images = new List<BitmapImage>()
            {
                new BitmapImage(new Uri("ad1.jpg", UriKind.Relative)),
                new BitmapImage(new Uri("ad2.jpg", UriKind.Relative)),
                new BitmapImage(new Uri("ad3.jpg", UriKind.Relative))
            };
            myBrush = new ImageBrush();
            Random rnd = new Random();
            Owner = owner;
            Width = 350;
            Height = 100;
            ResizeMode = ResizeMode.NoResize;
            WindowStyle = WindowStyle.ToolWindow;
            Title = "Support us by clicking the ads";
            Cursor = Cursors.Hand;
            ShowActivated = false;
            MouseDown += OnClick;
            
            imgNmb = rnd.Next(0, 2);

            // Run the timer that changes the ad's image 
            adTimer = new DispatcherTimer();
            adTimer.Interval = TimeSpan.FromSeconds(3);
            adTimer.Tick += ChangeAds;
            adTimer.Start();
        }

        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(link);
            Close();
        }
        
        protected override void OnClosed(EventArgs e)
        {
            Unsubscribe();
            base.OnClosed(e);
        } 

        public void Unsubscribe()
        {
            adTimer.Tick -= ChangeAds;
        }

        private void ChangeAds(object sender, EventArgs eventArgs)
        {
            myBrush.ImageSource = images[imgNmb++];
            Background = myBrush;
            link = "http://example.com";
            if(imgNmb >= 2)
            {
                imgNmb = 0;
            }
        }
    }
}