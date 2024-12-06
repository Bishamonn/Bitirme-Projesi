using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NotAt.Class2;

namespace NotAt
{
    public partial class NotSil : Form
    {
        public NotSil()
        {
            InitializeComponent();
        }

        private void NotSil_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            flowLayoutPanel1.AutoScroll = true; // Dikey kaydırma çubuğunu etkinleştirir

            string connectionString = "Data Source=mydatabase.sqlite;Version=3;";
            using (SQLiteConnection baglan = new SQLiteConnection(connectionString))
            {
                try
                {
                    baglan.Open();

                    // Tüm mesajları ve kullanıcı bilgilerini getiren SQL sorgusu
                    string sql = "SELECT mesaj.id AS MesajId, mesaj.mesaj AS Mesaj, kullanicilar.kullanici_ad AS KullaniciAd " +
                                 "FROM mesaj " +
                                 "JOIN kullanicilar ON mesaj.kisi_id = kullanicilar.id";

                    SQLiteCommand komut = new SQLiteCommand(sql, baglan);
                    SQLiteDataReader reader = komut.ExecuteReader();

                    while (reader.Read())
                    {
                        // Sütunlara adlarla erişim
                        int mesajId = Convert.ToInt32(reader["MesajId"]);
                        string mesaj = reader["Mesaj"].ToString();
                        string kullaniciAd = reader["KullaniciAd"].ToString();

                        // Panel oluştur
                        Panel notPanel = new Panel
                        {
                            Size = new Size(200, 100),
                            BorderStyle = BorderStyle.FixedSingle,
                            BackColor = ColorTranslator.FromHtml("#F7ED84"),
                            Tag = mesajId // Mesaj ID'sini panelin Tag özelliğine sakla
                        };

                        // Label'ları oluştur
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

                        // Panel'e click olayı ekle
                        notPanel.Click += (s, ev) => SilmeIslemi(notPanel);

                        // Alt kontrol click olaylarını da panele yönlendir
                        notLabel.Click += (s, ev) => SilmeIslemi(notPanel);
                        kullaniciAdLabel.Click += (s, ev) => SilmeIslemi(notPanel);

                        // Panel'e label'ları ekle
                        notPanel.Controls.Add(kullaniciAdLabel);
                        notPanel.Controls.Add(notLabel);

                        // FlowLayoutPanel'e paneli ekle
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
        }

        /// <summary>
        /// Silme işlemini gerçekleştirir.
        /// </summary>

        private void SilmeIslemi(Panel panel)
        {
            int mesajId = (int)panel.Tag; // Panelin Tag özelliğinden mesaj ID'sini al

            // Kullanıcıya onay sor
            DialogResult result = MessageBox.Show(
                "Bu mesajı silmek istiyor musunuz?",
                "Mesaj Sil",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string connectionString = "Data Source=mydatabase.sqlite;Version=3;";
                using (SQLiteConnection baglan = new SQLiteConnection(connectionString))
                {
                    try
                    {
                        baglan.Open();
                        string sql = "DELETE FROM mesaj WHERE id = @id";
                        SQLiteCommand komut = new SQLiteCommand(sql, baglan);
                        komut.Parameters.AddWithValue("@id", mesajId);
                        komut.ExecuteNonQuery();

                        // Paneli FlowLayoutPanel'den kaldır
                        flowLayoutPanel1.Controls.Remove(panel);

                        MessageBox.Show("Mesaj başarıyla silindi!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Mesaj silinirken hata oluştu: " + ex.Message);
                    }
                }
            }
        }
    }
}




