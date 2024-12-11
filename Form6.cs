using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NotAt.Class2;

namespace NotAt
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }



        private void Form6_Load(object sender, EventArgs e)
        {
            FormNotification notification = new FormNotification();
            notification.Show();

            this.MinimumSize = new Size(0, 0); // Kısıtlamayı kaldır
            this.Size = new Size(100, 300);    // Formun boyutunu ayarla

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.BackColor = ColorTranslator.FromHtml("#D5E9EC");

            // Butonları yatayda ortala
            button1.Left = (this.ClientSize.Width - button1.Width) / 2;
            button3.Left = (this.ClientSize.Width - button3.Width) / 2;

            this.Width = 100;

            // Kullanıcı bilgilerini al
            string kullaniciad = GlobalVariables.KullaniciAd;
            string sifre = GlobalVariables.Sifre;
            string kisi_id = GlobalVariables.kisi_id;

            // MySQL bağlantı dizesi
            string connectionString = "Server=notat-db-do-user-18525492-0.h.db.ondigitalocean.com;Port=25060;Database=proje;Uid=doadmin;Pwd=AVNS_i5KKCR44-CAV6oo7xLn;SslMode=Required;";

            using (MySqlConnection baglan = new MySqlConnection(connectionString))
            {
                try
                {
                    baglan.Open();

                    // Kullanıcıyı kontrol etmek için SQL sorgusu
                    string sql = "SELECT id FROM kullanicilar WHERE kullanici_ad = @kullaniciad AND sifre = @sifre";

                    using (MySqlCommand komut = new MySqlCommand(sql, baglan))
                    {
                        // Parametreleri ekle
                        komut.Parameters.AddWithValue("@kullaniciad", kullaniciad);
                        komut.Parameters.AddWithValue("@sifre", sifre);

                        object result = komut.ExecuteScalar();

                        if (result != null)
                        {
                            // Kullanıcı ID'sini global değişkene ata
                            GlobalVariables.kisi_id = result.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı adı veya şifre hatalı!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda mesaj göster
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }
            

            private void button1_Click(object sender, EventArgs e)
            {
            NotForm notForm = new NotForm();
            notForm.Show();
            }

        private void button3_Click(object sender, EventArgs e)
        {
            NotGörüntüle goruntu = new NotGörüntüle();
            goruntu.Show();
        }
    }
}
