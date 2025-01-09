using Microsoft.Data.SqlClient;

namespace GM_Shop.Database
{
    public class DBConnect
    {
        // Chuỗi kết nối sửa đúng định dạng
        private readonly string _connectionString = "Data Source=DESKTOP-7813HKV;Initial Catalog=GM_Shop;Integrated Security=True;TrustServerCertificate=False;Encrypt=False";

        private SqlConnection? _connect;

        // Tạo kết nối
        public SqlConnection GetConnection()
        {
            if (_connect == null)
            {
                _connect = new SqlConnection(_connectionString);
            }
            return _connect;
        }

        // Hàm mở kết nối
        public void OpenConnection()
        {
            var connection = GetConnection();
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        // Hàm đóng kết nối
        public void CloseConnection()
        {
            var connection = GetConnection();
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
