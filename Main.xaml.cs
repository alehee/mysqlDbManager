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
        MagicQuery magicQuery = new MagicQuery();
        List<string>[] tableList;
        string tableName;
        string continuousMessage = "";
        string VERSION = "1.0.0";

        public Main()
        {
            InitializeComponent();

            // CHECKING CONNECTION TO INITIALIZE THE VARIABLE
            connection.checkConnection(Properties.Settings.Default["dbIP"].ToString(), Properties.Settings.Default["dbUser"].ToString(), Properties.Settings.Default["dbPassword"].ToString(), Properties.Settings.Default["dbName"].ToString());

            this.Title = "Database `" + Properties.Settings.Default["dbName"].ToString() + "` management";

            Dispatcher.Invoke(new Action(() =>
            {
                L_Version.Content = "v. " + VERSION;
            }));

            mainRefresh();
        }

        /// FUNCTION REFRESHES DATATABLES
        public void mainRefresh()
        {
            updateLog("Refresh function started!");

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
                CB_Tables.Items.Clear();
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

            /// IF MESSAGE FROM PREVIOUS SESSION EXISTS PRINT IT
            if (continuousMessage != "")
            {
                updateLog(continuousMessage);
                continuousMessage = "";
            }
            /// ==========
        }

        /// REFRESH TABLE FUNCTION
        private void tableRefresh()
        {
            /// CLEAR DISPLAYED TABLE IF EXISTS
            clearTable();
            /// ==========

            string selectedTable = "";
            int primaryKeyColumn = -1;
            if (CB_Tables.Items.Count > 0)
            {
                selectedTable = CB_Tables.SelectedItem.ToString();
                tableName = selectedTable;
                tableList = connection.getTable(selectedTable);
            }
            else
            {
                tableList = new List<string>[1];
                tableList[0] = new List<string>();
                tableList[0].Add("!ERR");
                tableList[0].Add("There's no data in this table!");
            }
            
            if (tableList[0].Count > 0 && tableList[0][0] == "!ERR")
            {
                updateLog("Databases download error! Output: " + tableList[0][1]);
            }
            else
            {
                updateLog("Succesfully downloaded table `" + selectedTable + "`");

                // DISPLAY TABLE
                for(int i=0; i<tableList.Length; i++)
                {
                    // CHECK IF PRIMARY KEY
                    string headerName = tableList[i][0];
                    if (primaryKeyColumn < 0 && headerName[0]=='.')
                    {
                        headerName = "";
                        primaryKeyColumn = i;
                        for(int j=1; j<tableList[i][0].Length; j++)
                        {
                            headerName += tableList[i][0][j];
                        }
                    }

                    // FIRST DISPLAY HEADER
                    Label headerLabel = new Label();
                    headerLabel.Content = headerName;
                    headerLabel.FontWeight = FontWeight.FromOpenTypeWeight(600);
                    headerLabel.BorderBrush = Brushes.Black;
                    headerLabel.BorderThickness = new Thickness(0.5);
                    Grid.SetColumn(headerLabel, i+1);
                    Grid.SetRow(headerLabel, 0);

                    if (i == 0) // ROWS NEEDS TO BE DEFINED ONLY ONE TIME
                    {
                        RowDefinition rowDefinition = new RowDefinition();
                        rowDefinition.Height = GridLength.Auto;
                        G_Table.RowDefinitions.Add(rowDefinition);
                    }

                    ColumnDefinition columnDefinition = new ColumnDefinition();
                    columnDefinition.Width = GridLength.Auto;
                    G_Table.ColumnDefinitions.Add(columnDefinition);

                    G_Table.Children.Add(headerLabel);

                    // THEN DISPLAY REST OF THE LIST FOR THIS HEADER
                    for(int j=1; j<tableList[i].Count; j++)
                    {
                        if (i == 0) // ROWS NEEDS TO BE DEFINED ONLY ONE TIME
                        {
                            RowDefinition rowDefinition = new RowDefinition();
                            rowDefinition.Height = GridLength.Auto;
                            G_Table.RowDefinitions.Add(rowDefinition);
                        }

                        if (i == 0) // ADD DELETE BUTTONS ON COLUMN 1
                        {
                            Label delCell = new Label();
                            delCell.Content = "x";
                            delCell.BorderBrush = Brushes.Black;
                            delCell.Foreground = Brushes.Red;
                            delCell.FontWeight = FontWeight.FromOpenTypeWeight(600);
                            delCell.Cursor = Cursors.Hand;
                            delCell.BorderThickness = new Thickness(0.5);
                            delCell.Name = "d" + j.ToString();
                            delCell.MouseDown += L_DeleteRow;
                            Grid.SetColumn(delCell, 0);
                            Grid.SetRow(delCell, j);

                            G_Table.Children.Add(delCell);
                        }

                        Label cell = new Label();
                        cell.Content = tableList[i][j];
                        cell.BorderBrush = Brushes.Black;
                        cell.BorderThickness = new Thickness(0.5);
                        if(i!=primaryKeyColumn)
                            cell.Name = "c" + j.ToString() + "_" + (i+1).ToString();
                        else
                            cell.Name = "p" + j.ToString() + "_" + (i+1).ToString();
                        cell.MouseDoubleClick += L_ChangeCell;
                        Grid.SetColumn(cell, i+1);
                        Grid.SetRow(cell, j);

                        columnDefinition = new ColumnDefinition();
                        columnDefinition.Width = GridLength.Auto;
                        G_Table.ColumnDefinitions.Add(columnDefinition);

                        G_Table.Children.Add(cell);
                    }
                }
            }
        }
        /// ==========

        /// DOUBLE CLICK EVENT FOR CHANGING CELL VALUE
        private void L_ChangeCell(object sender, MouseButtonEventArgs e)
        {
            string name = ((Label)sender).Name;
            string value = ((Label)sender).Content.ToString();

            if (name[0] == 'c')
            {
                int j=-1;
                int i=-1;
                string buffer = "";
                bool flag = false;

                foreach(char a in name)
                {
                    if(a!='p' && a != 'c')
                    {
                        if (a == '_')
                        {
                            flag = true;
                            j = Convert.ToInt32(buffer);
                            buffer = "";
                        }
                        else
                            buffer += a;
                    }
                }
                i = Convert.ToInt32(buffer);

                G_Table.Children.Remove((UIElement)sender);

                TextBox cell = new TextBox();
                cell.Text = value;
                cell.BorderBrush = Brushes.Black;
                cell.BorderThickness = new Thickness(0.5);
                cell.Name = "t" + j.ToString() + "_" + i.ToString();
                cell.KeyDown += TB_ChangeValue;
                Grid.SetColumn(cell, i);
                Grid.SetRow(cell, j);
                G_Table.Children.Add(cell);
                cell.SelectionStart = 0;
                cell.SelectionLength = cell.Text.Length;
                cell.Focus();

                L_ChangeCellAlert.Opacity = 100;
                updateLog("Performing change on row " + j.ToString() + " and column " + i.ToString());
            }
            else
                updateLog("You can't change value of primary key!");
        }
        /// ==========

        /// GET DATA TO CHANGE IN DATABASE
        private void TB_ChangeValue(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                string jToFind = "";
                string iToFind = "";
                string name = ((TextBox)sender).Name;
                string value = ((TextBox)sender).Text.ToString();
                string colName = "";
                string primaryKeyValue = "";
                bool flag = false;

                foreach (char a in name)
                {
                    if (flag == false)
                    {
                        if (a == '_')
                        {
                            flag = true;
                        }
                        else if (a != 't')
                        {
                            jToFind += a;
                        }
                    }
                    else
                    {
                        iToFind += a;
                    }
                }

                colName = tableList[Convert.ToInt32(iToFind) - 1][0];

                foreach (List<string> column in tableList)
                {
                    if (column[0][0] == '.')
                    {
                        primaryKeyValue = column[Convert.ToInt32(jToFind)].ToString();
                    }
                }

                L_ChangeCellAlert.Opacity = 0;
                if(!connection.changeCell(tableName, value, colName, primaryKeyValue))
                {
                    continuousMessage = "Error! Cannot update value in database! Try again!"; 
                }
                updateLog("Changing " + colName + "=" + value + " on PrimaryKey=" + primaryKeyValue);
                mainRefresh();
            }
        }
        /// ==========

        /// DELETE ROW BUTTON FUNCTION
        private void L_DeleteRow(object sender, MouseButtonEventArgs e)
        {
            string jToFind = "";
            string name = ((Label)sender).Name;
            string primaryKeyValue = "";

            foreach (char a in name)
            {
                if (a != 'd')
                {
                    jToFind += a;
                }
            }

            foreach (List<string> column in tableList)
            {
                if (column[0][0] == '.')
                {
                    primaryKeyValue = column[Convert.ToInt32(jToFind)].ToString();
                }
            }

            if (MessageBox.Show("Do you want to delete row with primary key = `"+primaryKeyValue+"`?\nData will be lost forever!", "Warning! Trying to delete row...", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (!connection.deleteRow(tableName, primaryKeyValue))
                {
                    continuousMessage = "Error! Cannot delete selected row in database! Try again!";
                }
                updateLog("Deleting row on Primary Key = " + primaryKeyValue);
                mainRefresh();
            }
        }
        /// ==========

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
            tableRefresh();
        }
        /// ==========
         
        /// BUTTON HANDLING SENDING QUERY TO DATABASE
        private void B_Query_Click(object sender, RoutedEventArgs e)
        {
            string sqlToSend = TB_Query.Text.ToString();
            if (!connection.customSql(tableName, sqlToSend))
            {
                continuousMessage = "Error! Cannot send current SQL to database! Try again!";
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    TB_Query.Text = "";
                }));
            }
            updateLog("Sending custom SQL to database!");
            mainRefresh();
        }
        /// ==========
         
        /// BUTTON HANDLING MAGIC QUERY FUNCTION OPENING
        private void B_MagicQuery_Click(object sender, RoutedEventArgs e)
        {
            List<string> colNames = new List<string>();
            foreach (List<string> column in tableList)
            {
                string buff = "";

                if (column[0][0] == '.')
                {
                    buff = column[0].Substring(1);
                }
                else
                    buff = column[0];

                colNames.Add(buff);
            }

            magicQuery.sendData(tableName, colNames, this);
            magicQuery.Show();
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

        /// WINDOW CLOSING OVERRIDE
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        /// ==========


    }
}
