using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLib.Packets
{
    public class JoinPacket : Packet
    {
        public string Name { get; private set; }

        public JoinPacket(int id, string name)
        {
            Type = "JOIN";
            Name = name;
        }
    }
}
