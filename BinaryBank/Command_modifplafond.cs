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
    public class Command_modifplafond : IRocketCommand
    {
        public BinaryBankDatabase BinaryBankDatabase;
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Both; }
        }
        public string Name
        {
            get { return "modifplafond"; }
        }
        public string Help
        {
            get { return "modifie le plafond d'un utilisateur"; }
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
                    "bbank_banquier"
                };
            }
        }
        public void Execute(IRocketPlayer caller, params string[] command)
        {
            if (command.Length == 2)
            {
                if (!(BinaryBank.Instance.BinaryBankDatabase.CheckIfExistLivret(command[0])))
                {
                    if (float.Parse(command[1]) > 0 && int.Parse(command[1]) >= int.Parse(BinaryBank.Instance.BinaryBankDatabase.VoirBalanceLivret(command[0])))
                    {
                        if(BinaryBank.Instance.BinaryBankDatabase.ModifPlafond(command[0], command[1]))
                        {
                            UnturnedChat.Say(caller, "La modification a été corectement effectuée", Color.green);
                        }
                        else
                        {
                            UnturnedChat.Say(caller, "Une erreur est survenue lors de la modification", Color.red);
                        }
                    }
                    else
                    {
                        UnturnedChat.Say(caller, "Le plafond chois est improbable", Color.red);
                    }
                }
                else
                {
                    UnturnedChat.Say(caller, "Le joueurs choisi n' pas de livret A", Color.red);
                }

            }
            else
            {
                UnturnedChat.Say(caller, "Verifie la syntaxe", Color.red);
            }
        }
    }
}
