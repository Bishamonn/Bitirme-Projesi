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
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using MySqlX.XDevAPI.Common;
using System.Security.Policy;
using System.Diagnostics;

namespace NotAt
{
    public partial class Form3 : Form
    {


        public Form3()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
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
    "server=notat-db-do-user-18525492-0.h.db.ondigitalocean.com;" +
    "port=25060;" +
    "database=proje;" +
    "user=doadmin;" +
    "password=AVNS_i5KKCR44-CAV6oo7xLn;" +
    "SslMode=Required;"
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
        private void ResetAllUserStatus()
        {
            string connectionString = "Server=notat-db-do-user-18525492-0.h.db.ondigitalocean.com;Port=25060;Database=proje;Uid=doadmin;Pwd=AVNS_i5KKCR44-CAV6oo7xLn;SslMode=Required;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // SQL_SAFE_UPDATES modunu kapat
                    string disableSafeUpdates = "SET SQL_SAFE_UPDATES = 0;";
                    MySqlCommand disableCmd = new MySqlCommand(disableSafeUpdates, conn);
                    disableCmd.ExecuteNonQuery();

                    // Tüm kullanıcıların aktiflik ve IP adreslerini sıfırla
                    string resetQuery = "UPDATE kullanicilar SET aktif = 0, ip_adresi = '0.0.0.0'";
                    MySqlCommand resetCmd = new MySqlCommand(resetQuery, conn);
                    int rowsAffected = resetCmd.ExecuteNonQuery();

                    MessageBox.Show($"Tüm kullanıcı durumları sıfırlandı. {rowsAffected} kullanıcı güncellendi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kullanıcı durumlarını sıfırlama sırasında bir hata oluştu: " + ex.Message);
                }
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

        private void button6_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    // Server uygulamasının bulunduğu yol
                    string serverPath = @"C:\Users\fidan\OneDrive\Masaüstü\Bitirme Projesi\server\Program.cs\bin\Debug\net8.0\Program.cs.exe";

                    // Sunucuyu çalıştır
                    Process serverProcess = new Process();
                    serverProcess.StartInfo.FileName = serverPath;
                    serverProcess.StartInfo.CreateNoWindow = true; // Konsol penceresini gösterme
                    serverProcess.StartInfo.UseShellExecute = false; // Yeni bir işlem başlat
                    serverProcess.Start();

                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Sunucu başlatılamadı: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Kullanıcı durumlarını sıfırla
            ResetAllUserStatus();

            // Kapanış mesajı
            MessageBox.Show("Sunucu kapatıldı ve tüm kullanıcı durumları sıfırlandı.");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Kullanıcı durumlarını sıfırla
            ResetAllUserStatus();

            // Kapanış mesajı
            MessageBox.Show("Sunucu kapatıldı ve tüm kullanıcı durumları sıfırlandı.");

         
       
        }
    }
}