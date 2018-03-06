using AppLib.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLib.Packets
{
    public class GameStateResponsePacket : Packet
    {
        public int Id { get; set; }
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public string State { get; set; }
        public string Turn { get; set; }
        public string Winner { get; set; }
        public int[,] Field { get; set; }

        public GameStateResponsePacket(GameInfo game)
        {
            Type = "GAMESTATE";
            Id = game.Id;
            Player1Name = game.Player1Name;
            Player2Name = game.Player2Name;
            State = game.State;
            Turn = game.Turn;
            Winner = game.Winner;
            Field = game.Field;
        }
    }
}
