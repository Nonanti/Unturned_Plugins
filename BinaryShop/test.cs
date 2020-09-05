using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions.Must;
using BBank = BinaryBank.BinaryBank;

namespace Radar
{
    public class Command_test : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "test"; }
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
                UnturnedChat.Say(joueur, "Ta position est" + joueur.Position, Color.magenta);
            }

        }
    }
}
