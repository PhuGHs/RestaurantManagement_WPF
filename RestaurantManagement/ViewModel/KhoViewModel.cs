using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using QuanLyNhaHang.Models;

namespace QuanLyNhaHang.ViewModel
{
    public class KhoViewModel : BaseViewModel
    {
        private ObservableCollection<Kho> _ListWareHouse;
        public ObservableCollection<Kho> ListWareHouse { get => _ListWareHouse; set { _ListWareHouse = value; OnPropertyChanged(); } }
        private Kho _Selected;
        public Kho Selected 
        {
            get => _Selected; 
            set
            {
                _Selected = value;
                OnPropertyChanged();
                if (Selected != null)
                {
                    Name = Selected.TenNguyenLieu;
                    Remaining = Selected.TonDu.ToString();
                    Unit = Selected.DonVi;
                    Value = Selected.DonGia;
                    DateIn = Selected.NgayNhap;
                    Suplier = Selected.NguonNhap;
                    SuplierPhone = Selected.LienLac;
                    Note = Selected.GhiChu;
                } 
            }
        }


        private string _Name;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }
        private string _Remaining;
        public string Remaining { get => _Remaining; set { _Remaining = value; OnPropertyChanged(); } }
        private string _Unit;
        public string Unit { get => _Unit; set { _Unit = value; OnPropertyChanged(); } }
        private string _Value;
        public string Value { get => _Value; set { _Value = value; OnPropertyChanged(); } }
        private string _DateIn;
        public string DateIn { get => _DateIn; set { _DateIn = value; OnPropertyChanged(); } }
        private string _Suplier;
        public string Suplier { get => _Suplier; set { _Suplier = value; OnPropertyChanged(); } }
        private string _SuplierPhone;
        public string SuplierPhone { get => _SuplierPhone; set { _SuplierPhone = value; OnPropertyChanged(); } }
        private string _Note;
        public string Note { get => _Note; set { _Note = value; OnPropertyChanged(); } }
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
                    strQuery = "SELECT * FROM NGUYENLIEU WHERE TENNL LIKE '%" + Search + "%'";
                }
                else
                    strQuery = "SELECT * FROM NGUYENLIEU";
                ListChanged(strQuery);
            } 
        }


        public ICommand AddCM { get; set; }
        public ICommand EditCM { get; set; }
        public ICommand DeleteCM { get; set; }
        public ICommand CheckCM { get; set; }
        private string strCon = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True";
        private SqlConnection sqlCon = null;
        public KhoViewModel()
        {
            OpenConnect();
            ListWareHouse = new ObservableCollection<Kho>();
            ListChanged("SELECT * FROM NGUYENLIEU");
            AddCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MyMessageBox mess = new MyMessageBox("Nhập thành công!");
                mess.ShowDialog();
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
                MyMessageBox mess = new MyMessageBox("Kiểm tra");
                mess.ShowDialog();
            });
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
        private void ListChanged(string strQuery)
        {
            OpenConnect();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strQuery;
            cmd.Connection = sqlCon;
            SqlDataReader reader = cmd.ExecuteReader();
            ListWareHouse.Clear();
            while (reader.Read())
            {
                string ten = reader.GetString(1);
                int tondu = reader.GetInt16(2);
                string donvi = reader.GetString(3);
                string ngaynhap = reader.GetDateTime(4).ToShortDateString();
                string nguonnhap = reader.GetString(5);
                string lienlac = reader.GetString(6);
                string ghichu = "";
                if (!reader.IsDBNull(7))
                    ghichu = reader.GetString(7);
                string dongia = reader.GetSqlMoney(8).ToString();
                ListWareHouse.Add(new Kho(ten, tondu, donvi, ngaynhap, nguonnhap, dongia, lienlac, ghichu));
            }

            CloseConnect();
        }
        bool IsExists()
        {
            return true;
        }
    }
}
