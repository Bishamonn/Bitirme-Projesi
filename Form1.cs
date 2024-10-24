﻿using System;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            label1.Top = (this.ClientSize.Width - button1.Height) / 7;
            button1.Left = (this.ClientSize.Width - button1.Width) / 2;
            button2.Left = (this.ClientSize.Width - button1.Width) / 2;


            // Maximize (Büyütme) düğmesini devre dışı bırak
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
