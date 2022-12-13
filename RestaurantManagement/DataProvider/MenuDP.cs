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

        public void PayABill(Int16 soban)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Exec PAY_A_BILL_PD @trigia, @manv, @soban, @ngayHD, @trangthai";
                cmd.Parameters.AddWithValue("@trigia", Calculate_Sum(soban));
                cmd.Parameters.AddWithValue("@manv", "NV01");
                cmd.Parameters.AddWithValue("@soban", soban);
                cmd.Parameters.AddWithValue("@ngayHD", DateTime.Now);
                cmd.Parameters.AddWithValue("@trangthai", "Đã thanh toán");
                cmd.Connection = SqlCon;
                DBOpen();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                DBClose();
            }
        }

        #region complementary functions
        public Decimal Calculate_Sum(Int16 Soban)
        {
            Decimal sum = 0;
            try
            {
                DBOpen();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Exec GET_SUM_OF_PRICE_PD @soban";
                cmd.Parameters.AddWithValue("@soban", Soban);
                cmd.Connection = SqlCon;

                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    sum = reader.GetDecimal(0);
                }
                reader.Close();
                return sum;
            }
            finally
            {
                DBClose();
            }
        }
        public int Get_Quantity(Int16 Soban)
        {
            int quantity = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Select SUM(Soluong) from CHEBIEN where SoBan = @soban";
                cmd.Parameters.AddWithValue("@soban", Soban);
                cmd.Connection = SqlCon;
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    quantity = reader.GetInt16(0);
                }
                reader.Close();
                return quantity;
            }
            finally
            {
                DBClose();
            }
        }
        #endregion
    }
}
