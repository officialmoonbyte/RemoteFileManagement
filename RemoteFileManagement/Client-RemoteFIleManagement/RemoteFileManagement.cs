using IndieGoat.Net.Tcp;
using System;
using System.IO;
using System.Text;

namespace MoonByte.Net.Plugins
{
    public class RemoteFileManagement
    {

        #region Variables

        UniversalClient Tclient = new UniversalClient();
        string ServerIP = null;

        #endregion

        #region Startup

        public RemoteFileManagement(string ServeriP, int Port)
        { Tclient.ConnectToRemoteServer(ServeriP, Port); ServerIP = ServeriP; }

        #endregion

        #region Commands

        #region DeleteServerFile

        public string DeleteServerFile(string FileDirectory)
        {
            if (!Tclient.Client.Connected) { Console.WriteLine("[RemoteFileManagement] Server is currently not connected!"); return false.ToString(); }

            FileDirectory = FileDirectory.Replace(" ", "&20");
            return Tclient.ClientSender.SendCommand("RemoteFileManagement", new string[] { "DELETEFILE", FileDirectory });
        }

        #endregion

        #region FileToClient

        public void FileToClient(string ServerFileDirectory, string SaveFileDirectory)
        {
            ServerFileDirectory = ServerFileDirectory.Replace(" ", "&20");
            if (!Tclient.Client.Connected) { Console.WriteLine("[RemoteFileManagement] Server is currently not connected!"); return; }

            string serverPort = Tclient.ClientSender.SendCommand("RemoteFileManagement", new string[] { "FILETOCLIENT", ServerFileDirectory });

            Client client = new Client(ServerIP, int.Parse(serverPort));
            client.ReceiveFile(SaveFileDirectory);
        }

        #endregion

        #region CopyFile

        public string CopyServerFile(string FileDirectory, string CopyFileDirectory)
        {
            FileDirectory = FileDirectory.Replace(" ", "&20");
            CopyFileDirectory = CopyFileDirectory.Replace(" ", "&20");
            if (!Tclient.Client.Connected) { Console.WriteLine("[RemoteFileManagement] Server is currently not connected!"); return false.ToString(); }

            string Request = Tclient.ClientSender.SendCommand("RemoteFileManagement", new string[] { "COPYFILE", FileDirectory, CopyFileDirectory });

            return Request;
        }

        #endregion

        #region GetServerDirectory

        public string[] GetServerDirectory(string DirectoryPath)
        {
            DirectoryPath = DirectoryPath.Replace(" ", "&20");
            if (!Tclient.Client.Connected) { Console.WriteLine("[RemoteFileManagement] Server is currently not connected!"); return new string[] { "false" }; }

            string n = Tclient.ClientSender.SendCommand("RemoteFileManagement", new string[] { "GETDIRECTORIES", DirectoryPath });
            Console.WriteLine("String s : " + n);
            string[] returns = n.Split(new string[] { "%20" }, StringSplitOptions.RemoveEmptyEntries);
            return returns;
        }

        #endregion GetServerDirectory

        #region GetServerFiles

        public string[] GetServerFiles(string DirectoryPath)
        {
            DirectoryPath = DirectoryPath.Replace(" ", "&20");
            if (!Tclient.Client.Connected) { Console.WriteLine("[RemoteFileManagement] Server is currently not connected!"); return new string[] { "false" }; }

            string s = Tclient.ClientSender.SendCommand("RemoteFileManagement", new string[] { "GETFILES", DirectoryPath });

            string[] returns = s.Split(new string[] { "%20" }, StringSplitOptions.RemoveEmptyEntries);
            return returns;
        }

        #endregion GetServerFiles

        #region FileToServer

        public void FileToServer(string ClientFileDirectory, string ServerFileDirectory)
        {
            ServerFileDirectory = ServerFileDirectory.Replace(" ", "&20");
            if (!Tclient.Client.Connected) { Console.WriteLine("[RemoteFileManagement] Server is currently not connected!"); return; }
            byte[] FileByte = null;
            if (File.Exists(ClientFileDirectory)) { FileByte = File.ReadAllBytes(ClientFileDirectory); }

            string ServerPort = Tclient.ClientSender.SendCommand("RemoteFileManagement", new string[] { "FILETOSERVER", ServerFileDirectory });

            Client client = new Client(ServerIP, int.Parse(ServerPort));
            client.SendFile(ClientFileDirectory);
        }

        #endregion

        #endregion

    }
}
