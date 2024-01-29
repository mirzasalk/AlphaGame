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
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;


namespace GameTreasury
{
   
    public partial class Prijavljivanje : UserControl
    {
        string connectionString;
        public Prijavljivanje()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
        }

        private void Prijavljivanje_click(object sender, RoutedEventArgs e)
        {
            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Unesite ispravnu email adresu.");
                return;
            }

           
            if (txtPassword.Password.Length < 8)
            {
                MessageBox.Show("Šifra mora imati najmanje 8 karaktera.");
                return;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    
                    using (SqlCommand selectCommand = new SqlCommand("SELECT ID, Ime, Prezime, Email, Clanarina, PlaceneIgrice, EmailVerifikacija, OcenjeneIgre FROM [User] WHERE Email = @Email AND Sifra = @Sifra", connection))
                    {
                        selectCommand.Parameters.AddWithValue("@Email", txtEmail.Text);
                        selectCommand.Parameters.AddWithValue("@Sifra", txtPassword.Password);

                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var korisnik = new Korisnik
                                {
                                    ID = reader.GetInt32(0),
                                    Ime = reader.GetString(1),
                                    Prezime = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    Clanarina = reader.GetDateTime(4), 
                                    PlaceneIgrice = reader.GetString(5),
                                    EmailVerifikacija = reader.GetBoolean(6),
                                    OcenjeneIgre = reader.GetString(7)
                                };

                                ((MainWindow)Application.Current.MainWindow).PostaviTrenutnogKorisnika(korisnik);

                               
                               var token = GenerisiToken(korisnik); 
                               ((MainWindow)Application.Current.MainWindow).PostaviTrenutniToken(token);

                                MessageBox.Show("Uspešno ste se prijavili!");
                            }
                            else
                            {
                                MessageBox.Show("Unesite ispravan email i šifru.");
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

        public Token GenerisiToken(Korisnik korisnik)
        {
            // Generišite ključ dužine 128 bitova
            //  var securityKey = new SymmetricSecurityKey(Guid.NewGuid().ToByteArray());
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dugi_tajni_kljuc_duži_od_32_karaktera_12345"));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://vasaaplikacija.com",
                audience: "https://vasaaplikacija.com",
                claims: new List<Claim>
                {
            new Claim("korisnik_id", korisnik.ID.ToString()),
            new Claim("email", korisnik.Email),
                    // Dodajte druge potrebne claim-ove
                },
                expires: DateTime.Now.AddDays(1), // Token će važiti 1 dan
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            // Kreirajte instancu vaše Token klase i postavite vrednosti
            var gameTreasuryToken = new Token
            {
                Vrednost = tokenString,
                DatumIsteka = token.ValidTo,
                PovezaniKorisnik = korisnik
            };

            return gameTreasuryToken;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
