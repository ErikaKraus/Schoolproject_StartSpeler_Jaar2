
using MySql.Data.MySqlClient;

using System.Configuration;

namespace Kassa.Data
{
    public static class DatabaseConnection
    {
        public static string Connectionstring(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }

}
