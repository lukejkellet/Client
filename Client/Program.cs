using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket client = null;
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddr = null;
            try
            {
                Console.WriteLine("Welcome to Socket Client Starter ***");
                Console.WriteLine("Please enter the server IP address: ");
                string strIP = Console.ReadLine();
                Console.WriteLine("Please enter the server Port: ");
                string strPort = Console.ReadLine();
                int nPort = 0;

                if (strIP == " ")
                {
                    strIP = "127.0.0.1";
                }
                if (strPort == " ")
                {
                    strPort = "25000";
                }

                if (!IPAddress.TryParse(strIP, out ipaddr))
                {
                    Console.WriteLine("Invalid IP address format");
                    return;
                }

                if (!int.TryParse(strPort.Trim(), out nPort))
                {
                    Console.WriteLine("Invalid port number");
                    return;
                }

                if (nPort <= 0 || nPort > 65535)
                {
                    Console.WriteLine("Port number must be between 0 and 65535");
                    return;
                }

                System.Console.WriteLine(string.Format("IPAddress: {0} - Port: {1}", ipaddr.ToString(), nPort));
                client.Connect(ipaddr, nPort);

                Console.WriteLine("Connected to the server, type text and press enter to send it to the server. Type <EXIT> to close connection and exit");
                string inputCommand = string.Empty;

                while (true)
                {
                    inputCommand = Console.ReadLine();
                    if (inputCommand == "<EXIT>")
                    {
                        break;
                    }
                    byte[] buffSend = Encoding.ASCII.GetBytes(inputCommand);
                    client.Send(buffSend);
                    byte[] buffReceived = new byte[128];
                    int nRecv = client.Receive(buffReceived);
                    Console.WriteLine("Data received: {0}", Encoding.ASCII.GetString(buffReceived, 0, nRecv));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (client != null)
                {
                    if (client.Connected)
                    {
                        client.Shutdown(SocketShutdown.Both);
                    }
                    client.Close();
                    client.Dispose();
                }
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}