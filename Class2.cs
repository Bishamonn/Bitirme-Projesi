using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotAt
{
    internal class Class2
    {
        public static class GlobalVariables
        {
            public static string KullaniciAd;
            public static string Sifre;
            public static string kisi_id;

            public static string Mesaj { get; set; }
            public static string MesajKullanici { get; set; }


            public class ComboBoxItem
            {
                public string Text { get; set; }
                public string Value { get; set; }

                public override string ToString()
                {
                    return Text; // ComboBox'ta sadece kullanıcı adı görünür
                }
            }
        }
    }
}
