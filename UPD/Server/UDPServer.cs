using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    /// <summary>
    /// Handle with UDP 
    /// </summary>
    public class UDPServer
    {
        private static UdpClient udpListener;

        public static int maxConnection { get; private set; }
        public static int port { get; private set; }

        public static Dictionary<int, User> users = new Dictionary<int, User>();


        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<int, PacketHandler> packetHandlers;



        /// <summary>
        /// Start UDP Server
        /// </summary>
        /// <param name="_maxConnection"></param>
        /// <param name="_port"></param>
        public static void Start(int _maxConnection, int _port)
        {

            //set port and maxConnection
            maxConnection = _maxConnection;
            port = _port;

            //
            Console.WriteLine($"Starting server...");
            InitializeServerData();
        

            //Begin accept UDP clients. 
            udpListener = new UdpClient(port);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            Console.WriteLine($"Server started on {port}.");

        }


        /// <summary>Initializes all necessary server data.</summary>
        private static void InitializeServerData()
        {
            //Initialize all empty clients 
            for (int i = 1; i <= maxConnection; i++)
            {
                users.Add(i, new User(i));
            }

            //Initialize PacketHandler dictionary that we reaceive from cliets
            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                { (int)ClientPackets.messageReceived, ServerHandle.MessageReceived }
            };

            Console.WriteLine($"Initialized packet.");
        }

        /// <summary>Receives incoming UDP data.</summary>
        public static void UDPReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpListener.EndReceive(_result, ref clientEndPoint);
                udpListener.BeginReceive(UDPReceiveCallback, null);

                if (data.Length < 4)
                {
                    return;
                }

                //Handle received Data
                using (Packet _packet = new Packet(data))
                {
                    int clientId = _packet.ReadInt();

                    //// If this is a new connection
                    if (clientId == 0)
                    {
                        for (int i = 1; i < users.Count; i++)
                        {
                            if (users[i].udp.endPoint == null)
                            {
                                users[i].udp.Connect(clientEndPoint);
                                clientId = i;
                                break;
                            }
                        }

                        using (Packet _pack = new Packet((int)ServerPackets.welcome))
                        {
                            //you have to catch these info at client on the same order
                            _pack.Write(clientId);
                            _pack.Write("Welcome");
                            _pack.WriteLength();


                            UDPServer.SendUDPData(clientEndPoint, _pack);
                        }

                        return;
               

                    }


                    if (users[clientId].udp.endPoint.ToString() == clientEndPoint.ToString())
                    {
                        users[clientId].udp.HandleData(_packet);
                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error receiving UDP data: {ex}");
            }
        }


        /// <summary>Sends a packet to the specified endpoint via UDP.</summary>
        /// <param name="_clientEndPoint">The endpoint to send the packet to.</param>
        /// <param name="_packet">The packet to send.</param>
        public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packed)
        {
            try
            {
                if (_clientEndPoint != null)
                {
                    udpListener.BeginSend(_packed.ToArray(), _packed.Length(), _clientEndPoint, null, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending data to {_clientEndPoint} via UDP: {ex}");
            }
        }


    }
}
