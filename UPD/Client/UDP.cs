using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class UDP
    {
        public UdpClient socket; //protocol
        public IPEndPoint endPoint; //endpoint

        public UDP()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(Client.Instance.ip), Client.Instance.port);
        }


        /// <summary>Attempts to connect to the server via UDP.</summary>
        /// <param name="_localPort">The port number to bind the UDP socket to.</param>
        public void Connect(int localPort)
        {
            //instantiate a udp client. Here4 we connot pass port to UdpClient, because the client has to have diff port
            socket = new UdpClient();

            socket.Connect(endPoint); //connect to endpoint. Here the port have to be the same Server
            socket.BeginReceive(ReceiveCallback, null); //Receives a datagram from a remote host asynchronously.

            //send first packet
            using (Packet packet = new Packet())
            {
                packet.Write(Client.Instance.name);
                SendData(packet);
            }
        }


        /// <summary>Sends data to the server via UDP.</summary>
        /// <param name="_packet">The packet to send.</param>
        public void SendData(Packet packet)
        {
            try
            {
                packet.InsertInt(Client.Instance.myId); // Insert the client's ID at the start of the packet
               

                if (socket != null)
                {
                    socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error sending data to server via UDP: {ex}");
            }
        }

        /// <summary>Receives incoming UDP data.</summary>
        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                byte[] data = socket.EndReceive(_result, ref endPoint); //Ends a pending asynchronous receive.
                socket.BeginReceive(ReceiveCallback, null); //Receives a datagram from a remote host asynchronously.

                //make sure we have data to handle
                if (data.Length < 4)
                {
                    //disconnect
                    Client.Instance.Disconnect();
                    return;
                }

                HandleData(data);
            }
            catch
            {
                // disconnect
                Disconnect();
            }
        }

        /// <summary>Prepares received data to be used by the appropriate packet handler methods.</summary>
        /// <param name="_data">The recieved data.</param>
        private void HandleData(byte[] _data)
        {
            using (Packet packet = new Packet(_data))
            {
                int packetLength = packet.ReadInt();
                _data = packet.ReadBytes(packetLength);
            }

            using (Packet packet = new Packet(_data))
            {
                int packetId = packet.ReadInt();
                Client.Instance.packetHandlers[packetId](packet); // Call appropriate method to handle the packet
            }
        }

        /// <summary>Disconnects from the server and cleans up the UDP connection.</summary>
        private void Disconnect()
        {
            Client.Instance.Disconnect();

            endPoint = null;
            socket = null;
        }
    }
}
