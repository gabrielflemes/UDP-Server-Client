using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Client
{
    public class ClientHandle
    {
        public static void Welcome(Packet _packet)
        {
            //Look out with the order, it's important
            int _myId = _packet.ReadInt();
            string _msg = _packet.ReadString();
            

            Console.WriteLine($"{_myId} : {_msg}");

            Client.Instance.myId = _myId;

            //send welcome received packet
            ClientSend.WelcomeReceived();


        }


        //
        public static void Message(Packet _packet)
        {

            int id = _packet.ReadInt();
            string message = _packet.ReadString();

            Console.WriteLine($"{id}:{message}");

        }
    }
}
