using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BroadCastChatt
{
    class Program
    {
        //Skapar en tråd som körs sammtidigt som huvudprogrammet
        private const int Listenport = 11000;
        static void Main(string[] args)
        {
            var listenThread = new Thread(Listener);
            listenThread.Start();

            //Skapar en lokal anslutning via udp och IPv4
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.EnableBroadcast = true;


            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 11000);
            Thread.Sleep(1000);

            while (true)
            {
                Console.Write(">");
                string msg = Console.ReadLine();

                byte[] sendbuf = Encoding.UTF8.GetBytes(msg);
                socket.SendTo(sendbuf, ep);
                Thread.Sleep(200);
            }

        }
        static void Listener()
          
        {
            //Skapar ett objekt som ska lyssna efter meddelanden
            UdpClient listener = new UdpClient(Listenport);
            try
            {
                while (true)
                {
                    //Skapar objekt som lyssnar efter trafik från valfri, ip-adress men via en port 
                    IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, Listenport);
                    //Tar med meddelanden i en array
                    byte[] bytes = listener.Receive(ref groupEP);
                    //skriver ut meddelanden
                    Console.WriteLine("Recive broadcast from{0} : {1}\n", groupEP.ToString(), Encoding.UTF8.GetString(bytes, 0, bytes.Length));


                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Close();
            }
        }
    }
}
