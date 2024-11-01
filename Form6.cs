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
            this.Size = new Size(100, 150);    // Formun boyutunu ayarla

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.BackColor = ColorTranslator.FromHtml("#D5E9EC");

            button1.Left = (this.ClientSize.Width - button1.Width) / 2;
            button1.Top = (this.ClientSize.Height - button1.Height) / 2;

            this.Width = 100;

            string kullaniciad = GlobalVariables.KullaniciAd;
            string sifre = GlobalVariables.Sifre;
            string kisi_id = GlobalVariables.kisi_id;

            MySqlConnection baglan = new MySqlConnection(
                "server=localhost;" +
                "database=proje;" +
                "user=root;" +
                "password=123456"
            );
            try
            {
                baglan.Open();
                string sql = "SELECT id FROM kullanicilar WHERE " +
                "kullanici_ad = @kullaniciad AND sifre = @Sifre";

                MySqlCommand komut = new MySqlCommand(sql, baglan);


                komut.Parameters.AddWithValue("@kullaniciad", kullaniciad);
                komut.Parameters.AddWithValue("@Sifre", sifre);
                komut.Parameters.AddWithValue("@kisi_id", kisi_id);

                komut.ExecuteNonQuery();

                GlobalVariables.kisi_id = komut.ExecuteScalar().ToString();



            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NotForm notForm = new NotForm();
            notForm.Show();
        }
    }
}
