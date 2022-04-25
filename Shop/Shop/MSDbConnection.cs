using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    class MSDbConnection
    {
        public SqlConnectionStringBuilder StringBuilder { get; set; }

        public SqlConnection Connection { get; set; }
        public MSDbConnection()
        {
            StringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB", //имя сервера источника данных, к которому будем подключаться
                InitialCatalog = "MSSQLLocalDB", //файл, к которому планируем подключаться
                IntegratedSecurity = true, //способ авторизации
                Pooling = false
            };
            Connection = new SqlConnection { ConnectionString = StringBuilder.ConnectionString };
        }
    }
}
