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
    public partial class NotForm : Form
    {
        public NotForm()
        {
            InitializeComponent();
        }
        
        private void NotForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }
        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            string kisi_id = GlobalVariables.kisi_id;

            if (e.KeyCode == Keys.Enter)
            {
                string mesaj = richTextBox1.Text;

                using (MySqlConnection baglan = new MySqlConnection(
                    "server=localhost;" +
                    "database=proje;" +
                    "user=root;" +
                    "password=123456"))
                {
                    try
                    {
                        baglan.Open();
                        string sql = "INSERT INTO mesaj (kisi_id, mesaj) VALUES (@kisi_id, @mesaj)";
                        MySqlCommand komut = new MySqlCommand(sql, baglan);

                        // Parametreleri ekle
                        komut.Parameters.AddWithValue("@kisi_id",kisi_id);
                        komut.Parameters.AddWithValue("@mesaj", mesaj);

                        komut.ExecuteNonQuery();

                        MessageBox.Show("Mesaj başarıyla kaydedildi!");

                        richTextBox1.Clear(); // Mesaj kaydedildikten sonra RichTextBox'ı temizle
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex.Message);
                    }
                }

                e.Handled = true; // Enter tuşunun yeni satır açmaması için
            }
        }

     
    }
}
