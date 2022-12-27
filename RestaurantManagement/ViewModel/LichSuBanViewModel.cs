using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LichSuBan.Models;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.IO;
using System.Windows;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;
using DataTable = System.Data.DataTable;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyNhaHang.ViewModel
{
    public class LichSuBanViewModel : BaseViewModel
    {
        private bool isGettingSource;
        public bool IsGettingSource
        {
            get { return isGettingSource; }
            set { isGettingSource = value; OnPropertyChanged(); }
        }

        private DateTime _getCurrentDate;
        public DateTime GetCurrentDate
        {
            get { return _getCurrentDate; }
            set { _getCurrentDate = value; }
        }
        private string _setCurrentDate;
        public string SetCurrentDate
        {
            get { return _setCurrentDate; }
            set { _setCurrentDate = value; }
        }

        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value; OnPropertyChanged(); }
        }
        private ComboBoxItem _SelectedItemFilter;
        public ComboBoxItem SelectedItemFilter
        {
            get { return _SelectedItemFilter; }
            set { _SelectedItemFilter = value; OnPropertyChanged(); }
        }
        private ComboBoxItem _SelectedImportItemFilter;
        public ComboBoxItem SelectedImportItemFilter
        {
            get { return _SelectedImportItemFilter; }
            set { _SelectedImportItemFilter = value; OnPropertyChanged(); }
        }
        private int _SelectedMonth;
        public int SelectedMonth
        {
            get { return _SelectedMonth; }
            set { _SelectedMonth = value; OnPropertyChanged(); }
        }

        private int _SelectedImportMonth;
        public int SelectedImportMonth
        {
            get { return _SelectedImportMonth; }
            set { _SelectedImportMonth = value; OnPropertyChanged(); }
        }




        private ObservableCollection<LichSuBanModel> _ListProduct;

        public ObservableCollection<LichSuBanModel> ListProduct { get => _ListProduct; set { _ListProduct = value; OnPropertyChanged(); } }

        private string timkiem;
        
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
                    strQuery = "select ct.SOHD, mn.MAMON, TENMON, SOLUONG, TRIGIA, NGAYHD from hoadon1 hd join CTHD1 ct on hd.SOHD = ct.SOHD join MENU1 mn on ct.MAMON = mn.MAMON WHERE TENMON LIKE N'%" + Search + "%'";
                }
                else
                    strQuery = "select ct.SOHD, mn.MAMON, TENMON, SOLUONG, TRIGIA, NGAYHD from hoadon1 hd join CTHD1 ct on hd.SOHD = ct.SOHD join MENU1 mn on ct.MAMON = mn.MAMON";
                ListViewDisplay(strQuery);
            }
        }
        private string strCon = @"Data Source=DESKTOP-ADQ1342;Initial Catalog=QuanlyDoAn;Integrated Security=True";
        private SqlConnection sqlCon = null;


        public ICommand LoadImportPageCM { get; set; }
        public ICommand LoadExportPageCM { get; set; }
        public ICommand ExportFileCM { get; set; }
        public ICommand CheckImportItemFilterCM { get; set; }
        public ICommand SelectedImportMonthCM { get; set; }
        public ICommand SelectedMonthCM { get; set; }
        public ICommand CheckCM { get; set; }

        public ICommand CheckItemFilterCM { get; set; }
      
        public LichSuBanViewModel()
        {

            ListProduct = new ObservableCollection<LichSuBanModel>();


            ListViewDisplay("select ct.SOHD, mn.MAMON, TENMON, SOLUONG, TRIGIA, NGAYHD from hoadon1 hd join CTHD1 ct on hd.SOHD = ct.SOHD join MENU1 mn on ct.MAMON = mn.MAMON");
            OpenConnect();

            GetCurrentDate = DateTime.Today;
            SelectedDate = GetCurrentDate;
            SelectedMonth = DateTime.Now.Month - 1;
            SelectedImportMonth = DateTime.Now.Month - 1;
            SelectedMonthCM = new RelayCommand<System.Windows.Controls.ComboBox>((p) => { return true; }, async (p) =>
            {
                await CheckMonthFilter();
            });
            CheckCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                MyMessageBox mess = new MyMessageBox("Kiểm tra");
                mess.ShowDialog();

            });
            CheckItemFilterCM = new RelayCommand<System.Windows.Controls.ComboBox>((p) => { return true; }, async (p) =>
            {
                await CheckItemFilter();
            });
            ExportFileCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ExportToFileFunc();
            });
            CloseConnect();
        }


        public void ExportToFileFunc()
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx", ValidateNames = true })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                    app.Visible = false;
                    Workbook wb = app.Workbooks.Add(XlSheetType.xlWorksheet);
                    Worksheet ws = (Worksheet)app.ActiveSheet;


                    ws.Cells[1, 1] = "Mã đơn";
                    ws.Cells[1, 2] = "Tên sản phẩm";
                    ws.Cells[1, 3] = "Số lượng";
                    ws.Cells[1, 4] = "Tổng giá";
                    ws.Cells[1, 5] = "Ngày nhập";

                    int i2 = 2;
                    foreach (var item in ListProduct)
                    {
                        
                        ws.Cells[i2, 1] = item.MaMon;
                        ws.Cells[i2, 2] = item.TenMon;
                        ws.Cells[i2, 3] = item.SoLuong;
                        ws.Cells[i2, 4] = item.TriGia;
                        ws.Cells[i2, 5] = item.ngayHD;


                        i2++;
                    }
                    ws.SaveAs(sfd.FileName, XlFileFormat.xlWorkbookDefault, Type.Missing, true, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges,Type.Missing, Type.Missing) ;
                   
                    app.Quit();

                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

                    MyMessageBox mb = new MyMessageBox("Xuất file thành công");
                    mb.ShowDialog();
                }
            }

        }
        private void OpenConnect()
        {
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }

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
            ListProduct.Clear();
            while (reader.Read())
            {
                int madon = reader.GetInt32(0);
                string mamon = reader.GetString(1);
                string ten = reader.GetString(2);
                int soluong = reader.GetInt32(3);
                string gia = reader.GetSqlMoney(4).ToString();
                string thoigian = reader.GetDateTime(5).ToShortDateString();

                ListProduct.Add(new LichSuBanModel(madon,mamon, ten, soluong, gia, thoigian));
            }

            CloseConnect();
        }
        public async Task CheckMonthFilter()
        {
            /* try
             {
                 ListBill = new ObservableCollection<HoaDon>(await BillService.Ins.GetBillByMonth(SelectedMonth + 1));
             }
             catch (Exception e)
             {
                 Console.WriteLine(e);
                 MyMessageBox mb = new MyMessageBox("Lỗi");
                 mb.ShowDialog();
             }*/


        }
        public async Task GetExportListSource(string s = "")
        {
            /*ListBill = new ObservableCollection<BillDTO>();
            switch (s)
            {
                case "date":
                    {
                        try
                        {
                            IsGettingSource = true;
                            ListBill = new ObservableCollection<BillDTO>(await BillService.Ins.GetBillByDate(SelectedDate));
                            ResultName.Content = ListBill.Count;
                            IsGettingSource = false;
                            return;
                        }
                        catch (System.Data.Entity.Core.EntityException e)
                        {
                            Console.WriteLine(e);
                            MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Mất kết nối cơ sở dữ liệu", MessageType.Error, MessageButtons.OK);
                            mb.ShowDialog();
                            throw;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            MessageBoxCustom mb = new MessageBoxCustom("Lỗi", "Lỗi hệ thống", MessageType.Error, MessageButtons.OK);
                            mb.ShowDialog();
                            throw;
                        }

                    }
                case "":
                    {
                     
                        
                            IsGettingSource = true;
                            ListProduct = new ObservableCollection<LichSuBanModel>(await BillService.Ins.GetAllBill());
                            ResultName.Content = ListBill.Count;
                            IsGettingSource = false;
                            return;
                        
                       
                        

                    }
                case "month":
                    {
                        IsGettingSource = true;
                        await CheckMonthFilter();
                        ResultName.Content = ListBill.Count;
                        IsGettingSource = false;
                        return;
                    }
            }*/
        }
        public async Task CheckItemFilter()
        {
            switch (SelectedItemFilter.Content.ToString())
            {
                case "Toàn bộ":
                    {
                        await GetExportListSource("");
                        return;
                    }
                case "Theo ngày":
                    {
                        await GetExportListSource("date");
                        return;
                    }
                case "Theo tháng":
                    {
                        await GetExportListSource("month");
                        return;
                    }
            }
        }




    }
}

