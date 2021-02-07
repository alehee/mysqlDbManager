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
using System.Windows.Shapes;

namespace mysqlDbManager
{
    public partial class Login : Window
    {
        Connection connection = new Connection();

        public Login()
        {
            InitializeComponent();

            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    TB_IP.Text = Properties.Settings.Default["dbIP"].ToString();
                    TB_User.Text = Properties.Settings.Default["dbUser"].ToString();
                    TB_Password.Password = Properties.Settings.Default["dbPassword"].ToString();
                    TB_Database.Text = Properties.Settings.Default["dbName"].ToString();
                }));
            }
            catch {  }
        }

        /// LOGING IN AND CHECKING CONNECTION TO DB
        private void B_Connect_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                L_Log.Content = "Trying to log in...";
            }));

            if (connection.checkConnection(TB_IP.Text, TB_User.Text, TB_Password.Password, TB_Database.Text))
            {
                Properties.Settings.Default["dbIP"] = TB_IP.Text.ToString();
                Properties.Settings.Default["dbUser"] = TB_User.Text.ToString();
                Properties.Settings.Default["dbPassword"] = TB_Password.Password.ToString();
                Properties.Settings.Default["dbName"] = TB_Database.Text.ToString();
                Properties.Settings.Default.Save();

                Main main = new Main();
                main.Show();
                this.Close();
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    L_Log.Content = "Connection error! Check your input!";
                }));
            }
        }
    }
}
