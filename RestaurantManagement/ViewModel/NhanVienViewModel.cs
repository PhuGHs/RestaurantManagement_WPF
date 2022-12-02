using QuanLyNhaHang.Models;
using RestaurantManagement.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyNhaHang.ViewModel
{
    public class NhanVienViewModel : BaseViewModel
    {
        private ObservableCollection<NhanVien> _ListStaff;
        public ObservableCollection<NhanVien> ListStaff { get => _ListStaff; set { _ListStaff = value; OnPropertyChanged(); } }

        #region // List View Selected Item
        private NhanVien _Selected;
        public NhanVien Selected
        {
            get => _Selected;
            set
            {
                _Selected = value;
                OnPropertyChanged();
                if (Selected != null)
                {
                    ID = Selected.MaNV;
                    Name = Selected.HoTen;
                    Position = Selected.ChucVu;
                    if (Selected.Fulltime) Fulltime = "Full-time";
                    else Fulltime = "Part-time";
                    Address = Selected.DiaChi;
                    Phone = Selected.SDT;
                    DateBorn = Selected.NgaySinh;
                    DateStartWork = Selected.NgayVaoLam;
                    Account = Selected.TaiKhoan;
                    Password = Selected.MatKhau;
                }
                OnPropertyChanged();
            }
        }
        #endregion

        #region // right card
        private string _ID;
        public string ID { get => _ID; set { _ID = value; OnPropertyChanged(); } }
        private string _Name;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }
        private string _Position;
        public string Position { get => _Position; set { _Position = value; OnPropertyChanged(); } }
        private string _Fulltime;
        public string Fulltime { get => _Fulltime; set { _Fulltime = value; OnPropertyChanged(); } }
        private string _Address;
        public string Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }
        private string _Phone;
        public string Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }
        private string _DateBorn;
        public string DateBorn { get => _DateBorn; set { _DateBorn = value; OnPropertyChanged(); } }
        private string _DateStartWork;
        public string DateStartWork { get => _DateStartWork; set { _DateStartWork = value; OnPropertyChanged(); } }
        private string _Account;
        public string Account { get => _Account; set { _Account = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }
        #endregion

        #region // Search bar
        private string _Search;
        public string Search
        {
            get => _Search;
            set
            {
                _Search = value;
                string strQuery;
                OnPropertyChanged();
                if (!String.IsNullOrEmpty(Search))
                {
                    strQuery = "SELECT * FROM NHANVIEN WHERE HoTen LIKE '%" + Search + "%'";
                }
                else
                    strQuery = "SELECT * FROM NHANVIEN";
                ListViewDisplay(strQuery);
            }
        }
        #endregion

        public ICommand AddCM { get; set; }
        public ICommand EditCM { get; set; }
        public ICommand DeleteCM { get; set; }
        public ICommand CheckCM { get; set; }

        private string strCon = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True";
        private SqlConnection sqlCon = null;

        public NhanVienViewModel()
        {
            OpenConnect();

            ListStaff = new ObservableCollection<NhanVien>();
            ListViewDisplay("SELECT * FROM NHANVIEN");

            AddCM = new RelayCommand<object>((p) => 
            {
                foreach(NhanVien item in ListStaff)
                {
                    if (item.MaNV == ID) return false;
                }
                if (string.IsNullOrEmpty(ID) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Position) || string.IsNullOrEmpty(Fulltime) || string.IsNullOrEmpty(DateStartWork))
                    return false;
                return true;
            }, (p) =>
            {
                OpenConnect();

                int ft;
                if (Fulltime == "Full-time") ft = 1;
                else ft = 0;

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO NHANVIEN VALUES ('" + ID + "',N'" + Name + "',N'" + Position + "',N'" + Address + "'," + ft + ",'" + Account + "','" + Password + "','" + Phone + "','" + DateStartWork + "','" + DateBorn + "')";
                cmd.Connection = sqlCon;

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MyMessageBox mess = new MyMessageBox("Nhập thành công!");
                    mess.ShowDialog();
                }
                else
                {
                    MyMessageBox mess = new MyMessageBox("Nhập không thành công!");
                    mess.ShowDialog();
                }
                ListViewDisplay("SELECT * FROM NHANVIEN");

                CloseConnect();
            });
            EditCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MyMessageBox mess = new MyMessageBox("Sửa thành công!");
                mess.ShowDialog();
            });
            DeleteCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MyMessageBox yesno = new MyMessageBox("Bạn có chắc chắn xóa ?", true);
                yesno.ShowDialog();
                if (yesno.ACCEPT())
                {
                    MyMessageBox mess = new MyMessageBox("Xóa thành công!");
                    mess.ShowDialog();
                }
            });
            CheckCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ChamCong chamCong = new ChamCong();
                chamCong.Show();
                return;
            });

            CloseConnect();
        }

        private void ListViewDisplay(string strQuery)
        {
            OpenConnect();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strQuery;
            cmd.Connection = sqlCon;
            SqlDataReader reader = cmd.ExecuteReader();

            ListStaff.Clear();
            while (reader.Read())
            {
                string id = reader.GetString(0);
                string ten = reader.GetString(1);
                string chucvu = reader.GetString(2);
                string diachi = reader.GetString(3);
                bool ftime = reader.GetBoolean(4);
                string tk = reader.GetString(5);
                string mk = reader.GetString(6);
                string sdt = reader.GetString(7);
                string ngvl = reader.GetDateTime(8).ToShortDateString();
                string ngsinh = reader.GetDateTime(9).ToShortDateString();
                ListStaff.Add(new NhanVien(id, ten, chucvu, diachi, ftime, sdt, ngvl, ngsinh, tk, mk));
            }

            CloseConnect();
        }
        private void OpenConnect()
        {
            sqlCon = new SqlConnection(strCon);
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
        }
        private void CloseConnect()
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }
}
