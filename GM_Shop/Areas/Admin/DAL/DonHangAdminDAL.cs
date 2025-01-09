using GM_Shop.Areas.Admin.Models;
using GM_Shop.Database;
using GM_Shop.Models;
using Microsoft.Data.SqlClient;
using System;
using static NuGet.Packaging.PackagingConstants;

namespace GM_Shop.Areas.Admin.DAL
{
    public class DonHangAdminDAL
    {
        DBConnect connect = new DBConnect();
        public List<DonHangAdmin> GetAll()
        {
            connect.OpenConnection();
            List<DonHangAdmin> list = new List<DonHangAdmin>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"SELECT * FROM DONHANG";

                command.CommandText = query;
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DonHangAdmin donHang = new DonHangAdmin()
                    {
                        OrderId = Convert.ToInt32(reader["MADONHANG"]),
                        CustomerId = Convert.ToInt32(reader["MAKH"]),
                        CreatedDate = Convert.ToDateTime(reader["NGAYTAO"]),
                        TotalAmount = Convert.ToDouble(reader["TONGTIEN"]),
                        CustomerLastName = reader["HOKH"].ToString() ?? "",
                        CustomerFirstName = reader["TENKH"].ToString() ?? "",
                        Phone = reader["SDT"].ToString() ?? "",
                        Email = reader["EMAIL"].ToString() ?? ""
                    };
                    list.Add(donHang);
                }
            }
            connect.CloseConnection();
            return list;
        }
        // Lấy thông tin chi tiết hóa đơn theo ID
        public DonHangAdmin GetOrderById(int orderId)
        {
            connect.OpenConnection();
            DonHangAdmin donHang = null;

            using (var command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                // Lấy thông tin hóa đơn
                string orderQuery = @"SELECT * FROM DONHANG WHERE MADONHANG = @OrderId";
                command.CommandText = orderQuery;
                command.Parameters.AddWithValue("@OrderId", orderId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        donHang = new DonHangAdmin
                        {
                            OrderId = Convert.ToInt32(reader["MADONHANG"]),
                            CustomerId = Convert.ToInt32(reader["MAKH"]),
                            CreatedDate = Convert.ToDateTime(reader["NGAYTAO"]),
                            TotalAmount = Convert.ToDouble(reader["TONGTIEN"]),
                            CustomerLastName = reader["HOKH"].ToString() ?? "",
                            CustomerFirstName = reader["TENKH"].ToString() ?? "",
                            Phone = reader["SDT"].ToString() ?? "",
                            Email = reader["EMAIL"].ToString() ?? "",
                            ChiTietDonHangs = new List<ChiTietDonHangAdmin>()
                        };
                    }
                }

                if (donHang != null)
                {
                    // Lấy thông tin chi tiết hóa đơn
                    string detailQuery = @"SELECT CT.MADONHANG, CT.MASP, CT.GIA, CT.SOLUONG, CT.TONGTIEN, CT.NGAYTAO, 
                                    sp.TENSP, sp.DONGIA, sp.MOTA, sp.ANH, sp.DANHGIA
                                    FROM CHITIETDONHANG CT
                                    INNER JOIN SANPHAM sp ON CT.MASP = sp.MASP
                                    WHERE CT.MADONHANG = @OrderId";

                    command.CommandText = detailQuery;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@OrderId", orderId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            donHang.ChiTietDonHangs.Add(new ChiTietDonHangAdmin
                            {
                                OrderId = Convert.ToInt32(reader["MADONHANG"]),
                                ProductId = Convert.ToInt32(reader["MASP"]),
                                Price = Convert.ToInt32(reader["GIA"]),
                                Quantity = Convert.ToInt32(reader["SOLUONG"]),
                                TotalPrice = Convert.ToDouble(reader["TONGTIEN"]),
                                CreatedDate = Convert.ToDateTime(reader["NGAYTAO"]),
                                SanPham = new SanPhamAdmin()
                                {
                                    MASP = Convert.ToInt32(reader["MASP"]),
                                    TENSP = reader["TENSP"].ToString() ?? "",
                                    DONGIA = Convert.ToDecimal(reader["DONGIA"]),
                                    MOTA = reader["MOTA"].ToString() ?? "",
                                    ANH = reader["ANH"].ToString() ?? "",
                                    DANHGIA = Convert.ToDouble(reader["DANHGIA"])
                                }
                            });
                        }
                    }
                }
            }

            connect.CloseConnection();
            return donHang;
        }

        // Xóa hóa đơn theo ID
        public bool DeleteOrderById(int orderId)
        {
            connect.OpenConnection();
            bool isDeleted = false;
            
            using (var transaction = connect.GetConnection().BeginTransaction())
            using (var command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.Transaction = transaction;

                try
                {
                    // Xóa chi tiết hóa đơn
                    string deleteDetailsQuery = @"DELETE FROM CHITIETDONHANG WHERE MADONHANG = @OrderId";
                    command.CommandText = deleteDetailsQuery;
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.ExecuteNonQuery();

                    // Xóa hóa đơn
                    string deleteOrderQuery = "DELETE FROM DONHANG WHERE MADONHANG = @OrderId";
                    command.CommandText = deleteOrderQuery;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                    isDeleted = true;
                }
                catch
                {
                    transaction.Rollback();
                }
            }

            connect.CloseConnection();
            return isDeleted;
        }
        public List<DonHangAdmin> getDonHang_Pagination(int pageIndex, int pageSize)
        {
            connect.OpenConnection();

            List<DonHangAdmin> list = new List<DonHangAdmin>();
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @" 
                        SELECT * FROM (
                         SELECT ROW_NUMBER() OVER(ORDER BY MADONHANG asc) AS RowNumber,
                            MADONHANG, MAKH, NGAYTAO, TONGTIEN, HOKH, TENKH, SDT, EMAIL
                        FROM DONHANG
     	                    ) TableResult
                        WHERE TableResult.RowNumber BETWEEN( @PageIndex -1) * @PageSize + 1 
                         AND @PageIndex * @PageSize ";


                command.CommandText = query;
                command.Parameters.AddWithValue("@PageIndex", pageIndex);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DonHangAdmin donHang = new DonHangAdmin()
                    {
                        OrderId = Convert.ToInt32(reader["MADONHANG"]),
                        CustomerId = Convert.ToInt32(reader["MAKH"]),
                        CreatedDate = Convert.ToDateTime(reader["NGAYTAO"]),
                        TotalAmount = Convert.ToDouble(reader["TONGTIEN"]),
                        CustomerLastName = reader["HOKH"].ToString() ?? "",
                        CustomerFirstName = reader["TENKH"].ToString() ?? "",
                        Phone = reader["SDT"].ToString() ?? "",
                        Email = reader["EMAIL"].ToString() ?? ""
                    };
                    list.Add(donHang);
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
                             FROM DONHANG";

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
