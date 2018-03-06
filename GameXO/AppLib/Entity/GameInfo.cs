using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLib.Entity
{
    public class GameInfo
    {
        public int Id { get; set; }
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }

        public GameInfo(string player1Name)
        {
            Player1Name = player1Name;
        }
    }
}
