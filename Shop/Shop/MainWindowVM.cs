using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;

namespace Shop
{
    internal class MainWindowVM : Bindable
    {
        private object objectForLock = "wer";
        AccessDbConnection productsDBConnection = new AccessDbConnection();
        //DBConnection productsDBConnection;
        SqlDataAdapter dataAdapterMS;
        DataTable dataTableMS;

        //OleDbDataAdapter dataAdapterAccess;
        //DataTable dataTableAccess;

        //OleDbDataAdapter dataAdapterClientsProducts;
        //DataTable dataTableClientsProducts;

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
                    FillClientsProducts();
                }
            }
        }
        private Client currentClient;
        public Client CurrentClient 
        {
            get => currentClient;
            set 
            {
                currentClient = value;
                OnPropertyChanged("CurrentClient");
            }
        }

        private DataRowView selectedProduct;
        public DataRowView SelectedProduct
        {
            get { return selectedClient; }
            set
            {
                selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
                if (selectedProduct != null)
                {
                    Product newProduct = new Product(
                        Convert.ToInt32(selectedProduct.Row.ItemArray[0].ToString()),
                        selectedProduct.Row.ItemArray[2].ToString(),
                        selectedProduct.Row.ItemArray[3].ToString(),
                        selectedProduct.Row.ItemArray[1].ToString()
                        );
                    CurrentProduct = newProduct;
                }
            }
        }

        private Product currentProduct;
        public Product CurrentProduct
        {
            get => currentProduct;
            set
            {
                currentProduct = value;
                OnPropertyChanged("CurrentProduct");
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
        public ICommand AddClientCommand { get; }
        public ICommand EditClientCommand { get; }
        public ICommand DeleteClientCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        public MainWindowVM()
        {

            AddClientCommand = new RelayCommand( obj =>
            {
                AddClientWindow addClientWindow = new AddClientWindow(this);
                addClientWindow.Show();
            });

            EditClientCommand = new RelayCommand(obj =>
            {
                EditClientwindow editClientWindow = new EditClientwindow(this, CurrentClient);
                editClientWindow.Show();
            });

            DeleteClientCommand = new RelayCommand( obj => 
            {
                MessageBoxResult messagebox =  MessageBox.Show($"Вы уверены, что хотите удалить пользователя с именем " +
                    $"{CurrentClient.Surname} {CurrentClient.FirstName} {CurrentClient.Patronymic}?", "Подтверждение операции", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (messagebox == MessageBoxResult.Yes)
                {
                    MSDbConnection clientsConnectionDelete = new MSDbConnection();
                    using (clientsConnectionDelete.Connection)
                    {
                        try
                        {
                            clientsConnectionDelete.Connection.Open();
                            dataAdapterMS = new SqlDataAdapter();
                            var deleteClient = $"delete from client_info where id={CurrentClient.Id}";
                            dataAdapterMS.DeleteCommand = new SqlCommand(deleteClient, clientsConnectionDelete.Connection);
                            dataAdapterMS.DeleteCommand.ExecuteNonQuery();
                            UpdateClients();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());
                        }
                    }                    
                }
            });

            AddProductCommand = new RelayCommand(obj =>
            {
                AddProductWindow addProductWindow = new AddProductWindow(this);
                addProductWindow.Show();
            });

            DeleteProductCommand = new RelayCommand(obj =>
            {
                MessageBoxResult messagebox = MessageBox.Show($"Вы уверены, что хотите удалить покупку " +
                    $"\"{CurrentProduct.ProductName}\" с кодом {CurrentProduct.ProductCode} ?", "Подтверждение операции", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (messagebox == MessageBoxResult.Yes)
                {
                   
                    AccessDbConnection productsConnectionDelete = new AccessDbConnection();

                    using (productsConnectionDelete.Connection)
                    {
                        try
                        {
                            productsConnectionDelete.Connection.Open();
                            OleDbDataAdapter dataAdapterAccessDelete = new OleDbDataAdapter();
                            var deleteClient = $"delete from products where id={CurrentProduct.Id}";
                            dataAdapterAccessDelete.DeleteCommand = new OleDbCommand(deleteClient, productsConnectionDelete.Connection);
                            dataAdapterAccessDelete.DeleteCommand.ExecuteNonQuery();
                            UpdateProducts();
                            FillClientsProducts();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());
                        }
                    }
                }
            });

            //клиенты
            MSDbConnection clientsConnection = new MSDbConnection();

            Task taskMS = new Task(ConnectToMSSqlDB, clientsConnection.StringBuilder);
            taskMS.Start();
            UpdateClients();

            //продукты
            Task taskAccess = new Task(ConnectToAccessDB);
            taskAccess.Start();
            UpdateProducts();
        }

        public void FillClientsProducts()
        {
            //продукты, выбранного клиента
            AccessDbConnection accessDbConnection = new AccessDbConnection();
            DataTable dataTableClientsProducts = new DataTable();
            OleDbDataAdapter dataAdapterClientsProducts = new OleDbDataAdapter();
            string selectClientsProducts = (CurrentClient != null) ? $"select * from products where email='{CurrentClient.Email}'" : null;
            dataAdapterClientsProducts.SelectCommand = new OleDbCommand(selectClientsProducts, accessDbConnection.Connection);
            dataAdapterClientsProducts.Fill(dataTableClientsProducts);
            ClientsProductsData = dataTableClientsProducts.DefaultView;
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
                    MsSqlConnectingString = $"{stringConnection.ConnectionString} \nState = {sqlConnection.State}";
                }
                catch (Exception e)
                {
                    MsSqlConnectingString = $"{stringConnection.ConnectionString} \nState = {e.Message}";
                }
            }
        }

        private void ConnectToAccessDB()
        {
            OleDbConnection oleDbConnection = new OleDbConnection {ConnectionString = productsDBConnection.StringBuilder.ConnectionString };
            using (oleDbConnection)
            {
                try
                {
                    oleDbConnection.Open();
                    AccessConnectingString = $"{oleDbConnection.ConnectionString} \nState = {oleDbConnection.State}";
                }
                catch (Exception e)
                {
                    AccessConnectingString = $"{oleDbConnection.State} \n State = {e.Message}";
                }
            }
        }

        public void UpdateClients()
        {
            MSDbConnection clientsConnection = new MSDbConnection();
            using (clientsConnection.Connection)
            {
                try
                {
                    dataTableMS = new DataTable();
                    dataAdapterMS = new SqlDataAdapter();
                    var selectClients = @"select * from client_info";
                    dataAdapterMS.SelectCommand = new SqlCommand(selectClients, clientsConnection.Connection);

                    dataAdapterMS.Fill(dataTableMS);
                    ClientsData = dataTableMS.DefaultView;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public void UpdateProducts()
        {
            AccessDbConnection productsConnection = new AccessDbConnection();
            using (productsConnection.Connection)
            {
                try
                {
                    Thread.Sleep(1000);
                    DataTable dataTableAccess = new DataTable();
                    OleDbDataAdapter dataAdapterAccess = new OleDbDataAdapter();
                    var selectProducts = @"select *from products order by id";
                    dataAdapterAccess.SelectCommand = new OleDbCommand(selectProducts, productsConnection.Connection);

                    dataAdapterAccess.Fill(dataTableAccess);
                    ProductsData = dataTableAccess.DefaultView;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
    }
}
