using Rocket.API;

namespace BinaryShop
{
    public class BinaryShopConfiguration : IRocketPluginConfiguration
    {
        public classDatabase BinaryShopDatabase;

        public void LoadDefaults()
        {
            BinaryShopDatabase = new classDatabase()
            {
                DatabaseAddress = "25.93.171.134",
                DatabaseUsername = "root",
                DatabasePassword = "",
                DatabaseName = "binaryshop",
                DatabaseTableName = "item",
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

