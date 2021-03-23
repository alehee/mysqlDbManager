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
        string databaseName;
        string primaryKeyColumn;

        /// FUNCTION TO GET CONNECTION AND ALSO CHECK IT
        public bool checkConnection(string dbUrl, string dbUser, string dbPassword, string dbName)
        {
            string connectionString = "server="+dbUrl+";user="+dbUser+";database="+dbName+";port=3306;password="+dbPassword+";";
            try
            {
                databaseName = dbName;
                connection = new MySqlConnection(connectionString);
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        /// ==========

        /// RETURNS LIST OF TABLES FOR THIS DATABASE
        public List<string> getDatabaseTables(string dbName)
        {
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
        /// ==========

        /// RETURNS LIST OF HEADERS FOR THE TABLE
        private List<string> getHeaders(string tableName)
        {
            List<string> returnList = new List<string>();
            string primaryKey = getPrimaryKey(tableName);
            string sql = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='" + databaseName + "' AND TABLE_NAME = '" + tableName + "'";
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
                        string headerBuffer = mySqlDataReader.GetValue(0).ToString();

                        if (headerBuffer == primaryKey)
                        {
                            headerBuffer = "." + headerBuffer;
                        }

                        returnList.Add(headerBuffer);
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
        /// ==========

        /// RETURNS STRING NAME OF PRIMARY KEY FOR THE TABLE
        private string getPrimaryKey(string tableName)
        {
            string pri = "";
            // SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='test' AND TABLE_NAME='tabela1' AND COLUMN_KEY='PRI'
            string sql = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='" + databaseName + "' AND TABLE_NAME='" + tableName + "' AND COLUMN_KEY='PRI'";
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
                        pri = mySqlDataReader.GetValue(0).ToString();
                    }
                }
            }
            catch (Exception e)
            {
                pri = "!ERR";
            }
            finally
            {
                connection.Close();
            }

            primaryKeyColumn = pri;
            return pri;
        }
        /// ==========

        /// RETURNS ARRAY OF LISTS OF COLUMNS FOR THE TABLE
        public List<string>[] getTable(string tableName)
        {
            /// RETURN VARIABLES DESCRIPTION
            /// 
            /// Function returns array of string Lists
            /// Each list is a different column from the table, and the first element of the list is always a header name
            /// For example, present table:
            /// ID      | ItemName  | SerialNum
            /// ================================
            /// 0       | Socks     | 72842
            /// 1       | Trousers  | 24612
            /// Returns:
            /// [0][0] = "ID"; [0][1] = "0"; [0][2] = "1"
            /// [1][0] = "ItemName"; [1][1] = "Socks"; [1][2] = "72842"
            /// [2][0] = "SerialNum"; [2][1] = "Trousers"; [2][2] = "24612"
            /// ==========

            /// GET TABLE HEADERS
            List<string> headersList = getHeaders(tableName);

            // INITIALIZE RETURN TABLE OF ARRAYS
            int arrayLenght = headersList.Count;
            List<string>[] returnList = new List<string>[arrayLenght];
            for(int i=0; i<returnList.Length; i++)
            {
                returnList[i] = new List<string>();
            }

            if (headersList.Count == 0)
            {
                returnList[0].Add("!ERR");
                returnList[0].Add("No headers for this table!");
                return returnList;
            }
            else if (headersList[0] == "!ERR")
            {
                returnList[0].Add("!ERR");
                returnList[0].Add(headersList[1]);
                return returnList;
            }
            else
            {
                for(int i=0; i<arrayLenght; i++)
                {
                    returnList[i].Add(headersList[i]);
                }
            }
            /// ==========
            
            string sql = "SELECT * FROM " + tableName;
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
                        for(int i=0; i<arrayLenght; i++)
                        {
                            returnList[i].Add(mySqlDataReader.GetValue(i).ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                returnList[0].Clear();
                returnList[0].Add("!ERR");
                returnList[0].Add(e.ToString());
            }
            finally
            {
                connection.Close();
            }
            return returnList;
        }
        /// ==========
        
        /// CHANGE CELL VALUE AND RETURN BOOL IF SUCCESSFUL
        public bool changeCell(string tableName, string columnValue, string columnName, string primaryKeyValue)
        {
            bool flag = true;
            string sql = "UPDATE `"+tableName+"` SET "+columnName+"='"+columnValue+"' WHERE "+primaryKeyColumn+"='"+primaryKeyValue+"'";
            MySqlCommand query = new MySqlCommand(sql, connection);
            query.CommandTimeout = 30;

            try
            {
                connection.Open();
                MySqlDataReader mySqlDataReader = query.ExecuteReader();
            }
            catch
            {
                flag = false;
            }
            finally
            {
                connection.Close();
            }

            return flag;
        }
        /// ==========

        /// DELETE ROW FUNCTION AND RETURN BOOL IF SUCCESSFUL
        public bool deleteRow(string tableName, string primaryKeyValue)
        {
            bool flag = true;
            string sql = "DELETE FROM "+tableName+" WHERE "+primaryKeyColumn+"='"+primaryKeyValue+"'";
            MySqlCommand query = new MySqlCommand(sql, connection);
            query.CommandTimeout = 30;

            try
            {
                connection.Open();
                MySqlDataReader mySqlDataReader = query.ExecuteReader();
            }
            catch
            {
                flag = false;
            }
            finally
            {
                connection.Close();
            }

            return flag;
        }
        /// ==========

        /// DELETE ROW FUNCTION AND RETURN BOOL IF SUCCESSFUL
        public bool customSql(string tableName, string sql)
        {
            bool flag = true;
            MySqlCommand query = new MySqlCommand(sql, connection);
            query.CommandTimeout = 30;

            try
            {
                connection.Open();
                MySqlDataReader mySqlDataReader = query.ExecuteReader();
            }
            catch
            {
                flag = false;
            }
            finally
            {
                connection.Close();
            }

            return flag;
        }
        /// ==========
    }
}
