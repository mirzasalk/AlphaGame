﻿using System;
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
    
    public partial class Space_Battle : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
       
        bool moveLeft, moveRight;
      
        List<Rectangle> itemstoremove = new List<Rectangle>();
       
        Random rand = new Random();
        int enemySpriteCounter;
        int enemyCounter = 100; 
        int playerSpeed = 10; 
        int limit = 50; 
        int score = 0;
        int damage = 0; 
        bool resetTemp;
        Rect playerHitBox; 

        public Space_Battle()
        {
            InitializeComponent();
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            
            gameTimer.Tick += gameEngine;
           
            gameTimer.Start();
           
            MyCanvas.Focus();
            
            ImageBrush bg = new ImageBrush();
      
            bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/purple.png"));
           
            bg.TileMode = TileMode.Tile;
           
           
            MyCanvas.Background = bg;

            ImageBrush playerImage = new ImageBrush();
           
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/player.png"));
           
            player.Fill = playerImage;
            resetTemp = false;
        }
        private void onKeyDown(object sender, KeyEventArgs e)
        {
          
            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
        }
        private void onKeyUp(object sender, KeyEventArgs e)
        {
          
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }
            if(e.Key == Key.Space && resetTemp == true)
            {
                foreach(var x in MyCanvas.Children.OfType<Rectangle>())
                {
                    if(x.Tag != "player")
                    {
                        itemstoremove.Add(x);
                    }
                    
                }
                foreach (var x in itemstoremove)
                {
                    MyCanvas.Children.Remove(x);
                }
                score = 0;
                damage = 0;
                limit = 50;
               
                damageText.Content = "Damaged: 0"; 
                damageText.Foreground = Brushes.White; 
                ImageBrush playerImage2 = new ImageBrush();
                playerImage2.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/player.png"));
                player.Fill = playerImage2;
                resetTemp = false;
                gameTimer.Start(); 

            }
         
            if (e.Key == Key.Space)
            {
                Rectangle newBullet = new Rectangle
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Red
                };
             
                Canvas.SetTop(newBullet, Canvas.GetTop(player) - newBullet.Height);
               
                Canvas.SetLeft(newBullet, Canvas.GetLeft(player) + player.Width / 2);
           
                MyCanvas.Children.Add(newBullet);
            }
        }
        private void makeEnemies()
        {
          
            ImageBrush enemySprite = new ImageBrush(); 
            enemySpriteCounter = rand.Next(1, 5); 
            switch (enemySpriteCounter)
            {
                case 1:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/1.png"));
                    break;
                case 2:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/2.png"));
                    break;
                case 3:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/3.png"));
                    break;
                case 4:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/4.png"));
                    break;
                case 5:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/5.png"));
                    break;
                default:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/resourses/1.png"));
                    break;
            }
         
            Rectangle newEnemy = new Rectangle
            {
                Tag = "enemy",
                Height = 50,
                Width = 56,
                Fill = enemySprite
            };
            Canvas.SetTop(newEnemy, -100);
          
            Canvas.SetLeft(newEnemy, rand.Next(30, 430));
         
            MyCanvas.Children.Add(newEnemy);
          
            GC.Collect(); 
        }
        private void gameEngine(object sender, EventArgs e)
        {
           
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
           
            enemyCounter--;
            scoreText.Content = "Score: " + score;
            damageText.Content = "Damaged " + damage; 
            if (enemyCounter < 0)
            {
                makeEnemies(); 
                enemyCounter = limit; 
            }
            
            if (moveLeft && Canvas.GetLeft(player) > 0)
            {
               
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }
            if (moveRight && Canvas.GetLeft(player) + 90 < Application.Current.MainWindow.Width)
            {
              
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }
          
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "bullet")
                {
                  
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);
                 
                    Rect bullet = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                   
                    if (Canvas.GetTop(x) < 10)
                    {
                        itemstoremove.Add(x);
                    }
               
                    foreach (var y in MyCanvas.Children.OfType<Rectangle>())
                    {
                        if (y is Rectangle && (string)y.Tag == "enemy")
                        {
                           
                            Rect enemy = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);
                         
                            if (bullet.IntersectsWith(enemy))
                            {
                                itemstoremove.Add(x); 
                                itemstoremove.Add(y); 
                                score++; 
                            }
                        }
                    }
                }
               
                if (x is Rectangle && (string)x.Tag == "enemy")
                {
                  
                    Canvas.SetTop(x, Canvas.GetTop(x) + 10); 
                    Rect enemy = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                   
                    if (Canvas.GetTop(x) + 150 > 700)
                    {
                    
                        itemstoremove.Add(x);
                        damage += 10; 
                    }
                  
                    if (playerHitBox.IntersectsWith(enemy))
                    {
                        damage += 5; 
                        itemstoremove.Add(x);
                    }
                }
            }
         
            if (score > 5)
            {
                limit = 20; 
            }
           
            if (damage > 99)
            {
                gameTimer.Stop(); 
                damageText.Content = "Damaged: 100"; 
                damageText.Foreground = Brushes.Red; 
                MessageBox.Show("Čestitamo" + Environment.NewLine + "Uništili ste " + score + " Neprijateljskih brodova");
               
                resetTemp = true;
            }
           
            foreach (Rectangle y in itemstoremove)
            {
               
                MyCanvas.Children.Remove(y);
            }
        }
    }
}
