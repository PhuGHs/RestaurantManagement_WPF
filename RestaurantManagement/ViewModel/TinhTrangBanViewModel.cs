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

namespace QuanLyNhaHang.ViewModel
{
    public class TinhTrangBanViewModel : BaseViewModel
    {
        public TinhTrangBanViewModel()
        {
            StatusOfTableCommand = new RelayCommand<NameOfColor>((p) => true, (p) => GetStatusOfTable());
        }
        #region attributes
        private ObservableCollection<NameOfColor> _colors;
        private ObservableCollection<SelectedMenuItems> _selectedItems;
        private string titleofbill = "";
        private string background = "Green";
        private string sumofbill = "0 VND";
        private int statusoftable = 0;
        private int statusofbill = 0;
        #endregion

        #region properties
        public ObservableCollection<NameOfColor> Colors { get { return _colors; } set { _colors = value; OnPropertyChanged(); } }
        public ObservableCollection<SelectedMenuItems> SelectedItems { get { return _selectedItems; } set { _selectedItems = value; } }
        public string TitleOfBill
        {
            get { return titleofbill; }
            set { titleofbill = value; OnPropertyChanged(); }
        }
        public string Background
        {
            get { return background; }
            set { background = value; OnPropertyChanged(); }
        }
        public string SumofBill
        {
            get { return sumofbill; }
            set { sumofbill = value; OnPropertyChanged(); }
        }
        public int StatusofTable
        {
            get { return statusoftable; }
            set { statusoftable = value; OnPropertyChanged(); }
        }
        public int StatusofBill
        {
            get { return statusofbill; }
            set { statusofbill = value; OnPropertyChanged(); }
        }
        #endregion
        #region commands
        public ICommand StatusOfTableCommand { get; set; }
        #endregion

        #region methods
        public void GetStatusOfTable()
        {
            if (StatusofTable == 0)
            {
                Background = "Red";
                StatusofTable = 1;
            }
            else
            {
                Background = "Green";
                StatusofTable = 0;
            }
            if (StatusofBill == 1)
            {
                Background = "Green";
                StatusofTable = 0;
            }
        }
        public void GetTitleOfBill()
        {

        }
        #endregion

    }
}
