using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLib.Packets
{
    public class JoinFailPacket : Packet
    {
        public string Message { get; set; }

        public JoinFailPacket(string message)
        {
            Type = "JOINFAIL";
            Message = message;
        }
    }
}
