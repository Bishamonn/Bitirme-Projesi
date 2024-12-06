using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
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

            // Ortalamayı dinamik hale getiren bir metot
            void CenterControl(Control control, int offsetX = 0)
            {
                control.Left = (this.ClientSize.Width - control.Width) / 2 + offsetX;
            }

            // Kontrollerin hizalanması
            CenterControl(button1);
            CenterControl(textBox1);
            CenterControl(textBox2);
            CenterControl(textBox3);
            CenterControl(textBox4);
            CenterControl(label1);
            CenterControl(label2);
            CenterControl(label3);
            CenterControl(label4);

            // SQLite bağlantısı
            string connectionString = "Data Source=mydatabase.sqlite;Version=3;";
            using (SQLiteConnection baglan = new SQLiteConnection(connectionString))
            {
                try
                {
                    baglan.Open();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanı bağlantısında hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {


            try
            {
                string ad = textBox1.Text.Trim();
                string soyad = textBox2.Text.Trim();
                string kullanici_ad = textBox3.Text.Trim();
                string sifre = textBox4.Text.Trim();

                // Veritabanı bağlantısı
                string connectionString = "Data Source=mydatabase.sqlite;Version=3;";
                using (SQLiteConnection baglan = new SQLiteConnection(connectionString))
                {
                    baglan.Open();

                    string sql = "INSERT INTO kullanicilar (ad, soyad, kullanici_ad, sifre) VALUES (@ad, @soyad, @kullanici_ad, @sifre)";

                    // SQL komutunu oluştur
                    using (SQLiteCommand komut = new SQLiteCommand(sql, baglan))
                    {
                        // Parametreleri ekliyoruz
                        komut.Parameters.AddWithValue("@ad", ad);
                        komut.Parameters.AddWithValue("@soyad", soyad);
                        komut.Parameters.AddWithValue("@kullanici_ad", kullanici_ad);
                        komut.Parameters.AddWithValue("@sifre", sifre);

                        try
                        {
                            // Komutu çalıştır
                            komut.ExecuteNonQuery();
                            MessageBox.Show("Kayıt başarılı!");

                            // TextBox'ları temizle
                            textBox1.Clear();
                            textBox2.Clear();
                            textBox3.Clear();
                            textBox4.Clear();

                            this.Close();
                            Form1 frm1 = new Form1();
                            frm1.Show();
                        }
                        catch (SQLiteException ex)
                        {
                            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                                string.IsNullOrWhiteSpace(textBox2.Text) ||
                                string.IsNullOrWhiteSpace(textBox3.Text) ||
                                string.IsNullOrWhiteSpace(textBox4.Text))
                            {
                                MessageBox.Show("Hata: Lütfen tüm alanları doldurunuz.");
                            }
                            else if (ex.ResultCode == SQLiteErrorCode.Constraint && ex.Message.Contains("UNIQUE"))
                            {
                                MessageBox.Show("Hata: Bu kullanıcı adı zaten alınmış.");
                                textBox3.Clear();
                            }
                            else
                            {
                                MessageBox.Show("Hata: Kayıt işlemi başarısız! " + ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Beklenmeyen bir hata oluştu: " + ex.Message);
            }
        }
    }
}
