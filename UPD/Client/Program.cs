using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "[POC TCP/UDP] Client";


            //get name from console
            Console.Write("Type your name: ");
            string inputName = Console.ReadLine();


            //Start UDP/FTP client.
            Client.Instance.ConnectToServer(inputName);

            //message/app loop
            while (true)
            {

                string inputText = Console.ReadLine();

                ClientSend.Message(inputName, inputText);

            }
        }


    }
}
