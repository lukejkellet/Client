﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create an object of socket class
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Create an object of an IP Address.socket listening on any ip address.
            IPAddress ipaddr = IPAddress.Any;
            //Define IP End Point
            IPEndPoint ipep = new IPEndPoint(ipaddr, 25000);
            //Bind socket to end point
            try
            {
                listenerSocket.Bind(ipep);
                //Call Listen Method
                listenerSocket.Listen(5);

                //Define buffer
                byte[] buff = new byte[128];
                //Define number of received bytes
                int numberOfReceivedBytes = 0;
                while (true)
                {
                    //Call Accept Method
                    Socket client = listenerSocket.Accept();

                    //Start new thread
                    Thread clientThread = new Thread(() => ClientHandler(client));
                    clientThread.Start();


                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ClientHandler(Socket client)
        {
            try
            {
                //Print out client ip end point
                Console.WriteLine("Client Connected " + client.ToString() + " " + client.RemoteEndPoint.ToString());

                byte[] buff = new byte[128];
                int numberOfReceivedBytes = 0;

                while (true)
                {
                    numberOfReceivedBytes = client.Receive(buff);
                    Console.WriteLine("Number of received bytes: " + numberOfReceivedBytes);
                    //Convert from byte array to ascii, using encoding.ascii.getstring
                    Console.WriteLine("Data sent by client: " + Encoding.ASCII.GetString(buff, 0, numberOfReceivedBytes));
                    string receivedText = Encoding.ASCII.GetString(buff, 0, numberOfReceivedBytes);
                    //Send back to client
                    buff = Encoding.ASCII.GetBytes(receivedText);
                    client.Send(buff);
                    if (receivedText == "x")
                    {
                        break;
                    }
                    Array.Clear(buff, 0, buff.Length);
                    numberOfReceivedBytes = 0;
                }
                client.Close();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}