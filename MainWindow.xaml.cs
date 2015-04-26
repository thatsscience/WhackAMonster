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
            arm1.Interval = new TimeSpan(0, 0, 0, 0, 150);
            arm1.Tick += new EventHandler(enterFrame);
            arm1.Start();

            //DispatcherTimer arm2 = new DispatcherTimer();
            //arm2.Interval = new TimeSpan(0, 0, 0, 0, 150);
            //arm2.Tick += new EventHandler(enterFrame);
            //arm2.Start(); 
            
        }

        private async void arm_Click(object sender, RoutedEventArgs e)
        {
            if(sender == arm1)
            await Arm_Clicked(arm1, 5000);
            if(sender == arm2)
            await Arm_Clicked(arm2, 2000);
            if (sender == arm3)
            await Arm_Clicked(arm3, 1000);
            if (sender == arm4)
            await Arm_Clicked(arm4, 6000);

            
        }

        private async Task Arm_Clicked(Button arm, Int32 delay)
        {
            score++;
            scorebox.Text = score.ToString();
            arm.Visibility = Visibility.Hidden;
            await Task.Delay(delay);
            arm.Visibility = Visibility.Visible;
        }

        


        // copy from source to destination bitmap which needs to be writeable
        BitmapImage sourceBitmap = new BitmapImage(new Uri("pack://application:,,,/arm1.png"));
        WriteableBitmap destinationBitmap = null;
        const int frameHeight = 80, totalFrames = 17;

        int currentFrame = 0, x = 0;
        private void enterFrame(object sender, EventArgs e)
        {
            ArmHandler(Arm);
            ArmHandler(Arm2);
            ArmHandler(Arm3);
            ArmHandler(Arm4);
        }

        private void ArmHandler(Image Arm)
        {
            destinationBitmap = new WriteableBitmap((int)Arm.Width, (int)Arm.Height,
                96, 96, PixelFormats.Pbgra32, null);
            Arm.Source = destinationBitmap;

            // make a copy buffer
            int nRowBytes = sourceBitmap.PixelWidth * sourceBitmap.Format.BitsPerPixel / 8;
            byte[] buffer = new byte[nRowBytes * frameHeight];
            // copy through buffer 
            sourceBitmap.CopyPixels(new Int32Rect(0, currentFrame * frameHeight, sourceBitmap.PixelWidth, frameHeight), buffer, nRowBytes, 0);
            destinationBitmap.WritePixels(new Int32Rect(x, 0, sourceBitmap.PixelWidth, frameHeight), buffer, nRowBytes, 0);
            if (++currentFrame == totalFrames) currentFrame = 0;

            if (x > (int)Arm.Width - sourceBitmap.PixelWidth) x = 0;
        }
    }
}
