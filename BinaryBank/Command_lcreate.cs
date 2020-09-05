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
    public class Command_lcreate : IRocketCommand
    {
        public BinaryBankDatabase BinaryBankDatabase;
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "lcreate"; }
        }
        public string Help
        {
            get { return "creer votre livret A"; }
        }
        public string Syntax
        {
            get { return "/lcreate"; }
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
                if (!BinaryBank.Instance.BinaryBankDatabase.CheckIfExist(joueur.Id))
                {
                    if (BinaryBank.Instance.BinaryBankDatabase.CheckIfExistLivret(BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id)))
                    {
                        if (BinaryBank.Instance.BinaryBankDatabase.CreateLivretA(BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id),joueur.SteamName))
                        {
                            UnturnedChat.Say(joueur, "Votre Livret A a bien été créé !!", Color.green);
                        }
                        else
                        {
                            UnturnedChat.Say(joueur, "Une erreur DE MERDE est survenue (encore) !!", Color.red);
                        }
                    }
                    else
                    {
                        UnturnedChat.Say(joueur, "Vous avez déja un livret !!", Color.red);
                    }
                }
                else
                {
                    UnturnedChat.Say(joueur, "Askip t'a pas de compte", Color.red);
                }
            }
        }
    }
}
