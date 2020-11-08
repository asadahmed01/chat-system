using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Resources;

namespace Server
{
    class Program
    {
        public static List<TcpClient> clientList = new List<TcpClient>();
        static void Main(string[] args)
        {
            TcpClient client = null;
            TcpListener server = null;
            try
            {


                //prepare the TcpListener on port 3000
                Int32 port = 13000;
                IPAddress localAddress = IPAddress.Parse("127.0.0.1");
                //create the listener/server
                server = new TcpListener(localAddress, port);
                client = default;
                //start the server
                server.Start();

                //start listening for connection
                while (true)
                {
                    Console.WriteLine("waiting for connection...");
                    client = server.AcceptTcpClient();
                    
                    Console.WriteLine("connected!");

                    clientList.Add(client);

                    ParameterizedThreadStart thread = new ParameterizedThreadStart(Helper);
                    Thread clientThread = new Thread(thread);
                    clientThread.Start(client);
                }
            }

            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                client.Close();
                server.Stop();
            }

            Console.WriteLine("Press ENTER to continue..");

        }

        public static void Helper(Object obj)
        {
            TcpClient client = (TcpClient)obj;
            //buffer for reading data
            Byte[] bytes = new Byte[13000];
            string data = string.Empty;
            byte[] msg = new byte[30000];
            int counter;
            //stream object for reading and writing
            NetworkStream netStream = client.GetStream();
            //keep looping untill all data from the client is recieved
            while ((counter = netStream.Read(bytes, 0, bytes.Length)) != 0)
            {
                //create string var for holding the decoded data
                // Translate data bytes to a ASCII string.
                data = Encoding.ASCII.GetString(bytes, 0, counter);
                Console.WriteLine("Received: {0}", data);

                // Process the data sent by the client.
                data = data.ToUpper();

                msg = Encoding.ASCII.GetBytes(data);

                // Send back a response.
                //foreach(TcpClient user in clientList)
                //{
                    netStream.Write(msg, 0, msg.Length);
                    Console.WriteLine("Server Sent: {0}", data);
                //}
                //netStream.Write(msg, 0, msg.Length);
                //Console.WriteLine("Sent: {0}", data);
            }

            
            // Shutdown and end connection
            client.Close();
            
        }
    }
}
