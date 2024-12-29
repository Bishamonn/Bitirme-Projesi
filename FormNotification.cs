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
using System.Drawing.Drawing2D; 
using static NotAt.Class2;

namespace NotAt
{
    public partial class FormNotification : Form
    {

        private Timer showTimer;
        private Timer closeTimer;
        private int xPos;


        public FormNotification()
        {
            InitializeComponent();
            // Formun arkaplan rengini ve diğer ayarları yapabilirsin
            this.BackColor = Color.LightGreen;
            this.FormBorderStyle = FormBorderStyle.None; // Kenarlık kaldır
            this.StartPosition = FormStartPosition.Manual;
            this.TopMost = true; // Her şeyin üstünde görünmesini sağla

            
        }

        private void SetRoundedCorners(int radius)
        {
            var path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(this.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(this.Width - radius, this.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, this.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            this.Region = new Region(path);
        }


        private void FormNotification_Load(object sender, EventArgs e)
        {
            SetRoundedCorners(60);

            this.BackColor = Color.Silver;
            this.Opacity = 0.95; // Formun opaklığını ayarla




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

                MySqlCommand komut = new MySqlCommand(sql, baglan);


                komut.Parameters.AddWithValue("@kullaniciad", kullaniciad);
                komut.Parameters.AddWithValue("@Sifre", sifre);

                // Sorgunun sonucunu alıyoruz
                object result = komut.ExecuteScalar();

                //Eğer sonuç null değilse kontrol edelim
                if (result != null && result.ToString() == "Admin")
                {
                    label1.Text = "Hoşgeldiniz. Rütbeniz: " + result;
                }
                else if(result != null && result.ToString() == "Kullanıcı")
                {
                    label1.Text = "Hoşgeldiniz. Rütbeniz: " + result;
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



            // Formun ekranda yandan kayarak gelmesi için başlama pozisyonu
            xPos = Screen.PrimaryScreen.WorkingArea.Width;
            this.Location = new Point(xPos, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
          


            // Kayan animasyon için timer
            showTimer = new Timer();
            showTimer.Interval = 15; // Hızlı animasyon
            showTimer.Tick += ShowTimer_Tick;
            showTimer.Start();
        }

        private void ShowTimer_Tick(object sender, EventArgs e)
        {
            // Bildirim formunu ekranın sağından sola kaydırarak getiriyoruz
            if (xPos > Screen.PrimaryScreen.WorkingArea.Width - this.Width)
            {
                xPos -= 10; // Hareket miktarı
                this.Location = new Point(xPos, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            }
            else
            {
                showTimer.Stop(); // Kayıp yerine oturduktan sonra durdur
                                  // 2 saniye sonra kapanması için timer başlat
                closeTimer = new Timer();
                closeTimer.Interval = 3000; // 3 saniye bekle
                closeTimer.Tick += CloseTimer_Tick;
                closeTimer.Start();
            }
        }

        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            // Bildirim formunu ekranın dışına kaydırarak kapatıyoruz
            if (xPos < Screen.PrimaryScreen.WorkingArea.Width)
            {
                xPos += 500; // Geri hareket miktarı  
                this.Location = new Point(xPos, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
            }
            else
            {
                closeTimer.Stop();
                this.Dispose(); // Formu kapat ve kaynağı serbest bırak


            }
        }
    }
}
