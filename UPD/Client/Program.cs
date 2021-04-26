using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "[POC TCP/UDP] Client";


            //Start UDP/FTP client.
            Client.Instance.ConnectToServer();


            //message/app loop
            while (true)
            {

                string inputText = Console.ReadLine();

                ClientSend.Message(inputText);

            }
        }


    }
}
