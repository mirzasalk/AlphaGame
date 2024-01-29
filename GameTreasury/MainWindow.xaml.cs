using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Media.Imaging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Windows.Threading;

namespace GameTreasury
{
    public partial class MainWindow : Window
    {
        bool validacijaTokena;
        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timerVremeIgranja = new DispatcherTimer();
        public static Korisnik TrenutniKorisnik { get; set; }
        public static Token TrenutniToken { get; set; }
        public static MainWindow CurrentInstance { get; private set; }

        Korisnik trenutniKorisnik = MainWindow.TrenutniKorisnik;
        string ime;
        string prezime;
        string email;
        string OcenjeneIgre;
        DateTime clanarina;
        string placeneIgrice;
        bool emailVerifikacija;
        int[] vremeIgranja = { 0, 0, 0, 0, 0, 0 };
        List<string> komentari = new List<string>();
       
        string IdKomentara;

        public MainWindow()
        {
            CurrentInstance = this;
            InitializeComponent();
           


            mainFrame.Content = new IgriceUserControl();
            validacijaTokena = false;
            timer.Tick += proveraValidacijeTokena;
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Start();

            timerVremeIgranja.Tick += proveraVremenaIgranja;
            timerVremeIgranja.Interval = TimeSpan.FromMilliseconds(1000);
            timerVremeIgranja.Start();

            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();


                    string query = "SELECT komentari FROM Game";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    string komentar = reader.GetString(0);
                                    komentari.Add(komentar);
                                }
                            }
                        }
                    }




                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri povezivanju sa bazom podataka: " + ex.Message);
                }
            }
        }

        private void proveraVremenaIgranja(object sender, EventArgs e)
        {
            if (vremeIgranja[0] > 0)
            {
                vremeIgranja[0]--;
            }
            if (vremeIgranja[1] > 0)
            {
                vremeIgranja[1]--;
            }
            if (vremeIgranja[2] > 0)
            {
                vremeIgranja[2]--;
            }
            if (vremeIgranja[3] > 0)
            {
                vremeIgranja[3]--;
            }
            if (vremeIgranja[4] > 0)
            {
                vremeIgranja[4]--;
            }
            if (vremeIgranja[5] > 0)
            {
                vremeIgranja[5]--;
            }
        }

        private void proveraValidacijeTokena(object sender, EventArgs e)
        {
            
            
            if (TrenutniToken != null)
            {
                validacijaTokena = ProveriToken(TrenutniToken.Vrednost);
            }
            else
            {
                validacijaTokena = false;
            }

           if(validacijaTokena == false)
            {
                RegBtn.Visibility = Visibility.Visible;
                LoginBtn.Visibility = Visibility.Visible;
                hesh.Visibility = Visibility.Visible;
                odjaviseBtn.Visibility = Visibility.Hidden;

            }
            else
            {
                RegBtn.Visibility = Visibility.Hidden;
                LoginBtn.Visibility = Visibility.Hidden;
                hesh.Visibility = Visibility.Hidden;
                odjaviseBtn.Visibility = Visibility.Visible;
            }
           
             
               

        }
       

        public void PostaviTrenutnogKorisnika(Korisnik korisnik)
        {
            TrenutniKorisnik = korisnik;
            trenutniKorisnik = korisnik;
            if (trenutniKorisnik != null)
            {
                
                ime = trenutniKorisnik.Ime;
                prezime = trenutniKorisnik.Prezime;
                email = trenutniKorisnik.Email;
                clanarina = trenutniKorisnik.Clanarina;
                placeneIgrice = trenutniKorisnik.PlaceneIgrice;
                OcenjeneIgre = trenutniKorisnik.OcenjeneIgre;
                emailVerifikacija = trenutniKorisnik.EmailVerifikacija;
            }
        
        }

        public void PostaviTrenutniToken(Token token)
        {
            TrenutniToken = token;
            mainFrame.Content = new IgriceUserControl();

        }
       
        private void IgriceButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new IgriceUserControl(); 
        }

        private void ONamaButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new ONamaUserControl(); 
        }

        private void ClanarinaButton_Click(object sender, RoutedEventArgs e)
        {
            if(validacijaTokena)
            {
                mainFrame.Content = new ClanarinaUserControl(); 
            }
            else
            {
                mainFrame.Content = new Prijavljivanje();
            }
           
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Prijavljivanje();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new RegistracijaUserControl();
        }
        public bool ProveriToken(string token)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dugi_tajni_kljuc_duži_od_32_karaktera_12345"));
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://vasaaplikacija.com",
                    ValidateAudience = true,
                    ValidAudience = "https://vasaaplikacija.com",
                    ValidateLifetime = true,
                    IssuerSigningKey = securityKey
                };

                SecurityToken validatedToken;
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                
                return true;
            }
            catch (Exception)
            {
               
                return false;
            }
        }

        private void OdjaviseBtn_Click(object sender, RoutedEventArgs e)
        {
            TrenutniToken = null;
            TrenutniKorisnik = null;
            ime = null;
            prezime = null;
            email =null;
            clanarina = DateTime.MinValue;




        }
        public void prikaziKomentare(String tag)
        {
            
            KometariText.Text = "";
            dodajKomentarBtn.Tag = tag;

            string[] tempForKom;
           
                
                    int i = 2;
            switch (tag)
            {
                case "Trkac":

                    i = 2;
                    if (komentari[0] != "")
                    { 
                            tempForKom = komentari[0].Split(',');
                           
                            KometariText.Text = "";
                            foreach (var textTemp in tempForKom)
                            {
                          
                                if (i % 2 == 0)
                                {
                                    KometariText.Text += Environment.NewLine + "------------------------------------------------------------------------" + Environment.NewLine + textTemp;
                                }
                                else
                                {
                                    KometariText.Text += Environment.NewLine + textTemp;
                                }
                            i++;

                        }
                    }
                    else
                    {
                        KometariText.Text = "nema komentara";
                    }
                    break;
                        case "Uhvati Poklone":
                    i = 2;
                    if (komentari[1] != "")
                    {
                        tempForKom = komentari[1].Split(',');

                        KometariText.Text = "";
                        foreach (var textTemp in tempForKom)
                        {
                            
                            if (i % 2 == 0)
                            {
                                KometariText.Text += Environment.NewLine + "------------------------------------------------------------------------" + Environment.NewLine + textTemp;
                            }
                            else
                            {
                                KometariText.Text += Environment.NewLine + textTemp;
                            }
                            i++;

                        }
                    }
                    else
                    {
                        KometariText.Text = "nema komentara";
                    }
                    break;
                        case "Space Battle":
                    i = 2;
                    if (komentari[2] != "")
                    {
                        tempForKom = komentari[2].Split(',');

                        KometariText.Text = "";
                        foreach (var textTemp in tempForKom)
                        {
                            
                            if (i % 2 == 0)
                            {
                                KometariText.Text += Environment.NewLine + "------------------------------------------------------------------------" + Environment.NewLine + textTemp;
                            }
                            else
                            {
                                KometariText.Text += Environment.NewLine + textTemp;
                            }
                            i++;

                        }
                    }
                    else
                    {
                        KometariText.Text = "nema komentara";
                    }
                    break;
                        case "FlappyBird":
                    i = 2;
                    if (komentari[3] != "")
                    {
                        tempForKom = komentari[3].Split(',');

                        KometariText.Text = "";
                        foreach (var textTemp in tempForKom)
                        {
                            
                            if (i % 2 == 0)
                            {
                                KometariText.Text += Environment.NewLine + "------------------------------------------------------------------------" + Environment.NewLine + textTemp;
                            }
                            else
                            {
                                KometariText.Text += Environment.NewLine + textTemp;
                            }
                            i++;

                        }
                    }
                    else
                    {
                        KometariText.Text = "nema komentara";
                    }
                    break;
                        case "Balonomanija":
                    i = 2;
                    if (komentari[4] != "")
                    {
                        tempForKom = komentari[4].Split(',');

                        KometariText.Text = "";
                        foreach (var textTemp in tempForKom)
                        {
                            
                            if (i % 2 == 0)
                            {
                                KometariText.Text += Environment.NewLine + "------------------------------------------------------------------------" + Environment.NewLine + textTemp;
                            }
                            else
                            {
                                KometariText.Text += Environment.NewLine + textTemp;
                            }
                            i++;
                        }
                    }
                    else
                    {
                        KometariText.Text = "nema komentara";
                    }
                    break;
                        case "Snajper":
                    i = 1;
                    if (komentari[5] != "")
                    {
                        tempForKom = komentari[5].Split(',');

                        KometariText.Text = "";
                        foreach (var textTemp in tempForKom)
                        {
                            i++;
                            if (i % 2 == 0)
                            {
                                KometariText.Text += Environment.NewLine + "------------------------------------------------------------------------" + Environment.NewLine + textTemp;
                            }
                            else
                            {
                                KometariText.Text += Environment.NewLine + textTemp;
                            }

                        }
                    }
                    else
                    {
                        KometariText.Text = "nema komentara";
                    }
                    break;


                    }
               



                
            
                  
                      
                    
            if (TrenutniToken != null)
            {
                DodajScroll.Visibility = Visibility.Visible;
            }
            myScrollViewer.Visibility = Visibility.Visible;
                 KometariText.Visibility = Visibility.Visible;
                 BlurBackground.Visibility = Visibility.Visible;
           
        }

       

        private void komentariExit(object sender, RoutedEventArgs e)
        {
            
            myScrollViewer.Visibility = Visibility.Hidden;
            BlurBackground.Visibility = Visibility.Hidden;
            DodajScroll.Visibility = Visibility.Hidden;
            KometariText.Visibility = Visibility.Hidden;
        }

        private void prikaziUnosWindov(object sender, RoutedEventArgs e)
        {
            myScrollViewer.Visibility = Visibility.Hidden;
           
            DodajScroll.Visibility = Visibility.Hidden;
            KometariText.Visibility = Visibility.Hidden;
            dodajKomentarBtn.Visibility = Visibility.Visible;
         
            poljeZaIzlaz.Visibility = Visibility.Visible;
            poljeZaUnos.Visibility = Visibility.Visible;
            recZaKom.Visibility = Visibility;
        }

        private void dodajKomentar(object sender, RoutedEventArgs e)
        {

            if(KometariText.Text == "nema komentara")
            {
                KometariText.Text = "";
            }
            KometariText.Text += Environment.NewLine + "------------------------------------------------------------------------"+ Environment.NewLine+ trenutniKorisnik.Ime +" "+ trenutniKorisnik.Prezime + Environment.NewLine + Environment.NewLine + poljeZaUnos.Text + Environment.NewLine;
            
            myScrollViewer.Visibility = Visibility.Visible;

            DodajScroll.Visibility = Visibility.Visible;
            KometariText.Visibility = Visibility.Visible;
            dodajKomentarBtn.Visibility = Visibility.Hidden;
            poljeZaIzlaz.Visibility = Visibility.Hidden;
            poljeZaUnos.Visibility = Visibility.Hidden;
            recZaKom.Visibility = Visibility.Hidden;
            string sqlQuery = "UPDATE Game SET komentari = @komentari WHERE Naziv = @Naziv";


            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
           
            using (SqlConnection connection = new SqlConnection(connectionString))
                {
              
                connection.Open();
                string Naziv = null;

                string komentariTemp = "";
               
                if (sender is FrameworkElement element)
                {

                    object tagValue = element.Tag;


                    if (tagValue != null)
                    {
                        Naziv = tagValue.ToString();

                    }
                }
                
                switch (Naziv)
                {
                    case "Trkac":
                            komentariTemp += komentari[0] + trenutniKorisnik.Ime + " "+ trenutniKorisnik.Prezime +  "," + poljeZaUnos.Text + ",";
                        komentari[0] = komentariTemp;
                        break;
                    case "Uhvati Poklone":
                        komentariTemp += komentari[1] + trenutniKorisnik.Ime + " " + trenutniKorisnik.Prezime + "," + poljeZaUnos.Text +",";
                        komentari[1] = komentariTemp;
                        break;
                    case "Space Battle":
                        komentariTemp += komentari[2] + trenutniKorisnik.Ime + " " + trenutniKorisnik.Prezime + "," + poljeZaUnos.Text +",";
                        komentari[2] = komentariTemp;
                        break;
                    case "FlappyBird":
                        komentariTemp += komentari[3] + trenutniKorisnik.Ime + " " + trenutniKorisnik.Prezime + ","+ poljeZaUnos.Text +",";
                        komentari[3] = komentariTemp;
                        break;
                    case "Balonomanija":
                        komentariTemp += komentari[4] + trenutniKorisnik.Ime + " " + trenutniKorisnik.Prezime + "," + poljeZaUnos.Text+ ",";
                        komentari[4] = komentariTemp;
                        break;
                    case "Snajper":
                        komentariTemp += komentari[5] + trenutniKorisnik.Ime + " " + trenutniKorisnik.Prezime + "," + poljeZaUnos.Text +",";
                        komentari[5] = komentariTemp;
                        break;

                       
                }
                poljeZaUnos.Text = "";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                  
                    command.Parameters.AddWithValue("@komentari", komentariTemp); 
                    command.Parameters.AddWithValue("@Naziv", Naziv);

                  
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        
                    }
                    else
                    {
                       
                    }
                }
            }
            }

        private void komentariExitDva(object sender, RoutedEventArgs e)
        {
            myScrollViewer.Visibility = Visibility.Visible;
            
            DodajScroll.Visibility = Visibility.Visible;
            KometariText.Visibility = Visibility.Visible;
            dodajKomentarBtn.Visibility = Visibility.Hidden;
            poljeZaIzlaz.Visibility = Visibility.Hidden;
            poljeZaUnos.Visibility = Visibility.Hidden;
            recZaKom.Visibility = Visibility.Hidden;
        }

        private void platiZaIgru(object sender, RoutedEventArgs e)
        {
            string tagg;
            if (sender is FrameworkElement element)
            {

                object tagValue = element.Tag;


                if (tagValue != null)
                {
                    if(UnesenoVreme.Text != "")
                    {
                        int number = int.Parse(UnesenoVreme.Text);
                        tagg = tagValue.ToString();
                        switch (tagg)
                        {
                            case "Trkac":
                                vremeIgranja[0]= number;
                                break;
                            case "Uhvati Poklone":
                                vremeIgranja[1] = number;
                                break;
                            case "Snajper":
                                vremeIgranja[2] = number;
                                break;
                            case "FlappyBird":
                                vremeIgranja[3] = number;
                                break;
                            case "Balonomanija":
                                vremeIgranja[4] = number;
                                break;
                            case "Space Battle":
                                vremeIgranja[5] = number;
                                break;

                        }
                        
                        UnosVremenaScroll.Visibility = Visibility.Hidden;
                        BlurBackground.Visibility = Visibility.Hidden;
                        dodajVremeBtn.Visibility = Visibility.Hidden;
                    }
                   
                }

              
            }
           
        }
        public int[] vrednostiPlacenogVremena()
        {
            return vremeIgranja;

        }

        private void unosVremExit(object sender, RoutedEventArgs e)
        {
            UnosVremenaScroll.Visibility = Visibility.Hidden;
            BlurBackground.Visibility = Visibility.Hidden;
            dodajVremeBtn.Visibility = Visibility.Hidden;
            UnesenoVreme.Text = "";
        }
        public void prikazProzoraZaUnosVremena(string t)
        {
            UnosVremenaScroll.Visibility = Visibility.Visible;
            BlurBackground.Visibility = Visibility.Visible;
            dodajVremeBtn.Visibility = Visibility.Visible;
            dodajVremeBtn.Tag = t;
        }
    }
}

