using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLib.Entity
{
    public class PlayerInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public PlayerInfo(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
