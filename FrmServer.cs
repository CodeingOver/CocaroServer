﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CocaroServer
{
    public partial class FrmServer : Form
    {
        public class ClientInfo
        {
            public TcpClient Client { get; set; }
            public string EndPoint { get; set; }
        }

        TcpListener server;
        List<ClientInfo> clients = new List<ClientInfo>(3);
        DateTime TimeNow;

        public FrmServer()
        {
            InitializeComponent();
        }

        private void FrmServer_Load(object sender, EventArgs e)
        {
            var IPserver = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var ip in IPserver.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    TxtHostName.Text = ip.ToString();
                }
            }


            Random random = new Random();
            int port = random.Next(1024, 65535);
            TxtPort.Text = port.ToString();
        }


        private async void BtnOpen_Click(object sender, EventArgs e)
        {
            if (server != null)
            {
                MessageBox.Show("Server Đang chạy.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IPAddress IPserver = IPAddress.Parse(TxtHostName.Text);
            int port = int.Parse(TxtPort.Text);
            server = new TcpListener(IPserver, port);
            server.Start();

            TimeNow = DateTime.Now;
            AppendTextToChatBox(TimeNow.ToString("HH:mm:ss") + " Server is running at " + IPserver.ToString() + ":" + port + "");
            AppendTextToChatBox(TimeNow.ToString("HH:mm:ss") + " Waiting for clients...");

            try
            {
                while (true)
                {
                    var client = await server.AcceptTcpClientAsync();
                    if (clients.Count == 2)
                    {
                        client.Close();
                        MessageBox.Show("Server đã đủ 2 người chơi", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    var clientInfo = new ClientInfo
                    {
                        Client = client,
                        EndPoint = client.Client.RemoteEndPoint.ToString()
                    };
                    clients.Add(clientInfo);

                    TimeNow = DateTime.Now;
                    AppendTextToChatBox(TimeNow.ToString("HH:mm:ss") + " Client connected: " + clientInfo.EndPoint);
                    _ = Task.Run(() => ReceiveData(clientInfo));
                    Invoke(new Action(() => LblClientCount.Text = clients.Count.ToString()));
                }
            }
            catch (ObjectDisposedException)
            {
                TimeNow = DateTime.Now;
                AppendTextToChatBox(TimeNow.ToString("HH:mm:ss") + " Server has been stopped.");
            }
        }

        private async Task ReceiveData(ClientInfo clientInfo)
        {
            NetworkStream stream = clientInfo.Client.GetStream();
            byte[] buffer = new byte[1024];
            while (true)
            {
                try
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        TimeNow = DateTime.Now;
                        Invoke(new Action(() => AppendTextToChatBox(TimeNow.ToString("HH:mm:ss") + " Client disconnected: " + clientInfo.EndPoint)));
                        break; // Client disconnected
                    }
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    AsyncDataReceived(clientInfo.EndPoint, message);


                    string[] arr = message.Split('|');
                    if (arr[0] == "CHAT")
                    {
                        string chatMessageName = arr[1];
                        string chatMessage = arr[2];
                        TimeNow = DateTime.Now;
                        Invoke(new Action(() => AppendTextToChatBox(TimeNow.ToString("HH:mm:ss") + " [" + chatMessageName + "]: " + chatMessage)));
                    }
                    else if (arr[0] == "MOVE")
                    {
                        string chatMessageName = arr[1];
                        int i = int.Parse(arr[2]);
                        int j = int.Parse(arr[3]);
                        string player = arr[4];
                        Invoke(new Action(() => AppendTextToChatBox(TimeNow.ToString("HH:mm:ss") + " [" + chatMessageName + "]: " + "đã đánh ở ô [" + i + "," + j + "]")));

                    }
                }
                catch
                {
                    // MessageBox.Show(ex.Message);
                    break;
                }
            }
            clients.Remove(clientInfo);
            clientInfo.Client.Close();
            Invoke(new Action(() => LblClientCount.Text = clients.Count.ToString())); // Update client count
        }

        private void AsyncDataReceived(string EndPoint, string Asysncdata)
        {
            foreach (var clientInfo in clients)
            {
                if (clientInfo.EndPoint != EndPoint)
                {
                    NetworkStream stream = clientInfo.Client.GetStream();
                    byte[] data = Encoding.UTF8.GetBytes(Asysncdata);
                    stream.Write(data, 0, data.Length);
                }
            }
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (clients.Count == 0 || TxtChatBoxText.Text == "") return;

            TimeNow = DateTime.Now;
            string message = "CHAT|Server|" + TxtChatBoxText.Text;
            byte[] data = Encoding.UTF8.GetBytes(message);
            foreach (var client in clients)
            {
                NetworkStream stream = client.Client.GetStream(); // Fix: Access the NetworkStream from the TcpClient
                stream.Write(data, 0, data.Length);
            }
            AppendTextToChatBox(TimeNow.ToString("HH:mm:ss") + " [Server]: " + TxtChatBoxText.Text);
            TxtChatBoxText.Clear();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            foreach (var client in clients)
            {
                client.Client.Close();
            }
            clients.Clear();
            if (server != null)
            {
                server.Stop();
                server = null;
            }
        }

        private void AppendTextToChatBox(string text)
        {
            TxtChatBox.AppendText(text + Environment.NewLine);
            TxtChatBox.SelectionStart = TxtChatBox.Text.Length;
            TxtChatBox.ScrollToCaret();
        }

        private void TxtChatBoxText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnSend.PerformClick();
                e.Handled = true;
            }
        }

        private void FrmServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnClose_Click(sender, e);
        }
    }
}
