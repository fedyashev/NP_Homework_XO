using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLib.Packets
{
    public class JoinSuccessPacket : Packet
    {
        public string Key { get; private set; }

        public JoinSuccessPacket(string key)
        {
            Type = "JOINSUCCESS";
            Key = key;
        }
    }
}
