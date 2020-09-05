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
    public class Command_linfos : IRocketCommand
    {
        public BinaryBankDatabase BinaryBankDatabase;
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "linfos"; }
        }
        public string Help
        {
            get { return "renvoie des infos sur ton livret A"; }
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
            if (command.Length == 0)
            {
                if (!(BinaryBank.Instance.BinaryBankDatabase.CheckIfExistLivret(BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id))))
                {
                    string taux = BinaryBank.Instance.BinaryBankDatabase.VoirTaux(BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id));
                    string Plafond = BinaryBank.Instance.BinaryBankDatabase.VoirPlafond(BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id));
                    if (taux != "error" && Plafond != "error")
                    {
                        UnturnedChat.Say(joueur, "Ton Taux est de : x" + taux + "/h et ton plafond est de : " + Plafond, Color.magenta);

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
