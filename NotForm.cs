using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static NotAt.Class2;
using static NotAt.Class2.GlobalVariables;
using System.Data.SQLite;

namespace NotAt
{
    public partial class NotForm : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public NotForm()
        {
            InitializeComponent();
        }

        private void NotForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            KullaniciListesiGetir();
            AktifKullanicilariYukle();
        }

        private void KullaniciListesiGetir()
        {
            var kullaniciListesi = dbHelper.GetKullaniciListesi(GlobalVariables.KullaniciAd);
            comboBox1.Items.Clear();

            foreach (var item in kullaniciListesi)
            {
                comboBox1.Items.Add(item);
            }
        }

        private void AktifKullanicilariYukle()
        {
            string serverIP = NetworkHelper.GetLocalIPAddress();
            int port = 5000;

            var activeUsers = NetworkHelper.ConnectToServer(serverIP, port);
            comboBox1.Items.Clear();

            foreach (var user in activeUsers)
            {
                comboBox1.Items.Add(user);
            }
        }

      


        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (comboBox1.SelectedItem is ComboBoxItem selectedItem)
                {
                    string selectedKullaniciAd = selectedItem.Text; // Seçilen kullanıcı adı
                    string selectedKullaniciId = selectedItem.Value; // Seçilen kullanıcı ID
                    string mesaj = richTextBox1.Text;

                    // MySQL bağlantısı
                    string connectionString =
                        "server=notat-db-do-user-18525492-0.h.db.ondigitalocean.com;" +
                        "port=25060;" +
                        "database=proje;" +
                        "user=doadmin;" +
                        "password=AVNS_i5KKCR44-CAV6oo7xLn;" +
                        "sslmode=Required;";

                    using (MySqlConnection baglan = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            baglan.Open();
                            string sql = "INSERT INTO mesaj (kisi_id, mesaj, alici_kullanici_ad) VALUES (@kisi_id, @mesaj, @alici_kullanici_ad)";
                            MySqlCommand komut = new MySqlCommand(sql, baglan);

                            komut.Parameters.AddWithValue("@kisi_id", GlobalVariables.kisi_id);
                            komut.Parameters.AddWithValue("@mesaj", mesaj);
                            komut.Parameters.AddWithValue("@alici_kullanici_ad", selectedKullaniciAd);

                            komut.ExecuteNonQuery();

                            MessageBox.Show("Mesaj başarıyla gönderildi!");
                            richTextBox1.Clear();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hata: " + ex.Message);
                        }
                    }

                    e.Handled = true;
                }
                else
                {
                    MessageBox.Show("Lütfen bir alıcı seçin!");
                }
            }
        }

        
    
        private void NotForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }
    }
}
