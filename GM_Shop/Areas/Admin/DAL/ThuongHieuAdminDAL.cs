using GM_Shop.Areas.Admin.Models;
using GM_Shop.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GM_Shop.Areas.Admin.DAL
{
    public class ThuongHieuAdminDAL
    {
        DBConnect connect = new DBConnect();

        // Lấy danh sách tất cả thương hiệu
        public List<ThuongHieuAdmin> getAll()
        {
            connect.OpenConnection();

            List<ThuongHieuAdmin> list = new List<ThuongHieuAdmin>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"SELECT * FROM THUONGHIEU";
                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ThuongHieuAdmin thuongHieu = new ThuongHieuAdmin()
                    {
                        MATH = Convert.ToInt32(reader["MATH"]),
                        TENTHUONGHIEU = reader["TENTHUONGHIEU"].ToString() ?? "",
                        QUOCGIA = reader["QUOCGIA"].ToString() ?? ""
                    };

                    list.Add(thuongHieu);
                }
            }
            connect.CloseConnection();
            return list;
        }
        // Thêm mới thương hiệu
        public bool AddNew(ThuongHieuAdmin thuongHieu)
        {
            connect.OpenConnection();

            int isInserted = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"INSERT INTO THUONGHIEU (TENTHUONGHIEU, QUOCGIA) 
                                 VALUES (@TENTHUONGHIEU, @QUOCGIA)";

                command.CommandText = query;
                command.Parameters.AddWithValue("@TENTHUONGHIEU", thuongHieu.TENTHUONGHIEU);
                command.Parameters.AddWithValue("@QUOCGIA", thuongHieu.QUOCGIA);
                Console.WriteLine("command Insert Thuong hieu: " + command.CommandText);
                isInserted = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return isInserted > 0;
        }
        // Lấy thương hiệu theo mã
        public ThuongHieuAdmin getThuongHieuById(int id)
        {
            connect.OpenConnection();

            ThuongHieuAdmin thuongHieu = new ThuongHieuAdmin();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"SELECT * FROM ThuongHieu WHERE MATH = " + id;
                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    thuongHieu.MATH = id; 
                    thuongHieu.TENTHUONGHIEU = reader["TENTHUONGHIEU"].ToString() ?? "";
                    thuongHieu.QUOCGIA = reader["QUOCGIA"].ToString() ?? "";
                }
            }
            connect.CloseConnection();
            return thuongHieu;
        }
        // Cập nhật thương hiệu
        public bool updateThuongHieuById(int id, ThuongHieuAdmin thuongHieu)
        {
            connect.OpenConnection();

            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"UPDATE ThuongHieu 
                                 SET TENTHUONGHIEU = @TENTHUONGHIEU, QUOCGIA = @QUOCGIA
                                 WHERE MATH = @id";

                command.CommandText = query;
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@TENTHUONGHIEU", thuongHieu.TENTHUONGHIEU);
                command.Parameters.AddWithValue("@QUOCGIA", thuongHieu.QUOCGIA);

                Console.WriteLine("Command update THUONG HIEU: " + command.CommandText);

                isSuccess = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return isSuccess > 0;
        }

        // Xóa thương hiệu
        public bool deleteThuongHieuById(int id)
        {
            connect.OpenConnection();

            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"DELETE FROM THUONGHIEU WHERE MATH = @id";

                command.CommandText = query;
                command.Parameters.AddWithValue("@id", id);

                Console.WriteLine("Command delete thuong hieu: " + command.CommandText);

                isSuccess = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return isSuccess > 0;
        }
        public List<ThuongHieuAdmin> getThuongHieu_Pagination(int pageIndex, int pageSize)
        {
            connect.OpenConnection();

            List<ThuongHieuAdmin> list = new List<ThuongHieuAdmin>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @" 
                        SELECT * FROM (
                         SELECT ROW_NUMBER() OVER(ORDER BY MATH asc) AS RowNumber,
                            MATH, TENTHUONGHIEU, QUOCGIA
                        from THUONGHIEU
     	                    ) TableResult
                        WHERE TableResult.RowNumber BETWEEN( @PageIndex -1) * @PageSize + 1 
                        AND @PageIndex * @PageSize ";


                command.CommandText = query;
                command.Parameters.AddWithValue("@PageIndex", pageIndex);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ThuongHieuAdmin thuongHieu = new ThuongHieuAdmin()
                    {
                        MATH = Convert.ToInt32(reader["MATH"]),
                        TENTHUONGHIEU = reader["TENTHUONGHIEU"].ToString() ?? "",
                        QUOCGIA = reader["QUOCGIA"].ToString() ?? ""
                    };

                    list.Add(thuongHieu);
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
                             from THUONGHIEU";

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
