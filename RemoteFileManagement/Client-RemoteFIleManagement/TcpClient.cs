using System.IO;
using System.Net.Sockets;

namespace MoonByte.Net.Plugins
{
    public class Client
    {
        TcpClient client;

        public Client(string IP, int Port)
        {
            client = new TcpClient();
            client.Connect(IP, Port);
        }

        public void SendFile(string FilePath)
        {
            client.Client.SendFile(FilePath);
        }

        public void ReceiveFile(string FilePath)
        {
            if (File.Exists(FilePath)) File.Delete(FilePath);
            using (var stream = client.GetStream())
            using (var output = File.Create(FilePath))
            {
                var buffer = new byte[1024];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, bytesRead);
                }
            }
        }
    }
}
