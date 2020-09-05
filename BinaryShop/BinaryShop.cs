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

namespace BinaryShop
{
    public class BinaryShop : RocketPlugin<BinaryShopConfiguration>
    {
        public BinaryShopDatabase BinaryShopDatabase;
        public static BinaryShop Instance;

        protected override void Load()
        {
            Instance = this;
            BinaryShopDatabase = new BinaryShopDatabase();
            if (BinaryShop.Instance.BinaryShopDatabase.testconnection())
            {
                Logger.Log("Connectée a la DB de BinaryShop");
            }
            Logger.Log("BinaryBank est load!");
        }

        protected override void Unload()
        {
            Logger.Log("BinaryBank est unload!");
        }
    }
}
