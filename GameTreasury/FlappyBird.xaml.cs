using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameTreasury
{
    /// <summary>
    /// Interaction logic for FlappyBird.xaml
    /// </summary>
    public partial class FlappyBird : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        bool gameOver;
        double score;
        int graviti;
        Rect flappyRect;
        public FlappyBird()
        {
            InitializeComponent();
            myCanvas.Focus();

            timer.Tick += gameEngine;
            timer.Interval = TimeSpan.FromMilliseconds(20);

            gameOver = true;
            score = 0;
            graviti = 0;
        }
        private void keyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {

                graviti = -4;
                flappyBird.RenderTransform = new RotateTransform(-20, flappyBird.Width / 2, flappyBird.Height / 2);

            }
            if (e.Key == Key.Enter && gameOver == true)
            {

                startGame();
            }
        }

        private void keyIdUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {

                graviti = 4;
                flappyBird.RenderTransform = new RotateTransform(5, flappyBird.Width / 2, flappyBird.Height / 2);
            }
        }

        private void startGame()
        {


            Canvas.SetLeft(flappyBird, 85);
            Canvas.SetTop(flappyBird, 190);

            foreach (var x in myCanvas.Children.OfType<Image>())
            {

                if ((string)x.Tag == "pipe1")
                {
                    Canvas.SetLeft(x, 328);

                }
                if ((string)x.Tag == "pipe2")
                {
                    Canvas.SetLeft(x, 529);

                }
                if ((string)x.Tag == "pipe3")
                {
                    Canvas.SetLeft(x, 727);

                }

            }

            flappyBird.RenderTransform = new RotateTransform(5, flappyBird.Width / 2, flappyBird.Height / 2);
            showScore.Content = "Skor: 0";
            graviti = 4;
            gameOver = false;
            score = 0;
            timer.Start();

        }
        private void gameEngine(object sender, EventArgs e)
        {
            showScore.Content = "Tvoj skor:" + score;

            if (gameOver == false)
            {
                flappyRect = new Rect(Canvas.GetLeft(flappyBird), Canvas.GetTop(flappyBird), flappyBird.Width - 15, flappyBird.Height - 15);
                Canvas.SetTop(flappyBird, Canvas.GetTop(flappyBird) + graviti);

                if (Canvas.GetTop(flappyBird) + flappyBird.Height > 480 || Canvas.GetTop(flappyBird) < 0)
                {
                    // if it has then we end the game and show the reset game text
                    timer.Stop();
                    gameOver = true;
                    showScore.Content = "Tvoj skor je:" + score + Environment.NewLine + "Pritisni enter ako zelis ponovo da igras";
                }


                foreach (var x in myCanvas.Children.OfType<Image>())
                {

                    if ((string)x.Tag == "pipe1" || (string)x.Tag == "pipe2" || (string)x.Tag == "pipe3")
                    {
                        Rect pipeRect = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width - 15, x.Height - 15);

                        if (Canvas.GetLeft(x) < 0)
                        {
                            Canvas.SetLeft(x, 930);
                            score += 0.5;
                        }

                        else
                        {
                            Canvas.SetLeft(x, Canvas.GetLeft(x) - 8);
                        }
                        if (flappyRect.IntersectsWith(pipeRect))
                        {
                            gameOver = true;
                            timer.Stop();
                            showScore.Content = "Tvoj skor je:" + score + Environment.NewLine + "Pritisni enter ako zelis ponovo da igras";
                        }
                    }

                }
            }


        }
    }
}
