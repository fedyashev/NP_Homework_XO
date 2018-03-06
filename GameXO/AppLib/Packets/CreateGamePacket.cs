using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLib.Packets
{
    public class CreateGamePacket : Packet
    {
        public string Name { get; set; }

        public CreateGamePacket(string name)
        {
            Type = "CREATEGAME";
            Name = name;
        }
    }
}
