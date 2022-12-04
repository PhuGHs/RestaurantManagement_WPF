using QuanLyNhaHang.Models;
using QuanLyNhaHang.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyNhaHang.ViewModel
{
    public class ChamCongViewModel : BaseViewModel
    {
        private ObservableCollection<NhanVien> _ListFulltime;
        public ObservableCollection<NhanVien> ListFulltime { get => _ListFulltime; set { _ListFulltime = value; OnPropertyChanged(); } }
        private ObservableCollection<NhanVien> _ListParttime;
        public ObservableCollection<NhanVien> ListParttime { get => _ListParttime; set { _ListParttime = value; OnPropertyChanged(); } }

        private string strCon = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True";
        private SqlConnection sqlCon = null;

        public ICommand CloseCM { get; set; }
        public ChamCongViewModel()
        {
            CloseCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (p == null) return;
                p.Close();
            });
        }

        private void ListViewDisplay(string strQuery)
        {
            OpenConnect();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strQuery;
            cmd.Connection = sqlCon;
            SqlDataReader reader = cmd.ExecuteReader();

            ListFulltime.Clear();
            ListParttime.Clear();
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
                if (ftime)
                    ListFulltime.Add(new NhanVien(id, ten, chucvu, diachi, ftime, sdt, ngvl, ngsinh, tk, mk));
                else
                    ListParttime.Add(new NhanVien(id, ten, chucvu, diachi, ftime, sdt, ngvl, ngsinh, tk, mk));
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
