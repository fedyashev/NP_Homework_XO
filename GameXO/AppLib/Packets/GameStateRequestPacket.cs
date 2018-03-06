using AppLib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLib.Packets
{
    public class GameStateRequestPacket : Packet
    {
        public string Key { get; private set; }

        public GameStateRequestPacket(int id, string key)
        {
            Type = "GETGAMESTATE";
            Key = key;
        }
    }
}
