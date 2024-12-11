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
    public partial class NotGörüntüle : Form
    {
        public NotGörüntüle()
        {
            InitializeComponent();
        }

        private void OpenMessageDetailsForm(string mesaj)
        {
            // Mesaj detaylarını göstermek için yeni form oluştur
            MessageDetailsForm detailsForm = new MessageDetailsForm();

            // Formun mesaj gösteren metodunu çağır
            detailsForm.SetMessage(mesaj);

            // Formu göster
            detailsForm.Show();
        }

        private void NotGörüntüle_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            flowLayoutPanel1.AutoScroll = true; // Dikey kaydırma çubuğunu etkinleştirir

            // MySQL bağlantısı
            string connectionString = "Server=notat-db-do-user-18525492-0.h.db.ondigitalocean.com;Port=25060;Database=proje;User ID=doadmin;Password=AVNS_i5KKCR44-CAV6oo7xLn;SslMode=Required;";

            using (MySqlConnection baglan = new MySqlConnection(connectionString))
            {
                try
                {
                    baglan.Open();

                    // Kullanıcının unvanını al
                    string unvanSql = "SELECT unvan FROM kullanicilar WHERE id = @kisi_id";
                    using (MySqlCommand unvanKomut = new MySqlCommand(unvanSql, baglan))
                    {
                        unvanKomut.Parameters.AddWithValue("@kisi_id", GlobalVariables.kisi_id);
                        object result = unvanKomut.ExecuteScalar();
                        string unvan = result?.ToString();

                        // SQL sorgusunu belirleyelim
                        string sql;
                        if (unvan == "Admin")
                        {
                            sql = "SELECT mesaj.mesaj, kullanicilar.kullanici_ad FROM mesaj " +
                                  "JOIN kullanicilar ON mesaj.kisi_id = kullanicilar.id";
                        }
                        else
                        {
                            sql = "SELECT mesaj.mesaj, kullanicilar.kullanici_ad FROM mesaj " +
                                  "JOIN kullanicilar ON mesaj.kisi_id = kullanicilar.id " +
                                  "WHERE mesaj.alici_kullanici_ad = @kullanici_ad";
                        }

                        using (MySqlCommand komut = new MySqlCommand(sql, baglan))
                        {
                            if (unvan != "Admin")
                            {
                                komut.Parameters.AddWithValue("@kullanici_ad", GlobalVariables.KullaniciAd);
                            }

                            using (MySqlDataReader reader = komut.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    // Sütunlara güvenli erişim
                                    string mesaj = reader["mesaj"].ToString();
                                    string kullaniciAd = reader["kullanici_ad"].ToString();

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

                                    // Panel, label ve içeriklere çift tıklama olayı ekle
                                    notPanel.DoubleClick += (s, args) =>
                                    {
                                        OpenMessageDetailsForm(mesaj);
                                    };

                                    notLabel.DoubleClick += (s, args) =>
                                    {
                                        OpenMessageDetailsForm(mesaj);
                                    };

                                    kullaniciAdLabel.DoubleClick += (s, args) =>
                                    {
                                        OpenMessageDetailsForm(mesaj);
                                    };

                                    // Label'ları panele ekle
                                    notPanel.Controls.Add(kullaniciAdLabel);
                                    notPanel.Controls.Add(notLabel);

                                    // Paneli FlowLayoutPanel'e ekle
                                    flowLayoutPanel1.Controls.Add(notPanel);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Mesajları yüklerken hata oluştu: " + ex.Message);
                }
                finally
                {
                    // Bağlantıyı manuel olarak kapatma
                    if (baglan.State == ConnectionState.Open)
                    {
                        baglan.Close();
                    }
                }
            }

        }

            private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
            {

            }
        }
    }

