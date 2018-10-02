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
            TcpClient client = listener.AcceptTcpClient();

            Thread thread = new Thread(new ThreadStart(() =>
            {
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
            TcpClient client = listener.AcceptTcpClient();

            Thread thread = new Thread(new ThreadStart(() =>
            {
                FileTransfer transferRequest = new FileTransfer();

                try
                {
                    transferRequest.FileName = new FileInfo(FileDirectory).Name;
                    transferRequest.FileSize = new FileInfo(FileDirectory).Length.ToString();
                    transferRequest.CheckSum = Encoding.CalculateMD5(FileDirectory);
                    string Seed = "Trequest";
                    transferRequest.Seed = Seed;
                    transferRequest.FileContent = Convert.ToBase64String(File.ReadAllBytes(FileDirectory));

                    NetworkStream stream = client.GetStream();
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, transferRequest);
                }
                catch { Console.WriteLine("Failed to send file!"); }
            })); thread.Start();
        }

        #endregion
    }
}
