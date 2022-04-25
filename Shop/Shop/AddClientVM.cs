using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Shop
{
    class AddClientVM : Bindable
    {
        SqlDataAdapter dataAdapterMS;
        //DataTable dataTableMS;
        private Client addedClient;
        public Client AddedClient
        {
            get => addedClient;
            set
            {
                addedClient = value;
                OnPropertyChanged("AddedClient");
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
        public AddClientVM(MainWindowVM mainWindowVM)
        {
            AddedClient = new Client();
            SaveCommand = new RelayCommand(obj =>
            {
                if (AddedClient == null || 
                AddedClient.Surname == "" || 
                AddedClient.Surname == null || 
                AddedClient.FirstName == "" ||
                AddedClient.FirstName == null ||
                AddedClient.Email == "")
                { 
                    ErrorEnable = "Visible"; 
                    return; 
                }
                else
                {
                    ErrorEnable = "Hidden";
                  
                    var insertNewClient = $"insert into client_info " +
                    $"(surname, first_name, patronymic, phone_number, email) " +
                    $"values (N'{AddedClient.Surname}', " +
                    $"N'{AddedClient.FirstName}', " +
                    $"N'{AddedClient.Patronymic}'," +
                    $" N'{AddedClient.PhoneNumber}', " +
                    $"N'{AddedClient.Email}');";

                    MSDbConnection clientDBConnection = new MSDbConnection();
                    using (clientDBConnection.Connection)
                     {
                        try
                        {
                            clientDBConnection.Connection.Open();
                            //dataTableMS = new DataTable();
                            dataAdapterMS = new SqlDataAdapter();
                            dataAdapterMS.InsertCommand = new SqlCommand(insertNewClient, clientDBConnection.Connection);
                            dataAdapterMS.InsertCommand.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());
                        }
                    }
                    Window window = Application.Current.Windows.OfType<AddClientWindow>().SingleOrDefault(w => w.IsActive);
                    mainWindowVM.UpdateClients();
                    window.Close();
                }  
            }); 


        }
    }
}
