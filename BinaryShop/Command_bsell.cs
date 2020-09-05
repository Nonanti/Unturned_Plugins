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

namespace BinaryShop
{
    public class Command_bsell : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "bsell"; }
        }
        public string Help
        {
            get { return "vend un objet sur le BShop"; }
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
            int amt = 1;
            ushort id = 0;
            if (command.Length > 1)
            {
                if (!int.TryParse(command[1], out amt))
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
                else
                {
                    foreach (ItemAsset vAsset in Assets.find(EAssetType.ITEM))
                    {

                        if (vAsset?.itemName != null && vAsset.id == id)
                        {
                            asset = vAsset;
                            id = vAsset.id;
                        }
                    }
                }
                List<InventorySearch> list = player.Inventory.search(id, true, true);

                if (!(list.Count >= amt))
                {
                    UnturnedChat.Say(player, "Tu n'as pas assez de cet item dans ton inventaire", Color.red);
                    return;
                }

                if (BBank.Instance.BinaryBankDatabase.CheckIfExist(player.Id))
                {
                    UnturnedChat.Say(player, "Tu n'as pas de compte BinaryBank", Color.red);
                }
                else
                {
                    if (player.Inventory.has(id) == null)
                    {
                        UnturnedChat.Say(player, "Tu n'as pas cet item dans ton inventaire", Color.red);
                        return;
                    }
                    else
                    {
                        if (BinaryShop.Instance.BinaryShopDatabase.CheckIfExist(id))
                        {
                            if (!(BinaryShop.Instance.BinaryShopDatabase.VoirPrix(id) == 0))
                            {
                                float prix = BinaryShop.Instance.BinaryShopDatabase.VoirPrix(id);
                                int vraiprix = (int)System.Math.Round(prix);
                                int prix_reel = amt * vraiprix;
                                while (amt > 0)
                                {
                                    if (BBank.Instance.BinaryBankDatabase.ImortExperience(player.Id, vraiprix.ToString()))
                                    {
                                        player.Inventory.removeItem(list[0].page, player.Inventory.getIndex(list[0].page, list[0].jar.x, list[0].jar.y));
                                        list.RemoveAt(0);
                                        if (!(BinaryShop.Instance.BinaryShopDatabase.BourseDown(id)))
                                        {
                                            UnturnedChat.Say("Erreur au niveau de la bourse", Color.red);
                                            return;
                                        }
                                        amt--;
                                    }
                                    else
                                    {
                                        UnturnedChat.Say("Erreur de merde", Color.red);
                                        return;
                                    }
                                }
                                UnturnedChat.Say("La valeur boursière de " + asset.name + " a changé", Color.magenta);
                                UnturnedChat.Say(player, "L'objet " + asset.name + " a bien été vendu, tu as reçu " + prix_reel + " sur ton compte BinaryBank", Color.green);
                            }
                            else
                            {
                                UnturnedChat.Say(player, "Erreur sur VoirPrix", Color.red);
                                return;
                            }


                        }
                        else
                        {
                            UnturnedChat.Say(player, "L'item n'est pas en vente !", Color.red);
                            return;
                        }

                    }

                }
            }
        }
    }
}