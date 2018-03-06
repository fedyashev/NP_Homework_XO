using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLib.Packets
{
    public class CreateGameSuccessPacket : Packet
    {
        public int Id { get; set; }
        public string Key { get; set; }

        public CreateGameSuccessPacket(int id, string key)
        {
            Type = "CREATEGAMESUCCESS";
            Id = id;
            Key = key;
        }
    }
}
