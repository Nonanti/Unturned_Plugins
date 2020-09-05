using System;
using System.Collections.Generic;
using Rocket.API;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;
using BBank = BinaryBank.BinaryBank;
using System.Data;
using System.IO.IsolatedStorage;

namespace BinaryShop
{
    public class Command_bmdp : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "bmdp"; }
        }
        public string Help
        {
            get { return "Montre votre mot de passe ou en génère un si vous n'en avez pas"; }
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
                    "bshop_player"
                };
            }
        }

        public void Execute(IRocketPlayer caller, params string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length !=0)
            {
                UnturnedChat.Say(player, "Verifie la syntaxe", Color.red);
                return;
            }
            else
            {
                if (!BBank.Instance.BinaryBankDatabase.CheckIfExist(player.Id))
                {
                    string mdp_actuel = BBank.Instance.BinaryBankDatabase.VoirMdp(player.Id);
                    if (mdp_actuel == "0")
                    {
                        var rand = new System.Random();
                        string rib = BBank.Instance.BinaryBankDatabase.VoirRib(player.Id);
                        if (int.TryParse(rib.Substring(0, 1), out int one))
                        {
                            if (int.TryParse(rib.Substring(1, 1), out int two))
                            {
                                if (int.TryParse(rib.Substring(2, 1), out int three))
                                {
                                    int un_truc = rand.Next(10, 99);
                                    int un_autre_truc = rand.Next(100, 999);
                                    string mdp = one.ToString() + un_truc.ToString() + un_autre_truc.ToString() + three.ToString() + two.ToString();
                                    if (BBank.Instance.BinaryBankDatabase.SetMpd(player.Id, mdp))
                                    {
                                        UnturnedChat.Say(player, "Ton mdp web est : " + mdp + ". Garde le bien précieusement", Color.magenta);
                                    }
                                    else
                                    {
                                        UnturnedChat.Say(player, "Erreur lors de l'ajout du MDP dans la BDD", Color.red);
                                    }                                
                                }
                            }
                        }
                    }
                    else
                    {
                        UnturnedChat.Say(player, "Ton mdp web est : " + mdp_actuel + ".", Color.magenta);
                    } 
                }
                else
                {
                    UnturnedChat.Say(player,"pas de compte bancaire",Color.red);
                }
            }
        }
    }
}