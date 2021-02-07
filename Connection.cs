using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace mysqlDbManager
{
    class Connection
    {
        MySqlConnection connection;

        public bool checkConnection(string dbUrl, string dbUser, string dbPassword, string dbName)
        {
            string connectionString = "server="+dbUrl+";user="+dbUser+";database="+dbName+";port=3306;password="+dbPassword+";";
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
