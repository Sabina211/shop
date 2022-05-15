using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Shop
{
    class EditClientVM : Bindable
    {
        private client_info editClient;
        public client_info EditClient
        {
            get => editClient;
            set
            {
                editClient = value;
                OnPropertyChanged("EditClient");
            }
        }
        private string errorEnable = "Hidden";
        public string ErrorEnable
        {
            get
            {
                return errorEnable;
            }
            set
            {
                errorEnable = value;
                OnPropertyChanged("ErrorEnable");
            }
        }
        public ICommand SaveCommand { get; }
        public EditClientVM(MainWindowVM mainWindowVM, client_info CurrentClient)
        {
            EditClient = new client_info(CurrentClient.id, CurrentClient.first_name, CurrentClient.surname, CurrentClient.patronymic, CurrentClient.phone_number, CurrentClient.email);
            //EditClient.Id = CurrentClient.Id;
            SaveCommand = new RelayCommand(obj =>
            {
                if (EditClient == null ||
                EditClient.surname == "" ||
                EditClient.surname == null ||
                EditClient.first_name == "" ||
                EditClient.first_name == null ||
                EditClient.email == "")
                {
                    ErrorEnable = "Visible";
                    return;
                }
                else
                {
                    ErrorEnable = "Hidden";
                    MSDbConnection clientDBConnection = new MSDbConnection();
                    using (clientDBConnection.Connection)
                    {
                        try
                        {
                            clientDBConnection.Connection.Open();
                            MSSQLLocalDBEntities clientsDB = new MSSQLLocalDBEntities();
                            var clientForUpdate = clientsDB.client_info.Where(e=>e.id == EditClient.id).First();
                            clientForUpdate.surname = EditClient.surname;
                            clientForUpdate.first_name = EditClient.first_name;
                            clientForUpdate.patronymic = EditClient.patronymic;
                            clientForUpdate.phone_number = EditClient.phone_number;
                            clientForUpdate.email = EditClient.email;
                            clientsDB.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());
                        }
                    }

                    AccessDbConnection productsConnection = new AccessDbConnection();
                    using (productsConnection.Connection)
                    {
                        try
                        {
                            productsConnection.Connection.Open();
                            ProductsDB productsDB = new ProductsDB();
                            var productsForUpdate = productsDB.Products.Where(e => e.Email == CurrentClient.email);
                            foreach (var item in productsForUpdate)
                            {
                                item.Email = EditClient.email;
                            }
                            productsDB.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());
                        }
                    }
                    Window window = Application.Current.Windows.OfType<EditClientwindow>().SingleOrDefault(w => w.IsActive);
                    mainWindowVM.UpdateClients();
                    mainWindowVM.UpdateProducts();
                    window.Close();
                }
            });
        }
    }
}
