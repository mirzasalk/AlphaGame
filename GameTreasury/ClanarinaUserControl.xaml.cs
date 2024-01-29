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
using static System.Net.Mime.MediaTypeNames;
using System.Configuration;
using System.Data.SqlClient;


namespace GameTreasury
{
    /// <summary>
    /// Interaction logic for ClanarinaUserControl.xaml
    /// </summary>
    public partial class ClanarinaUserControl : UserControl
    {
        Korisnik trenutniKorisnik;
        DateTime datumZa30Dana;
        DateTime trenutniDatum = DateTime.Now;
        public ClanarinaUserControl()
        {
            InitializeComponent();
            trenutniKorisnik = MainWindow.TrenutniKorisnik;
            datumZa30Dana = trenutniDatum.AddDays(-30);
            if (trenutniKorisnik == null || trenutniKorisnik.Clanarina < datumZa30Dana)
            {
                naslov.Text = "Postanite Član i Igrajte Sve Igre na Sajtu";
                tekst.Text = "Dobrodošli u ALPHAGAME - vašu destinaciju za beskrajnu zabavu i uzbuđenje! Da biste iskusili pun potencijal našeg sajta i pristupili svim našim uzbudljivim igricama, pozivamo vas da postanete član.Kao član , otvarate vrata za neograničenu zabavu. ";
                pijaviSeBtn.Visibility = Visibility.Visible;


            }
            else
            {
                TimeSpan razlika = trenutniDatum - trenutniKorisnik.Clanarina;

                // Dobijanje broja dana iz TimeSpan objekta
                int brojDana = razlika.Days;
                int trajanje = 30 - brojDana;
                naslov.Text = "Hvala Vam Na Poverenju";
                tekst.Text = "Vaša članarina ističe za "+ trajanje + "dana.";
                pijaviSeBtn.Visibility = Visibility.Hidden;
            }
        }

        private void PostaniClanClick(object sender, RoutedEventArgs e)
        {
            trenutniKorisnik.Clanarina = DateTime.Now;
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Prvo definirajte SQL upit za ažuriranje članarine korisnika
                    string updateQuery = "UPDATE [User] SET Clanarina = @Clanarina WHERE ID = @ID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Clanarina", trenutniKorisnik.Clanarina);
                        cmd.Parameters.AddWithValue("@ID", trenutniKorisnik.ID);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Uspješno ažuriranje članarine
                            MessageBox.Show("Uspešno ste postali član! Sada možete uživati u svim igrama na sajtu.");
                        }
                        else
                        {
                            MessageBox.Show("Nije uspelo ažuriranje članarine.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri povezivanju sa bazom podataka: " + ex.Message);
                }
            }

            MessageBox.Show("Uspešno ste postali član! Sada možete uživati u svim igrama na sajtu.");
            datumZa30Dana = trenutniDatum.AddDays(-30);
            if (trenutniKorisnik == null || trenutniKorisnik.Clanarina < datumZa30Dana)
            {
                naslov.Text = "Postanite Član i Igrajte Sve Igre na Sajtu";
                tekst.Text = "Dobrodošli u ALPHAGAME - vašu destinaciju za beskrajnu zabavu i uzbuđenje! Da biste iskusili pun potencijal našeg sajta i pristupili svim našim uzbudljivim igricama, pozivamo vas da postanete član.Kao član , otvarate vrata za neograničenu zabavu. ";
                pijaviSeBtn.Visibility = Visibility.Visible;


            }
            else
            {
                TimeSpan razlika = trenutniDatum - trenutniKorisnik.Clanarina;

                // Dobijanje broja dana iz TimeSpan objekta
                int brojDana = razlika.Days;
                int trajanje = 30 - brojDana;
                naslov.Text = "Hvala Vam Na Poverenju";
                tekst.Text = "Vaša članarina ističe za " + trajanje + "dana.";
                pijaviSeBtn.Visibility = Visibility.Hidden;
            }
        }
    }
}
