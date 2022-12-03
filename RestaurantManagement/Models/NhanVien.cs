using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhaHang.Models
{
    public class NhanVien
    {
        private string _MaNV;
        public string MaNV { get => _MaNV; set => _MaNV = value; }
        private string _HoTen;
        public string HoTen { get => _HoTen; set => _HoTen = value; }
        private string _ChucVu;
        public string ChucVu { get => _ChucVu; set => _ChucVu = value; }
        private string _DiaChi;
        public string DiaChi { get => _DiaChi; set => _DiaChi = value; }
        private bool _Fulltime;
        public bool Fulltime { get => _Fulltime; set => _Fulltime = value; }
        private string _SDT;
        public string SDT { get => _SDT; set => _SDT = value; }
        private string _NgayVaoLam;
        public string NgayVaoLam { get => _NgayVaoLam; set => _NgayVaoLam = value; }
        private string _NgaySinh;
        public string NgaySinh { get => _NgaySinh; set => _NgaySinh = value; }
        private string _TaiKhoan;
        public string TaiKhoan { get => _TaiKhoan; set => _TaiKhoan = value; }
        private string _MatKhau;
        public string MatKhau { get => _MatKhau; set => _MatKhau = value; }
        public NhanVien(string id, string ten, string chucvu, string diachi, bool fulltime, string sdt, string ngayvl, string ngsinh, string tk, string mk)
        {
            MaNV = id;
            HoTen = ten;
            ChucVu = chucvu;
            DiaChi = diachi;
            Fulltime = fulltime;
            SDT = sdt;
            NgayVaoLam = ngayvl;
            NgaySinh = ngsinh;
            TaiKhoan = tk;
            MatKhau = mk;
        }
    }
}
