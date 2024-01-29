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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Windows.Threading;

using System.Configuration;

namespace GameTreasury
{
    
    public partial class IgriceUserControl : UserControl
    {
       
        
        Trkac trkac;
        Uhvati_Poklone uhvatiPoklone;
        Sniper sniper;
        balonomanija balon;
        FlappyBird flappy;
        Space_Battle space; 
        DispatcherTimer timerIgrice = new DispatcherTimer();
        List<Button> removeButtonList = new List<Button>();
        List<Igra> igre = new List<Igra>();
        List<int> getTopGame = new List<int> { 50, 550, 1050 };
        List<int> getLeftGame = new List<int> { 50, 500, 950 };
        int redIgara = 0;
        int kolonaIgara = 0;
        Korisnik trenutniKorisnik;
        Token trenutniToken;
        int[] vreme = { 0,0,0,0,0,0}; 
        
        DateTime trenutniDatum = DateTime.Now;
        
        public IgriceUserControl()
        {
            InitializeComponent();
            timerIgrice.Tick += proveraClanarine;
            timerIgrice.Interval = TimeSpan.FromMilliseconds(20);
            timerIgrice.Start();
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            

            trenutniToken = MainWindow.TrenutniToken;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    


                    string query = "SELECT * FROM Game";
                    SqlCommand command = new SqlCommand(query, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Igra igra = new Igra
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                Naziv = reader.GetString(reader.GetOrdinal("Naziv")),
                                Opis = reader.GetString(reader.GetOrdinal("Opis")),
                                SlikaPutanja = reader.GetString(reader.GetOrdinal("SlikaPutanja")),
                                ZbirOcena = reader.GetInt32(reader.GetOrdinal("ZbirOcena")),
                                BrojOcena = reader.GetInt32(reader.GetOrdinal("BrojOcena"))
                            };
                            igre.Add(igra);
                        }
                    }

                    CreateGameCards();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri povezivanju sa bazom podataka: " + ex.Message);
                }
            }
        }

        private void proveraClanarine(object sender, EventArgs e)
        {
            vreme = ((MainWindow)Application.Current.MainWindow).vrednostiPlacenogVremena();

            trenutniKorisnik = MainWindow.TrenutniKorisnik;
            DateTime datumZa30Dana = trenutniDatum.AddDays(-30);

      if(trenutniKorisnik == null)
            {
                foreach(Image i in igriceCanvas.Children.OfType<Image>())
                {
                    if(i.Tag != "Game")
                    {
                        i.Visibility = Visibility.Hidden;
                    }
                }
                foreach (TextBlock r in igriceCanvas.Children.OfType<TextBlock>())
                {
                    if (r.Tag == "Prijava")
                    {
                        r.Visibility = Visibility.Visible;
                    }
                }
                foreach (Button r in igriceCanvas.Children.OfType<Button>())
                {
                    if(r.Content == "Plati i igraj" )
                    {     
                        switch(r.Tag)
                        {
                              case "Trkac":
                                if (vreme[0] > 0)
                                {
                                    r.Visibility = Visibility.Hidden;
                                    
                                }
                                else
                                {
                                    if (trkac != null && trkac.IsLoaded)
                                    {
                                        trkac.Close();
                                    }
                                    r.Visibility = Visibility.Visible;
                                }
                        break;
                    case "Uhvati Poklone":
                                if (vreme[1] > 0)
                                {
                                    r.Visibility = Visibility.Hidden;
                                }
                                else
                                {
                                    if (uhvatiPoklone != null && uhvatiPoklone.IsLoaded)
                                    {
                                        uhvatiPoklone.Close();
                                    }
                                    r.Visibility = Visibility.Visible;
                                }
                                break;
                    case "Snajper":
                                if (vreme[2] > 0)
                                {
                                    r.Visibility = Visibility.Hidden;
                                }
                                else
                                {
                                    if (sniper != null && sniper.IsLoaded)
                                    {
                                        sniper.Close();
                                    }
                                    r.Visibility = Visibility.Visible;
                                }
                                break;
                            case "FlappyBird":

                                if (vreme[3] > 0)
                                {
                                    r.Visibility = Visibility.Hidden;
                                }
                                else
                                {
                                    if (flappy != null && flappy.IsLoaded)
                                    {
                                        flappy.Close();
                                    }
                                    r.Visibility = Visibility.Visible;
                                }
                                break;
                    case "Balonomanija":
                                if (vreme[4] > 0)
                                {
                                    r.Visibility = Visibility.Hidden;
                                }
                                else
                                {
                                    if (balon != null && balon.IsLoaded)
                                    {
                                        balon.Close();
                                    }
                                    r.Visibility = Visibility.Visible;
                                }
                                break;
                    case "Space Battle":
                                if (vreme[5] > 0)
                                {
                                    r.Visibility = Visibility.Hidden;
                                }
                                else
                                {
                                    if (space != null && space.IsLoaded)
                                    {
                                        space.Close();
                                    }
                                    r.Visibility = Visibility.Visible;
                                }
                                break;


                        }

                }
                    if (r.Content == "Plati")
                    {
                        r.Visibility = Visibility.Hidden;
                    }
                    if (r.Content == "Igraj")
                    {
                        switch (r.Tag)
                        {
                            case "Trkac":
                                if (vreme[0] > 0)
                                {
                                    r.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    r.Visibility = Visibility.Hidden;
                                }
                                break;
                            case "Uhvati Poklone":
                                if (vreme[1] > 0)
                                {
                                    r.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    r.Visibility = Visibility.Hidden;
                                }
                                break;
                            case "Snajper":
                                if (vreme[2] > 0)
                                {
                                    r.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    r.Visibility = Visibility.Hidden;
                                }
                                break;
                            case "FlappyBird":

                                if (vreme[3] > 0)
                                {
                                    r.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    r.Visibility = Visibility.Hidden;
                                }
                                break;
                            case "Balonomanija":
                                if (vreme[4] > 0)
                                {
                                    r.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    r.Visibility = Visibility.Hidden;
                                }
                                break;
                            case "Space Battle":
                                if (vreme[5] > 0)
                                {
                                    r.Visibility = Visibility.Visible;
                                }
                                else
                                {
                                    r.Visibility = Visibility.Hidden;
                                }
                                break;


                        }
                    }
                   
                }
            }
            else
            {
                foreach (Button b in igriceCanvas.Children.OfType<Button>())
                {

                    if (b.Content == "Plati i igraj")
                    {
                        b.Visibility = Visibility.Hidden;
                    }
                };



                foreach (TextBlock r in igriceCanvas.Children.OfType<TextBlock>())
                {
                   
                   string[] igre = trenutniKorisnik.PlaceneIgrice.Split(',');

                    if (r.Tag == "Prijava")
                    {
                        r.Visibility = Visibility.Hidden;
                    }
                }

                if (trenutniKorisnik.Clanarina < datumZa30Dana)
                {

                    string[] delovi = trenutniKorisnik.PlaceneIgrice.Split(',');
                    string[] ocI = trenutniKorisnik.OcenjeneIgre.Split(',');
                    foreach (Image b in igriceCanvas.Children.OfType<Image>())
                    {
                        if (b.Tag != "Game")
                        {
                            DateTime trenutniDatumMinus30Dana = DateTime.Now.AddDays(-30);
                            switch (b.Tag)
                            {
                                case "Trkac":
                                    if (DateTime.TryParse(delovi[0], out DateTime datum))
                                    {
                                        // Datum je uspešno konvertovan

                                        if (trenutniDatumMinus30Dana > datum)
                                        {
                                            b.Visibility = Visibility.Hidden;
                                        }
                                        else
                                        {
                                            if (ocI[0]=="0")
                                            {
                                                b.Visibility = Visibility.Visible;
                                            }
                                            else
                                            {
                                                b.Visibility = Visibility.Hidden;
                                            }


                                        }
                                    }

                                    break;
                                case "Uhvati Poklone":
                                    if (DateTime.TryParse(delovi[1], out DateTime datum2))
                                    {
                                        // Datum je uspešno konvertovan

                                        if (trenutniDatumMinus30Dana > datum2)
                                        {
                                            b.Visibility = Visibility.Hidden;
                                        }
                                        else
                                        {
                                            if (ocI[1] == "0")
                                            {
                                                b.Visibility = Visibility.Visible;
                                            } else
                                            {
                                                b.Visibility = Visibility.Hidden;
                                            }
                                        }
                                    }

                                    break;
                                case "Snajper":
                                    if (DateTime.TryParse(delovi[2], out DateTime datum3))
                                    {
                                        // Datum je uspešno konvertovan

                                        if (trenutniDatumMinus30Dana > datum3)
                                        {
                                            b.Visibility = Visibility.Hidden;
                                        }
                                        else
                                        {
                                            if (ocI[2] == "0")
                                            {
                                                b.Visibility = Visibility.Visible;
                                            }
                                            else
                                            {
                                                b.Visibility = Visibility.Hidden;
                                            }
                                        }
                                    }

                                    break;
                                case "Balonomanija":
                                    if (DateTime.TryParse(delovi[3], out DateTime datum4))
                                    {
                                        // Datum je uspešno konvertovan

                                        if (trenutniDatumMinus30Dana > datum4)
                                        {
                                            b.Visibility = Visibility.Hidden;

                                        }
                                        else
                                        {
                                            if (ocI[3] == "0")
                                            {
                                                b.Visibility = Visibility.Visible;
                                            }
                                            else
                                            {
                                                b.Visibility = Visibility.Hidden;
                                            }
                                        }
                                    }

                                    break;
                                case "FlappyBird":
                                    if (DateTime.TryParse(delovi[4], out DateTime datum5))
                                    {
                                        // Datum je uspešno konvertovan

                                        if (trenutniDatumMinus30Dana > datum5)
                                        {
                                            b.Visibility = Visibility.Hidden;
                                        }
                                        else
                                        {
                                            if (ocI[4] == "0")
                                            {
                                                b.Visibility = Visibility.Visible;
                                            }
                                            else
                                            {
                                                b.Visibility = Visibility.Hidden;
                                            }
                                        }
                                    }
                                    break;
                                case "Space Battle":
                                    if (DateTime.TryParse(delovi[5], out DateTime datum6))
                                    {
                                        // Datum je uspešno konvertovan

                                        if (trenutniDatumMinus30Dana > datum6)
                                        {
                                            b.Visibility = Visibility.Hidden;
                                        }
                                        else
                                        {
                                            if (ocI[5] == "0")
                                            {
                                                b.Visibility = Visibility.Visible;
                                            }
                                            else
                                            {
                                                b.Visibility = Visibility.Hidden;
                                            }
                                        }
                                    }
                                    break;
                            }


                        }

                    }



                    foreach (Button b in igriceCanvas.Children.OfType<Button>())
                    {
                      
                        if (b.Content == "Plati" )
                        {
                            DateTime trenutniDatumMinus30Dana = DateTime.Now.AddDays(-30);
                            switch (b.Tag)
                            {
                                case "Trkac":
                                    if (DateTime.TryParse(delovi[0], out DateTime datum))
                                    {
                                        // Datum je uspešno konvertovan
                                     
                                        if(trenutniDatumMinus30Dana<datum)
                                        {
                                            b.Visibility = Visibility.Hidden;
                                        }
                                        else
                                        {
                                            b.Visibility = Visibility.Visible;
                                           
                                        }
                                    }
                                    
                                    break;
                                case "Uhvati Poklone":
                                    if (DateTime.TryParse(delovi[1], out DateTime datum2))
                                    {
                                        // Datum je uspešno konvertovan
                                        
                                        if (trenutniDatumMinus30Dana < datum2)
                                        {
                                            b.Visibility = Visibility.Hidden;
                                        }
                                        else
                                        {
                                            b.Visibility = Visibility.Visible;
                                            
                                        }
                                    }
                                   
                                    break;
                                case "Snajper":
                                    if (DateTime.TryParse(delovi[2], out DateTime datum3))
                                    {
                                        // Datum je uspešno konvertovan
                                        
                                        if (trenutniDatumMinus30Dana < datum3)
                                        {
                                            b.Visibility = Visibility.Hidden;
                                        }
                                        else
                                        {
                                            b.Visibility = Visibility.Visible;
                                            
                                        }
                                    }
                                    
                                    break;
                                case "Balonomanija":
                                    if (DateTime.TryParse(delovi[3], out DateTime datum4))
                                    {
                                        // Datum je uspešno konvertovan
                                        
                                        if (trenutniDatumMinus30Dana < datum4)
                                        {
                                            b.Visibility = Visibility.Hidden;
                                            
                                        }
                                        else
                                        {
                                            b.Visibility = Visibility.Visible;
                                        }
                                    }
                                   
                                    break;
                                case "FlappyBird":
                                    if (DateTime.TryParse(delovi[4], out DateTime datum5))
                                    {
                                        // Datum je uspešno konvertovan
                                        
                                        if (trenutniDatumMinus30Dana < datum5)
                                        {
                                            b.Visibility = Visibility.Hidden;
                                        }
                                        else
                                        {
                                            b.Visibility = Visibility.Visible;
                                           
                                        }
                                    }
                                    break;
                                case "Space Battle":
                                    if (DateTime.TryParse(delovi[5], out DateTime datum6))
                                    {
                                        // Datum je uspešno konvertovan
                                      
                                        if (trenutniDatumMinus30Dana < datum6)
                                        {
                                            b.Visibility = Visibility.Hidden;
                                        }
                                        else
                                        {
                                            b.Visibility = Visibility.Visible;
                                           
                                        }
                                    }
                                    break;
                            }
                         
                          
                        }
                        
                    }
                }
                else
                {
                    string[] ocI = trenutniKorisnik.OcenjeneIgre.Split(',');
                    foreach (Image i in igriceCanvas.Children.OfType<Image>())
                    {
                        if (i.Tag != "Game")
                        {
                            switch (i.Tag)
                            {
                                case "Trkac":
                                    
                                            if (ocI[0] == "0")
                                            {
                                                i.Visibility = Visibility.Visible;
                                    }
                                    else
                                    {
                                        i.Visibility = Visibility.Hidden;
                                    }



                                    break;
                                case "Uhvati Poklone":
                                    
                                            if (ocI[1] == "0")
                                            {
                                                i.Visibility = Visibility.Visible;
                                            }
                                    else
                                    {
                                        i.Visibility = Visibility.Hidden;
                                    }


                                    break;
                                case "Snajper":
                                    
                                            if (ocI[2] == "0")
                                            {
                                                i.Visibility = Visibility.Visible;
                                            }
                                    else
                                    {
                                        i.Visibility = Visibility.Hidden;
                                    }

                                    break;
                                case "Balonomanija":
                                    
                                            if (ocI[3] == "0")
                                            {
                                                i.Visibility = Visibility.Visible;
                                            }
                                    else
                                    {
                                        i.Visibility = Visibility.Hidden;
                                    }


                                    break;
                                case "FlappyBird":
                                    
                                            if (ocI[4] == "0")
                                            {
                                                i.Visibility = Visibility.Visible;
                                            }
                                    else
                                    {
                                        i.Visibility = Visibility.Hidden;
                                    }


                                    break;
                                case "Space Battle":
                                    
                                            if (ocI[5] == "0")
                                            {
                                                i.Visibility = Visibility.Visible;
                                            }
                                    else
                                    {
                                        i.Visibility = Visibility.Hidden;
                                    }


                                    break;
                            }
                            
                        }
                    }

                    foreach (Button r in igriceCanvas.Children.OfType<Button>())
                    {

                        if (r.Content == "Plati")
                        {

                            r.Visibility = Visibility.Hidden;
                        }
                        if (r.Content == "Igraj")
                        {
                            r.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
                
           
        }

        private void CreateGameCards()
        {
            foreach (Igra igra in igre)
            {
                Rectangle newRec = new Rectangle
                {
                    Tag = "Game",
                    Width = 400,
                    Height = 470,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC61A"))
                };



                Image image = new Image
                {
                    Tag = "Game",
                    Source = new BitmapImage(new Uri(igra.SlikaPutanja)),
                    Width = 100,
                    Height = 100,
                };

                TextBlock textBlock = new TextBlock
                {
                    Tag = "Game",
                    Width = 350,

                    Text = igra.Naziv,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                };
                TextBlock textBlockOpis = new TextBlock
                {
                    Tag = "Game",
                    Width = 350, // Prilagodite širinu prema potrebi
                    Text = igra.Opis,
                    FontSize = 16,
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,

                };
                float ocena;
                if (igra.ZbirOcena == 0)
                {
                    ocena = 0;
                }
                else
                {
                     ocena = (float)igra.ZbirOcena / igra.BrojOcena;
                }

                string formatiranaOcena = ocena.ToString("0.0");

                TextBlock textOcena = new TextBlock
                {
                    Width = 50, // Prilagodite širinu prema potrebi
                    Text = ""+ formatiranaOcena,
                    FontSize = 16,
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,

                };

                Button otvoriNoviProzorButton = new Button
                {
                    Tag = igra.Naziv,
                    Content = "Igraj",
                    Width = 180,
                    Height = 40,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333230")),
                    Foreground = Brushes.White
                };
              
                Button PlatiZaIgricu = new Button
                {
                    
                    Content = "Plati",
                    Width = 180,
                    Height = 40,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333230")),
                    Foreground = Brushes.White
                };
                Button PlatiZaIgricuNonReg = new Button
                {
                    Name = "PlatiNonReg",
                    Content = "Plati i igraj",
                    Width = 180,
                    Height = 40,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333230")),
                    Foreground = Brushes.White
                };
                Button komentariBtn = new Button
                {
                    
                    Content = "komentari",
                    Width = 90,
                    Height = 25,
                    BorderThickness = new Thickness(0),
                    Background = Brushes.Transparent,
                    Foreground = Brushes.White,
                    FontSize = 16

                };

              
                Image zvezda1 = new Image
                {
                    Name = "prvaZvezda",
                    Width = 25,
                    Height = 25,
                    Source = new BitmapImage(new Uri("pack://application:,,,/resourses/belazvezda.png")),
                }; 
                zvezda1.MouseLeftButtonDown += zvezda1_Click;

                Image zvezda2 = new Image
                {
                    Name = "drugaZvezda",
                    Width = 25,
                    Height = 25,
                    Source = new BitmapImage(new Uri("pack://application:,,,/resourses/belazvezda.png")),
                }; 

                zvezda2.MouseLeftButtonDown += zvezda2_Click;

                Image zvezda3 = new Image
                {
                    Name = "trecaZvezda",

                    Width = 25,
                    Height = 25,
                    Source = new BitmapImage(new Uri("pack://application:,,,/resourses/belazvezda.png")),
                };
                zvezda3.MouseLeftButtonDown += zvezda3_Click;

                Image zvezda4 = new Image
                {
                    Name = "cetvrtaZvezda",
                    Width = 25,
                    Height = 25,
                    Source = new BitmapImage(new Uri("pack://application:,,,/resourses/belazvezda.png")),
                }; 

                zvezda4.MouseLeftButtonDown += zvezda4_Click;

                Image zvezda5 = new Image
                {
                    Name = "petaZvezda",
                    Width = 25,
                    Height = 25,
                    Source = new BitmapImage(new Uri("pack://application:,,,/resourses/belazvezda.png")),
                };

                zvezda5.MouseLeftButtonDown += zvezda5_Click;
                Image zvezdaOcena = new Image
                {
                    Name = "ZvezdaOcena",
                    Tag = "Game",
                    Width = 11,
                    Height = 11,
                    Source = new BitmapImage(new Uri("pack://application:,,,/resourses/belazvezda.png")),
                };

                if (igra.Naziv == "Trkac")
                    
                {
                    PlatiZaIgricu.Click += platiTrkac_Click;

                    komentariBtn.Click += komentariTrkac_Click;

                    komentariBtn.Tag = "Trkac";

                    PlatiZaIgricu.Tag = "Trkac";

                    PlatiZaIgricuNonReg.Tag = "Trkac";

                    PlatiZaIgricuNonReg.Click += platiNonRegTrkac_Click;

                    zvezda1.Tag = "Trkac";
                    zvezda2.Tag = "Trkac";
                    zvezda3.Tag = "Trkac";
                    zvezda4.Tag = "Trkac";
                    zvezda5.Tag = "Trkac";



                    otvoriNoviProzorButton.Click += OtvoriNoviProzor_Click;
                }
                if (igra.Naziv == "Uhvati Poklone")
                {
                    PlatiZaIgricu.Click += platiUhvatiPoklone_Click;

                    komentariBtn.Click += komentariUhvatiPoklone_Click;

                    komentariBtn.Tag = "Uhvati Poklone";

                    PlatiZaIgricu.Tag = "Uhvati Poklone";

                    PlatiZaIgricuNonReg.Tag = "Uhvati Poklone";

                    PlatiZaIgricuNonReg.Click += platiNonRegUhvatiPoklone_Click;

                    zvezda1.Tag = "Uhvati Poklone";
                    zvezda2.Tag = "Uhvati Poklone";
                    zvezda3.Tag = "Uhvati Poklone";
                    zvezda4.Tag = "Uhvati Poklone";
                    zvezda5.Tag = "Uhvati Poklone";


                    otvoriNoviProzorButton.Click += OtvoriUhvatiPoklone_Click;
                }
                if (igra.Naziv == "Snajper")
                {
                    PlatiZaIgricu.Click += platiSnajper_Click;

                    komentariBtn.Click += komentariSnajper_Click;

                    komentariBtn.Tag = "Snajper";

                    PlatiZaIgricu.Tag = "Snajper";

                    PlatiZaIgricuNonReg.Tag = "Snajper";

                    PlatiZaIgricuNonReg.Click += platiNonRegSnajper_Click;

                    zvezda1.Tag = "Snajper";
                    zvezda2.Tag = "Snajper";
                    zvezda3.Tag = "Snajper";
                    zvezda4.Tag = "Snajper";
                    zvezda5.Tag = "Snajper";

                    otvoriNoviProzorButton.Click += OtvoriSnajper_Click;
                }
                if (igra.Naziv == "Balonomanija")
                {
                    PlatiZaIgricu.Click += platiBalonomanija_Click;

                    komentariBtn.Click += komentariBalonomanija_Click;

                    komentariBtn.Tag = "Balonomanija";

                    PlatiZaIgricu.Tag = "Balonomanija";

                    PlatiZaIgricuNonReg.Tag = "Balonomanija";

                    PlatiZaIgricuNonReg.Click += platiNonRegBalonomanija_Click;

                    zvezda1.Tag = "Balonomanija";
                    zvezda2.Tag = "Balonomanija";
                    zvezda3.Tag = "Balonomanija";
                    zvezda4.Tag = "Balonomanija";
                    zvezda5.Tag = "Balonomanija";

                    otvoriNoviProzorButton.Click += OtvoriBalonomanija_Click;
                }
                if (igra.Naziv == "FlappyBird")
                {
                    PlatiZaIgricu.Tag = "FlappyBird";

                    komentariBtn.Click += komentariFlappyBird_Click;

                    komentariBtn.Tag = "FlappyBird";

                    PlatiZaIgricuNonReg.Tag = "FlappyBird";

                    PlatiZaIgricu.Click += platiFlappyBirdClick;

                    PlatiZaIgricuNonReg.Click += platiNonRegFlappyBirdClick;

                    zvezda1.Tag = "FlappyBird";
                    zvezda2.Tag = "FlappyBird";
                    zvezda3.Tag = "FlappyBird";
                    zvezda4.Tag = "FlappyBird";
                    zvezda5.Tag = "FlappyBird";

                    otvoriNoviProzorButton.Click += OtvoriFlappyBird_Click;
                }
                if (igra.Naziv == "Space Battle")
                {
                    PlatiZaIgricu.Tag = "Space Battle";

                    komentariBtn.Click += komentariSpaceBattle_Click;

                    komentariBtn.Tag = "Space Battle";

                    PlatiZaIgricuNonReg.Tag = "Space Battle";

                    PlatiZaIgricu.Click += platiSpaceBattle_Click;

                    PlatiZaIgricuNonReg.Click += platiNonRegSpaceBattle_Click;

                    zvezda1.Tag = "Space Battle";
                    zvezda2.Tag = "Space Battle";
                    zvezda3.Tag = "Space Battle";
                    zvezda4.Tag = "Space Battle";
                    zvezda5.Tag = "Space Battle";

                    otvoriNoviProzorButton.Click += OtvoriSpaceBattle_Click;
                }


                Canvas.SetZIndex(otvoriNoviProzorButton, 1);
                Canvas.SetZIndex(PlatiZaIgricu, 1);
                Canvas.SetZIndex(PlatiZaIgricuNonReg, 1);
                


                igriceCanvas.Children.Add(otvoriNoviProzorButton);
                igriceCanvas.Children.Add(PlatiZaIgricu);

                Canvas.SetZIndex(textBlockOpis, 1);

                
                Canvas.SetTop(newRec, getTopGame[redIgara]);
                Canvas.SetLeft(newRec, getLeftGame[kolonaIgara]);

                Canvas.SetLeft(image, getLeftGame[kolonaIgara] + 150);
                Canvas.SetTop(image, getTopGame[redIgara] + 10);

                Canvas.SetLeft(textBlock, getLeftGame[kolonaIgara] + 25);
                Canvas.SetTop(textBlock, getTopGame[redIgara] + image.Width + 10);

                Canvas.SetLeft(textBlockOpis, getLeftGame[kolonaIgara] + 25);
                Canvas.SetTop(textBlockOpis, Canvas.GetTop(textBlock) + 25);

                Canvas.SetTop(otvoriNoviProzorButton, Canvas.GetTop(newRec) + newRec.Width - 10);
                Canvas.SetLeft(otvoriNoviProzorButton, Canvas.GetLeft(newRec) + 125);
                
                Canvas.SetTop(PlatiZaIgricu, Canvas.GetTop(newRec) + newRec.Width - 10);
                Canvas.SetLeft(PlatiZaIgricu, Canvas.GetLeft(newRec) + 125);
                
                Canvas.SetTop(PlatiZaIgricuNonReg, Canvas.GetTop(newRec) + newRec.Width - 10);
                Canvas.SetLeft(PlatiZaIgricuNonReg, Canvas.GetLeft(newRec) + 125);

                Canvas.SetTop(komentariBtn, Canvas.GetTop(PlatiZaIgricu) - 30);
                Canvas.SetLeft(komentariBtn, Canvas.GetLeft(newRec) + newRec.Width - komentariBtn.Width);
                
                Canvas.SetTop(textOcena, Canvas.GetTop(PlatiZaIgricu) + PlatiZaIgricu.Height + 10);
                Canvas.SetLeft(textOcena, Canvas.GetLeft(newRec) + newRec.Width - 60);
                
                Canvas.SetTop(zvezdaOcena, Canvas.GetTop(textOcena) - 5);
                Canvas.SetLeft(zvezdaOcena, Canvas.GetLeft(newRec) + newRec.Width - 25);
                
                Canvas.SetTop(zvezda1, Canvas.GetTop(PlatiZaIgricu) + PlatiZaIgricu.Height + 10);
                Canvas.SetLeft(zvezda1, Canvas.GetLeft(newRec) + 10);
                
                Canvas.SetTop(zvezda2, Canvas.GetTop(PlatiZaIgricu) + PlatiZaIgricu.Height + 10);
                Canvas.SetLeft(zvezda2, Canvas.GetLeft(newRec) + 37);

                Canvas.SetTop(zvezda3, Canvas.GetTop(PlatiZaIgricu) + PlatiZaIgricu.Height + 10);
                Canvas.SetLeft(zvezda3, Canvas.GetLeft(newRec) + 64);

                Canvas.SetTop(zvezda4, Canvas.GetTop(PlatiZaIgricu) + PlatiZaIgricu.Height + 10);
                Canvas.SetLeft(zvezda4, Canvas.GetLeft(newRec)+ 91);

                Canvas.SetTop(zvezda5, Canvas.GetTop(PlatiZaIgricu) + PlatiZaIgricu.Height + 10);
                Canvas.SetLeft(zvezda5, Canvas.GetLeft(newRec) + 118);
                
              

                igriceCanvas.Children.Add(newRec);
                igriceCanvas.Children.Add(image);
                igriceCanvas.Children.Add(textBlock);
                igriceCanvas.Children.Add(textBlockOpis);
                
                igriceCanvas.Children.Add(textOcena);
                igriceCanvas.Children.Add(zvezda1);
                igriceCanvas.Children.Add(zvezda2);
                igriceCanvas.Children.Add(zvezda3);
                igriceCanvas.Children.Add(zvezda4);
                igriceCanvas.Children.Add(zvezda5);
                igriceCanvas.Children.Add(zvezdaOcena);
                igriceCanvas.Children.Add(komentariBtn);
                igriceCanvas.Children.Add(PlatiZaIgricuNonReg);

                if (kolonaIgara == 2)
                {
                    kolonaIgara = 0;
                    redIgara++;

                }
                else
                {
                    kolonaIgara++;
                }


            }
        }

        private void platiNonRegBalonomanija_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).prikazProzoraZaUnosVremena("Balonomanija");
        }

        private void platiNonRegSpaceBattle_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).prikazProzoraZaUnosVremena("Space Battle");
        }

        private void platiNonRegFlappyBirdClick(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).prikazProzoraZaUnosVremena("FlappyBird");
        }

        private void platiNonRegSnajper_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).prikazProzoraZaUnosVremena("Snajper");
        }

        private void platiNonRegUhvatiPoklone_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).prikazProzoraZaUnosVremena("Uhvati Poklone");
        }

        private void platiNonRegTrkac_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).prikazProzoraZaUnosVremena("Trkac");
        }

        private void komentariTrkac_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).prikaziKomentare("Trkac");


        }

        private void komentariUhvatiPoklone_Click(object sender, RoutedEventArgs e)
        {
           ((MainWindow)Application.Current.MainWindow).prikaziKomentare("Uhvati Poklone");
        }

        private void komentariSnajper_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).prikaziKomentare("Space Battle");

        }

        private void komentariBalonomanija_Click(object sender, RoutedEventArgs e)
        {

            ((MainWindow)Application.Current.MainWindow).prikaziKomentare("FlappyBird");
        }

        private void komentariFlappyBird_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).prikaziKomentare("Balonomanija");
        }

        private void komentariSpaceBattle_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).prikaziKomentare("Snajper");
        }

        private void zvezda5_Click(object sender, MouseButtonEventArgs e)
        {
            
            string Naziv = null;
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

                if (sender is FrameworkElement element)
                {
               
                    object tagValue = element.Tag;

            
                    if (tagValue != null)
                    {
                        Naziv = tagValue.ToString();

                       
                    }
                }



                string updateQuery = "UPDATE Game SET BrojOcena = BrojOcena + 1, ZbirOcena = ZbirOcena + 5 WHERE Naziv = @Naziv";

                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Naziv", Naziv);


                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Hvala Vam na oceni igre: " + Naziv);
                    }
                    
                }
                string OcenjeneIgre = GetUserOcenjeneIgrice(trenutniKorisnik.ID, connection);


                string[] ocenjeneIgriceNiz = OcenjeneIgre.Split(',');

                
                if (ocenjeneIgriceNiz.Length >= 4)
                {
                    switch (Naziv)
                    {
                        case "Trkac":
                            ocenjeneIgriceNiz[0] = "5";
                            break;
                        case "Uhvati Poklone":
                            ocenjeneIgriceNiz[1] = "5";
                            break;
                        case "Snajper":
                            ocenjeneIgriceNiz[2] = "5"; break;
                        case "Balonomanija":
                            ocenjeneIgriceNiz[3] = "5";

                            break;
                        case "FlappyBird":
                            ocenjeneIgriceNiz[4] = "5";
                            break;
                        case "Space Battle":
                            ocenjeneIgriceNiz[5] = "5";
                            break;

                    }

                }

              
                string noviOcenjeneIgrice = string.Join(",", ocenjeneIgriceNiz);

                int ID = trenutniKorisnik.ID;
          
                string updateOcenjeneIgriceQuery = "UPDATE [User] SET OcenjeneIgre = @OcenjeneIgrice WHERE ID = @ID";

                using (SqlCommand ocenjeneIgriceCmd = new SqlCommand(updateOcenjeneIgriceQuery, connection))
                {
                    ocenjeneIgriceCmd.Parameters.AddWithValue("@OcenjeneIgrice", noviOcenjeneIgrice);
                    ocenjeneIgriceCmd.Parameters.AddWithValue("@ID", ID);

                    int ocenjeneIgriceRowsAffected = ocenjeneIgriceCmd.ExecuteNonQuery();


                }
            }
            foreach (Image i in igriceCanvas.Children.OfType<Image>())
            {
                if(i.Tag != "Game" && i.Tag == Naziv)
                {
                    i.Source = new BitmapImage(new Uri("pack://application:,,,/resourses/crnazvezda.png"));
                }
            }
        }

        private void zvezda4_Click(object sender, MouseButtonEventArgs e)
        {

            string Naziv = null;
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

                if (sender is FrameworkElement element)
                {
                  
                    object tagValue = element.Tag;

                    
                    if (tagValue != null)
                    {
                        Naziv = tagValue.ToString();

                      
                    }
                }



                string updateQuery = "UPDATE Game SET BrojOcena = BrojOcena + 1, ZbirOcena = ZbirOcena + 4 WHERE Naziv = @Naziv";

                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Naziv", Naziv);


                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Hvala Vam na  oceni igre: " + Naziv);
                    }
                    else
                    {

                    }
                }
                string OcenjeneIgre = GetUserOcenjeneIgrice(trenutniKorisnik.ID, connection);

                
                string[] ocenjeneIgriceNiz = OcenjeneIgre.Split(',');

             
                if (ocenjeneIgriceNiz.Length >= 4)
                {
                    switch(Naziv)
                    {
                        case "Trkac":
                            ocenjeneIgriceNiz[0] = "4";
                            break;
                        case "Uhvati Poklone":
                            ocenjeneIgriceNiz[1] = "4";
                            break;
                        case "Snajper":
                            ocenjeneIgriceNiz[2] = "4"; break;
                        case "Balonomanija":
                            ocenjeneIgriceNiz[3] = "4";

                            break;
                        case "FlappyBird":
                            ocenjeneIgriceNiz[4] = "4";
                            break;
                        case "Space Battle":
                            ocenjeneIgriceNiz[5] = "4";
                            break;
                    
                    }
                  
                }

          
                string noviOcenjeneIgrice = string.Join(",", ocenjeneIgriceNiz);

                int ID = trenutniKorisnik.ID; 
          
                string updateOcenjeneIgriceQuery = "UPDATE [User] SET OcenjeneIgre = @OcenjeneIgrice WHERE ID = @ID";

                using (SqlCommand ocenjeneIgriceCmd = new SqlCommand(updateOcenjeneIgriceQuery, connection))
                {
                    ocenjeneIgriceCmd.Parameters.AddWithValue("@OcenjeneIgrice", noviOcenjeneIgrice);
                    ocenjeneIgriceCmd.Parameters.AddWithValue("@ID", ID);

                    int ocenjeneIgriceRowsAffected = ocenjeneIgriceCmd.ExecuteNonQuery();

                    
                }
            }
            foreach (Image i in igriceCanvas.Children.OfType<Image>())
            {
                
                if (i.Tag != "Game" && i.Name != "petaZvezda" && i.Tag == Naziv)
                {
                    i.Source = new BitmapImage(new Uri("pack://application:,,,/resourses/crnazvezda.png"));
                }
            }
        }

        private void zvezda3_Click(object sender, MouseButtonEventArgs e)
        {

            string Naziv = null;
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

                if (sender is FrameworkElement element)
                {
              
                    object tagValue = element.Tag;

                
                    if (tagValue != null)
                    {
                        Naziv = tagValue.ToString();

                  
                    }
                }



                string updateQuery = "UPDATE Game SET BrojOcena = BrojOcena + 1, ZbirOcena = ZbirOcena + 3 WHERE Naziv = @Naziv";

                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Naziv", Naziv);


                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Hvala Vam na  oceni igre: " + Naziv);
                    }
                    else
                    {

                    }
                }
                string OcenjeneIgre = GetUserOcenjeneIgrice(trenutniKorisnik.ID, connection);


                string[] ocenjeneIgriceNiz = OcenjeneIgre.Split(',');

          
                if (ocenjeneIgriceNiz.Length >= 4)
                {
                    switch (Naziv)
                    {
                        case "Trkac":
                            ocenjeneIgriceNiz[0] = "3";
                            break;
                        case "Uhvati Poklone":
                            ocenjeneIgriceNiz[1] = "3";
                            break;
                        case "Snajper":
                            ocenjeneIgriceNiz[2] = "3"; break;
                        case "Balonomanija":
                            ocenjeneIgriceNiz[3] = "3";

                            break;
                        case "FlappyBird":
                            ocenjeneIgriceNiz[4] = "3";
                            break;
                        case "Space Battle":
                            ocenjeneIgriceNiz[5] = "3";
                            break;

                    }

                }

      
                string noviOcenjeneIgrice = string.Join(",", ocenjeneIgriceNiz);

                int ID = trenutniKorisnik.ID;
               
                string updateOcenjeneIgriceQuery = "UPDATE [User] SET OcenjeneIgre = @OcenjeneIgrice WHERE ID = @ID";

                using (SqlCommand ocenjeneIgriceCmd = new SqlCommand(updateOcenjeneIgriceQuery, connection))
                {
                    ocenjeneIgriceCmd.Parameters.AddWithValue("@OcenjeneIgrice", noviOcenjeneIgrice);
                    ocenjeneIgriceCmd.Parameters.AddWithValue("@ID", ID);

                    int ocenjeneIgriceRowsAffected = ocenjeneIgriceCmd.ExecuteNonQuery();


                }
            }

            foreach (Image i in igriceCanvas.Children.OfType<Image>())
            {

                if (i.Tag == Naziv)
                {
                    switch (i.Name)
                    {
                        case "trecaZvezda":
                            i.Source = new BitmapImage(new Uri("pack://application:,,,/resourses/crnazvezda.png"));
                            break;
                        case "drugaZvezda":
                            i.Source = new BitmapImage(new Uri("pack://application:,,,/resourses/crnazvezda.png"));
                            break;
                        case "prvaZvezda":
                            i.Source = new BitmapImage(new Uri("pack://application:,,,/resourses/crnazvezda.png"));
                            break;
                    }
                }

            }

        }

        private void zvezda2_Click(object sender, MouseButtonEventArgs e)
        {

            string Naziv = null;
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

                if (sender is FrameworkElement element)
                {
                 
                    object tagValue = element.Tag;

                    if (tagValue != null)
                    {
                        Naziv = tagValue.ToString();

                     
                    }
                }



                string updateQuery = "UPDATE Game SET BrojOcena = BrojOcena + 1, ZbirOcena = ZbirOcena + 2 WHERE Naziv = @Naziv";

                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Naziv", Naziv);


                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Hvala Vam na oceni igre: " + Naziv);
                    }
                    else
                    {

                    }
                }
                string OcenjeneIgre = GetUserOcenjeneIgrice(trenutniKorisnik.ID, connection);


                string[] ocenjeneIgriceNiz = OcenjeneIgre.Split(',');

             
                if (ocenjeneIgriceNiz.Length >= 4)
                {
                    switch (Naziv)
                    {
                        case "Trkac":
                            ocenjeneIgriceNiz[0] = "2";
                            break;
                        case "Uhvati Poklone":
                            ocenjeneIgriceNiz[1] = "2";
                            break;
                        case "Snajper":
                            ocenjeneIgriceNiz[2] = "2"; break;
                        case "Balonomanija":
                            ocenjeneIgriceNiz[3] = "2";

                            break;
                        case "FlappyBird":
                            ocenjeneIgriceNiz[4] = "2";
                            break;
                        case "Space Battle":
                            ocenjeneIgriceNiz[5] = "2";
                            break;

                    }

                }

          
                string noviOcenjeneIgrice = string.Join(",", ocenjeneIgriceNiz);

                int ID = trenutniKorisnik.ID;
           
                string updateOcenjeneIgriceQuery = "UPDATE [User] SET OcenjeneIgre = @OcenjeneIgrice WHERE ID = @ID";

                using (SqlCommand ocenjeneIgriceCmd = new SqlCommand(updateOcenjeneIgriceQuery, connection))
                {
                    ocenjeneIgriceCmd.Parameters.AddWithValue("@OcenjeneIgrice", noviOcenjeneIgrice);
                    ocenjeneIgriceCmd.Parameters.AddWithValue("@ID", ID);

                    int ocenjeneIgriceRowsAffected = ocenjeneIgriceCmd.ExecuteNonQuery();


                }
            }
            foreach (Image i in igriceCanvas.Children.OfType<Image>())
            {
                if(i.Tag == Naziv)
                {
                    switch (i.Name)
                    {
                        case "drugaZvezda":
                            i.Source = new BitmapImage(new Uri("pack://application:,,,/resourses/crnazvezda.png"));
                            break;
                        case "prvaZvezda":
                            i.Source = new BitmapImage(new Uri("pack://application:,,,/resourses/crnazvezda.png"));
                            break;
                    }
                }

                
            }
        }

        private void zvezda1_Click(object sender, MouseButtonEventArgs e)
        {
            
           
            string Naziv = null;
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                connection.Open();

                if (sender is FrameworkElement element)
                {
                   
                    object tagValue = element.Tag;

                    
                    if (tagValue != null)
                    {
                         Naziv = tagValue.ToString();

                      
                    }
                }



                string updateQuery = "UPDATE Game SET BrojOcena = BrojOcena + 1, ZbirOcena = ZbirOcena + 1 WHERE Naziv = @Naziv";

                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@Naziv", Naziv);

                    
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Hvala Vam na oceni igre: " + Naziv);
                    }
                    else
                    {
                      
                    }
                }
                string OcenjeneIgre = GetUserOcenjeneIgrice(trenutniKorisnik.ID, connection);


                string[] ocenjeneIgriceNiz = OcenjeneIgre.Split(',');

            
                if (ocenjeneIgriceNiz.Length >= 4)
                {
                    switch (Naziv)
                    {
                        case "Trkac":
                            ocenjeneIgriceNiz[0] = "1";
                            break;
                        case "Uhvati Poklone":
                            ocenjeneIgriceNiz[1] = "1";
                            break;
                        case "Snajper":
                            ocenjeneIgriceNiz[2] = "1"; break;
                        case "Balonomanija":
                            ocenjeneIgriceNiz[3] = "1";

                            break;
                        case "FlappyBird":
                            ocenjeneIgriceNiz[4] = "1";
                            break;
                        case "Space Battle":
                            ocenjeneIgriceNiz[5] = "1";
                            break;

                    }

                }


                string noviOcenjeneIgrice = string.Join(",", ocenjeneIgriceNiz);

                int ID = trenutniKorisnik.ID;
         
                string updateOcenjeneIgriceQuery = "UPDATE [User] SET OcenjeneIgre = @OcenjeneIgrice WHERE ID = @ID";

                using (SqlCommand ocenjeneIgriceCmd = new SqlCommand(updateOcenjeneIgriceQuery, connection))
                {
                    ocenjeneIgriceCmd.Parameters.AddWithValue("@OcenjeneIgrice", noviOcenjeneIgrice);
                    ocenjeneIgriceCmd.Parameters.AddWithValue("@ID", ID);

                    int ocenjeneIgriceRowsAffected = ocenjeneIgriceCmd.ExecuteNonQuery();


                }
            }
            foreach (Image i in igriceCanvas.Children.OfType<Image>())
            {

               if(i.Name == "prvaZvezda" && i.Tag == Naziv)
                {
                    i.Source = new BitmapImage(new Uri("pack://application:,,,/resourses/crnazvezda.png"));
                }
                    
                
            }
        }

        private string GetUserOcenjeneIgrice(int ID, SqlConnection connection)
        {
            string ocenjeneIgrice = null;
            string selectQuery = "SELECT OcenjeneIgre FROM [User] WHERE ID = @ID";

            using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
            {
                cmd.Parameters.AddWithValue("@ID", ID);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ocenjeneIgrice = reader["OcenjeneIgre"].ToString();
                    }
                }
            }

            return ocenjeneIgrice;
        }
      







        private void platiSpaceBattle_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            int ID = trenutniKorisnik.ID;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string currentPlaceneIgrice = GetUserPlaceneIgrice(ID, connection);

              
                string[] igre = currentPlaceneIgrice.Split(',');


                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                igre[5] = currentDate;


                string newPlaceneIgrice = string.Join(",", igre);

                trenutniKorisnik.PlaceneIgrice = newPlaceneIgrice;

                string updateQuery = "UPDATE [User] SET PlaceneIgrice = @NewPlaceneIgrice WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@NewPlaceneIgrice", newPlaceneIgrice);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Uspesno ste se pretplatili za igricu Space Battle");
        }

        private void platiFlappyBirdClick(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            int ID = trenutniKorisnik.ID;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string currentPlaceneIgrice = GetUserPlaceneIgrice(ID, connection);

         
                string[] igre = currentPlaceneIgrice.Split(',');


                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                igre[4] = currentDate;


                string newPlaceneIgrice = string.Join(",", igre);


                trenutniKorisnik.PlaceneIgrice = newPlaceneIgrice;

                string updateQuery = "UPDATE [User] SET PlaceneIgrice = @NewPlaceneIgrice WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@NewPlaceneIgrice", newPlaceneIgrice);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Uspesno ste se pretplatili za igricu Flappy Bird");
        }

        private void platiBalonomanija_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            int ID = trenutniKorisnik.ID;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string currentPlaceneIgrice = GetUserPlaceneIgrice(ID, connection);

            
                string[] igre = currentPlaceneIgrice.Split(',');


                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                igre[3] = currentDate;


                string newPlaceneIgrice = string.Join(",", igre);

                trenutniKorisnik.PlaceneIgrice = newPlaceneIgrice;


                string updateQuery = "UPDATE [User] SET PlaceneIgrice = @NewPlaceneIgrice WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@NewPlaceneIgrice", newPlaceneIgrice);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Uspesno ste se pretplatili za igricu Balonomanija");
        }

        private void platiSnajper_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            int ID = trenutniKorisnik.ID;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string currentPlaceneIgrice = GetUserPlaceneIgrice(ID, connection);

              
                string[] igre = currentPlaceneIgrice.Split(',');


                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                igre[2] = currentDate;


                string newPlaceneIgrice = string.Join(",", igre);


                trenutniKorisnik.PlaceneIgrice = newPlaceneIgrice;

                string updateQuery = "UPDATE [User] SET PlaceneIgrice = @NewPlaceneIgrice WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@NewPlaceneIgrice", newPlaceneIgrice);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Uspesno ste se pretplatili za igricu Snajper");
        }

        private void platiUhvatiPoklone_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            int ID = trenutniKorisnik.ID;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string currentPlaceneIgrice = GetUserPlaceneIgrice(ID, connection);

             
                string[] igre = currentPlaceneIgrice.Split(',');


                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                igre[1] = currentDate;


                string newPlaceneIgrice = string.Join(",", igre);


                trenutniKorisnik.PlaceneIgrice = newPlaceneIgrice;

                string updateQuery = "UPDATE [User] SET PlaceneIgrice = @NewPlaceneIgrice WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@NewPlaceneIgrice", newPlaceneIgrice);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Uspesno ste se pretplatili za igricu Uhvati Poklone");
        }

        private void platiTrkac_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            int ID = trenutniKorisnik.ID;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string currentPlaceneIgrice = GetUserPlaceneIgrice(ID, connection);

          
                string[] igre = currentPlaceneIgrice.Split(',');

                
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd"); 
                igre[0] = currentDate;

               
                string newPlaceneIgrice = string.Join(",", igre);


                trenutniKorisnik.PlaceneIgrice = newPlaceneIgrice;

                string updateQuery = "UPDATE [User] SET PlaceneIgrice = @NewPlaceneIgrice WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@NewPlaceneIgrice", newPlaceneIgrice);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Uspesno ste se pretplatili za igricu Trkac");
        }

      
        private void OtvoriNoviProzor_Click(object sender, RoutedEventArgs e)
        {
            trkac = new Trkac();
           
            trkac.Show();
            
        }
        private void OtvoriUhvatiPoklone_Click(object sender, RoutedEventArgs e)
        {
            uhvatiPoklone = new Uhvati_Poklone();
            
            uhvatiPoklone.Show();
        }
        private void OtvoriSnajper_Click(object sender, RoutedEventArgs e)
        {
            sniper = new Sniper();
            
            sniper.Show();
        }
        private void OtvoriBalonomanija_Click(object sender, RoutedEventArgs e)
        {
           balon = new balonomanija();
           
            balon.Show();
        }
        private void OtvoriFlappyBird_Click(object sender, RoutedEventArgs e)
        {
            flappy = new FlappyBird();
            
            flappy.Show();
        }
        private void OtvoriSpaceBattle_Click(object sender, RoutedEventArgs e)
        {
            space = new Space_Battle();
            space.Show();
        }
        private string GetUserPlaceneIgrice(int ID, SqlConnection connection)
        {
            string query = "SELECT PlaceneIgrice FROM [User] WHERE ID = @ID";
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@ID", ID);
                return cmd.ExecuteScalar() as string;
            }
        }

    }
   

    public class Igra
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public string SlikaPutanja { get; set; }
        public int BrojOcena { get; set; }
        public int ZbirOcena { get; set; }
    }
}

