using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyNhaHang.Models;

namespace QuanLyNhaHang.ViewModel
{
    public class CaiDatViewModel : BaseViewModel
    {
        public CaiDatViewModel()
        {
            connectionString = ConfigurationManager.ConnectionStrings["QuanLyNhaHang"].ToString();
            OpenConnection();
            ReadData();
        }
        private NhanVien _nhanVien;
        public NhanVien NhanVien { get { return _nhanVien; } set { _nhanVien = value; OnPropertyChanged(); } }
        private SqlConnection SqlCon = null;
        private string connectionString;
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
                //_nhanVien.MaNV = read.GetInt32(0).ToString();
                //_nhanVien.HoTen = read.GetString(1);
                //_nhanVien.ChucVu = read.GetString(2);
                //_nhanVien.DiaChi = read.GetString(3);
                //_nhanVien.Fulltime = read.GetBoolean(4);
                //_nhanVien.TaiKhoan = read.GetString(5);
                //_nhanVien.MatKhau = read.GetString(6);
                //_nhanVien.SDT = read.GetString(7);
                //_nhanVien.NgayVaoLam = read.GetDateTime(8).ToString();
                //_nhanVien.NgaySinh = read.GetDateTime(9).ToString();
            }
            CloseConnection();
        }
        private void CloseConnection()
        {
            if(SqlCon.State == ConnectionState.Open)
            {
                SqlCon.Close();
            }
        }
    }
}
