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

    public class Command_btran : IRocketCommand
    {
        public BinaryBankDatabase BinaryBankDatabase;
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "btransaction"; }
        }
        public string Help
        {
            get { return "imorte de l'argent sur le compte d'une personne"; }
        }
        public string Syntax
        {
            get { return "/btransaction <rib> <amount>"; }
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
            if (command.Length != 2)
            {
                UnturnedChat.Say(joueur, "Verifie la syntaxe", Color.red);
            }
            else
            {
                if (!(BinaryBank.Instance.BinaryBankDatabase.CheckIfExist(joueur.Id)))
                {
                    if(!(BinaryBank.Instance.BinaryBankDatabase.CheckIfRIBExist(command[0])))
                    {
                        if (joueur.Experience >= int.Parse(command[1]) && int.Parse(command[1]) > 1)
                        {
                            if (BinaryBank.Instance.BinaryBankDatabase.ImortExportWithRib(joueur.Id,command[0],command[1]))
                            {
                                DateTime time = DateTime.Now;
                                UnturnedChat.Say(joueur, "Somme corectement importée", Color.green);
                                joueur.Experience -= uint.Parse(command[1]);
                                /*UnturnedPlayer beneficiaire = UnturnedPlayer.FromName(BinaryBank.Instance.BinaryBankDatabase.VoirName(command[0]));
                                UnturnedChat.Say(beneficiaire, "Tu as reçu "+command[1]+" de "+ joueur.SteamName, Color.green);*/
                                BinaryBank.Instance.BinaryBankDatabase.HistoryLog(time.ToString(), "Transaction", joueur.SteamName, BinaryBank.Instance.BinaryBankDatabase.VoirName(command[0]), command[1]);

                            }
                            else
                            {
                                UnturnedChat.Say(joueur, "Une erreur est survenue lors de l'importation", Color.red);
                            }
                        }
                        else
                        {
                            UnturnedChat.Say(joueur, "Somme improbable (soit t'a pas assez soit tu essaye de faire bugger mon plugin)", Color.red);
                        }
                    }
                    else
                    {
                        UnturnedChat.Say(joueur, "Le rib renseigné ne correspond a aucun compte", Color.red);
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
