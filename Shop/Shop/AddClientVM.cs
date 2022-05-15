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
        private client_info addedClient;
        public client_info AddedClient
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
            AddedClient = new client_info();
            SaveCommand = new RelayCommand(obj =>
            {
                if (AddedClient == null || 
                AddedClient.surname == "" || 
                AddedClient.surname == null || 
                AddedClient.first_name == "" ||
                AddedClient.first_name == null ||
                AddedClient.email == "")
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
                            clientsDB.client_info.Add(AddedClient);
                            clientsDB.SaveChanges();

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
