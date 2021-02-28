using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mysqlDbManager
{
    public partial class Main : Window
    {
        Connection connection = new Connection();

        public Main()
        {
            InitializeComponent();

            // CHECKING CONNECTION TO INITIALIZE THE VARIABLE
            connection.checkConnection(Properties.Settings.Default["dbIP"].ToString(), Properties.Settings.Default["dbUser"].ToString(), Properties.Settings.Default["dbPassword"].ToString(), Properties.Settings.Default["dbName"].ToString());

            this.Title = "Database `" + Properties.Settings.Default["dbName"].ToString() + "` management";

            mainRefresh();
        }

        /// FUNCTION REFRESHES DATATABLES
        private void mainRefresh()
        {
            /// CLEAR DISPLAYED TABLE IF EXISTS
            clearTable();
            /// ==========

            /// GET TABLES NAMES
            List<string> databasesList = connection.getDatabaseTables(Properties.Settings.Default["dbName"].ToString());
            if (databasesList.Count>0 && databasesList[0] == "!ERR")
            {
                updateLog("Databases download error! Output: " + databasesList[1]);
            }
            else
            {
                foreach(string table in databasesList)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        CB_Tables.Items.Add(table);
                    }));
                }
                CB_Tables.SelectedIndex = 0;
            }
            /// ==========

            /// DISPLAY TABLE CONTENT
            tableRefresh();
            /// ==========
             

        }

        /// REFRESH TABLE FUNCTION
        private void tableRefresh()
        {
            List<string>[] tableList = connection.getTable(CB_Tables.SelectedItem.ToString());
            if (tableList[0].Count > 0 && tableList[0][0] == "!ERR")
            {
                updateLog("Databases download error! Output: " + tableList[0][1]);
            }
            else
            {
                updateLog("Succesfully downloaded table `" + CB_Tables.SelectedItem.ToString());

                /*
                for (int i=0; i<tableList.Length; i++)
                {
                    for(int j=0; j<tableList[i].Count; j++)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            TB_Log.Text = TB_Log.Text.ToString() + tableList[i][j] + "; ";
                        }));
                    }
                }
                */
            }
        }

        /// CLEARING THE TABLE FROM WINDOW
        private void clearTable()
        {
            G_Table.RowDefinitions.Clear();
            G_Table.ColumnDefinitions.Clear();
            G_Table.Children.Clear();
        }
        /// ==========

        /// BUTTON HANDLING LOGOUT SCRIPT
        private void B_Logout_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
        /// ==========

        /// BUTTON HANDLING REFERENCE TO REFRESH FUNCTIONS
        private void B_Refresh_Click(object sender, RoutedEventArgs e)
        {
            mainRefresh();
        }
        /// ==========
        
        /// FUNCTION HANDLES LOG UPDATING/CLEARING
        private void updateLog(string message="", bool clearLog=true)
        {
            if (clearLog)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    TB_Log.Text = "";
                }));
            }

            Dispatcher.Invoke(new Action(() =>
            {
                TB_Log.Text = message + "\n";
            }));
        }
        /// ==========
    }
}
