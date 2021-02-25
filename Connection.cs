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

        public List<string> getDatabaseTables(string dbName)
        {
            // SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA='dbName'
            List<string> returnList = new List<string>();
            string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA='"+ dbName +"'";
            MySqlCommand query = new MySqlCommand(sql, connection);
            query.CommandTimeout = 30;
            try
            {
            connection.Open();
            MySqlDataReader mySqlDataReader = query.ExecuteReader();

                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                    {
                        returnList.Add(mySqlDataReader.GetValue(0).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                returnList.Clear();
                returnList.Add("!ERR");
                returnList.Add(e.ToString());
            }
            finally
            {
                connection.Close();
            }
            return returnList;
        }

        public List<string> getTables()
        {
            List<string> returnList = new List<string>();

            return returnList;
        }
    }
}
