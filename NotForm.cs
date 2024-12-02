using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NotAt.Class2;
using static NotAt.Class2.GlobalVariables;

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

            KullaniciListesiGetir();

            string serverIP = "127.0.0.1"; // Sunucu bilgisayarının IP adresini yazın
            int port = 5000;

            TcpClient client = new TcpClient(serverIP, port);
            NetworkStream stream = client.GetStream();
        }


        private void KullaniciListesiGetir()
        {
            string connectionString = "server=localhost;" +
                                      "database=proje;" +
                                      "user=root;" +
                                      "password=123456";

            using (MySqlConnection baglan = new MySqlConnection(connectionString))
            {
                try
                {
                    baglan.Open();
                    string sql = "SELECT id, kullanici_ad FROM kullanicilar";
                    MySqlCommand komut = new MySqlCommand(sql, baglan);

                    MySqlDataReader reader = komut.ExecuteReader();

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

                    using (MySqlConnection baglan = new MySqlConnection(
                        "server=localhost;" +
                        "database=proje;" +
                        "user=root;" +
                        "password=123456"))
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

        private string GetUserIpAddress(string userId)
        {
            string ipAddress = null;
            using (MySqlConnection baglan = new MySqlConnection(
                "server=localhost;" +
                "database=proje;" +
                "user=root;" +
                "password=123456"))
            {
                try
                {
                    baglan.Open();
                    string sql = "SELECT ip_adresi FROM kullanicilar WHERE id = @id";
                    MySqlCommand komut = new MySqlCommand(sql, baglan);
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
