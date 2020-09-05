using System;
using System.Collections.Generic;
using Rocket.Core.Plugins;
using Logger = Rocket.Core.Logging.Logger;
using Rocket.API;
using MySql.Data.MySqlClient;
using System.Threading;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Provider.Services.Statistics.Global;
using System.Drawing;
using Rocket.Unturned.Chat;
using Color = UnityEngine.Color;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BinaryBank
{
    public class BinaryBank : RocketPlugin<BinaryBankConfiguration>
    {
        public BinaryBankDatabase BinaryBankDatabase;
        public static BinaryBank Instance;

        private void Events_OnPlayerConnected(UnturnedPlayer joueur)
        {
            if(BinaryBank.Instance.BinaryBankDatabase.CheckIfExist(BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id)))
            {
                if (BinaryBank.Instance.BinaryBankDatabase.PlayerConnected(BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id)))
                {
                    UnturnedChat.Say(joueur, "Synchronisation effectuée", Color.green);
                }
            }
            
        }
        private void Events_OnPlayerDisconnected(UnturnedPlayer joueur)
        {
            if (BinaryBank.Instance.BinaryBankDatabase.CheckIfExist(BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id)))
            {
                if (BinaryBank.Instance.BinaryBankDatabase.PlayerDisconnected(BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id)))
                {
                    UnturnedChat.Say(joueur, "Désynchronisation effectuée", Color.green);
                }
            }

        }
        protected override void Load()
        {
            Instance = this;
            U.Events.OnPlayerDisconnected += Events_OnPlayerDisconnected;
            U.Events.OnPlayerConnected += Events_OnPlayerConnected;
            BinaryBankDatabase = new BinaryBankDatabase();
            Logger.Log("BinaryBank est load!");
        }

        protected override void Unload()
        {
            Logger.Log("BinaryBank est unload!");
        }
    }
}
