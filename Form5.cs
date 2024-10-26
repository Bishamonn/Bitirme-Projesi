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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

            string kullaniciad = GlobalVariables.KullaniciAd;
            string sifre = GlobalVariables.Sifre;

            MySqlConnection baglan = new MySqlConnection(
                "server=localhost;" +
                "database=proje;" +
                "user=root;" +
                "password=123456"
            );
        private void Form5_Load(object sender, EventArgs e)
        {

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.BackColor = ColorTranslator.FromHtml("#D5E9EC");

            try
            {
                baglan.Open();
                string sql = "SELECT kullanici_ad FROM kullanicilar WHERE " +
                    "unvan IS NULL ";

                MySqlCommand komut = new MySqlCommand(sql, baglan);



                MySqlDataReader reader = komut.ExecuteReader();

              

                while (reader.Read())
                {
                    string kullaniciAd = reader["kullanici_ad"].ToString();
                    comboBox1.Items.Add(kullaniciAd);  
                }

                reader.Close(); // Reader'ı kapatalım
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglan.Close(); 
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {   string k_ad = comboBox1.SelectedItem.ToString();
                string secilenDeger = comboBox2.SelectedItem.ToString();
                baglan.Open();
                string sql = "UPDATE kullanicilar SET unvan = @unvan WHERE kullanici_ad = @kullaniciad";

                MySqlCommand komut = new MySqlCommand(sql, baglan);

                komut.Parameters.AddWithValue("@unvan", secilenDeger);
                komut.Parameters.AddWithValue("@kullaniciad", k_ad);

                int sonuc = komut.ExecuteNonQuery(); // Güncellenen satır sayısını döner

                if (sonuc > 0)
                {
                    MessageBox.Show("Unvan başarıyla güncellendi.");
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Güncelleme başarısız. Kayıt bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }

        }
    }
}
