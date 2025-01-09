using GM_Shop.Areas.Admin.Models;
using GM_Shop.Database;
using Microsoft.Data.SqlClient;

namespace GM_Shop.Areas.Admin.DAL
{
    public class RoleAdminDAL
    {
        DBConnect connect = new DBConnect();
        public List<RoleAdmin> getAll()
        {
            connect.OpenConnection();

            List<RoleAdmin> list = new List<RoleAdmin>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"select * from role ";

                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    RoleAdmin role = new RoleAdmin()
                    {
                        RoleID = Convert.ToInt32(reader["RoleID"]),
                        RoleName = reader["RoleName"].ToString() ?? ""
                    };

                    list.Add(role);
                }
            }
            connect.CloseConnection();
            return list;
        }
        public bool AddNew(RoleAdmin roleAdd)
        {
            connect.OpenConnection();

            int id = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"INSERT INTO ROLE (RoleName) 
                                 VALUES (@roleName)";

                command.CommandText = query;

                // Thêm tham số cho câu truy vấn
                command.Parameters.AddWithValue("@roleName", roleAdd.RoleName);

                // In ra câu truy vấn để kiểm tra
                Console.WriteLine("command Insert Role: " + command.CommandText);

                id = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return id > 0;
        }
        public RoleAdmin getRoleById(int id)
        {
            connect.OpenConnection();

            RoleAdmin role = new RoleAdmin();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"select * from role where RoleID = " + id;

                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    role.RoleID = id;
                    role.RoleName = reader["RoleName"].ToString() ?? "";
                }
            }
            connect.CloseConnection();
            return role;
        }
        public bool updateRoleById(int id, RoleAdmin roleUpdate)
        {
            connect.OpenConnection();

            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"UPDATE ROLE 
                         SET RoleName = @roleName 
                         WHERE RoleID = @id";

                command.CommandText = query;

                // Cập nhật tham số cho câu truy vấn
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@roleName", roleUpdate.RoleName);

                Console.WriteLine("Command update Role: " + command.CommandText);

                isSuccess = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return isSuccess > 0;
        }
        public bool deleteRoleById(int id)
        {
            connect.OpenConnection();

            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"DELETE FROM role WHERE RoleID = @id";

                command.CommandText = query;

                command.Parameters.AddWithValue("@id", id);

                Console.WriteLine("Command delete Role: " + command.CommandText);

                isSuccess = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return isSuccess > 0;
        }
        public List<RoleAdmin> getRole_Pagination(int pageIndex, int pageSize)
        {
            connect.OpenConnection();

            List<RoleAdmin> list = new List<RoleAdmin>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @" 
                        SELECT * FROM (
                         SELECT ROW_NUMBER() OVER(ORDER BY RoleID asc) AS RowNumber,
                            RoleID, RoleName
                        from ROLE
     	                    ) TableResult
                        WHERE TableResult.RowNumber BETWEEN( @PageIndex -1) * @PageSize + 1 
                        AND @PageIndex * @PageSize ";


                command.CommandText = query;
                command.Parameters.AddWithValue("@PageIndex", pageIndex);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    RoleAdmin role = new RoleAdmin()
                    {
                        RoleID = Convert.ToInt32(reader["RoleID"]),
                        RoleName = reader["RoleName"].ToString() ?? ""
                    };
                    list.Add(role);
                }
            }
            connect.CloseConnection();

            return list;
        }
        public int getCountRow_Pagination(int pageIndex, int pageSize)
        {
            connect.OpenConnection();

            // khai báo biến lưu số lượng Row truy vấn được
            int count = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                //Câu truy vấn
                string query = @" 
                             SELECT Count(*) as CountRow
                             from ROLE";

                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    count = Convert.ToInt32(reader["CountRow"]);
                }
            }
            connect.CloseConnection();

            return count;
        }
    }
}
