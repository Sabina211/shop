using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shop
{
    internal class MainWindowVM : Bindable
    {
        private string msSqlConnectingString;
        public string MsSqlConnectingString
        {
            get => msSqlConnectingString;
            set
            {
                msSqlConnectingString = value;
                OnPropertyChanged("MsSqlConnectingString");
            }
        }
        private string accessConnectingString;
        public string AccessConnectingString
        {
            get => accessConnectingString;
            set
            {
                accessConnectingString = value;
                OnPropertyChanged("AccessConnectingString");
            }
        }

        public MainWindowVM()
        {
            Task taskMS = new Task(ConnectToMSSqlDB);
            taskMS.Start();

            Task taskAccess = new Task(ConnectToAccessDB);
            taskAccess.Start();
        }

        private void ConnectToMSSqlDB()
        {
            SqlConnectionStringBuilder strCon = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB", //имя сервера источника данных, к которому будем подключаться
                InitialCatalog = "MSSQLLocalDB", //файл, к которому планируем подключаться
                IntegratedSecurity = true, //способ авторизации
                Pooling = false
            };
            SqlConnection sqlConnection = new SqlConnection { ConnectionString = strCon.ConnectionString };
            MsSqlConnectingString = strCon.ConnectionString;
            using (sqlConnection)
            {
                try
                {
                    sqlConnection.Open();
                    MsSqlConnectingString = $"{strCon.ConnectionString} \nState = {sqlConnection.State} \nThread Id = {Thread.CurrentThread.ManagedThreadId}";
                }
                catch (Exception e)
                {
                    MsSqlConnectingString = $"{strCon.ConnectionString} \nState = {e.Message}";
                }
            }
        }

        private void ConnectToAccessDB()
        {
            OleDbConnectionStringBuilder accessConSring = new OleDbConnectionStringBuilder()
            {
                Provider = "Microsoft.ACE.OLEDB.12.0",
                DataSource = "E:\\Сабина, курс\\Задание 17\\Shop\\AccessDB.accdb",
                PersistSecurityInfo = true
            };
            OleDbConnection accessConnection = new OleDbConnection { ConnectionString = accessConSring.ConnectionString };
            using (accessConnection)
            {
                try
                {
                    accessConnection.Open();
                    AccessConnectingString = $"{accessConSring.ConnectionString} \nState = {accessConnection.State} \nThread Id = {Thread.CurrentThread.ManagedThreadId}";
                }
                catch (Exception e)
                {
                    AccessConnectingString = $"{accessConnection.State} \n State = {e.Message}";
                }
            }
        }
    }
}
