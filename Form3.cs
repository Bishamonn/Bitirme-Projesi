using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using static NotAt.Class2;
using System.Net.Sockets;
using System.Net;

namespace NotAt
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
          
        }


        private void Form3_Load(object sender, EventArgs e)
        {
            FormNotification notification = new FormNotification();
            notification.Show(); // Bildirim formunu göster

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.BackColor = ColorTranslator.FromHtml("#D5E9EC");
            string kullaniciad = GlobalVariables.KullaniciAd;
            string sifre = GlobalVariables.Sifre;


            MySqlConnection baglan = new MySqlConnection(
                "server=localhost;" +
                "database=proje;" +
                "user=root;" +
                "password=123456"
            );
            try
            {
                baglan.Open();
                string sql = "SELECT unvan FROM kullanicilar WHERE " +
                    "kullanici_ad = @kullaniciad AND sifre = @Sifre";
                string sql2 = "SELECT id FROM kullanicilar WHERE " +
                    "kullanici_ad = @kullaniciad AND sifre = @Sifre";

                MySqlCommand komut = new MySqlCommand(sql, baglan);
                MySqlCommand komut2 = new MySqlCommand(sql2, baglan);


                komut.Parameters.AddWithValue("@kullaniciad", kullaniciad);
                komut.Parameters.AddWithValue("@Sifre", sifre);

                komut2.Parameters.AddWithValue("@kullaniciad", kullaniciad);
                komut2.Parameters.AddWithValue("@Sifre", sifre);


                // Sorgunun sonucunu alıyoruz
                object result = komut.ExecuteScalar();
                object result2 = komut2.ExecuteScalar();

                //Eğer sonuç null değilse kontrol edelim
                if (result != null && result.ToString() == "Admin")
                {
                   
                    GlobalVariables.kisi_id = result2.ToString();
                }
                else
                {
                    GlobalVariables.kisi_id = result2.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                // Bağlantıyı kapatıyoruz
                baglan.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form5 frm5 = new Form5();
            frm5.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NotForm notForm = new NotForm();
            notForm.Show();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NotGörüntüle goruntu = new NotGörüntüle();
            goruntu.Show();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            NotSil sil = new NotSil();
            sil.Show();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NotUpdate update = new NotUpdate();
            update.Show();
            
        }
    }
}
