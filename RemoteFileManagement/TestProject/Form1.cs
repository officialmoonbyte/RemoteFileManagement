using MoonByte.Net.Plugins;
using System;
using System.Windows.Forms;

namespace TestProject
{
    public partial class Form1 : Form
    {
        private RemoteFileManagement remoteFile;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            remoteFile.FileToServer(@"C:\Users\Alexander Ritter\Desktop\TestServer\Client\SendToServer.txt", @"C:\Users\Alexander Ritter\Desktop\TestServer\Server\ReceivedFromClient.txt");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            remoteFile.FileToClient(@"C:\Users\Alexander Ritter\Desktop\TestServer\Server\SendToClient.txt", @"C:\Users\Alexander Ritter\Desktop\TestServer\Client\ReceivedFromServer.txt");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] m = remoteFile.GetServerDirectory(@"C:\Users\Alexander Ritter\Desktop\Dir\Server");
            string retur = null;
            foreach (string b in m)
            {
                retur += "%20" + b;
            }

            MessageBox.Show(retur);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] s = remoteFile.GetServerFiles(@"C:\Users\Alexander Ritter\Desktop\Dir\Server");
            string retur = null;
            foreach (string b in s)
            {
                if (retur == null) { retur = b; }
                else { retur += "%2" + b; }
            }

            MessageBox.Show(retur);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            remoteFile.DeleteServerFile(@"C:\Users\Alexander Ritter\Desktop\Dir\Server\Delete.txt");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            remoteFile.CopyServerFile(@"C:\Users\Alexander Ritter\Desktop\Dir\Server\Copy.txt", @"C:\Users\Alexander Ritter\Desktop\Dir\Server\Copy2.txt");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            remoteFile = new RemoteFileManagement("localhost", 7777);
        }
    }
}