using Rocket.API;

namespace BinaryBank
{
    public class BinaryBankConfiguration : IRocketPluginConfiguration
    {
        public classDatabase BinaryBankDatabase;

        public void LoadDefaults()
        {
            BinaryBankDatabase = new classDatabase()
            {
                DatabaseAddress = "25.93.171.134",
                DatabaseUsername = "root",
                DatabasePassword = "",
                DatabaseName = "binarybank",
                DatabaseTableName = "Bank_Accounts",
                DatabasePort = 3306
            };
        }
    }
    public class classDatabase
    {
        public string DatabaseAddress;
        public string DatabaseUsername;
        public string DatabasePassword;
        public string DatabaseName;
        public string DatabaseTableName;
        public string DatabaseViewName;
        public int DatabasePort;
    }
}

