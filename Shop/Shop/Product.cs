using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    class Product
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Email { get; set; }
        public Product()
        { }

        public Product(int Id, string ProductCode, string ProductName, string Email)
        {
            this.Id = Id;
            this.ProductCode = ProductCode;
            this.ProductName = ProductName;
            this.Email = Email;
        }
    }
}
