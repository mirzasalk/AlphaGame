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
    
    public partial class Uhvati_Poklone : Window
    {
        int maxItem = 5;
        int currentItems = 0; 
        Random r = new Random();
        bool restartTemp;
        int score = 0; 
        int missed = 0; 
        DispatcherTimer GameTimer = new DispatcherTimer(); 
        List<Rectangle> itemstoremove = new List<Rectangle>(); 
        ImageBrush playerImage = new ImageBrush(); 
        ImageBrush backgroundImage = new ImageBrush(); 
        public Uhvati_Poklone()
        {
            InitializeComponent();

            myCanvas.Focus(); 

            GameTimer.Tick += dropItems;
            GameTimer.Interval = TimeSpan.FromMilliseconds(20);
            GameTimer.Start();
            
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/netLeft.png"));
            backgroundImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/background.jpg"));
            
            player1.Fill = playerImage;
            myCanvas.Background = backgroundImage;

            restartTemp = false; 
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
           
            System.Windows.Point position = e.GetPosition(this);
            double pX = position.X;
            double pY = position.Y;
           
            Canvas.SetLeft(player1, pX - 10);
            
            if (Canvas.GetLeft(player1) < 260)
            {
                playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/netLeft.png"));
            }
           
            if (Canvas.GetLeft(player1) > 260)
            {
                playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/netRight.png"));
            }
        }
        private void dropItems(object sender, EventArgs e)
        {
            ScoreText.Content = "Uhvaceno: " + score; 
            missedText.Content = "Promaseno: " + missed; 
            
            if (currentItems < maxItem)
            {
                makePresents(); 
                currentItems++; 
                itemstoremove.Clear();
            }
            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {
               
                if ((string)x.Tag == "drops")
                {
                   
                    int dropThis = r.Next(8, 20); 

                    Canvas.SetTop(x, Canvas.GetTop(x) + dropThis);

                    Rect items = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    Rect playerObject = new Rect(Canvas.GetLeft(player1), Canvas.GetTop(player1), player1.Width, player1.Height);
                   
                    if (playerObject.IntersectsWith(items))
                    {
                        itemstoremove.Add(x);
                      
                        currentItems--;
                     
                        score++;
                    }
                    else if (Canvas.GetTop(x) > 700)
                    {
                      
                        itemstoremove.Add(x);
                       
                        currentItems--;
                     
                        missed++;
                    }
                }
               
                if (missed > 6 && restartTemp == false)
                {
                    
                    GameTimer.Stop();
                    
                    restartTemp = true;
                    MessageBox.Show("Ako zelite ponovo da igrate pritisnite space");
           
                }
            }
           
            foreach (Rectangle y in itemstoremove)
            {
                myCanvas.Children.Remove(y);
            }
        }
       
        private void makePresents()
        {
            ImageBrush presents = new ImageBrush(); 
            int i = r.Next(1, 6); 

            switch (i)
            {
                case 1:
                    presents.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/present_01.png"));
                    break;
                case 2:
                    presents.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/present_02.png"));
                    break;
                case 3:
                    presents.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/present_03.png"));
                    break;
                case 4:
                    presents.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/present_04.png"));
                    break;
                case 5:
                    presents.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/present_05.png"));
                    break;
                case 6:
                    presents.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/present_06.png"));
                    break;
            }
           Rectangle newRec = new Rectangle
            {
                Tag = "drops",
                Width = 50,
                Height = 50,
                Fill = presents,
            };
          
            Canvas.SetTop(newRec, r.Next(60, 150) * -1);
            Canvas.SetLeft(newRec, r.Next(10, 450));
           
            myCanvas.Children.Add(newRec);
        }

        private void keyisDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Space && restartTemp ==true)
            {
                
                foreach(var x in myCanvas.Children.OfType<Rectangle>())
                {
                    if(x.Tag == "drops")
                    {
                        itemstoremove.Add(x);
                    }
                }
                foreach(var x in itemstoremove)
                {
                    myCanvas.Children.Remove(x);
                }
                itemstoremove.Clear();
                score = 0;
                missed = 0;
                currentItems = 0;
                restartTemp = false;
                Canvas.SetLeft(player1, 217);
                GameTimer.Start();

            }
        }
    }
}
