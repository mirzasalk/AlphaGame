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
using System.Net.Mail;

namespace GameTreasury
{
   
    public partial class RegistracijaUserControl : UserControl
    {
        string connectionString;
        public RegistracijaUserControl()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
        }

        private void Register_click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtLastName.Text))
            {
                MessageBox.Show("Unesite ime i prezime.");
                return;
            }

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

                   
                    using (SqlCommand checkCommand = new SqlCommand("SELECT COUNT(*) FROM [User] WHERE Email = @Email", connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Email", txtEmail.Text);

                        int existingUserCount = (int)checkCommand.ExecuteScalar();

                        if (existingUserCount > 0)
                        {
                            MessageBox.Show("Email adresa već u upotrebi.");
                        }
                        else
                        {
                            
                            using (SqlCommand insertCommand = new SqlCommand("INSERT INTO [User] (Ime, Prezime, Email, Sifra, Clanarina, PlaceneIgrice,OcenjeneIgre ,EmailVerifikacija) VALUES (@Ime, @Prezime, @Email, @Sifra, @Clanarina, @PlaceneIgrice,@OcenjeneIgre ,@EmailVerifikacija)", connection))
                            {
                                DateTime danas = DateTime.Now;

                              
                                DateTime pre40Dana = danas.AddDays(-40);

                                string pre40DanaString = pre40Dana.ToString("yyyy-MM-dd");

                                string[] placeneIgriceArray = { pre40DanaString, pre40DanaString, pre40DanaString, pre40DanaString, pre40DanaString, pre40DanaString }; // Dodajte igre koje želite



                             
                                string placeneIgriceString = string.Join(",", placeneIgriceArray);

                                insertCommand.Parameters.AddWithValue("@Ime", txtName.Text);
                                insertCommand.Parameters.AddWithValue("@Prezime", txtLastName.Text);
                                insertCommand.Parameters.AddWithValue("@Email", txtEmail.Text);
                                insertCommand.Parameters.AddWithValue("@Sifra", txtPassword.Password);
                                insertCommand.Parameters.AddWithValue("@Clanarina", new DateTime(1753, 1, 1)); 
                                insertCommand.Parameters.AddWithValue("@PlaceneIgrice", placeneIgriceString); 
                                insertCommand.Parameters.AddWithValue("@OcenjeneIgre", "0,0,0,0,0,0");
                                insertCommand.Parameters.AddWithValue("@EmailVerifikacija", 0); 

                                insertCommand.ExecuteNonQuery();

                                MessageBox.Show("Uspešno ste se registrovali!");
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
