using Rocket.API;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Items;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger = Rocket.Core.Logging.Logger;
using System.Net;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using Rocket.Core.Plugins;
using Rocket.Core;
using UnityEngine;

namespace BinaryBank
{
    public class Command_bbalance : IRocketCommand
    {
        public BinaryBankDatabase BinaryBankDatabase;
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "bbalance"; }
        }
        public string Help
        {
            get { return "renvoie conbien tu as sur ton compte"; }
        }
        public string Syntax
        {
            get { return string.Empty; }
        }
        public List<string> Aliases
        {
            get
            {
                return new List<string>()
                {
                    "bbal",
                    "bb"
                };
            }
        }
        public List<string> Permissions
        {
            get
            {
                return new List<string>()
                {
                    "bbank_player"
                };
            }
        }
        public void Execute(IRocketPlayer caller, params string[] command)
        {
            UnturnedPlayer joueur = (UnturnedPlayer)caller;
            if (command.Length != 1)
            {
                if (!(BinaryBank.Instance.BinaryBankDatabase.CheckIfExist(joueur.Id)))
                {
                    string value = BinaryBank.Instance.BinaryBankDatabase.VoirBalance(joueur.Id);
                    if (value != "error")
                    {
                        UnturnedChat.Say(joueur, "Tu as actuellement " + value + " dans ta BinaryBank", Color.magenta);

                    }
                }
                else
                {
                    UnturnedChat.Say(joueur, "Askip t'a pas de compte", Color.red);
                }
                
            }
            else
            {
                UnturnedChat.Say(joueur, "Verifie la syntaxe", Color.red);
            }
        }
    }
}
