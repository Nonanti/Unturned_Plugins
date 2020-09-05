using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using System;
using System.Data;
using I18N.Common;
using Steamworks;
using SDG.Unturned;

namespace BinaryShop
{
    public class BinaryShopDatabase
    {
        internal BinaryShopDatabase()
        {
            new I18N.West.CP1250();
        }
        public MySqlConnection CreateConnection()
        {
            MySqlConnection connection = null;
            try
            {
                if (BinaryShop.Instance.Configuration.Instance.BinaryShopDatabase.DatabasePort == 0) BinaryShop.Instance.Configuration.Instance.BinaryShopDatabase.DatabasePort = 3306;
                connection = new MySqlConnection(String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};", BinaryShop.Instance.Configuration.Instance.BinaryShopDatabase.DatabaseAddress, BinaryShop.Instance.Configuration.Instance.BinaryShopDatabase.DatabaseName, BinaryShop.Instance.Configuration.Instance.BinaryShopDatabase.DatabaseUsername, BinaryShop.Instance.Configuration.Instance.BinaryShopDatabase.DatabasePassword, BinaryShop.Instance.Configuration.Instance.BinaryShopDatabase.DatabasePort));
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
            return connection;
        }

        public bool testconnection()
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return false;
            }
        }
        public bool CheckIfExist(uint ID)
        {
            try
            {
                int exists = 0;
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM binaryshop.item WHERE ID=" + ID, connection);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null) Int32.TryParse(result.ToString(), out exists);

                    if (exists == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return false;
            }
        }
        public float VoirPrix(uint ID)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM binaryshop.item WHERE ID=@ID", connection);
                connection.Open();
                float Amount = 0 ;
                if (connection.State == ConnectionState.Open)
                {

                    cmd.Parameters.AddWithValue("@ID", ID);
                    using (MySqlDataReader Lire = cmd.ExecuteReader())
                    {

                        while (Lire.Read())
                        {
                            Amount = float.Parse(Lire["Prix"].ToString());
                        }
                        return Amount;
                    }
                }
                else
                {
                    return Amount;
                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return 0;
            }
        }
        public bool BourseDown(uint ID)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("UPDATE binaryshop.item SET Prix=Prix/TauxDown WHERE ID=@ID", connection);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return false;
            }
        }
        public ushort[] VoirID(string ID)
        {
            ushort[] iDs = new ushort[100] ;
            iDs[0]= 0;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM binaryshop.panier WHERE SteamID=@ID AND Status='OK'", connection);
                connection.Open();
                int i = 0;
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    using (MySqlDataReader Lire = cmd.ExecuteReader())
                    {
                        while (Lire.Read())
                        {
                            iDs[i] = ushort.Parse(Lire["ItemID"].ToString());
                            i++;
                        }
                        return iDs;
                    }
                }
                else
                {
                    return iDs;
                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return iDs;
            }
        }
        public byte[] VoirNumber(string ID)
        {
            byte[] Number = new byte[100];
            Number[0] = 0;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM binaryshop.panier WHERE SteamID=@ID AND Status='OK'", connection);
                connection.Open();
                int i = 0;
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    using (MySqlDataReader Lire = cmd.ExecuteReader())
                    {
                        while (Lire.Read())
                        {
                            Number[i] = byte.Parse(Lire["Number"].ToString());
                            i++;
                        }
                        return Number;
                    }
                }
                else
                {
                    return Number;
                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return Number;
            }
        }
        public bool ClearPanier(string ID)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM binaryshop.panier WHERE panier.SteamID=@ID AND panier.Status= 'OK'", connection);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return false;
            }
        }
        public bool PutItemIntoMarche(uint itemID, string itemName, string SteamID, string VendeurName, int Number, int prix, EItemType type, EItemRarity rarity)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO binaryshop.marchecomu (`ItemID`, `ItemName`, `SteamID`, `VendeurName`, `Number`, `Prix`, `Type`, `Rarity`) VALUES (@ItemID, @ItemName,@SteamID,@VendeurName,@Number,@Prix,@Type,@Rarity)", connection);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@ItemID", itemID);
                    cmd.Parameters.AddWithValue("@ItemName", itemName);
                    cmd.Parameters.AddWithValue("@SteamID", SteamID);
                    cmd.Parameters.AddWithValue("@VendeurName", VendeurName);
                    cmd.Parameters.AddWithValue("@Number", Number);
                    cmd.Parameters.AddWithValue("@Prix", prix);
                    cmd.Parameters.AddWithValue("@Type", type.ToString());
                    cmd.Parameters.AddWithValue("@Rarity", rarity.ToString());
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return false;
            }
        }
    }
}