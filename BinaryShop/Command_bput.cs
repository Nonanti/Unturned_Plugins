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
    public class Command_bput : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }
        public string Name
        {
            get { return "bput"; }
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
            int prix = 0;
            ushort id = 0;
            if (command.Length != 3)
            {
                UnturnedChat.Say(player, "Verifie la syntaxe", Color.red);
                return;
            }
            else
            {
                if (!int.TryParse(command[1], out prix))
                {
                    UnturnedChat.Say(player, "le prix renseigné est invalide", Color.magenta);
                    return;
                }
                if (!int.TryParse(command[2], out amt))
                {
                    UnturnedChat.Say(player, "l'amount renseigné est invalide", Color.magenta);
                    return;
                }
                if (!ushort.TryParse(command[0],out id)){
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
                if (!(prix > 0))
                {
                    UnturnedChat.Say(player, "Le prix choisi est invalide", Color.red);
                    return;
                }

                if (BBank.Instance.BinaryBankDatabase.CheckIfExist(player.Id))
                {
                    UnturnedChat.Say(player, "Tu n'as pas de compte BinaryBank (/bcreate)", Color.red);
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
                        int amtmem = amt;
                        while (amt > 0)
                        {
                            player.Inventory.removeItem(list[0].page, player.Inventory.getIndex(list[0].page, list[0].jar.x, list[0].jar.y));
                            list.RemoveAt(0);
                            amt--;
                        }
                        if (BinaryShop.Instance.BinaryShopDatabase.PutItemIntoMarche(id, asset.name, player.Id, player.SteamName, amtmem, prix, asset.type, asset.rarity)){
                            UnturnedChat.Say(player.CharacterName + " a mis en vente "+ amtmem + " "+asset.name+" pour "+prix+" BinaryCoin, Allez chequez ça sur le marché", Color.yellow);
                        }
                    }
                }
            }
        }
    }
}