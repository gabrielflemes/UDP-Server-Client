using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    public class Client
    {
        private static Client instance = null;
        private static readonly object padlock = new object();

        public string name;

        public static int dataBufferSize = 4096; //4mb

        public string ip = "127.0.0.1"; //localhost
        public int port = 26951; //the same server

        public int myId = 0;
        public UDP udp;

        private bool isConnected = false;


        public delegate void PacketHandler(Packet _packet);
        public Dictionary<int, PacketHandler> packetHandlers;


        //singleton, so that we have only one instance and not multiple connection on server
        Client()
        {

        }

        public static Client Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Client();
                    }

                    return instance;
                }
            }
        }

        public void ConnectToServer(string _name)
        {
            //SET NAME TO CLIENT
            name = _name;


            udp = new UDP();

            InitializeClientData();
            udp.Connect(port);

            isConnected = true;
        }

        private void InitializeClientData()
        {
            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ServerPackets.welcome, ClientHandle.Welcome },
                { (int)ServerPackets.message, ClientHandle.Message }
            };

            Console.WriteLine("Initialized packetes.");

        }

        public void Disconnect()
        {
            if (isConnected)
            {
                isConnected = false;
                udp.socket.Close();

                Console.WriteLine("Disconnected from server.");
            }
        }

    }
}
