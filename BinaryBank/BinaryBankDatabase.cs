using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using System;
using System.Data;
using I18N.Common;
using Steamworks;

namespace BinaryBank
{
    public class BinaryBankDatabase
    {
        internal BinaryBankDatabase()
        {
            new I18N.West.CP1250();
        }
        public MySqlConnection CreateConnection()
        {
            MySqlConnection connection = null;
            try
            {
                if (BinaryBank.Instance.Configuration.Instance.BinaryBankDatabase.DatabasePort == 0) BinaryBank.Instance.Configuration.Instance.BinaryBankDatabase.DatabasePort = 3306;
                connection = new MySqlConnection(String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};", BinaryBank.Instance.Configuration.Instance.BinaryBankDatabase.DatabaseAddress, BinaryBank.Instance.Configuration.Instance.BinaryBankDatabase.DatabaseName, BinaryBank.Instance.Configuration.Instance.BinaryBankDatabase.DatabaseUsername, BinaryBank.Instance.Configuration.Instance.BinaryBankDatabase.DatabasePassword, BinaryBank.Instance.Configuration.Instance.BinaryBankDatabase.DatabasePort));
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return connection ;
            }
            return connection;

        }
        public bool CheckIfExist(string ID)
        {
            try
            {
                int exists = 0;
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM bank_accounts WHERE SteamID=" + ID;

                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    object result = command.ExecuteScalar();
                    if (result != null) Int32.TryParse(result.ToString(), out exists);

                    if (exists == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
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
        public bool CheckIfRIBExist(string RIB)
        {
            try
            {
                int exists = 0;
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM bank_accounts WHERE ID=" + int.Parse(RIB);

                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    object result = command.ExecuteScalar();
                    if (result != null) Int32.TryParse(result.ToString(), out exists);

                    if (exists == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
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
        public bool CreateAcount(string ID, string NAME)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO bank_accounts(SteamID,Amount,Name) VALUES(@SteamID,0,@Name)", connection);
                MySqlCommand add_event = new MySqlCommand("CREATE DEFINER=`root`@`localhost` EVENT `"+ NAME +"` ON SCHEDULE EVERY 10 MINUTE STARTS '2020-06-10 00:00:00' ON COMPLETION NOT PRESERVE ENABLE DO INSERT INTO `account_log`( account_log.SteamID, account_log.Montant, account_log.Time) VALUES (@SteamID, (SELECT bank_accounts.Amount FROM `bank_accounts` WHERE bank_accounts.SteamID=@SteamID), (SELECT CURRENT_TIME()))", connection);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@Name", NAME);
                    cmd.Parameters.AddWithValue("@SteamID", ID);
                    add_event.Parameters.AddWithValue("@Name", NAME);
                    add_event.Parameters.AddWithValue("@SteamID", ID);
                    cmd.ExecuteNonQuery();
                    add_event.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    add_event.Parameters.Clear();
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
        public bool ImortExperience(string ID, string Amount)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("UPDATE bank_accounts SET Amount=Amount+@Amount WHERE SteamID=@SteamID", connection);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@Amount", int.Parse(Amount));
                    cmd.Parameters.AddWithValue("@SteamID", ID);
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
        public string VoirBalance(string ID)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM bank_accounts WHERE SteamID=@SteamID", connection);
                connection.Open();
                string Amount = "error";
                if (connection.State == ConnectionState.Open)
                {

                    cmd.Parameters.AddWithValue("@SteamID", ID);
                    using (MySqlDataReader Lire = cmd.ExecuteReader())
                    {

                        while (Lire.Read())
                        {
                            Amount = Lire["Amount"].ToString();
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
                return "error";
            }
        }
        public string VoirRib(string ID)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM bank_accounts WHERE SteamID=@SteamID", connection);
                connection.Open();
                string rib = "error";
                if (connection.State == ConnectionState.Open)
                {

                    cmd.Parameters.AddWithValue("@SteamID", ID);
                    using (MySqlDataReader Lire = cmd.ExecuteReader())
                    {

                        while (Lire.Read())
                        {
                            rib = Lire["ID"].ToString();
                        }
                        return rib;
                    }
                }
                else
                {
                    return rib;
                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return "error";
            }
        }
        public string VoirName(string RIB)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM bank_accounts WHERE ID=@ID", connection);
                connection.Open();
                string name = "error";
                if (connection.State == ConnectionState.Open)
                {

                    cmd.Parameters.AddWithValue("@ID", int.Parse(RIB));
                    using (MySqlDataReader Lire = cmd.ExecuteReader())
                    {

                        while (Lire.Read())
                        {
                            name = Lire["Name"].ToString();
                        }
                        return name;
                    }
                }
                else
                {
                    return name;
                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return "error";
            }
        }
        public bool ExportExperience(string ID, string Amount)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("UPDATE bank_accounts SET Amount=Amount-@Amount WHERE SteamID=@SteamID", connection);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@Amount", uint.Parse(Amount));
                    cmd.Parameters.AddWithValue("@SteamID", ID);
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
        public bool ImortExportWithRib(string ID, string RIB, string Amount)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("UPDATE bank_accounts SET Amount=Amount+@Amount WHERE ID=@ID", connection);
                MySqlCommand cmd2 = new MySqlCommand("UPDATE bank_accounts SET Amount=Amount-@Amount WHERE SteamID=@SteamID", connection);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd2.Parameters.AddWithValue("@SteamID", ID);
                    cmd2.Parameters.AddWithValue("@Amount", int.Parse(Amount));
                    cmd.Parameters.AddWithValue("@Amount", int.Parse(Amount));
                    cmd.Parameters.AddWithValue("@ID", int.Parse(RIB));
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    cmd2.ExecuteNonQuery();
                    cmd2.Parameters.Clear();
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
        public bool HistoryLog(string Date, string Type, string Src, string Dst, string Montant)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO history(Date,Type,Source,Destination,Montant) VALUES(@Date,@Type,@Source,@Destination,@Montant)", connection);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@Date", Date);
                    cmd.Parameters.AddWithValue("@Type", Type);
                    cmd.Parameters.AddWithValue("@Source", Src);
                    cmd.Parameters.AddWithValue("@Destination", Dst);
                    cmd.Parameters.AddWithValue("@Montant", int.Parse(Montant));
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
        public string VoirMdp(string ID)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM bank_accounts WHERE SteamID=@SteamID", connection);
                connection.Open();
                string Pass = "error";
                if (connection.State == ConnectionState.Open)
                {

                    cmd.Parameters.AddWithValue("@SteamID", ID);
                    using (MySqlDataReader Lire = cmd.ExecuteReader())
                    {

                        while (Lire.Read())
                        {
                            Pass = Lire["Pass"].ToString();
                        }
                        return Pass;
                    }
                }
                else
                {
                    return Pass;
                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return "error";
            }
        }
        public bool SetMpd(string ID, string Mdp)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("UPDATE bank_accounts SET Pass=@Pass WHERE SteamID=@SteamID", connection);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@Pass", Mdp);
                    cmd.Parameters.AddWithValue("@SteamID", ID);
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


        public string VoirBalanceLivret(string ID)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM livreta WHERE ID=@ID", connection);
                connection.Open();
                string Amount = "error";
                if (connection.State == ConnectionState.Open)
                {

                    cmd.Parameters.AddWithValue("@ID", int.Parse(ID));
                    using (MySqlDataReader Lire = cmd.ExecuteReader())
                    {

                        while (Lire.Read())
                        {
                            Amount = Lire["Montant"].ToString();
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
                return "error";
            }
        }
        public bool CheckIfExistLivret(string ID)
        {
            try
            {
                int exists = 0;
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM livreta WHERE ID=" + ID;

                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    object result = command.ExecuteScalar();
                    if (result != null) Int32.TryParse(result.ToString(), out exists);

                    if (exists == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
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
        public bool CreateLivretA(string ID, string Name)
        {
            string EventName = "LivretA" + Name;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO livreta(ID,Montant) VALUES(@ID,0)", connection);
                MySqlCommand add_event = new MySqlCommand("CREATE DEFINER=`root`@`localhost` EVENT `" + EventName + "` ON SCHEDULE EVERY 1 HOUR STARTS '2020-05-30 14:00:00' ON COMPLETION NOT PRESERVE ENABLE DO INSERT INTO `history`(history.Date,history.Type,history.Source,history.Destination,history.Montant) VALUES(CURRENT_TIMESTAMP,'Livret Virement','Livret A',(SELECT bank_accounts.Name FROM `bank_accounts` WHERE bank_accounts.ID = @Rib),(SELECT livreta.Montant*livreta.Taux - livreta.Montant FROM `livreta` WHERE livreta.ID = @Rib && livreta.Montant = livreta.Plafond && livreta.Online = 1))", connection);

                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@ID", int.Parse(ID));
                    add_event.Parameters.AddWithValue("@Rib", int.Parse(ID));
                    cmd.ExecuteNonQuery();
                    add_event.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    add_event.Parameters.Clear();
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
        public bool ImortExperienceLivret(string ID, string Amount)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("UPDATE livreta SET Montant=Montant+@Montant WHERE ID=@ID", connection);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@Montant", int.Parse(Amount));
                    cmd.Parameters.AddWithValue("@ID", int.Parse(ID));
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
        public bool ExportExperienceLivret(string ID, string Amount)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("UPDATE livreta SET Montant=Montant-@Montant WHERE ID=@ID", connection);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@Montant", int.Parse(Amount));
                    cmd.Parameters.AddWithValue("@ID", int.Parse(ID));
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
        public string VoirTaux(string ID)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM livreta WHERE ID=@ID", connection);
                connection.Open();
                string taux = "error";
                if (connection.State == ConnectionState.Open)
                {

                    cmd.Parameters.AddWithValue("@ID", int.Parse(ID));
                    using (MySqlDataReader Lire = cmd.ExecuteReader())
                    {

                        while (Lire.Read())
                        {
                            taux = Lire["Taux"].ToString();
                        }
                        return taux;
                    }
                }
                else
                {
                    return taux;
                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return "error";
            }
        }
        public string VoirPlafond(string ID)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM livreta WHERE ID=@ID", connection);
                connection.Open();
                string Plafond = "error";
                if (connection.State == ConnectionState.Open)
                {

                    cmd.Parameters.AddWithValue("@ID", int.Parse(ID));
                    using (MySqlDataReader Lire = cmd.ExecuteReader())
                    {

                        while (Lire.Read())
                        {
                            Plafond = Lire["Plafond"].ToString();
                        }
                        return Plafond;
                    }
                }
                else
                {
                    return Plafond;
                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return "error";
            }
        }


        public bool ModifTaux(string RIB, string Taux)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("UPDATE livreta SET Taux=@Taux WHERE ID=@ID", connection);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@Taux", float.Parse(Taux));
                    cmd.Parameters.AddWithValue("@ID", int.Parse(RIB));
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
        public bool ModifPlafond(string RIB, string Plafond)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("UPDATE livreta SET Plafond=@Plafond WHERE ID=@ID", connection);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@Plafond", float.Parse(Plafond));
                    cmd.Parameters.AddWithValue("@ID", int.Parse(RIB));
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

        public bool PlayerConnected(string RIB)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("UPDATE livreta SET Online=1 WHERE ID=@ID", connection);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@ID", int.Parse(RIB));
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
        public bool PlayerDisconnected(string RIB)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand cmd = new MySqlCommand("UPDATE livreta SET Online=0 WHERE ID=@ID", connection);
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd.Parameters.AddWithValue("@ID", int.Parse(RIB));
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