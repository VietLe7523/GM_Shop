using GM_Shop.Areas.Admin.Models;
using GM_Shop.Database;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GM_Shop.Areas.Admin.DAL
{
    public class KhachHangAdminDAL
    {
        DBConnect connect = new DBConnect();
        // Lấy danh sách tất cả khách hàng
        public List<KhachHangAdmin> GetAll()
        {
            connect.OpenConnection();
            List<KhachHangAdmin> list = new List<KhachHangAdmin>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"SELECT kh.MAKH, kh.HOKH, kh.TENKH, kh.DIACHI, kh.SDT, kh.EMAIL, kh.CCCD,
                                        kh.ANH, kh.NGAYSINH, kh.GIOITINH,
                                        kh.UserID, u.TAIKHOAN, u.MATKHAU
                                FROM KHACHHANG kh
                                JOIN USERS u ON kh.UserID = u.UserID";

                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    KhachHangAdmin khachHang = new KhachHangAdmin()
                    {
                        MAKH = Convert.ToInt32(reader["MAKH"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        HOKH = reader["HOKH"].ToString() ?? "",
                        TENKH = reader["TENKH"].ToString() ?? "",
                        DIACHI = reader["DIACHI"].ToString() ?? "",
                        SDT = reader["SDT"].ToString() ?? "",
                        EMAIL = reader["EMAIL"].ToString() ?? "",
                        CCCD = reader["CCCD"].ToString() ?? "",
                        GIOITINH = reader["GIOITINH"].ToString() ?? "",
                        ANH = reader["ANH"].ToString() ?? "",
                        NGAYSINH = reader["NGAYSINH"] == DBNull.Value
                            ? null
                            : DateOnly.FromDateTime(Convert.ToDateTime(reader["NGAYSINH"])),
                        User = new UserAdmin
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            TAIKHOAN = reader["TAIKHOAN"].ToString() ?? "",
                            MATKHAU = reader["MATKHAU"].ToString() ?? ""
                        }
                    };
                    list.Add(khachHang);
                }
            }
            connect.CloseConnection();
            return list;
        }
        public bool AddNew(KhachHangAdmin khachHangNew)
        {
            if (khachHangNew.User == null) throw new ArgumentNullException(nameof(khachHangNew.User), "User cannot be null");

            connect.OpenConnection();
            int id = 0;

            // 1. Thêm tài khoản vào bảng USERS
            int? userId = null; // Khai báo userId là nullable int
            using (SqlCommand userCommand = new SqlCommand())
            {
                userCommand.Connection = connect.GetConnection();
                userCommand.CommandType = System.Data.CommandType.Text;

                string userQuery = @"INSERT INTO USERS (TAIKHOAN, MATKHAU)
                                    VALUES (@TAIKHOAN, @MATKHAU);
                                    SELECT SCOPE_IDENTITY();";// Sử dụng SELECT SCOPE_IDENTITY() để lấy ID vừa thêm";

                userCommand.CommandText = userQuery;

                userCommand.Parameters.AddWithValue("@TAIKHOAN", khachHangNew.User.TAIKHOAN);
                userCommand.Parameters.AddWithValue("@MATKHAU", khachHangNew.User.MATKHAU);

                // Lấy UserID vừa được thêm, xử lý với nullable int
                var result = userCommand.ExecuteScalar();

                if (result != null)
                {
                    userId = Convert.ToInt32(result); // Nếu có giá trị, gán vào userId
                }
            }
            if (userId.HasValue)
            {
                // 2. Thêm khách hàng vào bảng KHACHHANG
                using (SqlCommand customerCommand = new SqlCommand())
                {
                    customerCommand.Connection = connect.GetConnection();
                    customerCommand.CommandType = System.Data.CommandType.Text;

                    string customerQuery = @"
                                INSERT INTO KHACHHANG (HOKH, TENKH, DIACHI, SDT, EMAIL, CCCD, ANH, NGAYSINH, GIOITINH, UserID)
                                VALUES (@HOKH, @TENKH, @DIACHI, @SDT, @EMAIL, @CCCD, @ANH, @NGAYSINH, @GIOITINH, @UserID);";

                    customerCommand.CommandText = customerQuery;

                    customerCommand.Parameters.AddWithValue("@HOKH", khachHangNew.HOKH);
                    customerCommand.Parameters.AddWithValue("@TENKH", khachHangNew.TENKH);
                    customerCommand.Parameters.AddWithValue("@DIACHI", khachHangNew.DIACHI);
                    customerCommand.Parameters.AddWithValue("@SDT", khachHangNew.SDT);
                    customerCommand.Parameters.AddWithValue("@EMAIL", khachHangNew.EMAIL);
                    customerCommand.Parameters.AddWithValue("@CCCD", khachHangNew.CCCD);
                    customerCommand.Parameters.AddWithValue("@ANH", khachHangNew.ANH);
                    customerCommand.Parameters.AddWithValue("@NGAYSINH", khachHangNew.NGAYSINH.HasValue
                        ? khachHangNew.NGAYSINH.Value.ToDateTime(TimeOnly.MinValue).Date
                        : (object)DBNull.Value);
                    customerCommand.Parameters.AddWithValue("@GIOITINH", khachHangNew.GIOITINH);
                    customerCommand.Parameters.AddWithValue("@UserID", userId);

                    // Execute and get the number of affected rows
                    id = customerCommand.ExecuteNonQuery();
                }
                connect.CloseConnection();
                return id > 0; // Return true if at least one row was affected
            }
            else
            {
                // Handle case where userId is null
                connect.CloseConnection();
                return false; // If userId is not valid, return false
            }
        }

        public KhachHangAdmin GetKhachHangById(int Id)
        {
            connect.OpenConnection();
            KhachHangAdmin khachHang = new KhachHangAdmin();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                // Truy vấn SQL để lấy thông tin chi tiết khách hàng
                string query = @"SELECT kh.MAKH, kh.HOKH, kh.TENKH, kh.DIACHI, kh.SDT, kh.EMAIL, kh.CCCD,
                                        kh.ANH, kh.NGAYSINH, kh.GIOITINH,
                                        kh.UserID, u.TAIKHOAN, u.MATKHAU
                                FROM KHACHHANG kh
                                JOIN USERS u ON kh.UserID = u.UserID
                                WHERE kh.MAKH = @Id";

                command.CommandText = query;
                command.Parameters.AddWithValue("@Id", Id);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    khachHang = new KhachHangAdmin()
                    {
                        MAKH = Convert.ToInt32(reader["MAKH"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        HOKH = reader["HOKH"].ToString() ?? "",
                        TENKH = reader["TENKH"].ToString() ?? "",
                        DIACHI = reader["DIACHI"].ToString() ?? "",
                        SDT = reader["SDT"].ToString() ?? "",
                        EMAIL = reader["EMAIL"].ToString() ?? "",
                        CCCD = reader["CCCD"].ToString() ?? "",
                        GIOITINH = reader["GIOITINH"].ToString() ?? "",
                        ANH = reader["ANH"].ToString() ?? "",
                        NGAYSINH = reader["NGAYSINH"] == DBNull.Value
                            ? null
                            : DateOnly.FromDateTime(Convert.ToDateTime(reader["NGAYSINH"])),
                        User = new UserAdmin()
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            TAIKHOAN = reader["TAIKHOAN"].ToString() ?? "",
                            MATKHAU = reader["MATKHAU"].ToString() ?? "",
                        }
                    };
                }
            }

            connect.CloseConnection();
            return khachHang;
        }
        public bool UpdateKhachHangById(KhachHangAdmin khachHangNew, int Id)
        {
            connect.OpenConnection();
            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"
            UPDATE KHACHHANG
            SET HOKH = @HOKH,
                TENKH = @TENKH,
                DIACHI = @DIACHI,
                SDT = @SDT,
                EMAIL = @EMAIL,
                CCCD = @CCCD,
                ANH = @ANH,
                NGAYSINH = @NGAYSINH,
                GIOITINH = @GIOITINH
            WHERE MAKH = @Id;

            UPDATE USERS
            SET TAIKHOAN = @TAIKHOAN,
                MATKHAU = @MATKHAU
            WHERE UserID = (
                SELECT UserID FROM KHACHHANG WHERE MAKH = @Id
            );";

                command.CommandText = query;

                // Gán giá trị cho các tham số của KHACHHANG
                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@HOKH", khachHangNew.HOKH);
                command.Parameters.AddWithValue("@TENKH", khachHangNew.TENKH);
                command.Parameters.AddWithValue("@DIACHI", khachHangNew.DIACHI);
                command.Parameters.AddWithValue("@SDT", khachHangNew.SDT);
                command.Parameters.AddWithValue("@EMAIL", khachHangNew.EMAIL);
                command.Parameters.AddWithValue("@CCCD", khachHangNew.CCCD);
                command.Parameters.AddWithValue("@ANH", khachHangNew.ANH);
                command.Parameters.AddWithValue("@NGAYSINH", khachHangNew.NGAYSINH.HasValue
                    ? khachHangNew.NGAYSINH.Value.ToDateTime(new TimeOnly())
                    : (object)DBNull.Value);
                command.Parameters.AddWithValue("@GIOITINH", khachHangNew.GIOITINH);

                // Gán giá trị cho các tham số của USERS
                command.Parameters.AddWithValue("@TAIKHOAN", khachHangNew.User.TAIKHOAN);
                command.Parameters.AddWithValue("@MATKHAU", khachHangNew.User.MATKHAU);

                // Thực thi truy vấn
                isSuccess = command.ExecuteNonQuery();
            }

            connect.CloseConnection();
            return isSuccess > 0; // Trả về true nếu ít nhất một dòng bị ảnh hưởng
        }
        public bool DeleteCustomerAndUserById(int customerId)
        {
            connect.OpenConnection();
            SqlTransaction transaction = connect.GetConnection().BeginTransaction();
            int isSuccess = 0;

            try
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connect.GetConnection();
                    command.Transaction = transaction; // Ánh xạ transaction vào command
                    command.CommandType = System.Data.CommandType.Text;

                    // 1. Lấy UserID liên kết với khách hàng
                    string getUserIdQuery = "SELECT UserID FROM KHACHHANG WHERE MAKH = @customerId";
                    command.CommandText = getUserIdQuery;
                    command.Parameters.AddWithValue("@customerId", customerId);

                    object userIdObj = command.ExecuteScalar();
                    if (userIdObj == null)
                    {
                        throw new Exception("Khách hàng không tồn tại hoặc không có UserID liên kết.");
                    }
                    int userId = Convert.ToInt32(userIdObj);

                    // 2. Xóa khách hàng khỏi bảng KHACHHANG
                    string deleteCustomerQuery = "DELETE FROM KHACHHANG WHERE MAKH = @customerId";
                    command.CommandText = deleteCustomerQuery;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@customerId", customerId);
                    isSuccess += command.ExecuteNonQuery();

                    // 3. Xóa user khỏi bảng USERS
                    string deleteUserQuery = "DELETE FROM USERS WHERE UserID = @userId";
                    command.CommandText = deleteUserQuery;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@userId", userId);
                    isSuccess += command.ExecuteNonQuery();
                }

                // Commit transaction nếu tất cả thành công
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Rollback transaction nếu xảy ra lỗi
                transaction.Rollback();
                Console.WriteLine($"Error deleting customer and user: {ex.Message}");
                isSuccess = 0;
            }
            finally
            {
                connect.CloseConnection();
            }

            return isSuccess > 0; // Trả về true nếu cả hai lệnh xóa thành công
        }
        public List<KhachHangAdmin> getKhachHang_Pagination(int pageIndex, int pageSize)
        {
            connect.OpenConnection();

            List<KhachHangAdmin> list = new List<KhachHangAdmin>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @" 
                        SELECT * FROM (
                         SELECT ROW_NUMBER() OVER(ORDER BY kh.MAKH asc) AS RowNumber,
                            kh.MAKH, kh.HOKH, kh.TENKH, kh.DIACHI, kh.SDT, kh.EMAIL, kh.CCCD,
                            kh.ANH, kh.NGAYSINH, kh.GIOITINH,
                            kh.UserID, u.TAIKHOAN, u.MATKHAU
                         FROM KHACHHANG kh
                         JOIN USERS u ON kh.UserID = u.UserID
     	                    ) TableResult
                        WHERE TableResult.RowNumber BETWEEN( @PageIndex -1) * @PageSize + 1 
                        AND @PageIndex * @PageSize ";


                command.CommandText = query;
                command.Parameters.AddWithValue("@PageIndex", pageIndex);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    KhachHangAdmin khachHang = new KhachHangAdmin()
                    {
                        MAKH = Convert.ToInt32(reader["MAKH"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        HOKH = reader["HOKH"].ToString() ?? "",
                        TENKH = reader["TENKH"].ToString() ?? "",
                        DIACHI = reader["DIACHI"].ToString() ?? "",
                        SDT = reader["SDT"].ToString() ?? "",
                        EMAIL = reader["EMAIL"].ToString() ?? "",
                        CCCD = reader["CCCD"].ToString() ?? "",
                        GIOITINH = reader["GIOITINH"].ToString() ?? "",
                        ANH = reader["ANH"].ToString() ?? "",
                        NGAYSINH = reader["NGAYSINH"] == DBNull.Value
                            ? null
                            : DateOnly.FromDateTime(Convert.ToDateTime(reader["NGAYSINH"])),
                        User = new UserAdmin
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            TAIKHOAN = reader["TAIKHOAN"].ToString() ?? "",
                            MATKHAU = reader["MATKHAU"].ToString() ?? ""
                        }
                    };

                    list.Add(khachHang);
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
                             FROM KHACHHANG kh
                             JOIN USERS u ON kh.UserID = u.UserID";

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
