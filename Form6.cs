using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
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

            button1.Left = (this.ClientSize.Width - button1.Width) / 2;
            button3.Left = (this.ClientSize.Width - button3.Width) / 2;

            this.Width = 100;

            string kullaniciad = GlobalVariables.KullaniciAd;
            string sifre = GlobalVariables.Sifre;
            string kisi_id = GlobalVariables.kisi_id;

            string connectionString = "Data Source=mydatabase.sqlite;Version=3;";
            using (SQLiteConnection baglan = new SQLiteConnection(connectionString))
            {
                try
                {
                    baglan.Open();

                    string sql = "SELECT id FROM kullanicilar WHERE kullanici_ad = @kullaniciad AND sifre = @sifre";

                    using (SQLiteCommand komut = new SQLiteCommand(sql, baglan))
                    {
                        komut.Parameters.AddWithValue("@kullaniciad", kullaniciad);
                        komut.Parameters.AddWithValue("@sifre", sifre);

                        object result = komut.ExecuteScalar();

                        if (result != null)
                        {
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
