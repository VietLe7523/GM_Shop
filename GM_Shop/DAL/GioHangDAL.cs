using GM_Shop.Database;
using GM_Shop.Models;
using Microsoft.Data.SqlClient;

namespace GM_Shop.DAL
{
    public class GioHangDAL
    {
        DBConnect connect = new DBConnect();
        public bool CheckOut(KhachHang customer, List<CartItem> cart)
        {
            connect.OpenConnection();

            bool insertPaymentSuccess = false;

            bool CheckOutSuccess = true;

            int? SCOPE_IDENTITY = null;

            decimal TOTAL_CART = cart.Sum(p => p.Total);
            // Insert vào bảng Payment và lấy Id được tạo tự động sau khi insert 
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @" 
                    insert into DONHANG (MAKH, HOKH, TENKH, SDT, EMAIL, NGAYTAO, TONGTIEN) 
                    values (@MAKH, @HOKH, @TENKH, @SDT, @EMAIL, GETDATE(), @TONGTIEN); 
                    SELECT SCOPE_IDENTITY() AS SCOPE_IDENTITY;   
                     
                ";

                command.CommandText = query;
                command.Parameters.AddWithValue("@MAKH", customer.MAKH);
                command.Parameters.AddWithValue("@HOKH", customer.HOKH);
                command.Parameters.AddWithValue("@TENKH", customer.TENKH);
                command.Parameters.AddWithValue("@SDT", customer.SDT);
                command.Parameters.AddWithValue("@EMAIL", customer.EMAIL);
                command.Parameters.AddWithValue("@TONGTIEN", TOTAL_CART);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    insertPaymentSuccess = true;
                    SCOPE_IDENTITY = reader["SCOPE_IDENTITY"] == DBNull.Value ? null : Convert.ToInt32(reader["SCOPE_IDENTITY"]);
                }
            }
            connect.CloseConnection();

            //Nếu InSert vào bảng Payment không thành công thì trả về false và dừnglại
            if (!insertPaymentSuccess || SCOPE_IDENTITY == null)
            {
                return false;
            }

            // Insert vào bảng Payment Detail với Id tự động vừa lấy được (SCOPE_IDENTITY)
            foreach (var itemCart in cart)
            {
                bool success = InsertToPaymentDetail(SCOPE_IDENTITY ?? 0, itemCart);

                if (!success)
                {
                    CheckOutSuccess = false;
                }
            }

            return CheckOutSuccess;
        }
        // Insert vào bảng Payment Detail 
        public bool InsertToPaymentDetail(int SCOPE_IDENTITY, CartItem itemCart)
        {
            connect.OpenConnection();

            int numberOfRows = 0;

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @" 
                            insert into CHITIETDONHANG (MADONHANG, MASP, GIA, SOLUONG, TONGTIEN, NGAYTAO) 
                            values (@MADONHANG, @MASP, @GIA, @SOLUONG, @TONGTIEN, GETDATE())  ";

                command.CommandText = query;
                command.Parameters.AddWithValue("@MADONHANG", SCOPE_IDENTITY);
                command.Parameters.AddWithValue("@MASP", itemCart.MASP);
                command.Parameters.AddWithValue("@GIA", itemCart.DONGIA);
                command.Parameters.AddWithValue("@SOLUONG", itemCart.SOLUONG);
                command.Parameters.AddWithValue("@TONGTIEN", itemCart.Total);

                numberOfRows = command.ExecuteNonQuery();
            }
            connect.CloseConnection();
            return numberOfRows > 0;
        }
    }
}
