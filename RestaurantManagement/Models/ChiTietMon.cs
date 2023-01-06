using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyNhaHang.ViewModel;

namespace QuanLyNhaHang.Models
{
    public class ChiTietMon : BaseViewModel
    {
        private string _maMon;
        private string _tenNL;
        private int _soluong;

        public string MaMon { get { return _maMon; } set { _maMon = value; OnPropertyChanged(); } }
        public string TenNL { get { return _tenNL; } set { _tenNL = value; OnPropertyChanged(); } }
        public int SoLuong { get { return _soluong; } set { _soluong = value; OnPropertyChanged(); } }
        public ChiTietMon(string TenNL, string MaMon, int SoLuong = 0)
        {
            _maMon = MaMon;
            _tenNL = TenNL;
            _soluong = SoLuong;
        }
    }
}
