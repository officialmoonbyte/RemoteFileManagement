using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using IndieGoat.UniversalServer.Interfaces;

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

            WorkLoad = workload;
            Context = context;

            if (Command == "FILETOSERVER") { FileToServer(Args[2]); }
            if (Command == "FILETOCLIENT") { FileToClient(Args[2]); }
            if (Command == "GETDIRECTORIES") { GetDirectories(Args[2]); }
            if (Command == "GETFILES") { GetFiles(Args[2]); }
            if (Command == "DELETEFILE") { }
            if (Command == "COPYFILE") { }
        }

        #endregion

        #region Commands

        #region FileToServer

        //RemoteFileManagement FILETOSERVER [FileDirectory]
        private void FileToServer(string FileDirectory)
        {
            WorkLoad.SendMessage(Context, Port.ToString());

            string FileByte = server.WaitForResult();
            byte[] fileBytes = Encoding.ASCII.GetBytes(FileByte);

            if (File.Exists(FileDirectory)) File.Delete(FileDirectory);
            File.WriteAllBytes(FileDirectory, fileBytes);
        }

        #endregion

        #region FileToClient

        //RemoteFileManagement FILETOCLIENT [FileDirectory]
        private void FileToClient(string FileDirectory)
        {
            byte[] FileByte = null;
            if (File.Exists(FileDirectory)) { FileByte = File.ReadAllBytes(FileDirectory); }
            WorkLoad.SendMessage(Context, Encoding.ASCII.GetString(FileByte));
        }

        #endregion

        #region GetDirectories

        private void GetDirectories(string Directory)
        {
            string returnString = null;

            DirectoryInfo directoryInfo = new DirectoryInfo(Directory);

            for(int i = 0; i < directoryInfo.GetDirectories().Length; i++)
            {
                if (returnString == null) { returnString = directoryInfo.GetDirectories()[i].Name; }
                else { returnString += "%20" + directoryInfo.GetDirectories()[i].Name; }
            }

            WorkLoad.SendMessage(Context, returnString);
        }

        #endregion

        #region GetFiles

        private void GetFiles(string Directory)
        {
            string returnString = null;

            DirectoryInfo directoryInfo = new DirectoryInfo(Directory);

            for (int i = 0; i < directoryInfo.GetFiles().Length; i++)
            {
                if (returnString == null) { returnString = directoryInfo.GetFiles()[i].Name; }
                else { returnString += "%20" + directoryInfo.GetFiles()[i].Name; }
            }
        }

        #endregion

        #endregion

    }
}
