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
            label1.Visible = true;
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

                MySqlCommand komut = new MySqlCommand(sql, baglan);

              
                komut.Parameters.AddWithValue("@kullaniciad", kullaniciad);
                komut.Parameters.AddWithValue("@Sifre", sifre);

                // Sorgunun sonucunu alıyoruz
                object result = komut.ExecuteScalar();

                //Eğer sonuç null değilse kontrol edelim
                if (result != null && result.ToString() == "Admin")
                {
                    label1.Text ="Hoşgeldiniz. Rütbeniz: "+ result;
                }
                else
                {
                    
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
    }
}
