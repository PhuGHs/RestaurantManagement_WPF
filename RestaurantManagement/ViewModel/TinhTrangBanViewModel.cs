using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Menu.Models;
using QuanLyNhaHang.View;
using RestaurantManagement.Models;
using TinhTrangBan.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using OfficeOpenXml.ConditionalFormatting;

namespace QuanLyNhaHang.ViewModel
{
    public class TinhTrangBanViewModel : BaseViewModel
    {
        string connectstring = ConfigurationManager.ConnectionStrings["QuanLyNhaHang"].ConnectionString;
        public TinhTrangBanViewModel()
        {
            StatusOfTableCommand = new RelayCommand<Table>((p) => true, (p) => GetStatusOfTable(p.ID));
            GetPaymentCommand = new RelayCommand<Table>((p) => true, (p) => Payment()); 
            LoadTables();
        }
        #region attributes
        private ObservableCollection<Table> _tables = new ObservableCollection<Table>();
        private ObservableCollection<SelectedMenuItems> _selectedItems = new ObservableCollection<SelectedMenuItems>();
        private string titleofbill = "";
        private decimal dec_sumofbill = 0;
        private string sumofbill = "0 VND";
        int A = 0;
        int IDofPaidTable = 0;
        #endregion

        #region properties
        public ObservableCollection<Table> Tables { get { return _tables; } set { _tables = value; OnPropertyChanged(); } }
        public ObservableCollection<SelectedMenuItems> SelectedItems { get { return _selectedItems; } set { _selectedItems = value; } }
        public string TitleOfBill
        {
            get { return titleofbill; }
            set { titleofbill = value; OnPropertyChanged(); }
        }
        public decimal Dec_sumofbill
        {
            get { return dec_sumofbill; }
            set { dec_sumofbill = value; OnPropertyChanged(); }
        }
        public string SumofBill
        {
            get { return sumofbill; }
            set { sumofbill = value; OnPropertyChanged(); }
        }
       
        #endregion
        #region commands
        public ICommand StatusOfTableCommand { get; set; }
        public ICommand GetPaymentCommand { get; set; }
        #endregion

        #region methods
        public void LoadTables()
        {
            _tables.Add(new Table { NumOfTable = "Bàn 1", ID = 1, Status = 0, Coloroftable = "Green", Bill_ID = 1000 });
            _tables.Add(new Table { NumOfTable = "Bàn 2", ID = 2, Status = 0, Coloroftable = "Green", Bill_ID = 1000 });
            _tables.Add(new Table { NumOfTable = "Bàn 3", ID = 3, Status = 0, Coloroftable = "Green", Bill_ID = 1000 });
            _tables.Add(new Table { NumOfTable = "Bàn 4", ID = 4, Status = 0, Coloroftable = "Green", Bill_ID = 1000 });
            _tables.Add(new Table { NumOfTable = "Bàn 5", ID = 5, Status = 0, Coloroftable = "Green", Bill_ID = 1000 });
            _tables.Add(new Table { NumOfTable = "Bàn 6", ID = 6, Status = 0, Coloroftable = "Green", Bill_ID = 1000 });
            _tables.Add(new Table { NumOfTable = "Bàn 7", ID = 7, Status = 0, Coloroftable = "Green", Bill_ID = 1000 });
            _tables.Add(new Table { NumOfTable = "Bàn 8", ID = 8, Status = 0, Coloroftable = "Green", Bill_ID = 1000 });
            _tables.Add(new Table { NumOfTable = "Bàn 9", ID = 9, Status = 0, Coloroftable = "Green", Bill_ID = 1000 });
            _tables.Add(new Table { NumOfTable = "Bàn 10", ID = 10, Status = 0, Coloroftable = "Green", Bill_ID = 1000 });
            _tables.Add(new Table { NumOfTable = "Bàn 11", ID = 11, Status = 0, Coloroftable = "Green", Bill_ID = 1000 });
            _tables.Add(new Table { NumOfTable = "Bàn 12", ID = 12, Status = 0, Coloroftable = "Green", Bill_ID = 1000 });
            _tables.Add(new Table { NumOfTable = "Bàn 13", ID = 13, Status = 0, Coloroftable = "Green", Bill_ID = 1000 });
            _tables.Add(new Table { NumOfTable = "Bàn 14", ID = 14, Status = 0, Coloroftable = "Green", Bill_ID = 1000 });
            _tables.Add(new Table { NumOfTable = "Bàn 15", ID = 15, Status = 0, Coloroftable = "Green", Bill_ID = 1000 });

            Tables = _tables;
        }
        public void DisplayBill(int BillID)
        {
            string FoodName;
            decimal Price;
            int Quantity;
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "Select TenMon, SoLuong, Gia * SoLuong " +
                    "from CTHD inner join MENU on CTHD.MaMon = MENU.MaMon " +
                    "where CTHD.SoHD = @SOHD";
                cmd.Parameters.AddWithValue("@SOHD", BillID);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        FoodName = reader.GetString(0);
                        Quantity = reader.GetInt32(1);
                        Price = reader.GetDecimal(2);
                        SelectedMenuItems selected = new SelectedMenuItems(FoodName, Price, Quantity);
                        SelectedItems.Add(selected);

                        Dec_sumofbill += Price;
                        SumofBill = String.Format("{0:0,0 VND}", Dec_sumofbill);
                    }
                    catch
                    {
                        FoodName = "";
                        Quantity = 0;
                        Price = 0;
                    }
                }
                con.Close();
            }
            
        }
        public void GetStatusOfTable(int ID)
        {
            foreach (Table table in _tables)
            {
                if (table.ID == ID)
                {
                    if (table.Status == 0)
                    {
                        table.Coloroftable = "Red";
                        table.Status = 1;
                        table.Bill_ID = 1000; //Demo hiển thị, gán = Bill_ID + A, vì hd sau khi đặt 1 bàn sẽ tăng dần, khi cộng sẽ thành sohd
                        A++;
                    }
                    else
                    {
                        TitleOfBill = table.NumOfTable;
                        DisplayBill(table.Bill_ID);
                        IDofPaidTable = table.ID;
                    }
                    break;
                }
            }
        }
        public void Payment()
        {
            foreach (Table table in _tables)
            {
                if (table.ID == IDofPaidTable)
                {
                    table.Coloroftable = "Green";
                    table.Status = 0;
                    Dec_sumofbill = 0;
                    SumofBill = String.Format("{0:0,0 VND}", Dec_sumofbill);
                    SelectedItems.Clear();
                    break;
                }
            }
        }
        
        #endregion

    }
}
