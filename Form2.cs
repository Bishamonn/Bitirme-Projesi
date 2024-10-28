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
using static NotAt.Class1;
using static NotAt.Class2;

namespace NotAt
{
    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
        Form3 frm3;
        private void button1_Click(object sender, EventArgs e)
        {

            

            string kullanici_ad = textBox1.Text;
            string sifre = textBox2.Text;


            // Veritabanı bağlantısı
            MySqlConnection baglan = new MySqlConnection(
                "server=localhost;" +
                "database=proje;" +
                "user=root;" +
                "password=123456"
            );
            
            try
            {
                // Bağlantıyı aç
                baglan.Open();

                // SQL sorgusu (Parametreli kullanım)
                string sql = "SELECT COUNT(*) FROM kullanicilar WHERE " +
                    "kullanici_ad = @kullanici_ad AND sifre = @sifre";

                string sql2 = "SELECT unvan FROM kullanicilar WHERE " +
                    "kullanici_ad = @kullaniciad AND sifre = @Sifre";

                // Komut oluştur
                MySqlCommand komut = new MySqlCommand(sql, baglan);
                MySqlCommand komut2 = new MySqlCommand(sql2, baglan);

                komut.Parameters.AddWithValue("@kullanici_ad", kullanici_ad);
                komut.Parameters.AddWithValue("@sifre", sifre);

                komut2.Parameters.AddWithValue("@kullaniciad", kullanici_ad);
                komut2.Parameters.AddWithValue("@Sifre", sifre);

                // Sorguyu çalıştır ve eşleşen kayıt sayısını al
                int count = Convert.ToInt32(komut.ExecuteScalar());

                object result = komut2.ExecuteScalar();



                if (count > 0 && result.ToString()=="Admin")
                {
                    // Giriş başarılı
                    MessageBox.Show("Giriş başarılı!");

                    GlobalVariables.KullaniciAd = textBox1.Text;
                    GlobalVariables.Sifre = textBox2.Text;
                    

                    Form3 frm3 = new Form3();
                    frm3.Show();
                    this.Hide();
                }

                else if (count > 0)
                {
                    MessageBox.Show("Giriş başarılı!");

                    GlobalVariables.KullaniciAd = textBox1.Text;
                    GlobalVariables.Sifre = textBox2.Text;

                    Form6 frm6 = new Form6();
                    frm6.Show();
                    this.Hide();
                }
                else
                {
                    // Giriş başarısız
                    MessageBox.Show("Hatalı kullanıcı adı veya şifre.");
                    textBox1.Text = "";
                    textBox2.Text = "";
                }

                // Bağlantıyı kapat
                baglan.Close();
            }
            catch (Exception ex)
            {
                // Hata oluşursa mesajı göster
                MessageBox.Show("Hata: " + ex.Message);
            }


            
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#D5E9EC");

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }
    }
}
