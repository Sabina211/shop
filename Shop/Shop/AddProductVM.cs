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
    class AddProductVM : Bindable 
    {
        private Product addedProduct;
        public Product AddedProduct
        {
            get => addedProduct;
            set
            {
                addedProduct = value;
                OnPropertyChanged("AddedProduct");
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
        public AddProductVM(MainWindowVM mainWindowVM)
        {
            AddedProduct = new Product();
            SaveCommand = new RelayCommand(obj =>
            {
                if (AddedProduct == null ||
                AddedProduct.Email == null || AddedProduct.Email == "" ||
                AddedProduct.ProductCode == null || AddedProduct.ProductCode == "" ||
                AddedProduct.ProductName == null || AddedProduct.ProductName == "")
                {
                    ErrorEnable = "Visible";
                    return;
                }
                else
                {
                    ErrorEnable = "Hidden";
                    MSDbConnection productDBConnection = new MSDbConnection();
                    using (productDBConnection.Connection)
                    {
                        try
                        {
                            productDBConnection.Connection.Open();
                            ProductsDB productsDB = new ProductsDB();
                            productsDB.Products.Add(AddedProduct);
                            productsDB.SaveChanges();

                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.ToString());
                        }
                    }
                    Window window = Application.Current.Windows.OfType<AddProductWindow>().SingleOrDefault(w => w.IsActive);
                    mainWindowVM.UpdateProducts();
                    mainWindowVM.FillClientsProducts();
                    window.Close();
                }
            });
        }
    }
}
