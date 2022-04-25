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
        private Client editClient;
        public Client EditClient
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
        public EditClientVM(MainWindowVM mainWindowVM, Client CurrentClient)
        {
            EditClient = new Client(CurrentClient.Id, CurrentClient.FirstName, CurrentClient.Surname, CurrentClient.Patronymic, CurrentClient.PhoneNumber, CurrentClient.Email);
            //EditClient.Id = CurrentClient.Id;
            SaveCommand = new RelayCommand(obj =>
            {
                if (EditClient == null ||
                EditClient.Surname == "" ||
                EditClient.Surname == null ||
                EditClient.FirstName == "" ||
                EditClient.FirstName == null ||
                EditClient.Email == "")
                {
                    ErrorEnable = "Visible";
                    return;
                }
                else
                {
                    ErrorEnable = "Hidden";

                    var updateClient = $"update client_info " +
                    $"set surname = N'{EditClient.Surname}'," +
                    $"first_name = N'{EditClient.FirstName}'," +
                    $"patronymic = N'{EditClient.Patronymic}'," +
                    $"phone_number = N'{EditClient.PhoneNumber}'," +
                    $"email = N'{EditClient.Email}'" +
                    $"where id={EditClient.Id}";

                    var updateClientProducts = $"update products set email = '{EditClient.Email}' where email='{CurrentClient.Email}'";

                    MSDbConnection clientDBConnection = new MSDbConnection();
                    using (clientDBConnection.Connection)
                    {
                        try
                        {
                            clientDBConnection.Connection.Open();
                            SqlDataAdapter dataAdapterMS = new SqlDataAdapter();
                            dataAdapterMS.UpdateCommand = new SqlCommand(updateClient, clientDBConnection.Connection);
                            dataAdapterMS.UpdateCommand.ExecuteNonQuery();
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
                            OleDbDataAdapter dataAdapterAccess = new OleDbDataAdapter();
                            dataAdapterAccess.UpdateCommand = new OleDbCommand(updateClientProducts, productsConnection.Connection);
                            dataAdapterAccess.UpdateCommand.ExecuteNonQuery();
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
