﻿using MySql.Data.MySqlClient;
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
    public partial class NotGörüntüle : Form
    {
        public NotGörüntüle()
        {
            InitializeComponent();
        }

        private void OpenMessageDetailsForm(string mesaj)
        {
            // Mesaj detaylarını göstermek için yeni form oluştur
            MessageDetailsForm detailsForm = new MessageDetailsForm();

            // Formun mesaj gösteren metodunu çağır
            detailsForm.SetMessage(mesaj);

            // Formu göster
            detailsForm.Show();
        }

        private void NotGörüntüle_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            flowLayoutPanel1.AutoScroll = true; // Dikey kaydırma çubuğunu etkinleştirir


            MySqlConnection baglan = new MySqlConnection(
                "server=localhost;" +
                "database=proje;" +
                "user=root;" +
                "password=123456");

            try
            {
                string sql;
                baglan.Open();

                string unvanSql = "SELECT unvan FROM kullanicilar WHERE id = @kisi_id";
                MySqlCommand unvanKomut = new MySqlCommand(unvanSql, baglan);
                unvanKomut.Parameters.AddWithValue("@kisi_id", GlobalVariables.kisi_id);
                string unvan = (string)unvanKomut.ExecuteScalar();

                
                if (unvan == "Admin")
                {
                     sql = "SELECT mesaj, kullanici_ad FROM mesaj " +
                                "JOIN kullanicilar ON mesaj.kisi_id = kullanicilar.id ";
                }
                else
                {
                     sql = "SELECT mesaj, kullanici_ad FROM mesaj " +
                                 "JOIN kullanicilar ON mesaj.kisi_id = kullanicilar.id " +
                                 "WHERE kullanicilar.id = @kisi_id";
                }
                MySqlCommand komut = new MySqlCommand(sql, baglan);
                komut.Parameters.AddWithValue("@kisi_id", GlobalVariables.kisi_id);
                MySqlDataReader reader = komut.ExecuteReader();

                while (reader.Read())
                {
                    string mesaj = reader.GetString("mesaj");
                    string kullaniciAd = reader.GetString("kullanici_ad");

                    // Panel oluştur
                    Panel notPanel = new Panel
                    {
                        Size = new Size(200, 100),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = ColorTranslator.FromHtml("#F7ED84")
                    };

                    Label notLabel = new Label
                    {
                        Text = mesaj,
                        AutoSize = false,
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Arial", 10, FontStyle.Regular)
                    };

                    Label kullaniciAdLabel = new Label
                    {
                        Text = $"Gönderen: {kullaniciAd}",
                        AutoSize = false,
                        ForeColor = Color.Green,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Bottom,
                        Height = 20
                    };

                    notPanel.DoubleClick += (s, args) =>
                    {
                        OpenMessageDetailsForm(mesaj);
                    };

                    notLabel.DoubleClick += (s, args) =>
                    {
                        OpenMessageDetailsForm(mesaj);
                    };

                    kullaniciAdLabel.DoubleClick += (s, args) =>
                    {
                        OpenMessageDetailsForm(mesaj);
                    };

                    notPanel.Controls.Add(kullaniciAdLabel);
                    notPanel.Controls.Add(notLabel);
                    flowLayoutPanel1.Controls.Add(notPanel);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mesajları yüklerken hata oluştu: " + ex.Message);
            }
            finally
            {
                baglan.Close();
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}