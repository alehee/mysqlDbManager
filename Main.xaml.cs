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
            /// GET TABLES NAMES
            List<string> databasesList = connection.getDatabaseTables(Properties.Settings.Default["dbName"].ToString());
            if (databasesList.Count>0 && databasesList[0] == "!ERR")
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    TB_Log.Text = "Databases download error! Output: " + databasesList[1];
                }));
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
        }

        /// LOGOUT SCRIPT
        private void B_Logout_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
