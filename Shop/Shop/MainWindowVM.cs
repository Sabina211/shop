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
using System.Diagnostics;
using System.Data.Entity;

namespace Shop
{
    internal class MainWindowVM : Bindable
    {
        AccessDbConnection productsDBConnection = new AccessDbConnection();

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

        private client_info selectedClient;
        public client_info SelectedClient
        {
            get { return selectedClient; }
            set
            {
                selectedClient = value;
                OnPropertyChanged("SelectedClient");
                OnPropertyChanged("CurrentClient");
                if (selectedClient != null)
                {
                    client_info testClient = new client_info();
                    testClient.id = Convert.ToInt32(selectedClient.id);
                    testClient.surname = selectedClient.surname;
                    testClient.first_name = selectedClient.first_name;
                    testClient.patronymic = selectedClient.patronymic;
                    testClient.phone_number = selectedClient.phone_number;
                    testClient.email = selectedClient.email;
                    CurrentClient = testClient;
                    FillClientsProducts();
                }
            }
        }
        private client_info currentClient;
        public client_info CurrentClient 
        {
            get => currentClient;
            set 
            {
                currentClient = value;
                OnPropertyChanged("CurrentClient");
            }
        }

        private Product selectedProduct;
        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
                if (selectedProduct != null )
                {
                    Product newProduct = new Product(
                        Convert.ToInt32(selectedProduct.Id),
                        selectedProduct.ProductCode,
                        selectedProduct.ProductName,
                        selectedProduct.Email
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
                    $"{CurrentClient.surname} {CurrentClient.first_name} {CurrentClient.patronymic}?", "Подтверждение операции", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (messagebox == MessageBoxResult.Yes)
                {
                    MSDbConnection clientsConnectionDelete = new MSDbConnection();
                    using (clientsConnectionDelete.Connection)
                    {
                        try
                        {
                            clientsConnectionDelete.Connection.Open();
                            MSSQLLocalDBEntities clientsInfoDB = new MSSQLLocalDBEntities();
                            var clientsForDelete = clientsInfoDB.client_info.Where(e=>e.id==CurrentClient.id);
                            clientsInfoDB.client_info.Remove(clientsForDelete.First());
                            clientsInfoDB.SaveChanges();
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

                    MSDbConnection productsConnectionDelete = new MSDbConnection();
                    using (productsConnectionDelete.Connection)
                    {
                        try
                        {
                            productsConnectionDelete.Connection.Open();
                            ProductsDB products = new ProductsDB();
                            var productsForDelete = products.Products.Where(e=> e.Id== CurrentProduct.Id);
                            products.Products.Remove(productsForDelete.First());
                            products.SaveChanges();

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
            ProductsDB productsDB = new ProductsDB();
            productsDB.Products.Load();
            var selectedProducts = (CurrentClient != null) ? productsDB.Products.Where(e=>e.Email==CurrentClient.email) : null;
            ClientsProductsData = selectedProducts.ToList<Product>();
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
                    MSSQLLocalDBEntities client_Info = new MSSQLLocalDBEntities();
                    client_Info.client_info.Load();
                    var selectClients = client_Info.client_info;
                    ClientsData = selectClients.Local.ToList<client_info>();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        public void UpdateProducts()
        {
            MSDbConnection clientsConnection = new MSDbConnection();
            using (clientsConnection.Connection)
            {
                try
                {
                    Thread.Sleep(1000);
                    ProductsDB productsDB = new ProductsDB();
                    productsDB.Products.Load();
                    var selectedProducts = productsDB.Products.OrderBy(e=>e.Id);
                    ProductsData = selectedProducts.ToList<Product>();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
    }
}
