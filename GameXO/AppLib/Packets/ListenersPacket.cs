using AppLib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLib.Packets
{
    public class ListenersPacket : Packet
    {
        public List<PlayerInfo> Players { get; set; }

        public ListenersPacket(List<PlayerInfo> players)
        {
            Type = "LISTENERS";
            Players = players;
        }
    }
}
