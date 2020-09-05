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

namespace BinaryShop
{
    public class Command_bbuy : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "bbuy"; }
        }
        public string Help
        {
            get { return "achete un objet sur le BShop"; }
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
            
            ItemAsset asset = null;
            byte amt = 1;
            ushort id = 0;
            
            if (command.Length > 1)
            {
                if (!byte.TryParse(command[1], out amt))
                {
                    UnturnedChat.Say(player, "l'amount renseigné est invalide", Color.magenta);
                    return;
                }
            }
            if (command.Length < 1 || command.Length > 2)
            {
                UnturnedChat.Say(player, "Verifie la syntaxe", Color.red);
                return;
            }
            else
            {
                if(!ushort.TryParse(command[0],out id)){
                    foreach (ItemAsset vAsset in Assets.find(EAssetType.ITEM))
                    {

                        if (vAsset?.itemName != null && vAsset.itemName.ToLower().Contains(command[0].ToLower()))
                        {
                            asset = vAsset;
                            id = vAsset.id;
                        }
                    }
                }
                if (BBank.Instance.BinaryBankDatabase.CheckIfExist(player.Id))
                {
                    UnturnedChat.Say(player, "Tu n'as pas de compte BinaryBank", Color.red);
                }
                else
                {
                    if (!BinaryShop.Instance.BinaryShopDatabase.CheckIfExist(id))
                    {
                        UnturnedChat.Say(player, "L'item renseigné n'est pas a acheter sur le BinaryShop", Color.red);
                        return;
                    }
                    else
                    {
                        if (!(BinaryShop.Instance.BinaryShopDatabase.VoirPrix(id) == 0))
                        {
                            int prix = (int)System.Math.Round(BinaryShop.Instance.BinaryShopDatabase.VoirPrix(id));
                            float vraiprix = prix * amt;
                            if (vraiprix > int.Parse(BBank.Instance.BinaryBankDatabase.VoirBalance(player.Id)))
                            {
                                UnturnedChat.Say(player, "Tu as pas assez sur ton compte BinaryBank", Color.red);
                                return;
                            }
                            else
                            {
                                if (!player.GiveItem(id, amt))
                                {
                                    UnturnedChat.Say(player, "Erreur lors du give", Color.red);
                                    return;
                                }
                                else
                                {
                                    BBank.Instance.BinaryBankDatabase.ExportExperience(player.Id, vraiprix.ToString());
                                    UnturnedChat.Say(player, "tu viens de perdre : " + vraiprix + " de ta BinaryBank", Color.magenta);
                                    UnturnedChat.Say(player, "La transaction a bien été effectué", Color.green);
                                }
                            }

                        }
                        else
                        {
                            UnturnedChat.Say(player, "Erreur sur VoirPrix", Color.red);
                            return;
                        }
                    }
                }   
            }
        }
    }
}