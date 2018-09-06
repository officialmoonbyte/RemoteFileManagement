
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace RemoteFileManagement
{
    public class TcpServer
    {
        TcpListener tcpListener;

        public TcpServer(int Port)
        {
            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
        }

        public ClientSocket AcceptClient()
        {
            TcpClient client = tcpListener.AcceptTcpClient();
            ClientSocket cs = new ClientSocket(client);
            return cs;
        }

        public class ClientSocket
        {
            TcpClient Client;

            public ClientSocket(TcpClient client)
            {
                Client = client;
            }
            public void SendFile(string FilePath)
            {
                Client.Client.SendFile(FilePath);
            }

            public void ReceiveFile(string FilePath)
            {
                if (File.Exists(FilePath)) File.Delete(FilePath);
                using (var stream = Client.GetStream())
                using (var output = File.Create(FilePath))
                {
                    var buffer = new byte[1024];
                    int bytesRead;
                    while((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        output.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }
    }
}
