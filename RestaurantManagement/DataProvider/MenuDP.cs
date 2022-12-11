using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using QuanLyNhaHang.Models;

namespace QuanLyNhaHang.DataProvider
{
    public class MenuDP : DataProvider
    {
        private static MenuDP flag;
        public static MenuDP Flag
        {
            get
            {
                if (flag == null) flag = new MenuDP();
                return flag;
            }
            set
            {
                flag = value;
            }
        }
        public ObservableCollection<MenuItem> ConvertToCollection()
        {
            ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>();
            try
            {
                DataTable dt = LoadInitialData("Select * from MENU");
                foreach(DataRow row in dt.Rows)
                {
                    string maMon = row["MaMon"].ToString();
                    string tenMon = row["TenMon"].ToString();
                    BitmapImage anhMon = Converter.ImageConverter.ConvertByteToBitmapImage((byte[])row["AnhMonAn"]);
                    Decimal gia = (Decimal)row["Gia"];
                    int thoiGianLam = (Int16)row["ThoiGianLam"];

                    
                    menuItems.Add(new MenuItem(maMon, tenMon, gia, anhMon, thoiGianLam));    
                }
            }
            finally
            {
                DBClose();
            }
            return menuItems;   
        }
        public void InformChef(string maMon, int soban, int soluong)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Exec Inform_Chef_PD @mamon, @soban, @soluong, @ngaycb, @trangthai";
                cmd.Parameters.AddWithValue("@mamon", maMon);
                cmd.Parameters.AddWithValue("@soban", soban);
                cmd.Parameters.AddWithValue("@soluong", soluong);
                cmd.Parameters.AddWithValue("@ngaycb", DateTime.Now);
                cmd.Parameters.AddWithValue("@trangthai", "Đang chế biến");
                DBOpen();
                cmd.Connection = SqlCon;
                cmd.ExecuteNonQuery();
            }
            finally
            {
                DBClose();
            }
        }
    }
}
