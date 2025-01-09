using GM_Shop.Areas.Admin.Models;
using GM_Shop.Database;
using GM_Shop.Models;
using Microsoft.Data.SqlClient;

namespace GM_Shop.DAL
{
    public class NhanVienDAL
    {
        DBConnect connect = new DBConnect();
        public NhanVien? GetNVById(int id)
        {
            connect.OpenConnection();
            NhanVien nhanVien = new NhanVien();
            bool hasData = false;

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
                        WHERE nv.UserID = @Id";

                command.CommandText = query;
                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    hasData = true;
                    nhanVien = new NhanVien()
                    {
                        MANV = Convert.ToInt32(reader["MANV"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
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
                        Role = new Role()
                        {
                            RoleID = Convert.ToInt32(reader["RoleID"]),
                            RoleName = reader["RoleName"].ToString() ?? ""
                        },
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
            return nhanVien;
        }
        //Đăng nhập
        public NhanVien? CheckEmployeeLogin(string username, string password)
        {
            connect.OpenConnection();
            NhanVien? nhanVien = null;

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
                                WHERE u.TAIKHOAN = @Username AND u.MATKHAU = @Password";

                command.CommandText = query;
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    nhanVien = new NhanVien()
                    {
                        MANV = Convert.ToInt32(reader["MANV"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        HONV = reader["HONV"].ToString() ?? "",
                        TENNV = reader["TENNV"].ToString() ?? "",
                        DIACHI = reader["DIACHI"].ToString() ?? "",
                        SDT = reader["SDT"].ToString() ?? "",
                        EMAIL = reader["EMAIL"].ToString() ?? "",
                        CCCD = reader["CCCD"].ToString() ?? "",
                        GIOITINH = reader["GIOITINH"].ToString() ?? "",
                        ANH = reader["ANH"].ToString() ?? "",
                        NGAYSINH = reader["NGAYSINH"] == DBNull.Value
                            ? null
                            : DateOnly.FromDateTime(Convert.ToDateTime(reader["NGAYSINH"])),
                        Role = new Role()
                        {
                            RoleID = Convert.ToInt32(reader["RoleID"]),
                            RoleName = reader["RoleName"].ToString() ?? ""
                        },
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
            return nhanVien;
        }
    }
}
