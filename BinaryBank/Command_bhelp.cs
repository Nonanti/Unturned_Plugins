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
    public class Command_bhelp : IRocketCommand
    {
        public BinaryBankDatabase BinaryBankDatabase;
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "bhelp"; }
        }
        public string Help
        {
            get { return "affiche le menu d'aide"; }
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
                UnturnedChat.Say(joueur, "Vous pouvez tapez [/bhelp syntaxe] pour avoir la liste des syntaxe", Color.cyan);
                UnturnedChat.Say(joueur, "Vous pouvez tapez [/bhelp description] pour avoir les descriptions des commandes", Color.cyan);
                UnturnedChat.Say(joueur, "/bbalance", Color.cyan);
                UnturnedChat.Say(joueur, "/bcreate", Color.cyan);
                UnturnedChat.Say(joueur, "/bexport", Color.cyan);
                UnturnedChat.Say(joueur, "/bhelp", Color.cyan);
                UnturnedChat.Say(joueur, "/bimport", Color.cyan);
                UnturnedChat.Say(joueur, "/btransaction", Color.cyan);
                UnturnedChat.Say(joueur, "/brib", Color.cyan);
            }
            else if (command.Length == 1)
            {
                if(command[0] == "syntaxe")
                {
                    UnturnedChat.Say(joueur, "/bbalance", Color.cyan);
                    UnturnedChat.Say(joueur, "/bcreate", Color.cyan);
                    UnturnedChat.Say(joueur, "/bexport <binarycoin_amount>", Color.cyan);
                    UnturnedChat.Say(joueur, "/bhelp <syntaxe|description>", Color.cyan);
                    UnturnedChat.Say(joueur, "/bimport <xp_amount>", Color.cyan);
                    UnturnedChat.Say(joueur, "/btransaction <RIB_beneficiaire> <binarycoin_amount>", Color.cyan);
                    UnturnedChat.Say(joueur, "/brib", Color.cyan);
                }
                else if (command[0] == "description")
                {
                    UnturnedChat.Say(joueur, "/bbalance : affiche le nombre de binarycoin que vous avez dans votre binarybanque", Color.cyan);
                    UnturnedChat.Say(joueur, "/bcreate : crée votre compte bancaire si il n'héxiste pas", Color.cyan);
                    UnturnedChat.Say(joueur, "/bexport : converti une somme donnée de binarycoin en experience", Color.cyan);
                    UnturnedChat.Say(joueur, "/bhelp : affiche le menu d'aide", Color.cyan);
                    UnturnedChat.Say(joueur, "/bimport : converti une somme donnée d'experience en binarycoin", Color.cyan);
                    UnturnedChat.Say(joueur, "/btransaction : permet de donner de l'argent a une personne si vous connaissez son RIB", Color.cyan);
                    UnturnedChat.Say(joueur, "/brib : affiche votre RIB", Color.cyan);
                }
                else
                {
                    UnturnedChat.Say(joueur, "Verifie la syntaxe", Color.red);
                }
            }
            else
            {
                UnturnedChat.Say(joueur, "Verifie la syntaxe", Color.red);
            }
            
        }
    }
}
