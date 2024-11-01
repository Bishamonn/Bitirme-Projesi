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

        private void NotGörüntüle_Load(object sender, EventArgs e)
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
                baglan.Open();

                // Tüm mesajları ve kullanıcı adlarını çekmek için SQL sorgusu
                string sql = "SELECT mesaj, kullanici_ad FROM mesaj " +
                             "JOIN kullanicilar ON mesaj.kisi_id = kullanicilar.id";

                MySqlCommand komut = new MySqlCommand(sql, baglan);

                MySqlDataReader reader = komut.ExecuteReader();

                while (reader.Read())
                {
                    string mesaj = reader.GetString("mesaj");
                    string kullaniciAd = reader.GetString("kullanici_ad");

                    // Her mesaj için yeni bir panel oluştur
                    Panel notPanel = new Panel
                    {
                        Size = new Size(200, 100), // Panel boyutu
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = ColorTranslator.FromHtml("#F7ED84")
                    };

                    Label notLabel = new Label
                    {
                        Text = mesaj,
                        AutoSize = false,
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Arial", 10, FontStyle.Regular) // Metin özellikleri
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

                    notPanel.Controls.Add(kullaniciAdLabel);
                    notPanel.Controls.Add(notLabel);

                    // FlowLayoutPanel'e ekleyin
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
}