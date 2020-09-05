using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace BinaryBank
{
    public class Command_bcreate : IRocketCommand
    {
        public BinaryBankDatabase BinaryBankDatabase;
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "bcreate"; }
        }
        public string Help
        {
            get { return "creer votre compte bancaire"; }
        }
        public string Syntax
        {
            get { return "/bcreate"; }
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
            if (command.Length > 0)
            {
                UnturnedChat.Say(joueur, "Verifie la syntaxe", Color.red);
            }
            else
            {

                if (BinaryBank.Instance.BinaryBankDatabase.CheckIfExist(joueur.Id))
                {
                    if (BinaryBank.Instance.BinaryBankDatabase.CreateAcount(joueur.Id, joueur.SteamName))
                    {
                        UnturnedChat.Say(joueur, "Votre compte a bien été créé !!", Color.green);
                    }
                    else
                    {
                        UnturnedChat.Say(joueur, "Une erreur DE MERDE est survenue (encore) !!", Color.red);
                    }
                }
                else
                {
                    UnturnedChat.Say(joueur, "Vous avez déja un compte !!", Color.red);
                }

            }
        }
    }
}
