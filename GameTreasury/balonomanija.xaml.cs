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
    
    public partial class balonomanija : Window
    {
       
        DispatcherTimer gameTimer = new DispatcherTimer();
        
        int speed = 3;
        
        int intervals = 90;
       
        Random rand = new Random();
        
        List<Rectangle> itemRemover = new List<Rectangle>();
       
        ImageBrush background = new ImageBrush();
       
        int balloonSkins;
       
        int i;
     
        int missedBalloons;
        
        bool gameisactive;
       
        int score;
        
        private MediaPlayer player = new MediaPlayer();
        public balonomanija()
        {
            InitializeComponent();

           
            gameTimer.Tick += gameEngine; 
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); 
           
            background.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/background-Image.jpg"));
            MyCanvas.Background = background;
          
            ResetGame();
        }
        private void canvasKeyIsDown(object sender, KeyEventArgs e)
        {
           
            if (e.Key == Key.Space && gameisactive == false)
            {
         
                ResetGame();
            }
        }
        private void popBalloons(object sender, MouseButtonEventArgs e)
        {
          
            if (gameisactive)
            {
                if (e.OriginalSource is Rectangle)
                {
                   
                    Rectangle activeRec = (Rectangle)e.OriginalSource; 
                   
                    player.Open(new Uri("../../resourses/pop_sound.mp3", UriKind.RelativeOrAbsolute));
                    player.Play(); 
                    MyCanvas.Children.Remove(activeRec); 
                    score++; 
                }
            }
        }
        private void startGame()
        {
           
            gameTimer.Start();
           
            missedBalloons = 0;
            
            score = 0;
            
            intervals = 90;
            
            gameisactive = true;
            speed = 3; 
        }
        private void ResetGame()
        {
            
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
             
                itemRemover.Add(x);
            }
           
            foreach (Rectangle y in itemRemover)
            {
               
                MyCanvas.Children.Remove(y);
            }
          
            itemRemover.Clear();
            
            startGame();
        }
        private void gameEngine(object sender, EventArgs e)
        {

            scoreLabel.Content = "Score: " + score; 
                                                  
            intervals -= 10;
            
            if (intervals < 1)
            {
               
                ImageBrush balloonImage = new ImageBrush();
                
                balloonSkins += 1;
                
                if (balloonSkins > 5)
                {
                    balloonSkins = 1;
                }
              
                switch (balloonSkins)
                {
                    case 1:
                        balloonImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/balloon1.png"));
                        break;
                    case 2:
                        balloonImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/balloon2.png"));
                        break;
                    case 3:
                        balloonImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/balloon3.png"));
                        break;
                    case 4:
                        balloonImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/balloon4.png"));
                        break;
                    case 5:
                        balloonImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/balloon5.png"));
                        break;
                }
              
                Rectangle newBalloon = new Rectangle
                {
                    Tag = "bloon",
                    Height = 70,
                    Width = 50,
                    Fill = balloonImage
                };
            
                Canvas.SetTop(newBalloon, 600);
           
                Canvas.SetLeft(newBalloon, rand.Next(50, 400));
                
                MyCanvas.Children.Add(newBalloon);
              
                intervals = rand.Next(80, 140);
            }
          
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
             
                if (x is Rectangle && (string)x.Tag == "bloon")
                {
                  
                    i = rand.Next(-5, 5);
                  
                    Canvas.SetTop(x, Canvas.GetTop(x) - speed);
                   
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - (i * -1));
                }
              
                if (Canvas.GetTop(x) < 20)
                {
               
                    itemRemover.Add(x);
               
                    missedBalloons++;
                }
            }
            
            if (missedBalloons > 10)
            {
                gameisactive = false;
                gameTimer.Stop(); 
                                  
                MessageBox.Show("You missed 10 Balloons, press space to restart");
            }
           
            if (score > 20)
            {
              
                speed = 6;
            }
           
            foreach (Rectangle y in itemRemover)
            {
                MyCanvas.Children.Remove(y);
            }
        }
    }
}
