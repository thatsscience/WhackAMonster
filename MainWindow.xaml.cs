/*******************************************
 *  Authors: Nate Ivy & Bryant Morrill
 *  Date: 4/26/2015
 *  2530 Final Project: WHACK-A-MONSTER
 *******************************************/

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
        TimeSpan time;
        DispatcherTimer timer = new DispatcherTimer();
        BitmapImage sourceBitmap = new BitmapImage(new Uri("pack://application:,,,/Images/arm_shaded.png"));
        WriteableBitmap destinationBitmap = null;
        const int frameHeight = 80, totalFrames = 17;
        int currentFrame = 0, x = 0;
        public static int score;

        public MainWindow()
        {
            Start();
        }

        #region clock
        private void CountdownClock()
        {
            time = TimeSpan.FromSeconds(5);

            timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                clock.Text = "CLOCK: " + time.ToString("c");
                if (time == TimeSpan.Zero)
                {
                    GameOver();
                }
                time = time.Add(TimeSpan.FromSeconds(-1));
                if (time == TimeSpan.FromSeconds(10)) clock.Foreground = new SolidColorBrush(Colors.Red);

            }, Application.Current.Dispatcher);

            timer.Start();
        }
        #endregion

        #region add initials GAME OVER
        // FOR PRESENTATION

        //private void YesButton_Click(object sender, RoutedEventArgs e)
        //{
        //    InputBox.Visibility = System.Windows.Visibility.Collapsed;

        //    String input = InputTextBox.Text;
        //    finalScore.Text =input; // Add Input to our ListBox.

        //    InputTextBox.Text = String.Empty;
        //}

        //private void NoButton_Click(object sender, RoutedEventArgs e)
        //{
        //    InputBox.Visibility = System.Windows.Visibility.Collapsed;

        //    InputTextBox.Text = String.Empty;
        //}
        #endregion

        #region on click event handlers
        private void menu_screen_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void game_over_Click(object sender, RoutedEventArgs e)
        {
            // TO-DO
            //finalScore.Text = "";
            //Start();
        }

        private async void arm_Click(object sender, RoutedEventArgs e)
        {
            if (sender == arm1)
                await Arm_Clicked(arm1, 5000);
            if (sender == arm2)
                await Arm_Clicked(arm2, 2000);
            if (sender == arm3)
                await Arm_Clicked(arm3, 1000);
            if (sender == arm4)
                await Arm_Clicked(arm4, 6000);
        }

        private async Task Arm_Clicked(Button arm, Int32 delay)
        {
            score += 100;
            scorebox.Text = "SCORE: " + score.ToString();
            arm.Visibility = Visibility.Hidden;
            await Task.Delay(delay);
            arm.Visibility = Visibility.Visible;
        }
        #endregion

        #region spriting code
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
        #endregion

        #region start/end game
        private void StartGame()
        {
            enableBtns();
            menu_screen.IsEnabled = false;
            menu_screen.Visibility = Visibility.Hidden;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 180);
            timer.Tick += new EventHandler(enterFrame);
            timer.Start();
            CountdownClock();
        }

        private void Start()
        {
            InitializeComponent();
            game_over.IsEnabled = false;
            game_over.Visibility = Visibility.Hidden;
            disableBtns();
        }

        private void GameOver()
        {
            disableBtns();
            timer.Stop();
            Canvas.SetZIndex(game_over, 15);
            game_over.Visibility = Visibility.Visible;
            game_over.IsEnabled = true;
            Canvas.SetZIndex(finalScore, 15);
            finalScore.Text = score.ToString();
            scorebox.Text = "";
            clock.Text = "";

            //InputBox.Visibility = System.Windows.Visibility.Visible;
        }
        #endregion

        #region Button Toggle
        private void disableBtns()
        {
            arm1.IsEnabled = false;
            arm2.IsEnabled = false;
            arm3.IsEnabled = false;
            arm4.IsEnabled = false;

            arm1.Visibility = Visibility.Hidden;
            arm2.Visibility = Visibility.Hidden;
            arm3.Visibility = Visibility.Hidden;
            arm4.Visibility = Visibility.Hidden;
        }

        private void enableBtns()
        {
            arm1.IsEnabled = true;
            arm2.IsEnabled = true;
            arm3.IsEnabled = true;
            arm4.IsEnabled = true;

            arm1.Visibility = Visibility.Visible;
            arm2.Visibility = Visibility.Visible;
            arm3.Visibility = Visibility.Visible;
            arm4.Visibility = Visibility.Visible;
        }
        #endregion
    }
}
