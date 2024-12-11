using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace NotAt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {




            string connectionString = "Server=notat-db-do-user-18525492-0.h.db.ondigitalocean.com;Port=25060;Database=proje;User ID=doadmin;Password=AVNS_i5KKCR44-CAV6oo7xLn;SslMode=Required;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Bağlantı başarılı!");
                    // Veritabanı işlemlerinizi burada gerçekleştirin
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Bağlantı hatası: {ex.Message}");
                }
            }




            label1.Top = (this.ClientSize.Width - button1.Height) / 7;

            label1.Location = new Point(
        (this.ClientSize.Width - label1.Width) / 2, // Yatay ortalama
        label1.Location.Y // Mevcut dikey konumu korur
        );
            button1.Left = (this.ClientSize.Width - button1.Width) / 2;
            button2.Left = (this.ClientSize.Width - button1.Width) / 2;


            // Maximize (Büyütme) düğmesini devre dışı bırak
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;


            this.BackColor = ColorTranslator.FromHtml("#D5E9EC");

        }






        private void button1_Click_1(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 frm4 = new Form4();
            frm4.Show();
            this.Hide();
        }
    }
}
        
    

