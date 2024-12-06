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
        public NotForm()
        {
            InitializeComponent();
        }

        private void NotForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Kullanıcı listesi getirme fonksiyonu
            KullaniciListesiGetir();

            // Sunucunun IP adresini almak için fonksiyonu çağırıyoruz
            string serverIP = GetLocalIPAddress(); // Sunucu bilgisayarının IP adresini dinamik olarak alıyoruz
            int port = 5000;

            // TCP bağlantısı başlatma
            try
            {
                TcpClient client = new TcpClient(serverIP, port);
                NetworkStream stream = client.GetStream();

                // Burada ağ üzerinden işlem yapılabilir
                // Örnek: stream.Write() ya da stream.Read()
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"Bağlantı hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sunucunun IP adresini almak için statik fonksiyon
        public static string GetLocalIPAddress()
        {
            string localIP = "127.0.0.1"; // Varsayılan olarak localhost
            try
            {
                foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    foreach (var address in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        // Geçerli bir IPv4 adresi varsa
                        if (address.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            // Adresi döndür
                            localIP = address.Address.ToString();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"IP adresi alınırken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return localIP;
        }


        private void KullaniciListesiGetir()
        {
            string connectionString = "Data Source=mydatabase.sqlite;Version=3;";

            using (SQLiteConnection baglan = new SQLiteConnection(connectionString))
            {
                try
                {
                    baglan.Open();

                    // Kullanıcının adını sorgudan hariç tut
                    string sql = "SELECT id, kullanici_ad FROM kullanicilar WHERE kullanici_ad != @girisYapanKullaniciAd";

                    using (SQLiteCommand komut = new SQLiteCommand(sql, baglan))
                    {
                        komut.Parameters.AddWithValue("@girisYapanKullaniciAd", GlobalVariables.KullaniciAd);

                        using (SQLiteDataReader reader = komut.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // ComboBox'a kullanıcı adını ekle
                                comboBox1.Items.Add(new ComboBoxItem
                                {
                                    Text = reader["kullanici_ad"].ToString(),
                                    Value = reader["id"].ToString()
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
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

                    // SQLite bağlantısı
                    string connectionString = "Data Source=mydatabase.sqlite;Version=3;";

                    using (SQLiteConnection baglan = new SQLiteConnection(connectionString))
                    {
                        try
                        {
                            baglan.Open();
                            string sql = "INSERT INTO mesaj (kisi_id, mesaj, alici_kullanici_ad) VALUES (@kisi_id, @mesaj, @alici_kullanici_ad)";
                            SQLiteCommand komut = new SQLiteCommand(sql, baglan);

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

        private string GetUserIpAddress(string userId)
        {
            string ipAddress = null;

            using (SQLiteConnection baglan = new SQLiteConnection("Data Source=mydatabase.sqlite;Version=3;"))
            {
                try
                {
                    baglan.Open();
                    string sql = "SELECT ip_adresi FROM kullanicilar WHERE id = @id";
                    SQLiteCommand komut = new SQLiteCommand(sql, baglan);
                    komut.Parameters.AddWithValue("@id", userId);

                    ipAddress = komut.ExecuteScalar()?.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("IP adresi alma hatası: " + ex.Message);
                }
            }

            return ipAddress;
        }
    
        private void NotForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }
    }
}
