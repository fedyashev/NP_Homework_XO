using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLib.Packets
{
    public class ActionFailPacket : Packet
    {
        public string Message { get; set; }

        public ActionFailPacket(string message)
        {
            Type = "ACTIONFAIL";
            Message = message;
        }
    }
}
