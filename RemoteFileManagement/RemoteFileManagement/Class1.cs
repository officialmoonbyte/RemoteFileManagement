using System;
using System.Collections.Generic;
using System.IO;
using IndieGoat.UniversalServer.Interfaces;

namespace RemoteFileManagement
{
    public class RemoteFileManagement : IServerPlugin
    {
        public string Name
        {
            get { return "RemoteFileManagement"; }
        }

        public event EventHandler<SendMessageEventArgs> SendMessage;

        #region Vars

        int Port = 9984;

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
        }

        #endregion

        #region OnExit

        public void Unload() { }

        #endregion

        #region Invoke

        public void Invoke(ClientSocketWorkload workload, ClientContext context, int port, List<string> Args, string ServerDirectory)
        {
            string Command = Args[1].ToUpper();

            if (Command == "FILETOSERVER") { }
            if (Command == "FILETOCLIENT") { }
            if (Command == "GETDIRECTORIES") { }
            if (Command == "GETFILES") { }
            if (Command == "DELETEFILE") { }
            if (Command == "EDITFILE") { }
            if (Command == "COPYFILE") { }
        }

        #endregion

        #region Commands

        #region FileToServer



        #endregion

        #region FileToClient



        #endregion

        #endregion

    }
}
