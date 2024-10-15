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

namespace NotAt
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            timer1.Interval = 2750; // 2.75 saniye
            timer1.Enabled = true;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label1.Location = new Point((this.ClientSize.Width - label1.Width) / 2,
                                (this.ClientSize.Height - label1.Height) / 2);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            label1.Visible = false; // Label'ı gizle
            label1.Enabled = false; //label'ı kullanılamaz hale getirir.
            timer1.Enabled = false; // Timer'ı durdur
        }

  
    }
}
