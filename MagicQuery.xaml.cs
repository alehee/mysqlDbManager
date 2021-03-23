using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class MagicQuery : Window
    {
        bool isInsert = true;
        string tableName = "";
        List<string> columns = new List<string>();
        Connection mqConn = new Connection();
        Main mainWindow;

        public MagicQuery()
        {
            InitializeComponent();

            // CHECKING CONNECTION TO INITIALIZE THE VARIABLE
            mqConn.checkConnection(Properties.Settings.Default["dbIP"].ToString(), Properties.Settings.Default["dbUser"].ToString(), Properties.Settings.Default["dbPassword"].ToString(), Properties.Settings.Default["dbName"].ToString());
        }

        /// FUNCTION FOR ASSIGNMENT PRESENT DATA TO MAGIC QUERY
        public void sendData(string tabNam, List<string> cols, Main mainObject)
        {
            tableName = tabNam;
            columns = cols;
            mainWindow = mainObject;
        }
        /// ==========

        /// FUNCTION THAT REQUESTS UPDATE/INSERT FUNCTION
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string desc1 = L_Desc1.Content.ToString();
            string desc2 = L_Desc2.Content.ToString();
            string tb1 = TB_Param11.Text.ToString();
            string tb2 = TB_Param1.Text.ToString();

            if (isInsert == true)
            {
                if(!mqConn.customSql(tableName, desc1 + desc2 + "(" + tb2 + ")"))
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        TB_Log.Text = "Sending query error! Check your input!";
                    }));
                }
                else
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        TB_Log.Text = "Insert successful!";
                    }));
                }
            }
            else
            {
                if(!mqConn.customSql(tableName, desc1 + " SET " + tb1 + " WHERE " + tb2))
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        TB_Log.Text = "Sending query error! Check your input!";
                    }));
                }
                else
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        TB_Log.Text = "Update successful!";
                    }));
                }
            }
            mainWindow.mainRefresh();
        }
        /// ==========

        /// FUNCTION CHANGING TYPE OF EDIT QUERY
        private void RadioChange_Click(object sender, RoutedEventArgs e)
        {
            string cont = ((RadioButton)sender).Content.ToString();
            string query = "";

            if(cont == "INSERT INTO")
            {
                string valuesNames = "";

                for(int i=0; i<columns.Count; i++)
                {
                    valuesNames += columns[i];
                    if (i + 1 < columns.Count)
                        valuesNames += ",";
                }

                isInsert = true;

                Dispatcher.Invoke(new Action(() =>
                {
                    L_Desc1.Content = "INSERT INTO `" + tableName + "`";
                    L_Desc2.Content = "("+valuesNames+") VALUES";
                    L_Desc22.Content = "";
                    TB_Param11.Visibility = Visibility.Hidden;
                    TB_Param11.Text = "";
                    L_Desc23.Content = "";
                    TB_Param1.Visibility = Visibility.Visible;
                    TB_Param1.Text = "";
                    L_BracketL.Content = "(";
                    L_BracketR.Content = ")";
                    TB_Log.Text = "Use ' ' for text values!";
                }));
            }
            else
            {
                isInsert = false;

                Dispatcher.Invoke(new Action(() =>
                {
                    L_Desc1.Content = "UPDATE `" + tableName + "`";
                    L_Desc2.Content = "";
                    L_Desc22.Content = "SET";
                    TB_Param11.Visibility = Visibility.Visible;
                    TB_Param11.Text = "";
                    L_Desc23.Content = "WHERE";
                    TB_Param1.Visibility = Visibility.Visible;
                    TB_Param1.Text = "";
                    L_BracketL.Content = "";
                    L_BracketR.Content = "";
                    TB_Log.Text = "Use ' ' for text values!";
                }));
            }
        }
        /// ==========

        /// OVERRIDE WINDOW CLOSING EVENT
        protected override void OnClosing(CancelEventArgs e)
        {
            base.Hide();
            Dispatcher.Invoke(new Action(() =>
            {
                R_Insert.IsChecked = false;
                R_Update.IsChecked = false;
                L_Desc1.Content = "";
                L_Desc2.Content = "";
                L_Desc22.Content = "";
                TB_Param11.Visibility = Visibility.Hidden;
                TB_Param11.Text = "";
                L_Desc23.Content = "";
                TB_Param1.Visibility = Visibility.Hidden;
                TB_Param1.Text = "";
                L_BracketL.Content = "";
                L_BracketR.Content = "";
                TB_Log.Text = "";
            }));
            e.Cancel = true;
            base.OnClosing(e);
        }
        /// ==========
    }
}
