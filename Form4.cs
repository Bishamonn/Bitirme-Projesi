using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace NotAt
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        Form4 frm4;



        private void Form4_Load(object sender, EventArgs e)
        {

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.BackColor = ColorTranslator.FromHtml("#D5E9EC");

            button1.Left = (this.ClientSize.Width - button1.Width) / 2;

            textBox1.Left = (this.ClientSize.Width - button1.Width) / 2;
            textBox2.Left = (this.ClientSize.Width - button1.Width) / 2;
            textBox3.Left = (this.ClientSize.Width - button1.Width) / 2;
            textBox4.Left = (this.ClientSize.Width - button1.Width) / 2;

            label1.Left = (this.ClientSize.Width - button1.Width) / 2;
            label2.Left = (this.ClientSize.Width - button1.Width) / 2;
            label3.Left = (this.ClientSize.Width - button1.Width) / 2;
            label4.Left = (this.ClientSize.Width - button1.Width) / 2;



            MySqlConnection baglan = new MySqlConnection(
                "server=localhost;" +
                "database=proje;" +
                "user=root;" +
                "password=123456");
            baglan.Open();
            baglan.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            

            try
            {

                string ad = textBox1.Text;
                string soyad = textBox2.Text;
                string kullanici_ad = textBox3.Text;
                string sifre = textBox4.Text;
                MySqlConnection baglan = new MySqlConnection(
                    "server=localhost;" +
                    "database=proje;" +
                    "user=root;" +
                    "password=123456");
                baglan.Open();
                string sql = "INSERT INTO kullanicilar (ad, soyad, kullanici_ad, sifre) VALUES (@ad, @soyad, @kullanici_ad, @sifre)";

                // SQL komutunu oluşturuyoruz
                MySqlCommand komut = new MySqlCommand(sql, baglan);

                // Parametreleri ekliyoruz
                komut.Parameters.AddWithValue("@ad", ad);
                komut.Parameters.AddWithValue("@soyad", soyad);
                komut.Parameters.AddWithValue("@kullanici_ad", kullanici_ad);
                komut.Parameters.AddWithValue("@sifre", sifre);

                // Komutu çalıştırıyoruz
                komut.ExecuteNonQuery();

                MessageBox.Show("Kayıt başarılı!");

                // TextBox'ları temizle
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";

                this.Close();
                Form1 frm1 = new Form1();
                frm1.Show();

                // Bağlantıyı kapatıyoruz
                baglan.Close();
            }
            catch (MySqlException ex)
            {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Hata: Lütfen tüm alanları doldurunuz.");
                }


                else if (ex.Number == 1062)
                {
                    MessageBox.Show("Hata: Kayıt işlemi başarısız. Bu kullanıcı adı zaten alınmış.");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                }
                else
                {
                    // Diğer hatalar için genel hata mesajı
                    MessageBox.Show("Hata: Kayıt işlemi başarısız!" + ex.Message);
                }
            }
        }
    }
}
