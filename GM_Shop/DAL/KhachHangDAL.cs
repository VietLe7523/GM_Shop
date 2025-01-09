using GM_Shop.Areas.Admin.Models;
using GM_Shop.Database;
using GM_Shop.Models;
using Microsoft.Data.SqlClient;

namespace GM_Shop.DAL
{
    public class KhachHangDAL
    {
        DBConnect connect = new DBConnect();

        // Lấy thông tin khách hàng theo MAKH
        public KhachHang? GetCustomerById(int id)
        {
            connect.OpenConnection();
            KhachHang customer = new KhachHang();
            bool hasData = false;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"SELECT kh.MAKH, kh.HOKH, kh.TENKH, kh.DIACHI, kh.SDT, kh.EMAIL, kh.CCCD,
                                        kh.ANH, kh.NGAYSINH, kh.GIOITINH,
                                        kh.UserID, u.TAIKHOAN, u.MATKHAU
                                FROM KHACHHANG kh
                                JOIN USERS u ON kh.UserID = u.UserID
                                WHERE kh.UserID = @Id";

                command.CommandText = query;
                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    hasData = true;
                    customer = new KhachHang()
                    {
                        MAKH = Convert.ToInt32(reader["MAKH"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        HOKH = reader["HOKH"].ToString() ?? "",
                        TENKH = reader["TENKH"].ToString() ?? "",
                        DIACHI = reader["DIACHI"].ToString() ?? "",
                        SDT = reader["SDT"].ToString() ?? "",
                        GIOITINH = reader["GIOITINH"].ToString() ?? "",
                        EMAIL = reader["EMAIL"].ToString() ?? "",
                        ANH = reader["ANH"].ToString() ?? "",
                        NGAYSINH = reader["NGAYSINH"] == DBNull.Value
                            ? null
                            : DateOnly.FromDateTime(Convert.ToDateTime(reader["NGAYSINH"])),
                        CCCD = reader["CCCD"].ToString() ?? "",
                        User = new User()
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            TAIKHOAN = reader["TAIKHOAN"].ToString() ?? "",
                            MATKHAU = reader["MATKHAU"].ToString() ?? ""
                        }
                    };
                }
            }

            connect.CloseConnection();

            if (!hasData) return null;
            return customer;
        }

        // Cập nhật thông tin khách hàng
        public bool UpdateCustomerDetails(KhachHang customerUpdate, int maKH)
        {
            connect.OpenConnection();

            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"UPDATE KHACHHANG
                                 SET HOKH = @HOKH, TENKH = @TENKH, DIACHI = @DIACHI, 
                                     SDT = @SDT, EMAIL = @EMAIL, ANH = @ANH, GIOITINH = @GIOITINH,
                                     NGAYSINH = @NGAYSINH, CCCD = @CCCD
                                 WHERE MAKH = @MAKH";

                command.CommandText = query;

                command.Parameters.AddWithValue("@MAKH", maKH);
                command.Parameters.AddWithValue("@HOKH", customerUpdate.HOKH);
                command.Parameters.AddWithValue("@TENKH", customerUpdate.TENKH);
                command.Parameters.AddWithValue("@DIACHI", customerUpdate.DIACHI);
                command.Parameters.AddWithValue("@SDT", customerUpdate.SDT);
                command.Parameters.AddWithValue("@EMAIL", customerUpdate.EMAIL);
                command.Parameters.AddWithValue("@GIOITINH", customerUpdate.GIOITINH);
                command.Parameters.AddWithValue("@ANH", customerUpdate.ANH ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@NGAYSINH", customerUpdate.NGAYSINH.HasValue
                    ? customerUpdate.NGAYSINH.Value.ToDateTime(TimeOnly.MinValue).Date
                    : (object)DBNull.Value);
                command.Parameters.AddWithValue("@CCCD", customerUpdate.CCCD);

                isSuccess = command.ExecuteNonQuery();
            }

            connect.CloseConnection();
            return isSuccess > 0;
        }

        //Đăng ký
        public bool SignUp(KhachHang khachHangNew)
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
        //Đăng nhập
        public KhachHang? CheckCustomerLogin(string username, string password)
        {
            connect.OpenConnection();
            KhachHang? khachHang = null;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"SELECT kh.MAKH, kh.HOKH, kh.TENKH, kh.DIACHI, kh.SDT, kh.EMAIL, kh.CCCD,
                                        kh.ANH, kh.NGAYSINH, kh.GIOITINH,
                                        kh.UserID, u.TAIKHOAN, u.MATKHAU
                                FROM KHACHHANG kh
                                JOIN USERS u ON kh.UserID = u.UserID
                                WHERE u.TAIKHOAN = @Username AND u.MATKHAU = @Password";

                command.CommandText = query;
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    khachHang = new KhachHang()
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
                        User = new User
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            TAIKHOAN = reader["TAIKHOAN"].ToString() ?? "",
                            MATKHAU = reader["MATKHAU"].ToString() ?? ""
                        }
                    };
                }
            }

            connect.CloseConnection();
            return khachHang;
        }
        //Đổi mật khẩu
        public bool ChangePassword(string username, string currentPassword, string newPassword)
        {
            connect.OpenConnection();

            bool isPasswordChanged = false;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                // Kiểm tra mật khẩu hiện tại
                string checkPasswordQuery = @"SELECT * 
                                      FROM USERS 
                                      WHERE TAIKHOAN = @Username AND MATKHAU = @CurrentPassword";

                command.CommandText = checkPasswordQuery;
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@CurrentPassword", currentPassword);

                int matchingRows = Convert.ToInt32(command.ExecuteScalar());

                if (matchingRows > 0)
                {
                    // Nếu mật khẩu cũ khớp, cập nhật mật khẩu mới
                    string updatePasswordQuery = @"UPDATE USERS 
                                           SET MATKHAU = @NewPassword 
                                           WHERE TAIKHOAN = @Username";

                    command.CommandText = updatePasswordQuery;
                    command.Parameters.Clear(); // Xóa các tham số cũ
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@NewPassword", newPassword);

                    int rowsAffected = command.ExecuteNonQuery();
                    isPasswordChanged = rowsAffected > 0; // Thành công nếu có ít nhất 1 dòng bị ảnh hưởng
                }
            }

            connect.CloseConnection();
            return isPasswordChanged;
        }


    }
}
