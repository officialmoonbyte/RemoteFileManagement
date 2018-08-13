
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace RemoteFileManagement
{
    public class TcpServer
    {
        public EventHandler<EventArgs> ReceivedMessage;

        public TcpServer(int Port)
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, Port);
                listener.Start();

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    new ClientHandler(this, client);
                }
            }
            catch (Exception ex) { Console.WriteLine("Could not start up RemoteFileManagement : " + ex.Message); }
        }
    }

    public class ClientHandler
    {
        Byte[] bytes = new byte[1024];
        string Data = null;

        TcpServer Server;
        TcpClient Client;

        public ClientHandler(TcpServer server, TcpClient client)
        {
            Server = server;
            Client = client;

            Thread thread = new Thread(new ThreadStart(() =>
            {

            })); thread.Start();
        }
    }
}
