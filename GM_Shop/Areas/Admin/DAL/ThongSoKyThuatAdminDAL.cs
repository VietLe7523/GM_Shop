using GM_Shop.Areas.Admin.Models;
using GM_Shop.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GM_Shop.Areas.Admin.DAL
{
    public class ThongSoKyThuatAdminDAL
    {
        DBConnect connect = new DBConnect();
        // Lấy tất cả thông số kỹ thuật
        public List<ThongSoKyThuatAdmin> getAll()
        {
            connect.OpenConnection();

            List<ThongSoKyThuatAdmin> list = new List<ThongSoKyThuatAdmin>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = "SELECT * FROM THONGSOKYTHUAT";
                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ThongSoKyThuatAdmin tskt = new ThongSoKyThuatAdmin()
                    {
                        MATSKT = Convert.ToInt32(reader["MATSKT"]),
                        TENTSKT = reader["TENTSKT"].ToString() ?? "",
                        HEDIEUHANH = reader["HEDIEUHANH"].ToString() ?? "",
                        RAM = Convert.ToInt32(reader["RAM"]),
                        ROM = reader["ROM"].ToString() ?? "",
                        KICHCOMANHINH = Convert.ToSingle(reader["KICHCOMANHINH"]),
                        VIXULY = reader["VIXULY"].ToString() ?? "",
                        PIN = Convert.ToInt32(reader["PIN"]),
                        CAMERA = reader["CAMERA"].ToString() ?? ""
                    };
                    list.Add(tskt);
                }
            }
            connect.CloseConnection();
            return list;
        }
        // Thêm mới thông số kỹ thuật
        public bool AddNew(ThongSoKyThuatAdmin tskt)
        {
            connect.OpenConnection();

            int id = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"INSERT INTO THONGSOKYTHUAT (TENTSKT, HEDIEUHANH, RAM, ROM, KICHCOMANHINH, VIXULY, PIN, CAMERA)
                                 VALUES (@TENTSKT, @HEDIEUHANH, @RAM, @ROM, @KICHCOMANHINH, @VIXULY, @PIN, @CAMERA)";

                command.CommandText = query;
                command.Parameters.AddWithValue("@TENTSKT", tskt.TENTSKT);
                command.Parameters.AddWithValue("@HEDIEUHANH", tskt.HEDIEUHANH);
                command.Parameters.AddWithValue("@RAM", tskt.RAM);
                command.Parameters.AddWithValue("@ROM", tskt.ROM);
                command.Parameters.AddWithValue("@KICHCOMANHINH", tskt.KICHCOMANHINH);
                command.Parameters.AddWithValue("@VIXULY", tskt.VIXULY);
                command.Parameters.AddWithValue("@PIN", tskt.PIN);
                command.Parameters.AddWithValue("@CAMERA", tskt.CAMERA);

                Console.WriteLine("command Insert tskt: " + command.CommandText);

                id = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return id > 0;
        }
        // Lấy thông số kỹ thuật theo ID
        public ThongSoKyThuatAdmin getTSKTById(int id)
        {
            connect.OpenConnection();

            ThongSoKyThuatAdmin tskt = new ThongSoKyThuatAdmin();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = "SELECT * FROM THONGSOKYTHUAT WHERE MATSKT = " + id;
                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tskt.MATSKT = id;
                    tskt.TENTSKT = reader["TENTSKT"].ToString() ?? "";
                    tskt.HEDIEUHANH = reader["HEDIEUHANH"].ToString() ?? "";
                    tskt.RAM = Convert.ToInt32(reader["RAM"]);
                    tskt.ROM = reader["ROM"].ToString() ?? "";
                    tskt.KICHCOMANHINH = Convert.ToSingle(reader["KICHCOMANHINH"]);
                    tskt.VIXULY = reader["VIXULY"].ToString() ?? "";
                    tskt.PIN = Convert.ToInt32(reader["PIN"]);
                    tskt.CAMERA = reader["CAMERA"].ToString() ?? "";
                }
            }
            connect.CloseConnection();
            return tskt;
        }
        // Cập nhật thông số kỹ thuật
        public bool UpdateTSKTById(int id, ThongSoKyThuatAdmin tskt)
        {
            connect.OpenConnection();

            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"UPDATE THONGSOKYTHUAT 
                                 SET TENTSKT = @TENTSKT, HEDIEUHANH = @HEDIEUHANH, RAM = @RAM, ROM = @ROM, 
                                     KICHCOMANHINH = @KICHCOMANHINH, VIXULY = @VIXULY, PIN = @PIN, CAMERA = @CAMERA
                                 WHERE MATSKT = @id";

                command.CommandText = query;

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@TENTSKT", tskt.TENTSKT);
                command.Parameters.AddWithValue("@HEDIEUHANH", tskt.HEDIEUHANH);
                command.Parameters.AddWithValue("@RAM", tskt.RAM);
                command.Parameters.AddWithValue("@ROM", tskt.ROM);
                command.Parameters.AddWithValue("@KICHCOMANHINH", tskt.KICHCOMANHINH);
                command.Parameters.AddWithValue("@VIXULY", tskt.VIXULY);
                command.Parameters.AddWithValue("@PIN", tskt.PIN);
                command.Parameters.AddWithValue("@CAMERA", tskt.CAMERA);

                Console.WriteLine("Command update tskt: " + command.CommandText);

                isSuccess = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return isSuccess > 0;
        }
        // Xóa thông số kỹ thuật
        public bool DeleteTSKTById(int id)
        {
            connect.OpenConnection();

            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = "DELETE FROM THONGSOKYTHUAT WHERE MATSKT = @id";
                command.CommandText = query;

                command.Parameters.AddWithValue("@id", id);
                Console.WriteLine("Command delete tskt: " + command.CommandText);


                isSuccess = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return isSuccess > 0;
        }
        public List<ThongSoKyThuatAdmin> getThongSoKyThuat_Pagination(int pageIndex, int pageSize)
        {
            connect.OpenConnection();

            List<ThongSoKyThuatAdmin> list = new List<ThongSoKyThuatAdmin>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @" 
                        SELECT * FROM (
                         SELECT ROW_NUMBER() OVER(ORDER BY MATSKT asc) AS RowNumber,
                            MATSKT, TENTSKT, HEDIEUHANH, RAM, ROM, KICHCOMANHINH, VIXULY, PIN, CAMERA
                        from THONGSOKYTHUAT
     	                    ) TableResult
                        WHERE TableResult.RowNumber BETWEEN( @PageIndex -1) * @PageSize + 1 
                        AND @PageIndex * @PageSize ";


                command.CommandText = query;
                command.Parameters.AddWithValue("@PageIndex", pageIndex);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ThongSoKyThuatAdmin tskt = new ThongSoKyThuatAdmin()
                    {
                        MATSKT = Convert.ToInt32(reader["MATSKT"]),
                        TENTSKT = reader["TENTSKT"].ToString() ?? "",
                        HEDIEUHANH = reader["HEDIEUHANH"].ToString() ?? "",
                        RAM = Convert.ToInt32(reader["RAM"]),
                        ROM = reader["ROM"].ToString() ?? "",
                        KICHCOMANHINH = Convert.ToSingle(reader["KICHCOMANHINH"]),
                        VIXULY = reader["VIXULY"].ToString() ?? "",
                        PIN = Convert.ToInt32(reader["PIN"]),
                        CAMERA = reader["CAMERA"].ToString() ?? ""
                    };
                    list.Add(tskt);
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
                             from THONGSOKYTHUAT";

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
