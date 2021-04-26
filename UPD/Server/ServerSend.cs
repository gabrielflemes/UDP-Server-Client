using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class ServerSend
    {
        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            UDPServer.users[_toClient].udp.SendData(_packet);
        }

        /// <summary>Sends a packet to all clients via UDP.</summary>
        /// <param name="_packet">The packet to send.</param>
        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= UDPServer.maxConnection; i++)
            {
                UDPServer.users[i].udp.SendData(_packet);
            }
        }
        /// <summary>Sends a packet to all clients except one via UDP.</summary>
        /// <param name="_exceptClient">The client to NOT send the data to.</param>
        /// <param name="_packet">The packet to send.</param>
        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= UDPServer.maxConnection; i++)
            {
                if (i != _exceptClient)
                {
                    UDPServer.users[i].udp.SendData(_packet);
                }
            }
        }


        #region Packets
        /// <summary>Sends a welcome message to the given client.</summary>
        /// <param name="_toClient">The client to send the packet to.</param>
        /// <param name="_msg">The message to send.</param>
        public static void Welcome(int _toClient)
        {

            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_toClient);
                _packet.Write("Welcome to the server.");

                SendUDPData(_toClient, _packet);
            }
        }

        /// <summary>Sends a message to the given client.</summary>
        /// <param name="_toClient">The client to send the packet to.</param>
        /// <param name="_msg">The message to send.</param>
        public static void MessageToAll(int _fromClient, string _msg)
        {

            using (Packet _packet = new Packet((int)ServerPackets.message))
            {
                _packet.Write(_fromClient);
                _packet.Write(_msg);

                SendUDPDataToAll(_fromClient, _packet);
            }
        }
        #endregion
    }
}

