
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
        TcpListener ServerSocket;
        bool HasReceived = false;
        string Received;

        public TcpServer(int Port)
        {
            ServerSocket = new TcpListener(new IPEndPoint(IPAddress.Any, Port));
            ServerSocket.Start();

            ServerSocket.BeginAcceptTcpClient(OnClientAccept, ServerSocket);
        }

        public string WaitForResult()
        {
            while(HasReceived == false)
            {

            }

            return Received;
        }

        public class ClientContext
        {
            public TcpClient Client;
            public Stream Stream;
            public bool ReceivedPublicKey = false;
            public bool ReceivedPrivateKey = false;
            public bool SentPublicKey = false;
            public bool SentPrivateKey = false;
            public byte[] buffer = new byte[65525];
            public StringBuilder sb = new StringBuilder();
        }

        private void OnClientAccept(IAsyncResult ar)
        {

            ClientContext context;

            if (ServerSocket == null) return;
            try
            {
                context = new ClientContext();
                context.Client = ServerSocket.EndAcceptTcpClient(ar);
                context.Stream = context.Client.GetStream();
                InitializeClientConnection(context);
            }
            finally
            {
                ServerSocket.BeginAcceptTcpClient(OnClientAccept, ServerSocket);
            }
        }

        private void InitializeClientConnection(ClientContext context)
        {
            try
            {
                context.Client.Client.BeginReceive(context.buffer, 0, context.buffer.Length, 0, new AsyncCallback(readCallback), context);
            }
            catch { }
        }

        private void readCallback(IAsyncResult ar)
        {
            ClientContext context = (ClientContext)ar.AsyncState;
            string req = ReadClient(context, ar);
            if (req != null)
            {
                try
                {
                    Received = req;
                    HasReceived = true;
                }
                catch { }
            }
        }

        private string ReadClient(ClientContext context, IAsyncResult ar)
        {
            string Content = String.Empty;

            context.Stream.Flush();
            context.sb = new StringBuilder();

            try
            {
                if (context == null) return null;
                Socket handler = context.Client.Client;

                int read = handler.EndReceive(ar);

                // Data was read from the client socket.
                if (read > 0)
                {
                    string b = context.sb.Append(Encoding.UTF8.GetString(context.buffer, 0, read)).ToString();
                    string[] Result = b.Split(new string[] { "<EOF>" }, StringSplitOptions.RemoveEmptyEntries);
                    return Result[0];
                }

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
