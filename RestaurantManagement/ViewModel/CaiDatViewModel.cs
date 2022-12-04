using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyNhaHang.Models;
using System.Windows.Input;

namespace QuanLyNhaHang.ViewModel
{
    public class CaiDatViewModel : BaseViewModel
    {
        //public CaiDatViewModel()
        //{
        //    connectionString = ConfigurationManager.ConnectionStrings["QuanLyNhaHang"].ToString();
        //    UpdateInfoCommand = new RelayCommand<object>((p) => true, (p) => UpdateInfo());
        //    OpenConnection();
        //    ReadData();
        //}
        #region variables
        private NhanVien _nhanVien;
        public NhanVien NhanVien { get { return _nhanVien; } set { _nhanVien = value; OnPropertyChanged(); } }
        public string LoaiNhanVien_Str
        {
            get
            {
                if (_nhanVien.Fulltime)
                {
                    return "Full-time";
                }
                return "Part-time";
            }
            set
            {
                if (value == "Full-time")
                {
                    _nhanVien.Fulltime = true;
                }
                else
                {
                    _nhanVien.Fulltime = false;
                }
            }
        }
        private SqlConnection SqlCon = null;
        private string connectionString;
        #endregion
        #region commands
        public ICommand UpdateInfoCommand { get; set; } 
        #endregion
        private void OpenConnection()
        {
            SqlCon = new SqlConnection(connectionString);
            if(SqlCon.State == ConnectionState.Closed)
            {
                SqlCon.Open();
            }
        }
        private void ReadData()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select * from NhanVien";
            cmd.Connection = SqlCon;
            
            SqlDataReader read = cmd.ExecuteReader();
            while(read.Read())
            {
                _nhanVien = new NhanVien(read.GetInt16(0).ToString(), read.GetString(1), read.GetString(2), read.GetString(3), read.GetBoolean(4), read.GetString(7), read.GetDateTime(8).ToString(), read.GetDateTime(9).ToString(), read.GetString(5), read.GetString(6));
            }
            read.Close();
            CloseConnection();
        }
        private void CloseConnection()
        {
            if(SqlCon.State == ConnectionState.Open)
            {
                SqlCon.Close();
            }
        }
        private void UpdateInfo()
        {
            OpenConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Update NhanVien set Hoten=@hoten, Diachi=@diachi, SDT=@sdt, NgaySinh=@ngaysinh where MaNV=@manv";
            cmd.Parameters.AddWithValue("@Hoten", NhanVien.HoTen);
            cmd.Parameters.AddWithValue("@diachi", NhanVien.DiaChi);
            cmd.Parameters.AddWithValue("@sdt", NhanVien.SDT);
            cmd.Parameters.AddWithValue("@ngaysinh", NhanVien.NgaySinh);
            cmd.Parameters.AddWithValue("@manv", NhanVien.MaNV);

            cmd.Connection = SqlCon;
            
            MyMessageBox msb = new MyMessageBox("Bạn có muốn thay đổi những thông tin này?", true);
            msb.Show();
            if(msb.ACCEPT() == true)
            {
                cmd.ExecuteNonQuery();
            }
            CloseConnection();
        }
    }
}
