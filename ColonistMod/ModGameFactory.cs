#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenCrowns.GameCore;

namespace ColonistMod
{
    public class ModGameFactory : GameFactory
    {
        public Game? CurrentGame
        {
            get;
            private set;
        }

        public ModGameFactory()
            : base()
        {

        }

        public override Game CreateGame(ModSettings pModSettings)
        {
            Game game = base.CreateGame(pModSettings);
            CurrentGame = game;
            Utils.DbgLog(String.Format("CURRENT GAME UPDATED: {0}", CurrentGame));
            return game;
        }
    }
}
