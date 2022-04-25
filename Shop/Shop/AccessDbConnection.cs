using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    class AccessDbConnection
    {
        public OleDbConnectionStringBuilder StringBuilder { get; set; } 

        public OleDbConnection Connection { get; set; }
        public AccessDbConnection()
        {
            StringBuilder = new OleDbConnectionStringBuilder()
            {
                Provider = "Microsoft.ACE.OLEDB.12.0",
                DataSource = "E:\\Сабина, курс\\Задание 17\\Shop\\AccessDB.accdb",
                PersistSecurityInfo = true
            };
            Connection = new OleDbConnection { ConnectionString = StringBuilder.ConnectionString };
        }

    }
}
