using GM_Shop.Areas.Admin.Models;
using GM_Shop.Database;
using Microsoft.Data.SqlClient;

namespace GM_Shop.Areas.Admin.DAL
{
    public class DanhMucSPAdminDAL
    {
        DBConnect connect = new DBConnect();

        // Lấy danh sách tất cả danh mục sản phẩm
        public List<DanhMucSPAdmin> getAll()
        {
            connect.OpenConnection();

            List<DanhMucSPAdmin> list = new List<DanhMucSPAdmin>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"SELECT * FROM DANHMUCSP";
                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DanhMucSPAdmin danhMuc = new DanhMucSPAdmin()
                    {
                        MADMSP = Convert.ToInt32(reader["MADMSP"]),
                        TENDMSP = reader["TENDMSP"].ToString() ?? ""
                    };

                    list.Add(danhMuc);
                }
            }
            connect.CloseConnection();
            return list;
        }

        // Thêm mới danh mục sản phẩm
        public bool AddNew(DanhMucSPAdmin danhMuc)
        {
            connect.OpenConnection();

            int id = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"INSERT INTO DANHMUCSP (TENDMSP) 
                                 VALUES (@TENDMSP)";

                command.CommandText = query;

                command.Parameters.AddWithValue("@TENDMSP", danhMuc.TENDMSP);

                Console.WriteLine("command Insert DMSP: " + command.CommandText);

                id = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return id > 0;
        }

        // Lấy danh mục sản phẩm theo mã
        public DanhMucSPAdmin getDanhMucById(int id)
        {
            connect.OpenConnection();

            DanhMucSPAdmin danhMuc = new DanhMucSPAdmin();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"SELECT * FROM DANHMUCSP WHERE MADMSP = " + id;

                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    danhMuc.MADMSP = id;
                    danhMuc.TENDMSP = reader["TENDMSP"].ToString() ?? "";
                }
            }
            connect.CloseConnection();
            return danhMuc;
        }

        // Cập nhật danh mục sản phẩm
        public bool updateDanhMucById(int id, DanhMucSPAdmin danhMuc)
        {
            connect.OpenConnection();

            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"UPDATE DANHMUCSP 
                                 SET TENDMSP = @TENDMSP 
                                 WHERE MADMSP = @id";

                command.CommandText = query;

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@TENDMSP", danhMuc.TENDMSP);

                Console.WriteLine("Command update DMSP: " + command.CommandText);

                isSuccess = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return isSuccess > 0;
        }

        // Xóa danh mục sản phẩm
        public bool deleteDanhMucById(int id)
        {
            connect.OpenConnection();

            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"DELETE FROM DANHMUCSP WHERE MADMSP = @id";

                command.CommandText = query;
                command.Parameters.AddWithValue("@id", id);

                Console.WriteLine("Command delete DMSP: " + command.CommandText);

                isSuccess = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return isSuccess > 0;
        }
        public List<DanhMucSPAdmin> getDanhMuc_Pagination(int pageIndex, int pageSize)
        {
            connect.OpenConnection();

            List<DanhMucSPAdmin> list = new List<DanhMucSPAdmin>();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @" 
                        SELECT * FROM (
                         SELECT ROW_NUMBER() OVER(ORDER BY MADMSP asc) AS RowNumber,
                            MADMSP,TENDMSP
                        FROM DANHMUCSP
     	                    ) TableResult
                        WHERE TableResult.RowNumber BETWEEN( @PageIndex -1) * @PageSize + 1 
                         AND @PageIndex * @PageSize ";


                command.CommandText = query;
                command.Parameters.AddWithValue("@PageIndex", pageIndex);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DanhMucSPAdmin danhMuc = new DanhMucSPAdmin()
                    {
                        MADMSP = Convert.ToInt32(reader["MADMSP"]),
                        TENDMSP = reader["TENDMSP"].ToString() ?? ""
                    };

                    list.Add(danhMuc);
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
                             FROM DANHMUCSP";

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
