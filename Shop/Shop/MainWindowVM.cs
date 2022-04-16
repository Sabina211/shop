using System;
using System.Collections.Generic;
using System.Data;
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
        SqlDataAdapter dataAdapterMS;
        DataTable dataTableMS;
        private object clientsData;
        public object ClientsData 
        {
            get => clientsData;
            set
            {
                clientsData = value;
                OnPropertyChanged("ClientsData");
            }
        }

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
            SqlConnectionStringBuilder connectionStringMS = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB", //имя сервера источника данных, к которому будем подключаться
                InitialCatalog = "MSSQLLocalDB", //файл, к которому планируем подключаться
                IntegratedSecurity = true, //способ авторизации
                Pooling = false
            };
            SqlConnection sqlConnection = new SqlConnection { ConnectionString = connectionStringMS.ConnectionString };

            Task taskMS = new Task(ConnectToMSSqlDB, connectionStringMS);
            taskMS.Start();

            Task taskAccess = new Task(ConnectToAccessDB);
            taskAccess.Start();

            dataTableMS = new DataTable();
            dataAdapterMS = new SqlDataAdapter();

            #region select
            var sqlClientsMS = @"select * from client_info";
            dataAdapterMS.SelectCommand = new SqlCommand(sqlClientsMS, sqlConnection);
            #endregion

            dataAdapterMS.Fill(dataTableMS);
            ClientsData = dataTableMS.DefaultView;
        }

        private void ConnectToMSSqlDB(object strCon)
        {
            SqlConnectionStringBuilder stringConnection = strCon as SqlConnectionStringBuilder;
            SqlConnection sqlConnection = new SqlConnection { ConnectionString = stringConnection.ConnectionString };
            MsSqlConnectingString = stringConnection.ConnectionString;
            using (sqlConnection)
            {
                try
                {
                    sqlConnection.Open();
                    MsSqlConnectingString = $"{stringConnection.ConnectionString} \nState = {sqlConnection.State} \nThread Id = {Thread.CurrentThread.ManagedThreadId}";
                }
                catch (Exception e)
                {
                    MsSqlConnectingString = $"{stringConnection.ConnectionString} \nState = {e.Message}";
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
