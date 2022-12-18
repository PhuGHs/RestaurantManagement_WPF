using QuanLyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhaHang.DataProvider
{
    public class CaiDatDP : DataProvider
    {
        private static CaiDatDP flag;
        public static CaiDatDP Flag
        {
            get
            {
                if (flag == null) flag = new CaiDatDP();
                return flag;
            }
            set
            {
                flag = value;
            }
        }
        public NhanVien GetCurrentEmployee()
        {
            NhanVien nv = null;
            string MaNV = getMaNVFromTaiKhoan();
            string pw = getAccountPasswordFromTaiKhoan();
            DataTable dt = new DataTable();
            dt = LoadInitialData("Select * from NhanVien where MaNV = '" + MaNV + "'");
            foreach(DataRow dr in dt.Rows)
            {
                nv = new NhanVien(dr["MaNV"].ToString(), dr["TenNV"].ToString(), dr["ChucVu"].ToString(), dr["DiaChi"].ToString(), (bool)dr["FullTime"], dr["SDT"].ToString(), dr["NgayVaoLam"].ToString(), dr["NgaySinh"].ToString());
            }
            nv.MatKhau = pw;
            return nv;
        }
        public void ChangePassword(string pw)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                //var hashPassword = PasswordHasher.Hash(pw);
                string ID = getAccountIDFromTaiKhoan();
                cmd.CommandText = "Update TaiKhoan set MatKhau = @hashedPassword where ID = @id";
                cmd.Parameters.AddWithValue("@hashedPassword", pw);
                cmd.Parameters.AddWithValue("@id", ID);
                DBOpen();

                cmd.Connection = SqlCon;
                cmd.ExecuteNonQuery();
            }
            finally
            {
                DBClose();
                MyMessageBox msb = new MyMessageBox("Đổi mật khẩu thành công!");
                msb.Show();
            }
             
        }
        
        public void UpdateInfo(string HoTen, string Diachi, string SDT, string NgaySinh, string MaNV)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Exec UPDATEINFO @hoten, @diachi, @manv, @sdt, @ngaysinh";
                cmd.Parameters.AddWithValue("@hoten", HoTen);
                cmd.Parameters.AddWithValue("@diachi", Diachi);
                cmd.Parameters.AddWithValue("@sdt", SDT);
                cmd.Parameters.AddWithValue("@ngaysinh", NgaySinh);
                cmd.Parameters.AddWithValue("@manv", MaNV);
                DBOpen();
                cmd.Connection = SqlCon;
                cmd.ExecuteNonQuery();
            }
            finally
            {
                DBClose();
                MyMessageBox msb = new MyMessageBox("Thay đổi và lưu thông tin thành công");
                msb.Show();
            }
        }
        public void LoadProfileImage(NhanVien nv)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Select AnhDaiDien from TaiKhoan where MaNV = @manv";
                cmd.Parameters.AddWithValue("@manv", nv.MaNV);
                DBOpen();
                cmd.Connection = SqlCon;
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    nv.AnhDaiDien = Converter.ImageConverter.ConvertByteToBitmapImage((byte[])reader[0]);
                }
            }
            finally
            {
                DBClose();
            }
        }
        public void ChangeProfileImage_SaveToDB(NhanVien nv)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                string ID = getAccountIDFromTaiKhoan();
                cmd.CommandText = "Update TaiKhoan set AnhDaiDien = @anhdaidien where ID = @id";
                cmd.Parameters.AddWithValue("@id", ID);
                cmd.Parameters.AddWithValue("@anhdaidien", Converter.ImageConverter.ConvertImageToBytes(nv.AnhDaiDien));

                DBOpen();
                cmd.Connection = SqlCon;
                cmd.ExecuteNonQuery();
            }
            finally
            {
                DBClose();
                MyMessageBox msb = new MyMessageBox("Đã thay đổi ảnh đại diện!");
                msb.Show();
            }
        }
        #region complementary methods
        public string getMaNVFromTaiKhoan()
        {
            string MaNV = "";
            DataTable dt = new DataTable();
            dt = LoadInitialData("Select * from TaiKhoan");
            foreach(DataRow dr in dt.Rows)
            {
                MaNV = dr["MaNV"].ToString();
            }
            return MaNV;
        }
        public string getAccountIDFromTaiKhoan()
        {
            string ID = "";
            DataTable dt = new DataTable();
            dt = LoadInitialData("Select * from TaiKhoan");
            foreach (DataRow dr in dt.Rows)
            {
                ID = dr["Id"].ToString();
            }
            return ID;
        }
        public string getAccountPasswordFromTaiKhoan()
        {
            try
            {
                string pw = "";
                string ID = getAccountIDFromTaiKhoan();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Select MatKhau from TAIKHOAN WHERE ID = @id";
                cmd.Parameters.AddWithValue("@id", ID);
                DBOpen();
                cmd.Connection = SqlCon;

                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    pw = reader.GetString(0);
                }
                reader.Close();
                return pw;
            }
            finally
            {
                DBClose();
            }
        }
        #endregion
    }
}
