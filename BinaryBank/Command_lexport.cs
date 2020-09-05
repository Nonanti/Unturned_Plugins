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
    public class Command_lexport : IRocketCommand
    {
        public BinaryBankDatabase BinaryBankDatabase;
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "lexport"; }
        }
        public string Help
        {
            get { return "exporte de l'argent de ton Livret A"; }
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
                UnturnedChat.Say(joueur, "Verifie la syntaxe", Color.red);
            }
            else
            {
                if (!(BinaryBank.Instance.BinaryBankDatabase.CheckIfExistLivret(BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id))))
                {
                    if (int.Parse(BinaryBank.Instance.BinaryBankDatabase.VoirBalanceLivret(BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id))) >= int.Parse(command[0]) && int.Parse(command[0]) > 0)
                        {
                        if (BinaryBank.Instance.BinaryBankDatabase.ExportExperienceLivret(BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id), command[0]))
                        {
                            DateTime time = DateTime.Now;
                            UnturnedChat.Say(joueur, "Somme corectement exportée", Color.green);
                            joueur.Experience += uint.Parse(command[0]);
                            BinaryBank.Instance.BinaryBankDatabase.HistoryLog(time.ToString(), "Export", "Livret A", joueur.SteamName, command[0]);
                        }
                        else
                        {
                            UnturnedChat.Say(joueur, "Une erreur est survenue lors de l'exportation", Color.red);
                        }
                    }
                    else
                    {
                        UnturnedChat.Say(joueur, "Somme improbable (soit t'a pas assez soit tu essaye de faire bugger mon plugin)", Color.red);
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
