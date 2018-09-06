using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using IndieGoat.UniversalServer.Interfaces;
using static RemoteFileManagement.TcpServer;

namespace RemoteFileManagement
{
    public class RemoteFileManagement : IServerPlugin
    {
        TcpServer server;
        public string Name
        {
            get { return "RemoteFileManagement"; }
        }

        public event EventHandler<SendMessageEventArgs> SendMessage;

        #region Vars

        int Port = 9984;
        ClientSocketWorkload WorkLoad;
        ClientContext Context;

        #endregion

        #region OnLoad

        public void onLoad(string ServerDirectory)
        {
            string PluginDirectory = ServerDirectory + @"\RemoteFileManagement";
            string FileDirectory = PluginDirectory + @"\Port.txt";

            if (!Directory.Exists(PluginDirectory)) Directory.CreateDirectory(PluginDirectory);

            if (File.Exists(FileDirectory))
            {
                string sPort = File.ReadAllText(FileDirectory);
                Port = int.Parse(sPort);
            }
            else
            {
                File.Create(FileDirectory).Close();
                File.WriteAllText(FileDirectory, Port.ToString());
            }

            server = new TcpServer(Port);
        }

        #endregion

        #region OnExit

        public void Unload() { }

        #endregion

        #region Invoke

        public void Invoke(ClientSocketWorkload workload, ClientContext context, int port, List<string> Args, string ServerDirectory)
        {
            string Command = Args[1].ToUpper();

            Console.WriteLine(Args[1].ToUpper());

            WorkLoad = workload;
            Context = context;

            if (Command == "FILETOSERVER") { FileToServer(Args[2]); }
            if (Command == "FILETOCLIENT") { FileToClient(Args[2]); }
            if (Command == "GETDIRECTORIES") { GetDirectories(Args[2]); }
            if (Command == "GETFILES") { GetFiles(Args[2]); }
            if (Command == "DELETEFILE") { DeleteFile(Args[2]); }
            if (Command == "COPYFILE") { CopyFile(Args[3], Args[4]); }
        }

        #endregion

        #region Commands

        #region FileToServer

        //RemoteFileManagement FILETOSERVER [FileDirectory]
        private void FileToServer(string FileDirectory)
        {
            FileDirectory = FileDirectory.Replace("&20", " ");

            try
            {
                WorkLoad.SendMessage(Context, Port.ToString());

                ClientSocket clientSocket = server.AcceptClient();
                clientSocket.ReceiveFile(FileDirectory);
            }
            catch (Exception e)
            {
                WorkLoad.SendMessage(Context, "error " + e.Message);
            }
        }

        #endregion

        #region FileToClient

        //RemoteFileManagement FILETOCLIENT [FileDirectory]
        private void FileToClient(string FileDirectory)
        {
            FileDirectory = FileDirectory.Replace("&20", " ");

            try
            {
                WorkLoad.SendMessage(Context, Port.ToString());
                ClientSocket clientSocket = server.AcceptClient();
                clientSocket.SendFile(FileDirectory);
            }
            catch (Exception e)
            {
                WorkLoad.SendMessage(Context, "error " + e.Message);
            }
        }

        #endregion

        #region GetDirectories

        private void GetDirectories(string Directory)
        {
            Directory = Directory.Replace("&20", " ");

            try
            {
                string returnString = "";

                DirectoryInfo directoryInfo = new DirectoryInfo(Directory);

                foreach(DirectoryInfo dir in directoryInfo.GetDirectories())
                {
                    if (returnString.Length == 0) { returnString = dir.Name; }
                    else { returnString += "%20" + dir.Name; }
                }

                WorkLoad.SendMessage(Context, returnString);
            }
            catch (Exception e)
            {
                Console.WriteLine("[EROR] Error in GetDirectories : " + e.Message);
                WorkLoad.SendMessage(Context, "error " + e.Message);
            }
        }

        #endregion

        #region GetFiles

        private void GetFiles(string Directory)
        {
            Directory = Directory.Replace("&20", " ");
            try
            {
                string returnString = "";

                DirectoryInfo directoryInfo = new DirectoryInfo(Directory);

                foreach(FileInfo file in directoryInfo.GetFiles())
                {
                    if (returnString.Length == 0) { returnString = file.Name; }
                    else { returnString += "%20" + file.Name; }
                }

                WorkLoad.SendMessage(Context, returnString);
            }
            catch (Exception e)
            {
                WorkLoad.SendMessage(Context, "error " + e.Message);
            }
        }

        #endregion

        #region DeletFile

        private void DeleteFile(string file)
        {
            file = file.Replace("&20", " ");
            try
            {
                File.Delete(file);
                WorkLoad.SendMessage(Context, "TRUE");
            }
            catch (Exception e)
            {
                WorkLoad.SendMessage(Context, "error " + e.Message);
            }
        }

        #endregion

        #region CopyFile

        private void CopyFile(string FilePath, string CopyFilePath)
        {
            FilePath = FilePath.Replace("&20", " ");
            CopyFilePath = CopyFilePath.Replace("&20", " ");

            try
            {
                File.Copy(FilePath, CopyFilePath);
                WorkLoad.SendMessage(Context, "TRUE");
            }
            catch (Exception e)
            {
                WorkLoad.SendMessage(Context, "error " + e.Message);
            }
        }

        #endregion

        #endregion

    }
}
