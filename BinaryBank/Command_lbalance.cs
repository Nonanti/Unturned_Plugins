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
    public class Command_lbalance : IRocketCommand
    {
        public BinaryBankDatabase BinaryBankDatabase;
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "lbalance"; }
        }
        public string Help
        {
            get { return "renvoie conbien tu as sur ton livret A"; }
        }
        public string Syntax
        {
            get { return string.Empty; }
        }
        public List<string> Aliases
        {
            get { return new List<string>(); }
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
                if (!(BinaryBank.Instance.BinaryBankDatabase.CheckIfExistLivret(BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id))))
                {
                    string value = BinaryBank.Instance.BinaryBankDatabase.VoirBalanceLivret(BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id));
                    if (value != "error")
                    {
                        UnturnedChat.Say(joueur, "Tu as actuellement " + value + " sur ton Livret A", Color.magenta);

                    }
                }
                else
                {
                    UnturnedChat.Say(joueur, "Askip t'a pas de livret", Color.red);
                }
                
            }
            else
            {
                UnturnedChat.Say(joueur, "Verifie la syntaxe", Color.red);
            }
        }
    }
}
