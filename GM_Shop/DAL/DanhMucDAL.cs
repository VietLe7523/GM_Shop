using GM_Shop.Database;
using GM_Shop.Models;
using Microsoft.Data.SqlClient;

namespace GM_Shop.DAL
{
    public class DanhMucDAL
    {
        DBConnect connect = new DBConnect();
        public List<MenuDanhMuc> getAllWithCount()
        {
            connect.OpenConnection();

            List<MenuDanhMuc> list = new List<MenuDanhMuc>();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connect.GetConnection();
                command.CommandType = System.Data.CommandType.Text;

                string query = @"SELECT dm.MADMSP, dm.TENDMSP, COUNT(sp.MASP) AS Count
                                 FROM DanhMucSP dm
                                 LEFT JOIN SanPham sp ON dm.MADMSP = sp.MADMSP
                                 GROUP BY dm.MADMSP, dm.TENDMSP
                                 ORDER BY Count DESC";

                command.CommandText = query;

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    MenuDanhMuc menuDanhMuc = new MenuDanhMuc
                    {
                        MADMSP = Convert.ToInt32(reader["MADMSP"]),
                        TENDMSP = reader["TENDMSP"].ToString() ?? "",
                        Count = Convert.ToInt32(reader["Count"].ToString())
                    };
                    list.Add(menuDanhMuc);
                }
            }

            connect.CloseConnection();
            return list;
        }
    }
}
