using GM_Shop.Database;
using GM_Shop.Models;
using Microsoft.Data.SqlClient;

namespace GM_Shop.DAL
{
    public class ThuongHieuDAL
    {
        DBConnect connect = new DBConnect();
        public List<MenuThuongHieu> GetAllWithCount()
        {
            connect.OpenConnection();

            List<MenuThuongHieu> list = new List<MenuThuongHieu>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"SELECT th.MATH, th.TENTHUONGHIEU, th.QUOCGIA, COUNT(sp.MASP) AS Count
                                 FROM ThuongHieu th
                                 LEFT JOIN SanPham sp ON th.MATH = sp.MATH
                                 GROUP BY th.MATH, th.TENTHUONGHIEU, th.QUOCGIA
                                 ORDER BY Count DESC";

                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    MenuThuongHieu menuThuongHieu = new MenuThuongHieu
                    {
                        MATH = Convert.ToInt32(reader["MATH"]),
                        TENTHUONGHIEU = reader["TENTHUONGHIEU"].ToString() ?? "",
                        QUOCGIA = reader["QUOCGIA"].ToString() ?? "",
                        Count = Convert.ToInt32(reader["Count"].ToString())
                    };
                    list.Add(menuThuongHieu);
                }
            }

            connect.CloseConnection();
            return list;
        }
    }
}
