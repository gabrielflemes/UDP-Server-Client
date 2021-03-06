using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    public class ClientSend
    {
        private static void SendUDPData(Packet packet)
        {
            packet.WriteLength();
            Client.Instance.udp.SendData(packet);
        }


        #region Packet
        public static void WelcomeReceived()
        {
            using (Packet packet = new Packet((int)ClientPackets.welcomeReceived))
            {

                packet.Write(Client.Instance.myId);
                packet.Write("User: " + Client.Instance.name);

                SendUDPData(packet);
            }
        }

        public static void Message(string name, string msg)
        {
            using (Packet packet = new Packet((int)ClientPackets.messageReceived))
            {

                packet.Write(Client.Instance.myId);
                packet.Write(name);
                packet.Write(msg);

                SendUDPData(packet);
            }
        }
        #endregion

    }
}
