using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class User
    {
        public int id;
        public UDP udp;

        public User(int _id)
        {
            id = _id;
            udp = new UDP(id);
        }
    }
}
