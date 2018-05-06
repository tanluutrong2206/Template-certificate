using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_To_MySql
{
    class DbConnect
    {
        private string MySQL_Conn_String = null;
        public DbConnect()
        {
            MySQL_Conn_String = ConfigurationManager.ConnectionStrings["MySql_FunixCertificate_ConnString"].ConnectionString;
        }

        public MySqlConnection GetConnection()
        {
            MySqlConnection  connection = new MySqlConnection(MySQL_Conn_String);
            connection.Open();
            return connection;
        }

        public void CloseConnection(MySqlConnection connection)
        {
            if (connection!= null)
            {
                connection.Close();
            }
        }
    }
}
