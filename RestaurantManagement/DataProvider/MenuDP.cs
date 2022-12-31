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
                foreach (DataRow row in dt.Rows)
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
                cmd.CommandText = "Exec Inform_Chef_PD @mamon, @soban, @soluong, @ngaycb, @trangthai, @trangthaiban";
                cmd.Parameters.AddWithValue("@mamon", maMon);
                cmd.Parameters.AddWithValue("@soban", soban);
                cmd.Parameters.AddWithValue("@soluong", soluong);
                cmd.Parameters.AddWithValue("@ngaycb", DateTime.Now);
                cmd.Parameters.AddWithValue("@trangthai", "Đang chế biến");
                cmd.Parameters.AddWithValue("@trangthaiban", "Đang được sử dụng");
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
                cmd.Parameters.AddWithValue("@trangthai", "Paid");
                cmd.Connection = SqlCon;
                DBOpen();
                cmd.ExecuteNonQuery();
                DBClose();

                Fill_CTHD(getDishQuantity(soban));
                DBClose();
            }
            finally
            {
                MyMessageBox msb = new MyMessageBox("Thanh toán thành công!");
                msb.Show();
                DBClose();
            }
        }
        public void AddDish(MenuItem x)
        {
            try
            {
                DBOpen();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO MENU VALUES (@MaMon, @TenMon, @Gia, @AnhMonAn, @ThoiGianLam)";
                cmd.Parameters.AddWithValue("@MaMon", x.ID);
                cmd.Parameters.AddWithValue("@TenMon", x.FoodName);
                cmd.Parameters.AddWithValue("@AnhMonAn", Converter.ImageConverter.ConvertImageToBytes(x.FoodImage));
                cmd.Parameters.AddWithValue("@Gia", x.Price);
                cmd.Parameters.AddWithValue("@ThoiGianLam", x.CookingTime);

                cmd.Connection = SqlCon;
                cmd.ExecuteNonQuery();
            }
            finally
            {
                DBClose();
            }
        }
        public void RemoveDish(string MaMon)
        {
            try
            {
                DBOpen();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Delete from MENU where MaMon = @mamon";
                cmd.Parameters.AddWithValue("@mamon", MaMon);

                cmd.Connection = SqlCon;
                cmd.ExecuteNonQuery();
            }
            finally
            {
                DBClose();
            }
        }
        public void EditDishInfo(MenuItem item)
        {
            try
            {
                DBOpen();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Update MENU set TenMon = @tenmon, AnhMonAn = @anh, Gia = @gia, ThoiGianLam = @thoigian where MaMon = @mamon ";
                cmd.Parameters.AddWithValue("@mamon", item.ID);
                cmd.Parameters.AddWithValue("@anh", Converter.ImageConverter.ConvertImageToBytes(item.FoodImage));
                cmd.Parameters.AddWithValue("@thoigian", item.CookingTime);
                cmd.Parameters.AddWithValue("@tenmon", item.FoodName);
                cmd.Parameters.AddWithValue("@gia", item.Price);
                cmd.Connection = SqlCon;

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
                if (reader.Read())
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
        public DataTable getDishQuantity(Int16 soban)
        {
            DataTable dt = new DataTable();
            try
            {
                DBOpen();
                SqlCommand cmd_GetQuantity = new SqlCommand();

                cmd_GetQuantity.CommandText = "Exec GET_QUANTITY_OF_EACH_DISH_PD @soban";
                cmd_GetQuantity.Parameters.AddWithValue("@soban", soban);
                cmd_GetQuantity.Connection = SqlCon;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd_GetQuantity);
                adapter.Fill(dt); ;
            }
            finally
            {
                DBClose();
            }
            return dt;
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
                if (reader.Read())
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
        public void Fill_CTHD(DataTable dt)
        {
            SqlCommand cmd_InsertDetail = new SqlCommand();
            cmd_InsertDetail.CommandText = "Exec INSERT_DETAIL_PD @mamon1, @soluong";
            DBOpen();
            cmd_InsertDetail.Connection = SqlCon;
            foreach (DataRow row in dt.Rows)
            {
                cmd_InsertDetail.Parameters.AddWithValue("@mamon1", row["MaMon"]);
                cmd_InsertDetail.Parameters.AddWithValue("@soluong", row["SoLuong"]);

                cmd_InsertDetail.ExecuteNonQuery();
                cmd_InsertDetail.Parameters.Clear();
            }
        }
        #endregion
    }
}
