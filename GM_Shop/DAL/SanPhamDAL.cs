using GM_Shop.Areas.Admin.Models;
using GM_Shop.Database;
using GM_Shop.Models;
using Microsoft.Data.SqlClient;

namespace GM_Shop.DAL
{
    public class SanPhamDAL
    {
        DBConnect connect = new DBConnect();
        public List<SanPham> GetListProduct(int? maDMSP, int? idBrand)
        {
            connect.OpenConnection();

            List<SanPham> list = new List<SanPham>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"SELECT sp.MASP, sp.TENSP, sp.DONGIA, sp.SOLUONG, sp.MOTA, sp.ANH, sp.DANHGIA,
                                       sp.MADMSP, dm.TENDMSP,
                                       sp.MATH, th.TENTHUONGHIEU,
                                       sp.MATSKT, tskt.TENTSKT, tskt.HEDIEUHANH, tskt.RAM, tskt.ROM, tskt.KICHCOMANHINH, 
                                       tskt.VIXULY, tskt.PIN, tskt.CAMERA
                                FROM SANPHAM sp
                                JOIN DANHMUCSP dm ON sp.MADMSP = dm.MADMSP
                                JOIN THUONGHIEU th ON sp.MATH = th.MATH
                                JOIN THONGSOKYTHUAT tskt ON sp.MATSKT = tskt.MATSKT";

                // Điều kiện lọc
                List<string> conditions = new List<string>();
                if (maDMSP.HasValue)
                {
                    conditions.Add("sp.MADMSP = @MaDMSP");
                    command.Parameters.AddWithValue("@MaDMSP", maDMSP.Value);
                }
                if (idBrand.HasValue)
                {
                    conditions.Add("sp.MATH = @IdBrand");
                    command.Parameters.AddWithValue("@IdBrand", idBrand.Value);
                }

                // Nếu có điều kiện lọc, thêm WHERE vào câu truy vấn
                if (conditions.Count > 0)
                {
                    query += " WHERE " + string.Join(" AND ", conditions);
                }

                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    SanPham product = new SanPham()
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
        public SanPham GetSPById(int Id)
        {
            connect.OpenConnection();
            SanPham sanPham = new SanPham();

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
                    sanPham = new SanPham()
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
        public List<SanPham> GetSPByDMSP(int Id)
        {
            connect.OpenConnection();
            var sanPhamList = new List<SanPham>();

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
                                WHERE sp.MADMSP = @Id";

                command.CommandText = query;
                command.Parameters.AddWithValue("@Id", Id);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var sanPham = new SanPham()
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
                    sanPhamList.Add(sanPham);
                }
            }

            connect.CloseConnection();
            return sanPhamList;
        }
        public List<SanPham> GetFeaturedProducts(int limitProduct)
        {
            connect.OpenConnection();

            List<SanPham> list = new List<SanPham>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = $@"
                                SELECT TOP {limitProduct} sp.MASP, sp.TENSP, sp.DONGIA, sp.SOLUONG, sp.MOTA, sp.ANH, sp.DANHGIA,
                                       sp.MADMSP, dm.TENDMSP,
                                       sp.MATH, th.TENTHUONGHIEU,
                                       sp.MATSKT, tskt.TENTSKT, tskt.HEDIEUHANH, tskt.RAM, tskt.ROM, tskt.KICHCOMANHINH, 
                                       tskt.VIXULY, tskt.PIN, tskt.CAMERA
                                FROM SANPHAM sp
                                JOIN DANHMUCSP dm ON sp.MADMSP = dm.MADMSP
                                JOIN THUONGHIEU th ON sp.MATH = th.MATH
                                JOIN THONGSOKYTHUAT tskt ON sp.MATSKT = tskt.MATSKT
                                ORDER BY sp.DANHGIA DESC";


                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    SanPham product = new SanPham()
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
        public List<SanPham> GetRelatedProducts(int maDMSP, int currentMASP)
        {
            connect.OpenConnection();
            List<SanPham> list = new List<SanPham>();

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
                                JOIN THONGSOKYTHUAT tskt ON sp.MATSKT = tskt.MATSKT
                                WHERE sp.MADMSP = @maDMSP AND sp.MASP != @currentMASP
                                ORDER BY sp.DANHGIA DESC";

                command.CommandText = query;

                // Thêm tham số truy vấn
                command.Parameters.AddWithValue("@maDMSP", maDMSP);
                command.Parameters.AddWithValue("@currentMASP", currentMASP);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    SanPham product = new SanPham()
                    {
                        MASP = Convert.ToInt32(reader["MASP"]),
                        TENSP = reader["TENSP"]?.ToString() ?? "",
                        DONGIA = Convert.ToDecimal(reader["DONGIA"]),
                        SOLUONG = Convert.ToInt32(reader["SOLUONG"]),
                        MOTA = reader["MOTA"]?.ToString() ?? "",
                        ANH = reader["ANH"]?.ToString() ?? "",
                        DANHGIA = Convert.ToDouble(reader["DANHGIA"].ToString()),
                        MADMSP = Convert.ToInt32(reader["MADMSP"]),
                        MATH = Convert.ToInt32(reader["MATH"]),
                        MATSKT = Convert.ToInt32(reader["MATSKT"]),
                        DanhMucSP = new DanhMucSPAdmin()
                        {
                            MADMSP = Convert.ToInt32(reader["MADMSP"]),
                            TENDMSP = reader["TENDMSP"]?.ToString() ?? ""
                        },
                        ThuongHieu = new ThuongHieuAdmin()
                        {
                            MATH = Convert.ToInt32(reader["MATH"]),
                            TENTHUONGHIEU = reader["TENTHUONGHIEU"]?.ToString() ?? ""
                        },
                        ThongSoKyThuat = new ThongSoKyThuatAdmin()
                        {
                            MATSKT = Convert.ToInt32(reader["MATSKT"]),
                            TENTSKT = reader["TENTSKT"]?.ToString() ?? "",
                            HEDIEUHANH = reader["HEDIEUHANH"]?.ToString() ?? "",
                            RAM = reader["RAM"] != DBNull.Value ? Convert.ToInt32(reader["RAM"]) : 0,
                            ROM = reader["ROM"]?.ToString() ?? "",
                            KICHCOMANHINH = reader["KICHCOMANHINH"] != DBNull.Value ? Convert.ToSingle(reader["KICHCOMANHINH"]) : 0,
                            VIXULY = reader["VIXULY"]?.ToString() ?? "",
                            PIN = reader["PIN"] != DBNull.Value ? Convert.ToInt32(reader["PIN"]) : 0,
                            CAMERA = reader["CAMERA"]?.ToString() ?? ""
                        }
                    };

                    list.Add(product);
                }
            }

            connect.CloseConnection();
            return list;
        }
        public List<SanPham> getProduct_Pagination(int? idCategory, int? idBrand, int pageIndex, int pageSize, string? searchString)
        {
            connect.OpenConnection();

            List<SanPham> list = new List<SanPham>();

            //Điều kiện Category
            string categoryCondition = "";
            if (idCategory.HasValue)
            {
                categoryCondition = @" WHERE sp.MADMSP = " + idCategory;
            }

            // Điều kiện Brand
            string brandCondition = "";
            if (idBrand.HasValue)
            {
                if (string.IsNullOrEmpty(categoryCondition))
                {
                    brandCondition = " WHERE sp.MATH = " + idBrand;
                }
                else
                {
                    brandCondition = " AND sp.MATH = " + idBrand;
                }
            }
            // Điều kiện tìm kiếm
            string searchCondition = "";
            if (!string.IsNullOrEmpty(searchString))
            {
                if (string.IsNullOrEmpty(categoryCondition) && string.IsNullOrEmpty(brandCondition))
                {
                    searchCondition = " WHERE (sp.TENSP LIKE @SearchString OR sp.MOTA LIKE @SearchString)";
                }
                else
                {
                    searchCondition = " AND (sp.TENSP LIKE @SearchString OR sp.MOTA LIKE @SearchString)";
                }
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
                         SELECT ROW_NUMBER() OVER ( ORDER BY sp.MASP )  AS RowNumber,
                                sp.MASP, sp.TENSP, sp.DONGIA, sp.SOLUONG, sp.MOTA, sp.ANH, sp.DANHGIA,
                                sp.MADMSP, dm.TENDMSP,
                                sp.MATH, th.TENTHUONGHIEU,
                                sp.MATSKT, tskt.TENTSKT, tskt.HEDIEUHANH, tskt.RAM, tskt.ROM, tskt.KICHCOMANHINH, 
                                tskt.VIXULY, tskt.PIN, tskt.CAMERA
                            FROM SANPHAM sp
                            JOIN DANHMUCSP dm ON sp.MADMSP = dm.MADMSP
                            JOIN THUONGHIEU th ON sp.MATH = th.MATH
                            JOIN THONGSOKYTHUAT tskt ON sp.MATSKT = tskt.MATSKT
                            " + categoryCondition + brandCondition + searchCondition + @"
     	                    ) TableResult
                        WHERE TableResult.RowNumber BETWEEN( @PageIndex -1) * @PageSize + 1 
                         AND @PageIndex * @PageSize ";


                command.CommandText = query;
                command.Parameters.AddWithValue("@PageIndex", pageIndex);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                if (!string.IsNullOrEmpty(searchString))
                {
                    command.Parameters.AddWithValue("@SearchString", "%" + searchString + "%");
                }
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    SanPham product = new SanPham()
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

    }
}
