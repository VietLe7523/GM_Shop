using GM_Shop.Areas.Admin.Models;
using GM_Shop.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GM_Shop.Areas.Admin.DAL
{
    public class SanPhamAdminDAL
    {
        DBConnect connect = new DBConnect();
        public List<SanPhamAdmin> getAll()
        {
            connect.OpenConnection();

            List<SanPhamAdmin> list = new List<SanPhamAdmin>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"
                                SELECT sp.MASP, sp.TENSP, sp.DONGIA, sp.SOLUONG, sp.MOTA, sp.ANH, sp.DANHGIA,
                                       sp.MADMSP, dm.TENDMSP,
                                       sp.MATH, th.TENTHUONGHIEU,
                                       sp.MATSKT, tskt.TENTSKT, tskt.HEDIEUHANH, tskt.RAM, tskt.ROM, tskt.KICHCOMANHINH, 
                                       tskt.VIXULY, tskt.PIN, tskt.CAMERA
                                FROM SANPHAM sp
                                JOIN DANHMUCSP dm ON sp.MADMSP = dm.MADMSP
                                JOIN THUONGHIEU th ON sp.MATH = th.MATH
                                JOIN THONGSOKYTHUAT tskt ON sp.MATSKT = tskt.MATSKT";

                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    SanPhamAdmin product = new SanPhamAdmin()
                    {
                        MASP = Convert.ToInt32(reader["MASP"]),
                        TENSP = reader["TENSP"].ToString() ?? "",
                        DONGIA = Convert.ToDecimal(reader["DONGIA"]),
                        SOLUONG = Convert.ToInt32(reader["SOLUONG"]),
                        MOTA = reader["MOTA"].ToString() ?? "",
                        ANH = reader["ANH"].ToString() ?? "",
                        DANHGIA = Convert.ToDouble(reader["DANHGIA"].ToString()),
                        MADMSP = Convert.ToInt32(reader["MADMSP"]),
                        MATH = Convert.ToInt32(reader["MATH"]),
                        MATSKT = Convert.ToInt32(reader["MATSKT"]),
                        DanhMucSP = new DanhMucSPAdmin()
                        {
                            MADMSP = Convert.ToInt32(reader["MADMSP"]),
                            TENDMSP = reader["TENDMSP"].ToString() ?? ""
                        },
                        ThuongHieu = new ThuongHieuAdmin()
                        {
                            MATH = Convert.ToInt32(reader["MATH"]),
                            TENTHUONGHIEU = reader["TENTHUONGHIEU"].ToString() ?? ""
                        },
                        ThongSoKyThuat = new ThongSoKyThuatAdmin()
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
                        }
                    };

                    list.Add(product);
                }
            }
            connect.CloseConnection();
            return list;
        }
        public bool AddNew(SanPhamFormAdmin sanPhamNew)
        {
            connect.OpenConnection();

            int id = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"
            INSERT INTO SanPham (TENSP, DONGIA, SOLUONG, MOTA, ANH, DANHGIA, MADMSP, MATH, MATSKT)
            VALUES (@TENSP, @DONGIA, @SOLUONG, @MOTA, @ANH, @DANHGIA, @MADMSP, @MATH, @MATSKT);";

                command.CommandText = query;

                command.Parameters.AddWithValue("@TENSP", sanPhamNew.TENSP);
                command.Parameters.AddWithValue("@DONGIA", sanPhamNew.DONGIA);
                command.Parameters.AddWithValue("@SOLUONG", sanPhamNew.SOLUONG);
                command.Parameters.AddWithValue("@MOTA", sanPhamNew.MOTA);
                command.Parameters.AddWithValue("@ANH", sanPhamNew.ANH); 
                command.Parameters.AddWithValue("@DANHGIA", sanPhamNew.DANHGIA); 
                command.Parameters.AddWithValue("@MADMSP", sanPhamNew.MADMSP);
                command.Parameters.AddWithValue("@MATH", sanPhamNew.MATH);
                command.Parameters.AddWithValue("@MATSKT", sanPhamNew.MATSKT);

                Console.WriteLine("Command Insert SanPham: " + command.CommandText);

                // Execute and get the number of affected rows
                id = command.ExecuteNonQuery();
            }

            connect.CloseConnection();
            return id > 0; // Return true if at least one row was affected
        }
        public SanPhamAdmin GetSPById(int Id)
        {
            connect.OpenConnection();
            SanPhamAdmin sanPham = new SanPhamAdmin();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                // Truy vấn SQL để lấy thông tin chi tiết sản phẩm
                string query = @"
                                SELECT sp.MASP, sp.TENSP, sp.DONGIA, sp.SOLUONG, sp.MOTA, sp.ANH, sp.DANHGIA,
                                       sp.MADMSP, dm.TENDMSP,
                                       sp.MATH, th.TENTHUONGHIEU,
                                       sp.MATSKT, tskt.TENTSKT, tskt.HEDIEUHANH, tskt.RAM, tskt.ROM, tskt.KICHCOMANHINH, 
                                       tskt.VIXULY, tskt.PIN, tskt.CAMERA
                                FROM SANPHAM sp
                                JOIN DANHMUCSP dm ON sp.MADMSP = dm.MADMSP
                                JOIN THUONGHIEU th ON sp.MATH = th.MATH
                                JOIN THONGSOKYTHUAT tskt ON sp.MATSKT = tskt.MATSKT
                                WHERE sp.MASP = @Id";

                command.CommandText = query;
                command.Parameters.AddWithValue("@Id", Id);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    sanPham = new SanPhamAdmin()
                    {
                        MASP = Convert.ToInt32(reader["MASP"]),
                        TENSP = reader["TENSP"].ToString() ?? "",
                        DONGIA = Convert.ToDecimal(reader["DONGIA"]),
                        SOLUONG = Convert.ToInt32(reader["SOLUONG"]),
                        MOTA = reader["MOTA"].ToString() ?? "",
                        ANH = reader["ANH"].ToString() ?? "",
                        DANHGIA = Convert.ToDouble(reader["DANHGIA"].ToString()),
                        MADMSP = Convert.ToInt32(reader["MADMSP"]),
                        MATH = Convert.ToInt32(reader["MATH"]),
                        MATSKT = Convert.ToInt32(reader["MATSKT"]),
                        DanhMucSP = new DanhMucSPAdmin()
                        {
                            MADMSP = Convert.ToInt32(reader["MADMSP"]),
                            TENDMSP = reader["TENDMSP"].ToString() ?? ""
                        },
                        ThuongHieu = new ThuongHieuAdmin()
                        {
                            MATH = Convert.ToInt32(reader["MATH"]),
                            TENTHUONGHIEU = reader["TENTHUONGHIEU"].ToString() ?? ""
                        },
                        ThongSoKyThuat = new ThongSoKyThuatAdmin()
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
                        }
                    };
                }
            }

            connect.CloseConnection();
            return sanPham;
        }
        public bool UpdateSPById(SanPhamFormAdmin sanPhamNew, int Id)
        {
            connect.OpenConnection();
            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"UPDATE SANPHAM
                                SET TENSP = @TENSP,
                                    DONGIA = @DONGIA,
                                    SOLUONG = @SOLUONG,
                                    MOTA = @MOTA,
                                    ANH = @ANH,
                                    DANHGIA = @DANHGIA,
                                    MADMSP = @MADMSP,
                                    MATH = @MATH,
                                    MATSKT = @MATSKT
                                WHERE MASP = @Id";

                command.CommandText = query;

                command.Parameters.AddWithValue("@Id", Id);
                command.Parameters.AddWithValue("@TENSP", sanPhamNew.TENSP);
                command.Parameters.AddWithValue("@DONGIA", sanPhamNew.DONGIA);
                command.Parameters.AddWithValue("@SOLUONG", sanPhamNew.SOLUONG);
                command.Parameters.AddWithValue("@MOTA", sanPhamNew.MOTA);
                command.Parameters.AddWithValue("@ANH", sanPhamNew.ANH);
                command.Parameters.AddWithValue("@DANHGIA", sanPhamNew.DANHGIA);
                command.Parameters.AddWithValue("@MADMSP", sanPhamNew.MADMSP);
                command.Parameters.AddWithValue("@MATH", sanPhamNew.MATH);
                command.Parameters.AddWithValue("@MATSKT", sanPhamNew.MATSKT);

                // Execute and get the number of affected rows
                isSuccess = command.ExecuteNonQuery();
            }

            connect.CloseConnection();
            return isSuccess > 0; // Return true if at least one row was affected
        }
        public bool DeleteSPById(int Id)
        {
            connect.OpenConnection();

            int isSuccess = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"DELETE FROM SANPHAM WHERE MASP = @id";

                command.CommandText = query;

                command.Parameters.AddWithValue("@id", Id);

                isSuccess = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return isSuccess > 0;
        }
        // Lấy danh sách sản phầm theo phân trang (số trang hiện tại, số hàng trong 1 trang)
        public List<SanPhamAdmin> getProduct_Pagination(int pageIndex, int pageSize, string? searchString)
        {
            connect.OpenConnection();

            List<SanPhamAdmin> list = new List<SanPhamAdmin>();

            //Nếu chuỗi tìm kiếm có dữ liệu thì thêm câu lệnh SQL WHERE
            string condition = "";
            if (searchString != "" && searchString != null)
            {
                condition = @" Where sp.TENSP Like '%" + searchString + "%' or sp.MOTA like '%" + searchString + "%' ";
            }


            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                //Truy vấn lồng nhau
                // ROW_NUMBER() OVER(ORDER BY a.id asc) AS RowNumber tạo thêm 1 cột để lưu index (tương tự số thứ tự)
                // sau đó sử dụng câu lệnh BETWEEN(start, end) để lấy dữ liệu có RowNumber (index ở trên): 
                //      start <= RowNumber <= end
                string query = @" 
                        SELECT * FROM (
                         SELECT ROW_NUMBER() OVER(ORDER BY sp.MASP asc) AS RowNumber,
                                sp.MASP, sp.TENSP, sp.DONGIA, sp.SOLUONG, sp.MOTA, sp.ANH, sp.DANHGIA,
                                sp.MADMSP, dm.TENDMSP,
                                sp.MATH, th.TENTHUONGHIEU,
                                sp.MATSKT, tskt.TENTSKT, tskt.HEDIEUHANH, tskt.RAM, tskt.ROM, tskt.KICHCOMANHINH, 
                                tskt.VIXULY, tskt.PIN, tskt.CAMERA
                            FROM SANPHAM sp
                            JOIN DANHMUCSP dm ON sp.MADMSP = dm.MADMSP
                            JOIN THUONGHIEU th ON sp.MATH = th.MATH
                            JOIN THONGSOKYTHUAT tskt ON sp.MATSKT = tskt.MATSKT
                            "  + condition +  @"
     	                    ) TableResult
                        WHERE TableResult.RowNumber BETWEEN( @PageIndex -1) * @PageSize + 1 
                         AND @PageIndex * @PageSize ";


                command.CommandText = query;
                command.Parameters.AddWithValue("@PageIndex", pageIndex);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    SanPhamAdmin product = new SanPhamAdmin()
                    {
                        MASP = Convert.ToInt32(reader["MASP"]),
                        TENSP = reader["TENSP"].ToString() ?? "",
                        DONGIA = Convert.ToDecimal(reader["DONGIA"]),
                        SOLUONG = Convert.ToInt32(reader["SOLUONG"]),
                        MOTA = reader["MOTA"].ToString() ?? "",
                        ANH = reader["ANH"].ToString() ?? "",
                        DANHGIA = Convert.ToDouble(reader["DANHGIA"].ToString()),
                        MADMSP = Convert.ToInt32(reader["MADMSP"]),
                        MATH = Convert.ToInt32(reader["MATH"]),
                        MATSKT = Convert.ToInt32(reader["MATSKT"]),
                        DanhMucSP = new DanhMucSPAdmin()
                        {
                            MADMSP = Convert.ToInt32(reader["MADMSP"]),
                            TENDMSP = reader["TENDMSP"].ToString() ?? ""
                        },
                        ThuongHieu = new ThuongHieuAdmin()
                        {
                            MATH = Convert.ToInt32(reader["MATH"]),
                            TENTHUONGHIEU = reader["TENTHUONGHIEU"].ToString() ?? ""
                        },
                        ThongSoKyThuat = new ThongSoKyThuatAdmin()
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
                        }
                    };

                    list.Add(product);
                }
            }
            connect.CloseConnection();

            return list;
        }

        // Lấy số lượng kết quả sau khi truy vấn có thêm điều kiện (Search)
        public int getCountRow_Pagination(int pageIndex, int pageSize, string? searchString)
        {
            connect.OpenConnection();

            // khai báo biến lưu số lượng Row truy vấn được
            int count = 0;

            //Nếu chuỗi tìm kiếm có dữ liệu thì thêm câu lệnh SQL WHERE
            string condition = "";
            if (searchString != "" && searchString != null)
            {
                condition = @" Where sp.TENSP Like '%" + searchString + "%' or sp.MOTA like '%" + searchString + "%' ";
            }

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                //Câu truy vấn
                    string query = @" SELECT Count(*) as CountRow
                                        FROM SANPHAM sp join DANHMUCSP dm on sp.MADMSP = dm.MADMSP ";

                // nối thêm điều kiện, Nếu điều kiện không có thì condition=""
                // query = query + ""; thì vẫn là query -> không ảnh hưởng
                query = query + condition;

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
