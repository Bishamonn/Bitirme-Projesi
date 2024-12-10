using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NotAt.Class2.GlobalVariables;
using System.Windows.Forms;

namespace NotAt
{
    internal class DatabaseHelper
    {
        private readonly string connectionString = "server=notat-db-do-user-18525492-0.h.db.ondigitalocean.com;" +
                                                   "port=25060;" +
                                                   "database=proje;" +
                                                   "user=doadmin;" +
                                                   "password=AVNS_i5KKCR44-CAV6oo7xLn;" +
                                                   "sslmode=Required;";

        public List<ComboBoxItem> GetKullaniciListesi(string excludeUser)
        {
            List<ComboBoxItem> kullanicilar = new List<ComboBoxItem>();

            using (MySqlConnection baglan = new MySqlConnection(connectionString))
            {
                try
                {
                    baglan.Open();
                    string sql = "SELECT id, kullanici_ad FROM kullanicilar WHERE kullanici_ad != @girisYapanKullaniciAd";
                    using (MySqlCommand komut = new MySqlCommand(sql, baglan))
                    {
                        komut.Parameters.AddWithValue("@girisYapanKullaniciAd", excludeUser);
                        using (MySqlDataReader reader = komut.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                kullanicilar.Add(new ComboBoxItem
                                {
                                    Text = reader["kullanici_ad"].ToString(),
                                    Value = reader["id"].ToString()
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }

            return kullanicilar;
        }

        public string GetUserIpAddress(string userId)
        {
            string ipAddress = null;
            using (MySqlConnection baglan = new MySqlConnection(connectionString))
            {
                try
                {
                    baglan.Open();
                    string sql = "SELECT ip_adresi FROM kullanicilar WHERE id = @id";
                    MySqlCommand komut = new MySqlCommand(sql, baglan);
                    komut.Parameters.AddWithValue("@id", userId);
                    ipAddress = komut.ExecuteScalar()?.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("IP adresi alma hatası: " + ex.Message);
                }
            }

            return ipAddress;
        }
    }
}
