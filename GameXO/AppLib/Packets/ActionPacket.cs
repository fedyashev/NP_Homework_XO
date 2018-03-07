using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLib.Packets
{
    public class ActionPacket : Packet
    {
        public string Name { get; set; }
        public string  Key { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }

        public ActionPacket(string name, string key, int i, int j)
        {
            Type = "ACTION";
            Name = name;
            Key = key;
            Row = i;
            Col = j;
        }
    }
}
