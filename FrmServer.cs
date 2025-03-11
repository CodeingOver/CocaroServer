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
        List<TcpClient> clients = new List<TcpClient>(3);
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
            AppendTextToChatBox(TimeNow.ToString("HH:mm:ss") +" Server is running at " + IPserver.ToString() + ":" + port + "");
            AppendTextToChatBox(TimeNow.ToString("HH:mm:ss") +" Waiting for clients...");

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
                    
                    clients.Add(client);
                    TimeNow = DateTime.Now;
                    AppendTextToChatBox(TimeNow.ToString("HH:mm:ss") + " Client connected");
                    _ = Task.Run(() => ReceiveData(client));
                    Invoke(new Action(() => LblClientCount.Text = clients.Count.ToString()));
                }
            }
            catch (ObjectDisposedException)
            {
                TimeNow = DateTime.Now;
                AppendTextToChatBox(TimeNow.ToString("HH:mm:ss") + " Server has been stopped.");
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
                        TimeNow = DateTime.Now;
                        Invoke(new Action(() => AppendTextToChatBox(TimeNow.ToString("HH:mm:ss") + " Client disconnected")));
                        break; // Client disconnected
                    }
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    string[] arr = message.Split('|');
                    if(arr[0] == "CHAT")
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
                        Invoke(new Action(() => AppendTextToChatBox(TimeNow.ToString("HH:mm:ss") + " [" + chatMessageName + "]: " + "đã đánh ở ô [" +i+","+j+"]")));
                        
                        //Đồng bộ dữ liệu (đưa dữ liệu move cho client còn lại bỏ client gửi move ra)



                    }
                }
                catch
                {
                    //MessageBox.Show(ex.Message);
                    break;
                }
            }
            clients.Remove(client);
            client.Close();
            Invoke(new Action(() => LblClientCount.Text = clients.Count.ToString()));// Update client count
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (clients.Count == 0 || TxtChatBoxText.Text == "") return;

            TimeNow = DateTime.Now;
            string message = "CHAT|Server|"+TxtChatBoxText.Text;
            byte[] data = Encoding.UTF8.GetBytes(message);
            foreach (var client in clients)
            {
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
            }
            AppendTextToChatBox(TimeNow.ToString("HH:mm:ss") + " [Server]: " + TxtChatBoxText.Text);
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
