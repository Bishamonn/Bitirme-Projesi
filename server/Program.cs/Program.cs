using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MySql.Data.MySqlClient;

class Server
{
    static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Sunucu dinlemeye başladı...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            NetworkStream stream = client.GetStream();

            // Bağlanan kullanıcının IP adresini al
            string clientIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            Console.WriteLine("Bağlantı alındı: " + clientIP);

            // Veritabanında kullanıcıyı güncelle
            UpdateUserStatus(clientIP, true);

            // Yanıt gönder
            byte[] response = Encoding.UTF8.GetBytes("Bağlantı başarılı!");
            stream.Write(response, 0, response.Length);

            client.Close();
        }
    }

    static void UpdateUserStatus(string ip, bool isActive)
    {
        string connectionString = "server=localhost;database=proje;user=root;password=123456";
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "UPDATE kullanicilar SET aktif = @aktif WHERE ip_adresi = @ip";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@aktif", isActive);
                cmd.Parameters.AddWithValue("@ip", ip);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
        }
    }
}