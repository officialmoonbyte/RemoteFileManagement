using IndieGoat.Net.Tcp;
using System;
using System.IO;
using System.Text;

namespace MoonByte.Net.Plugins
{
    public class RemoteFileManagement
    {

        #region Variables

        UniversalClient client = new UniversalClient();
        string ServerIP = null;

        #endregion

        #region Startup

        public RemoteFileManagement(string ServeriP, int Port)
        { client.ConnectToRemoteServer(ServeriP, Port); ServerIP = ServeriP; }

        #endregion

        #region Commands

        #region DeleteServerFile

        public string DeleteServerFile(string FileDirectory)
        {
            if (!client.Client.Connected) { Console.WriteLine("[RemoteFileManagement] Server is currently not connected!"); return false.ToString(); }

            FileDirectory = FileDirectory.Replace(" ", "&20");
            return client.ClientSender.SendCommand("RemoteFileManagement", new string[] { "DELETEFILE", FileDirectory });
        }

        #endregion

        #region FileToClient

        public void FileToClient(string ServerFileDirectory, string SaveFileDirectory)
        {
            ServerFileDirectory = ServerFileDirectory.Replace(" ", "&20");
            if (!client.Client.Connected) { Console.WriteLine("[RemoteFileManagement] Server is currently not connected!"); return; }

            string fileByte = client.ClientSender.SendCommand("RemoteFileManagement", new string[] { "FILETOCLIENT", ServerFileDirectory });

            byte[] ByteStream = Encoding.ASCII.GetBytes(fileByte);
            if (File.Exists(SaveFileDirectory)) File.Delete(SaveFileDirectory);
            File.WriteAllBytes(SaveFileDirectory, ByteStream);
        }

        #endregion

        #region CopyFile

        public string CopyServerFile(string FileDirectory, string CopyFileDirectory)
        {
            FileDirectory = FileDirectory.Replace(" ", "&20");
            CopyFileDirectory = CopyFileDirectory.Replace(" ", "&20");
            if (!client.Client.Connected) { Console.WriteLine("[RemoteFileManagement] Server is currently not connected!"); return false.ToString(); }

            string Request = client.ClientSender.SendCommand("RemoteFileManagement", new string[] { "COPYFILE", FileDirectory, CopyFileDirectory });

            return Request;
        }

        #endregion

        #region GetServerDirectory

        public string[] GetServerDirectory(string DirectoryPath)
        {
            DirectoryPath = DirectoryPath.Replace(" ", "&20");
            if (!client.Client.Connected) { Console.WriteLine("[RemoteFileManagement] Server is currently not connected!"); return new string[] { "false" }; }

            string n = client.ClientSender.SendCommand("RemoteFileManagement", new string[] { "GETDIRECTORIES", DirectoryPath });
            Console.WriteLine("String s : " + n);
            string[] returns = n.Split(new string[] { "%20" }, StringSplitOptions.RemoveEmptyEntries);
            return returns;
        }

        #endregion GetServerDirectory

        #region GetServerFiles

        public string[] GetServerFiles(string DirectoryPath)
        {
            DirectoryPath = DirectoryPath.Replace(" ", "&20");
            if (!client.Client.Connected) { Console.WriteLine("[RemoteFileManagement] Server is currently not connected!"); return new string[] { "false" }; }

            string s = client.ClientSender.SendCommand("RemoteFileManagement", new string[] { "GETFILES", DirectoryPath });

            string[] returns = s.Split(new string[] { "%20" }, StringSplitOptions.RemoveEmptyEntries);
            return returns;
        }

        #endregion GetServerFiles

        #region FileToServer

        public void FileToServer(string ClientFileDirectory, string ServerFileDirectory)
        {

            ServerFileDirectory = ServerFileDirectory.Replace(" ", "&20");
            if (!client.Client.Connected) { Console.WriteLine("[RemoteFileManagement] Server is currently not connected!"); return; }
            byte[] FileByte = null;
            if (File.Exists(ClientFileDirectory)) { FileByte = File.ReadAllBytes(ClientFileDirectory); }

            string ServerPort = client.ClientSender.SendCommand("RemoteFileManagement", new string[] { "FILETOSERVER", ServerFileDirectory });

            UniversalClient serverClient = new UniversalClient();
            Console.WriteLine(ServerPort);
            serverClient.ConnectToRemoteServer(ServerIP, int.Parse(ServerPort));
            serverClient.ClientSender.SendMessage(Encoding.ASCII.GetString(FileByte));
        }

        #endregion

        #endregion

    }
}
