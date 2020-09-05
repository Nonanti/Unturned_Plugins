using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger = Rocket.Core.Logging.Logger;
using UnityEngine;

namespace BinaryBank
{
    public class Command_rib : IRocketCommand
    {
        public BinaryBankDatabase BinaryBankDatabase;
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "brib"; }
        }
        public string Help
        {
            get { return "affiche votre rib"; }
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
            if (command.Length != 0)
            {
                UnturnedChat.Say(joueur, "Verifie la syntaxe", Color.red);
            }
            else
            {
                if (!(BinaryBank.Instance.BinaryBankDatabase.CheckIfExist(joueur.Id)))
                {
                    UnturnedChat.Say(joueur, "Ton Rib est : "+ BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id), Color.magenta);
                }
                else
                {
                    UnturnedChat.Say(joueur, "Askip t'a pas de compte", Color.red);
                }
            }

        }
    }
}
