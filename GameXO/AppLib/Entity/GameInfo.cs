using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLib.Entity
{
    public class GameInfo
    {
        private static readonly int CROSS = 1;
        private static readonly int ROUND = -1;
        private static readonly int EMPTY = 0;

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
            Field = new int[3, 3] { { EMPTY, EMPTY, EMPTY }, { EMPTY, EMPTY, EMPTY }, { EMPTY, EMPTY, EMPTY } };
        }

        public bool CompareKey(string key)
        {
            return Key.Equals(key);
        }

        public bool isListen()
        {
            return State.Equals("LISTEN");
        }

        //public void SetCross(int i, int j)
        //{
        //    Field[i, j] = CROSS;
        //}

        //public void SetRound(int i, int j)
        //{
        //    Field[i, j] = ROUND;
        //}

        private void SetSign(int sign, int i, int j)
        {
            Field[i, j] = sign;
        }

        private bool isSing(int sign, int i, int j)
        {
            return Field[i, j] == sign;
        }

        private bool isCross(int i, int j)
        {
            return isSing(CROSS, i, j);
        }

        private bool isRound(int i, int j)
        {
            return isSing(ROUND, i, j);
        }

        private bool isEmpty(int i, int j)
        {
            return isSing(EMPTY, i, j);
        }

        //public int GetValue(int i, int j)
        //{
        //    return Field[i, j];
        //}

        public bool Join(string player2Name)
        {
            if (!isListen()) return false;
            if (String.IsNullOrWhiteSpace(player2Name)) return false;
            if (Player1Name.Equals(player2Name)) return false;

            Player2Name = player2Name;
            State = "GAME";

            var rand = new Random(DateTime.Now.Millisecond);
            Turn = rand.Next(1000) % 2 == 0 ? Player1Name : Player2Name;

            return true;
        }

        public bool Action(string playerName, int i, int j)
        {
            if (!State.Equals("GAME")) return false;
            if (String.IsNullOrWhiteSpace(playerName)) return false;
            if (!Player1Name.Equals(playerName) && !Player2Name.Equals(playerName)) return false;
            if (!Turn.Equals(playerName)) return false;
            if (i < 0 || i > 2 || j < 0 || j > 2) return false;
            if (!isEmpty(i, j)) return false;

            var sign = Player1Name.Equals(playerName) ? CROSS : ROUND;
            Field[i, j] = sign;

            if (isWin(sign))
            {
                State = "FINISH";
                Turn = String.Empty;
                Winner = sign == CROSS ? Player1Name : Player2Name;
            }
            else
            {
                if (isDraw())
                {
                    State = "DRAW";
                    Turn = String.Empty;
                }
                else
                {
                    Turn = Turn.Equals(Player1Name) ? Player2Name : Player1Name;
                }
            }
            return true;
        }

        private bool isDraw()
        {
            for (var i = 0; i <= 2; i++)
            {
                for (var j = 0; j <= 2; j++)
                {
                    if (isEmpty(i, j)) return false;
                }
            }
            return true;
        }

        private bool isWin(int sign)
        {
            return (isSing(sign, 0, 0) && isSing(sign, 0, 1) && isSing(sign, 0, 2)) ||
                   (isSing(sign, 1, 0) && isSing(sign, 1, 1) && isSing(sign, 1, 2)) ||
                   (isSing(sign, 2, 0) && isSing(sign, 2, 1) && isSing(sign, 2, 2)) ||
                   (isSing(sign, 0, 0) && isSing(sign, 1, 0) && isSing(sign, 2, 0)) ||
                   (isSing(sign, 0, 1) && isSing(sign, 1, 1) && isSing(sign, 2, 1)) ||
                   (isSing(sign, 0, 2) && isSing(sign, 1, 2) && isSing(sign, 2, 2)) ||
                   (isSing(sign, 0, 0) && isSing(sign, 1, 1) && isSing(sign, 2, 2)) ||
                   (isSing(sign, 0, 2) && isSing(sign, 1, 1) && isSing(sign, 2, 0));
        }
    }
}
