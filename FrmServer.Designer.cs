namespace CocaroServer
{
    partial class FrmServer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.TxtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtHostName = new System.Windows.Forms.TextBox();
            this.TxtChatBox = new System.Windows.Forms.TextBox();
            this.BtnSend = new System.Windows.Forms.Button();
            this.BtnOpen = new System.Windows.Forms.Button();
            this.TxtChatBoxText = new System.Windows.Forms.TextBox();
            this.BtnClose = new System.Windows.Forms.Button();
            this.LblClientCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(84, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port:";
            // 
            // TxtPort
            // 
            this.TxtPort.Location = new System.Drawing.Point(168, 149);
            this.TxtPort.Name = "TxtPort";
            this.TxtPort.ReadOnly = true;
            this.TxtPort.Size = new System.Drawing.Size(180, 22);
            this.TxtPort.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(84, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Host Name:";
            // 
            // TxtHostName
            // 
            this.TxtHostName.Location = new System.Drawing.Point(168, 104);
            this.TxtHostName.Name = "TxtHostName";
            this.TxtHostName.ReadOnly = true;
            this.TxtHostName.Size = new System.Drawing.Size(180, 22);
            this.TxtHostName.TabIndex = 1;
            // 
            // TxtChatBox
            // 
            this.TxtChatBox.Location = new System.Drawing.Point(497, 48);
            this.TxtChatBox.Multiline = true;
            this.TxtChatBox.Name = "TxtChatBox";
            this.TxtChatBox.ReadOnly = true;
            this.TxtChatBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtChatBox.Size = new System.Drawing.Size(274, 225);
            this.TxtChatBox.TabIndex = 2;
            // 
            // BtnSend
            // 
            this.BtnSend.Location = new System.Drawing.Point(691, 279);
            this.BtnSend.Name = "BtnSend";
            this.BtnSend.Size = new System.Drawing.Size(80, 27);
            this.BtnSend.TabIndex = 3;
            this.BtnSend.Text = "Gửi";
            this.BtnSend.UseVisualStyleBackColor = true;
            this.BtnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // BtnOpen
            // 
            this.BtnOpen.Location = new System.Drawing.Point(120, 281);
            this.BtnOpen.Name = "BtnOpen";
            this.BtnOpen.Size = new System.Drawing.Size(116, 50);
            this.BtnOpen.TabIndex = 3;
            this.BtnOpen.Text = "Mở Server";
            this.BtnOpen.UseVisualStyleBackColor = true;
            this.BtnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // TxtChatBoxText
            // 
            this.TxtChatBoxText.Location = new System.Drawing.Point(497, 281);
            this.TxtChatBoxText.Name = "TxtChatBoxText";
            this.TxtChatBoxText.Size = new System.Drawing.Size(188, 22);
            this.TxtChatBoxText.TabIndex = 4;
            this.TxtChatBoxText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtChatBoxText_KeyPress);
            // 
            // BtnClose
            // 
            this.BtnClose.Location = new System.Drawing.Point(252, 281);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(116, 50);
            this.BtnClose.TabIndex = 3;
            this.BtnClose.Text = "Đóng Server";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // LblClientCount
            // 
            this.LblClientCount.AutoSize = true;
            this.LblClientCount.Location = new System.Drawing.Point(201, 48);
            this.LblClientCount.Name = "LblClientCount";
            this.LblClientCount.Size = new System.Drawing.Size(14, 16);
            this.LblClientCount.TabIndex = 5;
            this.LblClientCount.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(129, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Số Client: ";
            // 
            // FrmServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LblClientCount);
            this.Controls.Add(this.TxtChatBoxText);
            this.Controls.Add(this.BtnOpen);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnSend);
            this.Controls.Add(this.TxtChatBox);
            this.Controls.Add(this.TxtHostName);
            this.Controls.Add(this.TxtPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FrmServer";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FrmServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtHostName;
        private System.Windows.Forms.TextBox TxtChatBox;
        private System.Windows.Forms.Button BtnSend;
        private System.Windows.Forms.Button BtnOpen;
        private System.Windows.Forms.TextBox TxtChatBoxText;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Label LblClientCount;
        private System.Windows.Forms.Label label3;
    }
}

