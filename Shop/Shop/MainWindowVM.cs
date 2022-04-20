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

        OleDbDataAdapter dataAdapterAccess;
        DataTable dataTableAccess;

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

        private object productsData;
        public object ProductsData
        {
            get => productsData;
            set
            {
                productsData = value;
                OnPropertyChanged("ProductsData");
            }
        }

        private object clientsProductsData;
        public object ClientsProductsData
        {
            get => clientsProductsData;
            set
            {
                clientsProductsData = value;
                OnPropertyChanged("ClientsProductsData");
            }
        }

        private DataRowView selectedClient;
        public DataRowView SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                OnPropertyChanged("SelectedClient");
                OnPropertyChanged("CurrentClient");
                /*if (selectedClient != null)
                {
                    Client testClient = new Client();
                    testClient.Id = Convert.ToInt32(selectedClient.Row.ItemArray[0].ToString());
                    testClient.Surname = selectedClient.Row.ItemArray[1].ToString();
                    testClient.FirstName = selectedClient.Row.ItemArray[2].ToString();
                    testClient.Patronymic = selectedClient.Row.ItemArray[3].ToString();
                    testClient.PhoneNumber = selectedClient.Row.ItemArray[4].ToString();
                    testClient.Email = selectedClient.Row.ItemArray[5].ToString();
                    CurrentClient = testClient;
                }*/
            }
        }
        private Client currentClient;
        public Client CurrentClient 
        {
            get => currentClient;
            set 
            {
                if (selectedClient != null)
                {
                    Client testClient = new Client();
                    testClient.Id = Convert.ToInt32(selectedClient.Row.ItemArray[0].ToString());
                    testClient.Surname = selectedClient.Row.ItemArray[1].ToString();
                    testClient.FirstName = selectedClient.Row.ItemArray[2].ToString();
                    testClient.Patronymic = selectedClient.Row.ItemArray[3].ToString();
                    testClient.PhoneNumber = selectedClient.Row.ItemArray[4].ToString();
                    testClient.Email = selectedClient.Row.ItemArray[5].ToString();
                    CurrentClient = testClient;
                }
                OnPropertyChanged("SelectedClient");
                OnPropertyChanged("CurrentClient");
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
            //клиенты
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

            dataTableMS = new DataTable();
            dataAdapterMS = new SqlDataAdapter();
            var selectClients = @"select * from client_info";
            dataAdapterMS.SelectCommand = new SqlCommand(selectClients, sqlConnection);

            dataAdapterMS.Fill(dataTableMS);
            ClientsData = dataTableMS.DefaultView;

            //продукты
            OleDbConnectionStringBuilder accessConSring = new OleDbConnectionStringBuilder()
            {
                Provider = "Microsoft.ACE.OLEDB.12.0",
                DataSource = "E:\\Сабина, курс\\Задание 17\\Shop\\AccessDB.accdb",
                PersistSecurityInfo = true
            };
            OleDbConnection accessConnection = new OleDbConnection { ConnectionString = accessConSring.ConnectionString };

            Task taskAccess = new Task(ConnectToAccessDB, accessConSring);
            taskAccess.Start();
            dataTableAccess = new DataTable();
            dataAdapterAccess = new OleDbDataAdapter();
            var selectProducts = @"select *from products order by id";
            dataAdapterAccess.SelectCommand = new OleDbCommand(selectProducts, accessConnection);
            dataAdapterAccess.Fill(dataTableAccess);
            productsData = dataTableAccess.DefaultView;

            var text = SelectedClient;

            //FillClientsProducts(accessConnection);

        }

        private void FillClientsProducts(OleDbConnection accessConnection)
        {
            //продукты, выбранного клиента
            string selectClientsProducts = (CurrentClient != null) ? $"select * from products where email={CurrentClient.Email}" : null;
            dataAdapterAccess.SelectCommand = new OleDbCommand(selectClientsProducts, accessConnection);
            dataAdapterAccess.Fill(dataTableAccess);
            ClientsProductsData = dataTableAccess.DefaultView;
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

        private void ConnectToAccessDB(object accessConSring)
        {
            OleDbConnectionStringBuilder stringProductConnection = accessConSring as OleDbConnectionStringBuilder;
            OleDbConnection oleDbConnection = new OleDbConnection {ConnectionString = stringProductConnection.ConnectionString };
            using (oleDbConnection)
            {
                try
                {
                    oleDbConnection.Open();
                    AccessConnectingString = $"{oleDbConnection.ConnectionString} \nState = {oleDbConnection.State} \nThread Id = {Thread.CurrentThread.ManagedThreadId}";
                }
                catch (Exception e)
                {
                    AccessConnectingString = $"{oleDbConnection.State} \n State = {e.Message}";
                }
            }
        }
    }
}
