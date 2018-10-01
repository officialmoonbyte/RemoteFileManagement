using MoonByte.TCP.IO.Spaceshuttle;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace RemoteFileManagement
{
    public class TcpServer
    {
        #region Vars

        TcpListener listener;

        #endregion

        #region Startup

        public TcpServer(int Port)
        {
            listener = new TcpListener(IPAddress.Any, Port);
            listener.Start(0);
        }

        #endregion

        #region ReceiveFile

        public void ReceiveFile(string FileDirectory)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                TcpClient client = listener.AcceptTcpClient();

                NetworkStream stream = client.GetStream();

                BinaryFormatter formatter = new BinaryFormatter();
                FileTransfer transfer = (FileTransfer)formatter.Deserialize(stream);

                string realFD = FileDirectory + "\\" + transfer.FileName;
                File.WriteAllBytes(realFD, Convert.FromBase64String(transfer.FileContent));

                if(Encoding.CalculateMD5(realFD) != transfer.CheckSum)
                {
                    Console.WriteLine("[WARNING] Coppied file with checksum not equal!");
                }
            })); thread.Start();
        }

        #endregion

        #region Send File

        public void SendFile(string FileDirectory)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
            })); thread.Start();
        }

        #endregion
    }
}
