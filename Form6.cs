using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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


            this.MinimumSize = new Size(0, 0); // Kısıtlamayı kaldır
            this.Size = new Size(100, 150);    // Formun boyutunu ayarla

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.BackColor = ColorTranslator.FromHtml("#D5E9EC");

            button1.Left = (this.ClientSize.Width - button1.Width) / 2;
            button1.Top = (this.ClientSize.Height - button1.Height) / 2;

            this.Width = 100;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NotForm notForm = new NotForm();
            notForm.Show();
        }
    }
}
