#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenCrowns.ClientCore;
using TenCrowns.GameCore;

namespace ColonistMod
{
    public class ModGameFactory : GameFactory
    {
        public static Game? CurrentGame
        {
            get;
            private set;
        }

        public static ClientManager? CurrentClientManager
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

        public override ClientManager CreateClientManager(ModSettings modSettings, Game gameClient, GameInterfaces gameInterfaces, IClientNetwork network)
        {
            ClientManager mgr = base.CreateClientManager(modSettings, gameClient, gameInterfaces, network);
            CurrentClientManager = mgr;
            Utils.DbgLog(String.Format("CURRENT CLIENT MANAGER UPDATED: {0}", CurrentClientManager));
            return mgr;
        }
    }
}
