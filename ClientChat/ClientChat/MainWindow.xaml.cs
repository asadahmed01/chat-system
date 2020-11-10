using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        //string data = null;
        //string IPaddress = "127.0.0.1";
        //int port = 1234;
        

        public MainWindow()
        {
            InitializeComponent();
        }

       
        TcpClient client;
       
        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            client = new TcpClient("127.0.0.1", 13000);
            status.Text = "Connected to the Server";

            ParameterizedThreadStart ts = new ParameterizedThreadStart(HandleClient);
            Thread clientThread = new Thread(ts);
            clientThread.Start(client);

            //int byteCount = Encoding.ASCII.GetByteCount(msgArea.Text);
            //byte[] sendData = new byte[byteCount];
            //sendData = Encoding.ASCII.GetBytes(msgArea.Text);
            //NetworkStream stream = client.GetStream();
            //stream.Write(sendData, 0, sendData.Length);
            //msgArea.Text = "";
            //stream.Close();
            //client.Close();

        } //end of send button

        public  void HandleClient(Object obj)
        {
            TcpClient client = (TcpClient)obj;

            //StreamReader sReader = new StreamReader(client.GetStream());
            this.Dispatcher.Invoke(() => {
                int byteCount = Encoding.ASCII.GetByteCount(msgArea.Text);
                byte[] sendData = new byte[byteCount];
                sendData = Encoding.ASCII.GetBytes(msgArea.Text);
                NetworkStream stream = client.GetStream();
                stream.Write(sendData, 0, sendData.Length);
                stream.Flush();
                msgArea.Text = "";
                ////
                //StreamReader reader = new StreamReader(client.GetStream());
                byte[] receivedData = new byte[10000];
                StringBuilder msg = new StringBuilder();
                int byteNumber = stream.Read(receivedData, 0, receivedData.Length);
                msg.AppendFormat("{0}", Encoding.ASCII.GetString(receivedData, 0, byteNumber));

                chatArea.Text += "Me: " + msg.ToString() + '\n';
                ////
                stream.Close();
                client.Close();
            });
                
            
        }

    }
    
}
