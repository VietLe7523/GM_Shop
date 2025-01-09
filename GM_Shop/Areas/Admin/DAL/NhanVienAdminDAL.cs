using GM_Shop.Areas.Admin.Models;
using GM_Shop.Database;
using Microsoft.Data.SqlClient;

namespace GM_Shop.Areas.Admin.DAL
{
    public class NhanVienAdminDAL
    {
        DBConnect connect = new DBConnect();

        // Lấy danh sách tất cả nhân viên
        public List<NhanVienAdmin> GetAll()
        {
            connect.OpenConnection();
            List<NhanVienAdmin> list = new List<NhanVienAdmin>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"SELECT nv.MANV, nv.HONV, nv.TENNV, nv.DIACHI, nv.SDT, nv.EMAIL, nv.CCCD, 
                                        nv.ANH, nv.NGAYSINH, nv.GIOITINH, 
                                        nv.RoleID, r.RoleName,
                                        nv.UserID, u.TAIKHOAN, u.MATKHAU
                                FROM NHANVIEN nv
                                JOIN USERS u ON nv.UserID = u.UserID
                                JOIN ROLE r ON nv.RoleID = r.RoleID";

                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    NhanVienAdmin nhanVien = new NhanVienAdmin()
                    {
                        MANV = Convert.ToInt32(reader["MANV"]),
                        UserId = Convert.ToInt32(reader["UserID"]),
                        HONV = reader["HONV"].ToString() ?? "",
                        TENNV = reader["TENNV"].ToString() ?? "",
                        DIACHI = reader["DIACHI"].ToString() ?? "",
                        SDT = reader["SDT"].ToString() ?? "",
                        EMAIL = reader["EMAIL"].ToString() ?? "",
                        CCCD = reader["CCCD"].ToString() ?? "",
                        GIOITINH = reader["GIOITINH"].ToString() ?? "",
                        ANH = reader["ANH"].ToString() ?? "",
                        NGAYSINH = reader["NGAYSINH"] == DBNull.Value ? null : DateOnly.FromDateTime(Convert.ToDateTime(reader["NGAYSINH"])),
                        RoleID = Convert.ToInt32(reader["RoleID"]),
                        Role = new RoleAdmin()
                        {
                            RoleID = Convert.ToInt32(reader["RoleID"]),
                            RoleName = reader["RoleName"].ToString() ?? ""
                        },
                        User = new UserAdmin()
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            TAIKHOAN = reader["TAIKHOAN"].ToString() ?? "",
                            MATKHAU = reader["MATKHAU"].ToString() ?? ""
                        }
                    };
                    list.Add(nhanVien);
                }
            }

            connect.CloseConnection();
            return list;
        }
        // Thêm mới nhân viên
        public bool AddNew(NhanVienAdmin nhanVienNew)
        {
            if (nhanVienNew.User == null) throw new ArgumentNullException(nameof(nhanVienNew.User), "User cannot be null");

            connect.OpenConnection();
            int id = 0;

            // 1. Thêm tài khoản vào bảng USERS
            int? userId = null;
            using (SqlCommand userCommand = new SqlCommand())
            {
                userCommand.Connection = connect.GetConnection();
                userCommand.CommandType = System.Data.CommandType.Text;

                string userQuery = @"INSERT INTO USERS (TAIKHOAN, MATKHAU)
                                    VALUES (@TAIKHOAN, @MATKHAU);
                                    SELECT SCOPE_IDENTITY();";

                userCommand.CommandText = userQuery;
                userCommand.Parameters.AddWithValue("@TAIKHOAN", nhanVienNew.User.TAIKHOAN);
                userCommand.Parameters.AddWithValue("@MATKHAU", nhanVienNew.User.MATKHAU);

                var result = userCommand.ExecuteScalar();
                if (result != null)
                {
                    userId = Convert.ToInt32(result);
                }
            }

            if (userId.HasValue)
            {
                // 2. Thêm nhân viên vào bảng NHANVIEN
                using (SqlCommand staffCommand = new SqlCommand())
                {
                    staffCommand.Connection = connect.GetConnection();
                    staffCommand.CommandType = System.Data.CommandType.Text;

                    string staffQuery = @"INSERT INTO NHANVIEN (HONV, TENNV, DIACHI, SDT, EMAIL, CCCD, ANH, NGAYSINH, GIOITINH, RoleID, UserID)
                                        VALUES (@HONV, @TENNV, @DIACHI, @SDT, @EMAIL, @CCCD, @ANH, @NGAYSINH, @GIOITINH, @RoleID, @UserID);";

                    staffCommand.CommandText = staffQuery;

                    staffCommand.Parameters.AddWithValue("@HONV", nhanVienNew.HONV);
                    staffCommand.Parameters.AddWithValue("@TENNV", nhanVienNew.TENNV);
                    staffCommand.Parameters.AddWithValue("@DIACHI", nhanVienNew.DIACHI);
                    staffCommand.Parameters.AddWithValue("@SDT", nhanVienNew.SDT);
                    staffCommand.Parameters.AddWithValue("@EMAIL", nhanVienNew.EMAIL);
                    staffCommand.Parameters.AddWithValue("@CCCD", nhanVienNew.CCCD);
                    staffCommand.Parameters.AddWithValue("@ANH", nhanVienNew.ANH);
                    staffCommand.Parameters.AddWithValue("@NGAYSINH", nhanVienNew.NGAYSINH.HasValue ? nhanVienNew.NGAYSINH.Value.ToDateTime(TimeOnly.MinValue).Date : (object)DBNull.Value);
                    staffCommand.Parameters.AddWithValue("@GIOITINH", nhanVienNew.GIOITINH);
                    staffCommand.Parameters.AddWithValue("@RoleID", nhanVienNew.RoleID);
                    staffCommand.Parameters.AddWithValue("@UserID", userId);

                    id = staffCommand.ExecuteNonQuery();
                }
                connect.CloseConnection();
                return id > 0;
            }
            else
            {
                connect.CloseConnection();
                return false;
            }
        }
        // Lấy nhân viên theo mã nhân viên
        public NhanVienAdmin GetNVById(int manv)
        {
            connect.OpenConnection();
            NhanVienAdmin nhanVien = null;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"SELECT nv.MANV, nv.HONV, nv.TENNV, nv.DIACHI, nv.SDT, nv.EMAIL, nv.CCCD, 
                                nv.ANH, nv.NGAYSINH, nv.GIOITINH, 
                                nv.RoleID, r.RoleName,
                                nv.UserID, u.TAIKHOAN, u.MATKHAU
                        FROM NHANVIEN nv
                        JOIN USERS u ON nv.UserID = u.UserID
                        JOIN ROLE r ON nv.RoleID = r.RoleID
                        WHERE nv.MANV = @MANV";

                command.CommandText = query;
                command.Parameters.AddWithValue("@MANV", manv);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    nhanVien = new NhanVienAdmin()
                    {
                        MANV = Convert.ToInt32(reader["MANV"]),
                        UserId = Convert.ToInt32(reader["UserID"]),
                        HONV = reader["HONV"].ToString() ?? "",
                        TENNV = reader["TENNV"].ToString() ?? "",
                        DIACHI = reader["DIACHI"].ToString() ?? "",
                        SDT = reader["SDT"].ToString() ?? "",
                        EMAIL = reader["EMAIL"].ToString() ?? "",
                        CCCD = reader["CCCD"].ToString() ?? "",
                        GIOITINH = reader["GIOITINH"].ToString() ?? "",
                        ANH = reader["ANH"].ToString() ?? "",
                        NGAYSINH = reader["NGAYSINH"] == DBNull.Value ? null : DateOnly.FromDateTime(Convert.ToDateTime(reader["NGAYSINH"])),
                        RoleID = Convert.ToInt32(reader["RoleID"]),
                        Role = new RoleAdmin()
                        {
                            RoleID = Convert.ToInt32(reader["RoleID"]),
                            RoleName = reader["RoleName"].ToString() ?? ""
                        },
                        User = new UserAdmin()
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            TAIKHOAN = reader["TAIKHOAN"].ToString() ?? "",
                            MATKHAU = reader["MATKHAU"].ToString() ?? ""
                        }
                    };
                }
            }

            connect.CloseConnection();
            return nhanVien;
        }
        // Cập nhật thông tin nhân viên
        public bool UpdateNVById(int manv, NhanVienAdmin nhanVienUpdate)
        {
            connect.OpenConnection();
            int rowsAffected = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"UPDATE NHANVIEN
                         SET HONV = @HONV,
                             TENNV = @TENNV,
                             DIACHI = @DIACHI,
                             SDT = @SDT,
                             EMAIL = @EMAIL,
                             CCCD = @CCCD,
                             ANH = @ANH,
                             NGAYSINH = @NGAYSINH,
                             GIOITINH = @GIOITINH,
                             RoleID = @RoleID
                         WHERE MANV = @MANV;

                         UPDATE USERS
                         SET TAIKHOAN = @TAIKHOAN,
                             MATKHAU = @MATKHAU
                         WHERE UserID = (SELECT UserID FROM NHANVIEN WHERE MANV = @MANV);";

                command.CommandText = query;

                // Tham số cho bảng NHANVIEN
                command.Parameters.AddWithValue("@HONV", nhanVienUpdate.HONV);
                command.Parameters.AddWithValue("@TENNV", nhanVienUpdate.TENNV);
                command.Parameters.AddWithValue("@DIACHI", nhanVienUpdate.DIACHI);
                command.Parameters.AddWithValue("@SDT", nhanVienUpdate.SDT);
                command.Parameters.AddWithValue("@EMAIL", nhanVienUpdate.EMAIL);
                command.Parameters.AddWithValue("@CCCD", nhanVienUpdate.CCCD);
                command.Parameters.AddWithValue("@ANH", nhanVienUpdate.ANH);
                command.Parameters.AddWithValue("@NGAYSINH", nhanVienUpdate.NGAYSINH.HasValue ? nhanVienUpdate.NGAYSINH.Value.ToDateTime(TimeOnly.MinValue).Date : (object)DBNull.Value);
                command.Parameters.AddWithValue("@GIOITINH", nhanVienUpdate.GIOITINH);
                command.Parameters.AddWithValue("@RoleID", nhanVienUpdate.RoleID);
                command.Parameters.AddWithValue("@MANV", manv);

                // Tham số cho bảng USERS
                command.Parameters.AddWithValue("@TAIKHOAN", nhanVienUpdate.User?.TAIKHOAN ?? "");
                command.Parameters.AddWithValue("@MATKHAU", nhanVienUpdate.User?.MATKHAU ?? "");

                rowsAffected = command.ExecuteNonQuery();
            }

            connect.CloseConnection();
            return rowsAffected > 0;
        }
        public bool DeleteNhanVienAndUserById(int manv)
        {
            connect.OpenConnection();
            SqlTransaction transaction = connect.GetConnection().BeginTransaction();
            int isSuccess = 0;

            try
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connect.GetConnection();
                    command.Transaction = transaction; // Liên kết transaction với command
                    command.CommandType = System.Data.CommandType.Text;

                    // 1. Lấy UserID liên kết với nhân viên
                    string getUserIdQuery = "SELECT UserID FROM NHANVIEN WHERE MANV = @manv";
                    command.CommandText = getUserIdQuery;
                    command.Parameters.AddWithValue("@manv", manv);

                    object userIdObj = command.ExecuteScalar();
                    if (userIdObj == null)
                    {
                        throw new Exception("Nhân viên không tồn tại hoặc không có UserID liên kết.");
                    }
                    int userId = Convert.ToInt32(userIdObj);

                    // 2. Xóa nhân viên khỏi bảng NHANVIEN
                    string deleteNhanVienQuery = "DELETE FROM NHANVIEN WHERE MANV = @manv";
                    command.CommandText = deleteNhanVienQuery;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@manv", manv);
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
                Console.WriteLine($"Error deleting NhanVien and User: {ex.Message}");
                isSuccess = 0;
            }
            finally
            {
                connect.CloseConnection();
            }

            return isSuccess > 0; // Trả về true nếu cả hai lệnh xóa thành công
        }
        public List<NhanVienAdmin> getNhanVien_Pagination(int pageIndex, int pageSize)
        {
            connect.OpenConnection();

            List<NhanVienAdmin> list = new List<NhanVienAdmin>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @" 
                        SELECT * FROM (
                         SELECT ROW_NUMBER() OVER(ORDER BY nv.MANV asc) AS RowNumber,
                            nv.MANV, nv.HONV, nv.TENNV, nv.DIACHI, nv.SDT, nv.EMAIL, nv.CCCD, 
                            nv.ANH, nv.NGAYSINH, nv.GIOITINH, 
                            nv.RoleID, r.RoleName,
                            nv.UserID, u.TAIKHOAN, u.MATKHAU
                        FROM NHANVIEN nv
                        JOIN USERS u ON nv.UserID = u.UserID
                        JOIN ROLE r ON nv.RoleID = r.RoleID
     	                    ) TableResult
                        WHERE TableResult.RowNumber BETWEEN( @PageIndex -1) * @PageSize + 1 
                        AND @PageIndex * @PageSize ";


                command.CommandText = query;
                command.Parameters.AddWithValue("@PageIndex", pageIndex);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    NhanVienAdmin nhanVien = new NhanVienAdmin()
                    {
                        MANV = Convert.ToInt32(reader["MANV"]),
                        UserId = Convert.ToInt32(reader["UserID"]),
                        HONV = reader["HONV"].ToString() ?? "",
                        TENNV = reader["TENNV"].ToString() ?? "",
                        DIACHI = reader["DIACHI"].ToString() ?? "",
                        SDT = reader["SDT"].ToString() ?? "",
                        EMAIL = reader["EMAIL"].ToString() ?? "",
                        CCCD = reader["CCCD"].ToString() ?? "",
                        GIOITINH = reader["GIOITINH"].ToString() ?? "",
                        ANH = reader["ANH"].ToString() ?? "",
                        NGAYSINH = reader["NGAYSINH"] == DBNull.Value ? null : DateOnly.FromDateTime(Convert.ToDateTime(reader["NGAYSINH"])),
                        RoleID = Convert.ToInt32(reader["RoleID"]),
                        Role = new RoleAdmin()
                        {
                            RoleID = Convert.ToInt32(reader["RoleID"]),
                            RoleName = reader["RoleName"].ToString() ?? ""
                        },
                        User = new UserAdmin()
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            TAIKHOAN = reader["TAIKHOAN"].ToString() ?? "",
                            MATKHAU = reader["MATKHAU"].ToString() ?? ""
                        }
                    };
                    list.Add(nhanVien);
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
                             FROM NHANVIEN nv
                        JOIN USERS u ON nv.UserID = u.UserID
                        JOIN ROLE r ON nv.RoleID = r.RoleID";

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
