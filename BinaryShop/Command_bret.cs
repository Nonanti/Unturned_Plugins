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
    public class Command_bret : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "bret"; }
        }
        public string Help
        {
            get { return "Retire ce que vous avez acheté sur le BinaryShop"; }
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
                ushort[] ID = BinaryShop.Instance.BinaryShopDatabase.VoirID(player.Id);
                byte[] Number= BinaryShop.Instance.BinaryShopDatabase.VoirNumber(player.Id);
                if (ID[0] == 0)
                {
                    UnturnedChat.Say(player, "Vous n'avez riena récupérer", Color.red);
                }
                else
                {
                    for (int i = 0; ID[i] != 0; i++)
                    {
                        UnturnedChat.Say(player, "L'item est" + ID[i], Color.magenta);
                        if (!player.GiveItem(ID[i], Number[i]))
                        {
                            UnturnedChat.Say(player, "Erreur lors du give", Color.red);
                            return;
                        }
                    }
                    if (!BinaryShop.Instance.BinaryShopDatabase.ClearPanier(player.Id))
                    {
                        UnturnedChat.Say(player, "Erreur lors du ClearPanier", Color.red);
                        return;
                    }
                    UnturnedChat.Say(player, "Vous avez correctement reçu vos items", Color.green);
                }
                
            }
        }
    }
}