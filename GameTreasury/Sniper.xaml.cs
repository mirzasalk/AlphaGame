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
 
    public partial class Sniper : Window
    {
        ImageBrush backGroundImage = new ImageBrush();
        ImageBrush ghostImage = new ImageBrush();
        List<int> topLocations;
        List<int> bottomLocations;
        List<Rectangle> RemoveList = new List<Rectangle>();
        Random rand = new Random();
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer gostTime = new DispatcherTimer();
        int misses;
        int score;
        public Sniper()
        {
            InitializeComponent();
            this.Cursor = Cursors.None;

            backGroundImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/background.png"));
            myCanvas.Background = backGroundImage;

            ghostImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/ghost.png"));


            scopeImage.Source = new BitmapImage(new Uri("pack://application:,,,/resourses/sniper-aim.png"));

            topLocations = new List<int> { 275, 540, 23, 540, 275, 23 };
            bottomLocations = new List<int> { 138, 673, 420, 673, 138, 420 };


            timer.Tick += mainEvent;
            timer.Interval = TimeSpan.FromMilliseconds(rand.Next(800, 2000));
            timer.Start();

            gostTime.Tick += ghostAnimation;
            gostTime.Interval = TimeSpan.FromMilliseconds(20);
            gostTime.Start();

            misses = 0;
            score = 0;
        }
        private void mainEvent(object sender, EventArgs e)
        {



            foreach (Rectangle y in RemoveList)
            {
                if (y.Tag != "ghost")
                {
                    misses++;
                    myCanvas.Children.Remove(y);
                }

            }

            RemoveList.Clear();

            creatingTarget(topLocations[rand.Next(0, 3)], 30, rand.Next(1, 4), "Top");
            creatingTarget(bottomLocations[rand.Next(0, 3)], 230, rand.Next(1, 4), "Bottom");

            var rectanglesInCanvas = myCanvas.Children.OfType<Rectangle>();
            foreach (Rectangle y in rectanglesInCanvas)
            {
                if (y.Tag == "Top" || y.Tag == "Bottom")
                {
                    RemoveList.Add(y);
                }
                if (y.Tag == "ghost")
                {
                    RemoveList.Add(y);
                }
            }
            showMisses.Content = "Misses: " + misses;

        }

        private void moveingScoope(object sender, MouseEventArgs e)
        {
            Point mousePosition = e.GetPosition(this);
            double pX = mousePosition.X;
            double pY = mousePosition.Y;
            Canvas.SetLeft(scopeImage, pX - scopeImage.Width / 2);
            Canvas.SetTop(scopeImage, pY - scopeImage.Height / 2);

        }

        private void shooting(object sender, MouseButtonEventArgs e)
        {


            if (e.OriginalSource is Rectangle)
            {
                Rectangle temp = (Rectangle)e.OriginalSource;
                if (temp.Tag != "ghost")
                {
                    score++;
                    Rectangle targetForRemove = (Rectangle)e.OriginalSource;
                    int left = (int)Canvas.GetLeft(targetForRemove);
                    int top = (int)Canvas.GetTop(targetForRemove);

                    myCanvas.Children.Remove(targetForRemove);
                    showScore.Content = "Score: " + score;
                    misses--;

                    Rectangle newGhost = new Rectangle()
                    {
                        Width = 80,
                        Height = 155,
                        Tag = "ghost",
                        Fill = ghostImage,
                    };

                    Canvas.SetLeft(newGhost, left);
                    Canvas.SetTop(newGhost, top);

                    myCanvas.Children.Add(newGhost);
                }

            }

        }

        private void creatingTarget(int x, int y, int skin, string tag)
        {

            ImageBrush targetSkin = new ImageBrush();
            switch (skin)
            {
                case 1:
                    targetSkin.ImageSource = new BitmapImage(new Uri("pack://Application:,,,/resourses/dummy01.png"));
                    break;
                case 2:
                    targetSkin.ImageSource = new BitmapImage(new Uri("pack://Application:,,,/resourses/dummy02.png"));
                    break;
                case 3:
                    targetSkin.ImageSource = new BitmapImage(new Uri("pack://Application:,,,/resourses/dummy03.png"));
                    break;
                case 4:
                    targetSkin.ImageSource = new BitmapImage(new Uri("pack://Application:,,,/resourses/dummy04.png"));
                    break;
            }

            Rectangle newTarget = new Rectangle()
            {
                Width = 80,
                Height = 155,
                Tag = tag,
                Fill = targetSkin,

            };

            Canvas.SetLeft(newTarget, x);
            Canvas.SetTop(newTarget, y);

            myCanvas.Children.Add(newTarget);

        }

        private void ghostAnimation(object sender, EventArgs e)
        {
            foreach (Rectangle r in RemoveList)
            {
                if (r.Tag == "ghost" && Canvas.GetTop(r) < -180)
                {
                    myCanvas.Children.Remove(r);
                }
            }

            foreach (Rectangle r in myCanvas.Children.OfType<Rectangle>())
            {
                if (r.Tag == "ghost")
                {
                    Canvas.SetTop(r, Canvas.GetTop(r) - 5);
                }
            }

        
    }
}
}
