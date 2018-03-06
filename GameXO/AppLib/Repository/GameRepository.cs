using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppLib.Entity;

namespace AppLib.Repository
{
    public class GameRepository
    {
        private static int _counter;
        private static List<GameInfo> _games;

        static GameRepository()
        {
            _counter = 0;
            _games = new List<GameInfo>();
        }

        public static List<GameInfo> GetGames()
        {
            return _games;
        }

        public static void Add(GameInfo game)
        {
            game.Id = _counter;
            _games.Add(game);
            _counter++;
        }
    }
}
