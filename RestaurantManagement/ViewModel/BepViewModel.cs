using QuanLyNhaHang.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ListView = System.Windows.Forms.ListView;
namespace QuanLyNhaHang.ViewModel
{

    public class BepViewModel : BaseViewModel
    {
        private ObservableCollection<Bep> _ListDone;

        public ObservableCollection<Bep> ListDone { get => _ListDone; set { _ListDone = value; OnPropertyChanged(); } }
        private ObservableCollection<Bep> _ListOrder;

        public ObservableCollection<Bep> ListOrder { get => _ListOrder; set { _ListOrder = value; OnPropertyChanged(); } }
        private Bep _DoneSelected;
        public Bep DoneSelected
        {
            get
            {
                return _DoneSelected;
            }
            set
            {
                _DoneSelected = value;
                OnPropertyChanged();

            }
        }
        private Bep _OrderSelected;
        public Bep OrderSelected
        {
            get
            {
                return _OrderSelected;
            }
            set
            {
                _OrderSelected = value;
                OnPropertyChanged();
            }
        }

        private string strCon = @"Data Source=DESKTOP-ADQ1342;Initial Catalog=QuanlyDoAn;Integrated Security=True";
        private SqlConnection sqlCon = null;


        public ICommand OrderCM { get; set; }
        public ICommand DoneCM { get; set; }

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
        public BepViewModel()
        {
            ListDone = new ObservableCollection<Bep>();
            ListOrder = new ObservableCollection<Bep>();
            GetListDone();
            GetListOrder();

            OpenConnect();

            DoneCM = new RelayCommand<object>((p) =>
            {

                if (DoneSelected == null) return false;
                return true;
            }, (p) =>
            {
                OpenConnect();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE CHEBIEN1 SET TINHTRANG = 'XONG' WHERE MaDatMon = " + DoneSelected.MaDM;
              

                cmd.Connection = sqlCon;

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MyMessageBox msb = new MyMessageBox("Da che bien xong!");
                    msb.ShowDialog();
                }
                else
                {
                    MyMessageBox msb = new MyMessageBox("Da co loi xay ra!");
                    msb.ShowDialog();
                }

                GetListDone();
                GetListOrder();

                CloseConnect();
            });

            OrderCM = new RelayCommand<object>((p) =>
            {
                if (OrderSelected == null) return false;
                return true;
            }, (p) =>
            {
                OpenConnect();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "DELETE FROM CHEBIEN1 WHERE MaDatMon = " + OrderSelected.MaDM;
              

                cmd.Connection = sqlCon;
                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MyMessageBox ms = new MyMessageBox("Da phuc vu khach hang!");
                    ms.ShowDialog();
                }
                else
                {
                    MyMessageBox ms = new MyMessageBox("Da co loi xay ra!");
                    ms.ShowDialog();
                }

                GetListOrder();
                GetListDone();

                CloseConnect();
            });

        }
        private void GetListDone()
        {
            OpenConnect();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT c.*, m.TENMON FROM CHEBIEN1 AS c JOIN MENU1 AS m ON c.MAMON = m.MAMON WHERE TINHTRANG = N'CHUA'  ORDER BY NGAYCB, THOIGIANLAM ";
            cmd.Connection = sqlCon;
            SqlDataReader reader = cmd.ExecuteReader();
            ListDone.Clear();
            while (reader.Read())
            {

                string maMon = reader.GetString(1);
                int soBan = reader.GetInt16(3);
                int soLuong = reader.GetInt16(2);
                string tinhTrang = reader.GetString(5);
                string tenMon = reader.GetString(6);
                string ngayCB = reader.GetDateTime(4).ToShortDateString();
                long maDM = reader.GetInt32(0);
                ListDone.Add(new Bep(maDM, maMon, soBan, soLuong, ngayCB, "CHUA", tenMon));
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListDone);
                view.SortDescriptions.Add(new SortDescription("SELECT * FROM CHEBIEN1 ORDER BY NGAYCB", ListSortDirection.Ascending));


            }

            CloseConnect();
        }
        private void GetListOrder()
        {
            OpenConnect();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT c.*, m.TENMON FROM CHEBIEN1 AS c JOIN MENU1 AS m ON c.MAMON = m.MAMON WHERE TINHTRANG = N'XONG'";
            //cho nay luc truoc ong de tinhtrang = 'chua' thi sao ma lay dung duoc
            cmd.Connection = sqlCon;
            SqlDataReader reader = cmd.ExecuteReader();
            ListOrder.Clear();
            while (reader.Read())
            {

                string maMon = reader.GetString(1);
                int soBan = reader.GetInt16(3);
                int soLuong = reader.GetInt16(2);
                string tinhTrang = reader.GetString(5);
                string tenMon = reader.GetString(6);
                string ngayCB = reader.GetDateTime(4).ToShortDateString();
                long maDM = reader.GetInt32(0);
                ListOrder.Add(new Bep(maDM, maMon, soBan, soLuong, ngayCB, "XONG", tenMon));
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListDone);
            }


            CloseConnect();
        }
    }
}
