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

    public class Command_limport : IRocketCommand
    {
        public BinaryBankDatabase BinaryBankDatabase;
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "limport"; }
        }
        public string Help
        {
            get { return "imorte de l'argent sur ton Livret A"; }
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
                    if (joueur.Experience >= int.Parse(command[0]) && int.Parse(command[0]) > 0 && (int.Parse(command[0])+int.Parse(BinaryBank.Instance.BinaryBankDatabase.VoirBalanceLivret(BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id)))<=2000))
                    {
                        if (BinaryBank.Instance.BinaryBankDatabase.ImortExperienceLivret(BinaryBank.Instance.BinaryBankDatabase.VoirRib(joueur.Id), command[0]))
                        {
                            DateTime time = DateTime.Now;
                            UnturnedChat.Say(joueur, "Somme corectement importée", Color.green);
                            joueur.Experience -= uint.Parse(command[0]);
                            BinaryBank.Instance.BinaryBankDatabase.HistoryLog(time.ToString(),"Import",joueur.SteamName, "Livret A",command[0]);

                        }
                        else
                        {
                            UnturnedChat.Say(joueur, "Une erreur est survenue lors de l'importation", Color.red);
                        }
                    }
                    else
                    {
                        UnturnedChat.Say(joueur, "Somme improbable verifie ton plafond", Color.red);
                    }
                    
                }
                else
                {
                    UnturnedChat.Say(joueur, "tu n'as pas créer de livret", Color.red);
                }
            }

        }
    }
}
