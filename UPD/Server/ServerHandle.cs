using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{UDPServer.users[_fromClient].id} connected successfully and is now user {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"User \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }


            //using (Packet _packet = new Packet((int)ServerPackets.welcome))
            //{
            //    _packet.Write("Welcome to the server.");

            //    UDPServer.users[_fromClient].udp.SendData(_packet);
            //}
            
        }

        public static void MessageReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _name = _packet.ReadString();
            string _msg = _packet.ReadString();

            Console.WriteLine($"{_name}: {_msg}");

            //broadcast send to all
            ServerSend.MessageToAll(_fromClient, _name, _msg);
       
           
        }
    }
}
