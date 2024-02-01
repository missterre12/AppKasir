using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// 1
using System.Data.SqlClient;

namespace AppKasir
{
    class Koneksi
    {
        // 2
        public SqlConnection GetConn()
        {
            SqlConnection Conn = new SqlConnection();
            Conn.ConnectionString = "Data Source=LAPTOP-1080RJAM\\SQLEXPRESS;Initial Catalog=DB_KASIR;Integrated Security=True";
            return Conn;
        }

        public void OpenConnection(SqlConnection conn)
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        public void CloseConnection(SqlConnection conn)
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }
}
