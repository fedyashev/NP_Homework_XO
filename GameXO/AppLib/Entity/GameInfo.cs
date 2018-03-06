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
        public string Key { get; set; }
        public string State { get; set; }
        public string Turn { get; set; }
        public string Winner { get; set; }
        public int[,] Field { get; set; }

        public GameInfo(string player1Name)
        {
            Player1Name = player1Name;
            Player2Name = String.Empty;
            Key = System.Guid.NewGuid().ToString();
            State = "LISTEN";
            Turn = String.Empty;
            Winner = String.Empty;
            Field = new int[3, 3] { {0, 0, 0}, {0, 0, 0}, {0, 0, 0} };
        }

        public bool isListen()
        {
            return State.Equals("LISTEN");
        }

        public void SetX(int i, int j)
        {
            Field[i, j] = 1;
        }

        public void SetO(int i, int j)
        {
            Field[i, j] = -1;
        }

        public int GetValue(int i, int j)
        {
            return Field[i, j];
        }

        public void Join(string player2Name)
        {

        }
    }
}
