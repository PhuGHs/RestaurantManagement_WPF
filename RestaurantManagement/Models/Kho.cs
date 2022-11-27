using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhaHang.Models
{
    public class Kho
    {
        private string _TenNguyenLieu;
        public string TenNguyenLieu { get => _TenNguyenLieu; set { _TenNguyenLieu = value; } }
        private int _TonDu;
        public int TonDu { get => _TonDu; set { _TonDu = value; } }
        private string _DonVi;
        public string DonVi { get => _DonVi; set { _DonVi = value; } }
        private string _NgayNhap;
        public string NgayNhap { get => _NgayNhap; set { _NgayNhap = value; } }
        private string _NguonNhap;
        public string NguonNhap { get => _NguonNhap; set { _NguonNhap = value; } }
        private string _LienLac;
        public string LienLac { get => _LienLac; set { _LienLac = value; } }
        private string _GhiChu;
        public string GhiChu { get => _GhiChu; set { _GhiChu = value; } }
        private string _DonGia;
        public string DonGia { get => _DonGia; set { _DonGia = value; } }


        public Kho(string ten, int tondu, string donvi, string ngaynhap, string nguonnhap, string dongia,string lienlac = "", string ghichu = "")
        {
            TenNguyenLieu = ten;
            TonDu = tondu;
            DonVi = donvi;
            NgayNhap = ngaynhap;
            NguonNhap = nguonnhap;
            LienLac = lienlac;
            GhiChu = ghichu;
            DonGia = dongia;
        }
    }
}
