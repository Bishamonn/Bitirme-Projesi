using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotAt
{
    internal class NetworkHelper
    {
        public static string GetLocalIPAddress()
        {
            string localIP = "127.0.0.1"; // Varsayılan olarak localhost
            try
            {
                foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    foreach (var address in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (address.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            localIP = address.Address.ToString();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"IP adresi alınırken bir hata oluştu: {ex.Message}");
            }

            return localIP;
        }

        public static List<string> ConnectToServer(string serverIP, int port)
        {
            List<string> activeUsers = new List<string>();

            try
            {
                TcpClient client = new TcpClient(serverIP, port);
                NetworkStream stream = client.GetStream();

                byte[] data = new byte[256];
                int bytes = stream.Read(data, 0, data.Length);

                string users = Encoding.UTF8.GetString(data, 0, bytes);
                activeUsers = users.Split(',').Select(u => u.Trim()).ToList();

                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bağlantı hatası: {ex.Message}");
            }

            return activeUsers;
        }
    }
}
