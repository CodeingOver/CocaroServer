using System;
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
        TcpListener server;
        List<TcpClient> clients = new List<TcpClient>();

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

            AppendTextToChatBox("Server is running at " + IPserver.ToString() + ":" + port + "");
            AppendTextToChatBox("Waiting for clients...");

            try
            {
                while (true)
                {
                    var client = await server.AcceptTcpClientAsync();
                    clients.Add(client);
                    AppendTextToChatBox("Client connected");
                    _ = Task.Run(() => ReceiveData(client));
                    Invoke(new Action(() => LblClientCount.Text = clients.Count.ToString()));
                }
            }
            catch (ObjectDisposedException)
            {
                AppendTextToChatBox("Server has been stopped.");
            }
        }

        private async Task ReceiveData(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            while (true)
            {
                try
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        Invoke(new Action(() => AppendTextToChatBox("Client disconnected")));
                        Invoke(new Action(() => LblClientCount.Text = clients.Count.ToString()));// Update client count
                        break; // Client disconnected
                    }
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Invoke(new Action(() => AppendTextToChatBox(message)));
                }
                catch
                {
                    break;
                }
            }
            clients.Remove(client);
            client.Close();
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (clients.Count == 0) return;

            string message = TxtChatBoxText.Text;
            byte[] data = Encoding.UTF8.GetBytes(message);
            foreach (var client in clients)
            {
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
            }
            AppendTextToChatBox("Server: " + message);
            TxtChatBoxText.Clear();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            foreach (var client in clients)
            {
                client.Close();
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
    }
}
