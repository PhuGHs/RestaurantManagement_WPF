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
using System.Windows;

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
                    GetInputInfo(Selected.TenSanPham);
                } 
                OnPropertyChanged();
            }
        }
        private ObservableCollection<NhapKho> _ListIn;
        public ObservableCollection<NhapKho> ListIn { get => _ListIn; set { _ListIn = value; OnPropertyChanged(); } }
        private string _TimeSelected;
        public string TimeSelected 
        { 
            get => _TimeSelected; 
            set 
            {
                _TimeSelected = value;
                OnPropertyChanged();
                if (!String.IsNullOrEmpty(TimeSelected))
                {
                    foreach (NhapKho item in ListIn)
                    {
                        if (item.NgayNhap.ToString() == TimeSelected)
                        {
                            ID = item.MaNhap;
                            Name = item.TenSP;
                            Count = item.SoLuong;
                            Unit = item.DonVi;
                            Value = item.DonGia;
                            DateIn = item.NgayNhap;
                            Suplier = item.NguonNhap;
                            SuplierInfo = item.LienLac;

                            IDBeforeEdit = ID;
                        }
                    }
                }
            } 
        }
        private ObservableCollection<string> _ListTime;
        public ObservableCollection<string> ListTime { get => _ListTime; set { _ListTime = value; OnPropertyChanged(); } }

        private string IDBeforeEdit;
        private string _ID;
        public string ID { get => _ID; set { _ID = value; OnPropertyChanged(); } }
        private string _Name;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }
        private int _Count;
        public int Count { get => _Count; set { _Count = value; OnPropertyChanged(); } }
        private string _Unit;
        public string Unit { get => _Unit; set { _Unit = value; OnPropertyChanged(); } }
        private string _Value;
        public string Value { get => _Value; set { _Value = value; OnPropertyChanged(); } }
        private string _DateIn;
        public string DateIn { get => _DateIn; set { _DateIn = value; OnPropertyChanged("DateIn"); } }
        private string _Suplier;
        public string Suplier { get => _Suplier; set { _Suplier = value; OnPropertyChanged(); } }
        private string _SuplierInfo;
        public string SuplierInfo { get => _SuplierInfo; set { _SuplierInfo = value; OnPropertyChanged(); } }
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
                    strQuery = "SELECT * FROM KHO WHERE TenSP LIKE '%" + Search + "%'";
                }
                else
                    strQuery = "SELECT * FROM KHO";
                ListViewDisplay(strQuery);
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
            ListIn = new ObservableCollection<NhapKho>();
            ListTime = new ObservableCollection<string>();
            DateIn = DateTime.Now.ToShortDateString();

            ListViewDisplay("SELECT * FROM KHO");


            #region //add command
            AddCM = new RelayCommand<object>((p) => 
            {
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Count.ToString()) || string.IsNullOrEmpty(DateIn.ToString()) || string.IsNullOrEmpty(Unit) || string.IsNullOrEmpty(Value))
                    return false;
                OnPropertyChanged("ID");
                foreach (NhapKho item in ListIn)
                {
                    if (ID == item.MaNhap)
                        return false;
                }
                return true;
            }, (p) =>
            {
                OpenConnect();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO CHITIETNHAP(MaNhap, TenSP, DonVi, DonGia, SoLuong, NgayNhap, NguonNhap, LienLac) VALUES ('" + ID +"',N'" + Name + "',N'" + Unit + "'," + Value + "," + Count + ",'"+ DateIn +"',N'" + Suplier + "','" + SuplierInfo + "')";
                cmd.Connection = sqlCon;

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MyMessageBox mess = new MyMessageBox("Nhập thành công!");
                    mess.ShowDialog();
                    GetInputInfo(Name);
                }
                else
                {
                    MyMessageBox mess = new MyMessageBox("Nhập không thành công!");
                    mess.ShowDialog();
                }    
                ListViewDisplay("SELECT * FROM KHO");


                CloseConnect();
            });
            #endregion


            #region // edit command
            EditCM = new RelayCommand<object>((p) => 
            {
                foreach (NhapKho item in ListIn)
                {
                    if (ID == item.MaNhap && Name == item.TenSP && Count == item.SoLuong && DateIn == item.NgayNhap && Value == item.DonGia && Unit == item.DonVi && Suplier == item.NguonNhap && SuplierInfo == item.LienLac)
                        return false;
                }
                if (string.IsNullOrEmpty(ID) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Count.ToString()) || string.IsNullOrEmpty(DateIn.ToString()) || string.IsNullOrEmpty(Unit) || string.IsNullOrEmpty(Value))
                    return false;
                return true;
            }, (p) =>
            {
                OpenConnect();

                if (ID != IDBeforeEdit)
                {
                    MyMessageBox mess = new MyMessageBox("Không được sửa ID!");
                    mess.ShowDialog();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE CHITIETNHAP SET TenSP = N'" + Name + "', DonVi = N'" + Unit + "', DonGia = " + Value + ", SoLuong = " + Count + ", NgayNhap = '" + DateIn + "', NguonNhap = N'" + Suplier + "', LienLac = '" + SuplierInfo + "' WHERE MaNhap = '" + ID + "'";
                    cmd.Connection = sqlCon;

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MyMessageBox mess = new MyMessageBox("Sửa thành công!");
                        mess.ShowDialog();
                        GetInputInfo(Name);
                    }
                    else
                    {
                        MyMessageBox mess = new MyMessageBox("Sửa không thành công!");
                        mess.ShowDialog();
                    }
                    ListViewDisplay("SELECT * FROM KHO");
                }

                CloseConnect();
            });
            #endregion
            
            
            DeleteCM = new RelayCommand<object>((p) => 
            {
                if (Selected == null) return false;
                return true; 
            }, (p) =>
            {
                OpenConnect();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM KHO WHERE TenSP = N'" + Selected.TenSanPham + "'";
                cmd.Connection = sqlCon;

                MyMessageBox yesno = new MyMessageBox("Bạn có chắc chắn xóa?", true);
                yesno.ShowDialog();

                if (yesno.ACCEPT())
                {
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MyMessageBox mess = new MyMessageBox("Xóa thành công!");
                        mess.ShowDialog();
                        RefreshRightCard();
                    }
                    else
                    {
                        MyMessageBox mess = new MyMessageBox("Xóa không thành công!");
                        mess.ShowDialog();
                    }    
                }
                ListViewDisplay("SELECT * FROM KHO");

                CloseConnect();
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

        private void ListViewDisplay(string strQuery)
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
                string ten = reader.GetString(0);
                int tondu = reader.GetInt16(1);
                string donvi = reader.GetString(2);
                string dongia = reader.GetSqlMoney(3).ToString();
                ListWareHouse.Add(new Kho(ten, tondu, donvi, dongia));
            }

            CloseConnect();
        }

        private void GetInputInfo(string tensanpham)
        {
            OpenConnect();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT TOP 5 * FROM CHITIETNHAP WHERE TenSP = N'" + tensanpham + "' ORDER BY NgayNhap DESC";
            cmd.Connection = sqlCon;
            SqlDataReader reader = cmd.ExecuteReader();
            ListIn.Clear();
            ListTime.Clear();
            while (reader.Read())
            {
                string ma = reader.GetString(0);
                string ten = reader.GetString(1);
                string donvi = reader.GetString(2);
                string dongia = reader.GetSqlMoney(3).ToString();
                int soluong = reader.GetInt16(4);
                string date = reader.GetDateTime(5).ToShortDateString();
                string nguon = reader.GetString(6);
                string lienlac = reader.GetString(7);
                ListIn.Add(new NhapKho(ma, ten, donvi, dongia, soluong, date, nguon, lienlac));
                ListTime.Add(date);
            }
            TimeSelected = ListTime[0].ToString();

            CloseConnect();
        }

        private void RefreshRightCard()
        {
            ID = "";
            Name = "";
            Count = 0;
            Unit = "";
            Value = "";
            DateIn = "";
            Suplier = "";
            SuplierInfo = "";

            Selected = null;
        }
    }
}
