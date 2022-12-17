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
using QuanLyNhaHang.DataProvider;

namespace QuanLyNhaHang.ViewModel
{
    public class CaiDatViewModel : BaseViewModel
    {
        public CaiDatViewModel()
        {
            NhanVien = CaiDatDP.Flag.GetCurrentEmployee();
            CaiDatDP.Flag.LoadProfileImage(NhanVien);
            UpdateInfoCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                CaiDatDP.Flag.UpdateInfo(NhanVien.HoTen, NhanVien.DiaChi, NhanVien.SDT, NhanVien.NgaySinh, NhanVien.MaNV);
            });
        }
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
        #endregion
        #region commands
        public ICommand UpdateInfoCommand { get; set; } 
        #endregion
    }
}
