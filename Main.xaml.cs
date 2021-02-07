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
