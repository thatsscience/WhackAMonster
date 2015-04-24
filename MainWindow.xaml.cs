using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;


namespace WhackAMonster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int score;
        
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer arm1 = new DispatcherTimer();
            arm1.Interval = new TimeSpan(0, 0, 0, 0, 100);
            arm1.Tick += new EventHandler(enterFrame);
            arm1.Start(); 
            
        }

        private async void arm2_Click(object sender, RoutedEventArgs e)
        {
            score++;
            scorebox.Text = score.ToString();
            arm2.Visibility = Visibility.Hidden;
            await Task.Delay(1000);
            arm2.Visibility = Visibility.Visible;
        }

        private async void arm1_Click(object sender, RoutedEventArgs e)
        {
            score++;
            scorebox.Text = score.ToString();
            arm1.Visibility = Visibility.Hidden;
            await Task.Delay(5000);
            arm1.Visibility = Visibility.Visible;
        }

        // copy from source to destination bitmap which needs to be writeable
        BitmapImage sourceBitmap = new BitmapImage(new Uri("pack://application:,,,/arm1.png"));
        WriteableBitmap destinationBitmap = null;
        const int nFrameHeight = 80, nFrames = 17;

        public Image[] arms = new Image[6];

        int nFrame = 0, x = 0;
        private void enterFrame(object sender, EventArgs e)
        {
            destinationBitmap = new WriteableBitmap((int)Arm1.Width, (int)Arm1.Height,
                96, 96, PixelFormats.Pbgra32, null);
            
            Arm1.Source = destinationBitmap;
            // make a copy buffer
            int nRowBytes = sourceBitmap.PixelWidth * sourceBitmap.Format.BitsPerPixel / 8;
            byte[] buffer = new byte[nRowBytes * nFrameHeight];
            // copy through buffer 
            
            sourceBitmap.CopyPixels(new Int32Rect(0, nFrame * nFrameHeight, sourceBitmap.PixelWidth, nFrameHeight), buffer, nRowBytes, 0);
            destinationBitmap.WritePixels(new Int32Rect(x, 0, sourceBitmap.PixelWidth, nFrameHeight), buffer, nRowBytes, 0);
            if (++nFrame == nFrames) nFrame = 0;

            if (x > (int)Arm1.Width - sourceBitmap.PixelWidth) x = 0;
            
        }

        private async void arm4_Click(object sender, RoutedEventArgs e)
        {
            score++;
            scorebox.Text = score.ToString();
            arm4.Visibility = Visibility.Hidden;
            await Task.Delay(5000);
            arm4.Visibility = Visibility.Visible;
        }

    }
}
