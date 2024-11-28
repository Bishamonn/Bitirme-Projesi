using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NotAt.Class2;

namespace NotAt
{
    public partial class NotUpdate : Form
    {
        private int selectedMessageId = -1; // Seçilen mesajın ID'sini tutar
        public NotUpdate()
        {
            InitializeComponent();
        }

        private void NotUpdate_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            flowLayoutPanel1.AutoScroll = true; // Dikey kaydırma çubuğunu etkinleştirir

            MySqlConnection baglan = new MySqlConnection(
                "server=localhost;" +
                "database=proje;" +
                "user=root;" +
                "password=123456");

            try
            {
                string sql;
                baglan.Open();

                string unvanSql = "SELECT unvan FROM kullanicilar WHERE id = @kisi_id";
                MySqlCommand unvanKomut = new MySqlCommand(unvanSql, baglan);
                unvanKomut.Parameters.AddWithValue("@kisi_id", GlobalVariables.kisi_id);
                string unvan = (string)unvanKomut.ExecuteScalar();

                // Admin için tüm mesajlar, kullanıcı için sadece kendisine ait olanlar
                if (unvan == "Admin")
                {
                    sql = "SELECT mesaj.id AS mesajId, mesaj.mesaj, kullanici_ad FROM mesaj " +
                          "JOIN kullanicilar ON mesaj.kisi_id = kullanicilar.id";
                }
                else
                {
                    sql = "SELECT mesaj.id AS mesajId, mesaj.mesaj, kullanici_ad FROM mesaj " +
                          "JOIN kullanicilar ON mesaj.kisi_id = kullanicilar.id " +
                          "WHERE kullanicilar.id = @kisi_id";
                }
                MySqlCommand komut = new MySqlCommand(sql, baglan);
                komut.Parameters.AddWithValue("@kisi_id", GlobalVariables.kisi_id);
                MySqlDataReader reader = komut.ExecuteReader();

                while (reader.Read())
                {
                    int mesajId = reader.GetInt32("mesajId"); // Mesaj ID'sini alıyoruz
                    string mesaj = reader.GetString("mesaj");
                    string kullaniciAd = reader.GetString("kullanici_ad");

                    // Panel oluştur
                    Panel notPanel = new Panel
                    {
                        Size = new Size(200, 100),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = ColorTranslator.FromHtml("#F7ED84")
                    };

                    Label notLabel = new Label
                    {
                        Text = mesaj,
                        AutoSize = false,
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Arial", 10, FontStyle.Regular)
                    };

                    Label kullaniciAdLabel = new Label
                    {
                        Text = $"Gönderen: {kullaniciAd}",
                        AutoSize = false,
                        ForeColor = Color.Green,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Bottom,
                        Height = 20
                    };

                    // Panelin çift tıklama olayını tanımlıyoruz
                    notPanel.DoubleClick += (s, args) =>
                    {
                        LoadSelectedMessage(mesajId, mesaj);
                    };
                    kullaniciAdLabel.DoubleClick += (s, args) =>
                    {
                        LoadSelectedMessage(mesajId, mesaj);
                    };
                    notLabel.DoubleClick += (s, args) =>
                    {
                        LoadSelectedMessage(mesajId, mesaj);
                    };

                    notPanel.Controls.Add(kullaniciAdLabel);
                    notPanel.Controls.Add(notLabel);
                    flowLayoutPanel1.Controls.Add(notPanel);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mesajları yüklerken hata oluştu: " + ex.Message);
            }
            finally
            {
                baglan.Close();
            }
        }

        private void LoadSelectedMessage(int mesajId, string mesaj)
        {
            selectedMessageId = mesajId; // Seçilen mesajın ID'sini kaydediyoruz
            richTextBox1.Text = mesaj; // Mesaj içeriğini RichTextBox'a yüklüyoruz
            richTextBox1.ReadOnly = false; // Mesaj düzenlenebilir
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && selectedMessageId != -1) // Enter tuşu ve geçerli mesaj ID
            {
                e.SuppressKeyPress = true; // Enter tuşunun varsayılan davranışını tamamen engelle
                UpdateMessage();
            }
        }
        private void UpdateMessage()
        {
            string updatedMessage = richTextBox1.Text;

            using (MySqlConnection baglan = new MySqlConnection(
                "server=localhost;" +
                "database=proje;" +
                "user=root;" +
                "password=123456"))
            {
                try
                {
                    baglan.Open();
                    string sql = "UPDATE mesaj SET mesaj = @mesaj WHERE id = @id";
                    MySqlCommand komut = new MySqlCommand(sql, baglan);
                    komut.Parameters.AddWithValue("@mesaj", updatedMessage);
                    komut.Parameters.AddWithValue("@id", selectedMessageId);
                    komut.ExecuteNonQuery();

                    MessageBox.Show("Mesaj başarıyla güncellendi!");
                    this.Close(); // Formu kapatıyoruz
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Güncelleme hatası: " + ex.Message);
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
