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
    public partial class MessageDetailsForm: Form
    {
        public MessageDetailsForm()
        {
            InitializeComponent();

            ContextMenuStrip contextMenu = new ContextMenuStrip();

            // Menü öğelerini ekle
            ToolStripMenuItem item1 = new ToolStripMenuItem("Kapat");
            ToolStripMenuItem item2 = new ToolStripMenuItem("Gizle");

            // Menü öğeleri üzerine tıklama olaylarını ekleyin
            item1.Click += (sender, e) => this.Close();
            item2.Click += (sender, e) => this.Hide();

            contextMenu.Items.Add(item1);
            contextMenu.Items.Add(item2);

            // Sağ tıklama ile menüyü açmak için Form'un MouseDown olayını kullanın
            richTextBox1.MouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Right) // Sağ tıklama0
                {
                    contextMenu.Show(this, e.Location); // Menü yeri sağ tıklanan konum olacak
                }
            };
        }

        private bool mouseDown;
        private Point lastLocation;

        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // Sol fare tuşuna basıldığında
            {
                mouseDown = true;
                lastLocation = e.Location;
            }
        }

        private void richTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X,
                    (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // Sol fare tuşu bırakıldığında
            {
                mouseDown = false;
            }
        }

        public void SetMessage(string mesaj)
        {
            string formattedMessage = "\n" + mesaj;
            richTextBox1.Text = formattedMessage;
            // Örneğin, mesajı bir Label veya TextBox kontrolüne yazdırabilirsiniz
            
        }

        private void MessageDetailsForm_Load(object sender, EventArgs e)
        {
           

            richTextBox1.BackColor = ColorTranslator.FromHtml("#F7ED84");


            this.FormBorderStyle = FormBorderStyle.None; 
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen; 

            

            richTextBox1.ReadOnly = true;
            richTextBox1.Width = this.Width;
            richTextBox1.Height = this.Height;

            richTextBox1.Left = (this.ClientSize.Width - richTextBox1.Width) / 2;
            richTextBox1.Top = (this.ClientSize.Height - richTextBox1.Height) / 2;


            richTextBox1.MouseDown += richTextBox1_MouseDown;
            richTextBox1.MouseMove += richTextBox1_MouseMove;
            richTextBox1.MouseUp += richTextBox1_MouseUp;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
