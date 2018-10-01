using MoonByte.TCP.IO.Spaceshuttle;
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace MoonByte.Net.Plugins
{
    public class Client
    {

        #region Vars

        TcpClient client;

        #endregion

        #region Events

        public EventHandler<EventArgs> SentFileFinished;
        public EventHandler<EventArgs> ReceivedFileFinished;

        #endregion

        #region Startup

        public Client(string IP, int Port)
        {
            client = new TcpClient();
            client.Connect(IP, Port);
        }

        #endregion

        #region SendFile

        public void SendFile(string FileDirectory)
        {
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

        #region ReceiveFile

        public void ReceiveFile(string FileDirectory)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {

            })); thread.Start();
        }

        #endregion

    }
}
