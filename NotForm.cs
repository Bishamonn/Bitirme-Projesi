using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NotAt.Class2;

namespace NotAt
{
    public partial class NotForm : Form
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        public NotForm()
        {
            InitializeComponent();
        }

        private void NotForm_Load(object sender, EventArgs e)
        {
            KullaniciListesiGetir();

            // Sayfa yüklendiğinde sunucuya bağlan
            SunucuyaOtomatikBaglan();
        }

        private void KullaniciListesiGetir()
        {
            try
            {
                var kullaniciListesi = dbHelper.GetKullaniciListesi(GlobalVariables.KullaniciAd);
                comboBox1.Items.Clear();

                foreach (var item in kullaniciListesi)
                {
                    comboBox1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kullanıcı listesini getirme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;

                if (comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen bir alıcı seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    string selectedUser = comboBox1.SelectedItem.ToString();
                    string mesaj = richTextBox1.Text.Trim();

                    if (string.IsNullOrWhiteSpace(mesaj))
                    {
                        MessageBox.Show("Boş mesaj gönderemezsiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string connectionString =
                        "server=notat-db-do-user-18525492-0.h.db.ondigitalocean.com;" +
                        "port=25060;" +
                        "database=proje;" +
                        "user=doadmin;" +
                        "password=AVNS_i5KKCR44-CAV6oo7xLn;" +
                        "sslmode=Required;";

                    using (MySqlConnection baglan = new MySqlConnection(connectionString))
                    {
                        baglan.Open();
                        string sql = "INSERT INTO mesaj (kisi_id, mesaj, alici_kullanici_ad) VALUES (@kisi_id, @mesaj, @alici_kullanici_ad)";
                        MySqlCommand komut = new MySqlCommand(sql, baglan);

                        komut.Parameters.AddWithValue("@kisi_id", GlobalVariables.kisi_id);
                        komut.Parameters.AddWithValue("@mesaj", mesaj);
                        komut.Parameters.AddWithValue("@alici_kullanici_ad", selectedUser);

                        komut.ExecuteNonQuery();
                        MessageBox.Show("Mesaj başarıyla gönderildi!");
                        richTextBox1.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Mesaj gönderme hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SunucuyaOtomatikBaglan()
        {
            {


                try
                {
                    string serverIP = "192.168.1.186"; // Sunucu IP adresi
                    int serverPort = 5000;

                    // TCP istemcisi oluştur ve sunucuya bağlan
                    using (TcpClient client = new TcpClient())
                    {
                        client.Connect(serverIP, serverPort);
                        NetworkStream stream = client.GetStream();

                        // Kullanıcı adı gönder
                        string kullaniciAd = $"kullanici_ad:{GlobalVariables.KullaniciAd}";
                        byte[] data = Encoding.UTF8.GetBytes(kullaniciAd);
                        stream.Write(data, 0, data.Length);

                        // Sunucudan gelen veriyi oku
                        byte[] buffer = new byte[1024];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        string activeUsers = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        // Aktif kullanıcıları işleyip ComboBox'a ekle
                        Invoke(new Action(() =>
                        {
                            comboBox1.Items.Clear();
                            string[] users = activeUsers.Split(',');
                            foreach (var user in users)
                            {
                                if (!string.IsNullOrWhiteSpace(user))
                                {
                                    comboBox1.Items.Add(user);
                                }
                            }
                        }));

                        MessageBox.Show("Sunucuya başarıyla bağlanıldı ve aktif kullanıcılar alındı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (SocketException ex)
                {
                    string errorMessage;

                    if (ex.SocketErrorCode == SocketError.ConnectionRefused)
                    {
                        errorMessage = "Bağlantı reddedildi. Sunucu çalışıyor mu?";
                    }
                    else if (ex.SocketErrorCode == SocketError.TimedOut)
                    {
                        errorMessage = "Bağlantı zaman aşımına uğradı. Lütfen ağı kontrol edin.";
                    }
                    else if (ex.SocketErrorCode == SocketError.HostUnreachable)
                    {
                        errorMessage = "Sunucuya erişilemiyor. IP adresini kontrol edin.";
                    }
                    else
                    {
                        errorMessage = $"Bir hata oluştu: {ex.Message}";
                    }

                    MessageBox.Show(errorMessage, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Sunucuya bağlanılamadı: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    


       
        

        private void NotForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Alıcı seçildiğinde yapılacak işlemler
        }
    }
}