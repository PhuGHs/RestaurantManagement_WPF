using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyNhaHang.Command;
using System.Windows.Input;
using Diacritics.Extensions;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.View;
using QuanLyNhaHang.DataProvider;
using QuanLyNhaHang.ViewModel;
using QuanLyNhaHang.State.Navigator;
using System.Windows.Data;
using System.ComponentModel;

namespace QuanLyNhaHang.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        public MenuViewModel()
        {
            //LoadMenu
            _menuItems = MenuDP.Flag.ConvertToCollection();
            _menuItemsView = new CollectionViewSource();
            _menuItemsView.Source = MenuItems;
            _menuItemsView.Filter += MenuItems_Filter;
            //Command actions
            OrderFeature_Command = new RelayCommand<MenuItem>((p) => true, (p) => OrderAnItem(p.ID));
            RemoveItemFeature_Command = new RelayCommand<SelectedMenuItem>((p) => true, (p) => RemoveAnItem(p));
            ClearAllSelectedDishes = new RelayCommand<object>((p) => true, (p) => {
                if (SelectedItems.Count > 0)
                {
                    MyMessageBox msb = new MyMessageBox("Bạn có muốn xoá tất cả những món đã chọn?", true);
                    msb.ShowDialog();
                    if (msb.ACCEPT() == false)
                    {
                        return;
                    }
                    SelectedItems.Clear();
                }
                else
                {
                    return;
                }
            });
            SortingFeature_Command = new RelayCommand<object>((p) => true, (p) => {
                SortMenuItems();
            });
            ShowDetailOrder_Command = new RelayCommand<object>((p) => true, (p) =>
            {
                OrderWindow OrderWin = new OrderWindow();
                OrderWin.ShowDialog();
            });
            //FindDishes = new RelayCommand<object>((p) => true, (p) => searchMealItems(SearchText));
            Inform_Chef_Of_OrderedDishes = new RelayCommand<object>((p) => true, (p) =>
            {
                foreach (SelectedMenuItem orderDish in SelectedItems)
                {
                    MenuDP.Flag.InformChef(orderDish.ID, 2, orderDish.Quantity);
                }
                MenuDP.Flag.PayABill(2);
                MyMessageBox msb = new MyMessageBox("Đã báo chế biến thành công!");
                msb.Show();
                SelectedItems.Clear();
                DecSubtotal = 0;
            });
            PayBillCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                
            });
            _selectedItems = new ObservableCollection<SelectedMenuItem>();
            _comboBox_2Items = new ObservableCollection<string>();
            LoadCombobox_2Items();
        }
          
        #region attributes
        private ObservableCollection<MenuItem> _menuItems;
        private ObservableCollection<SelectedMenuItem> _selectedItems;
        private ObservableCollection<string> _comboBox_2Items;
        private CollectionViewSource _menuItemsView;
        private string myComboboxSelection = "A -> Z";
        private decimal dec_subtotal = 0;
        private string str_subtotal = "0 VND";
        private string _searchText;
        #endregion

        #region properties
        public ObservableCollection<MenuItem> MenuItems { get { return _menuItems; } set { _menuItems = value; OnPropertyChanged(); } }
        public ObservableCollection<SelectedMenuItem> SelectedItems { get { return _selectedItems; } set { _selectedItems = value; OnPropertyChanged(); } }
        public ObservableCollection<string> ComboBox_2Items { get { return _comboBox_2Items; } set { _comboBox_2Items = value; } }
        public string MyComboboxSelection { get { return myComboboxSelection; } set { myComboboxSelection = value; OnPropertyChanged(); }}
        public ICollectionView MenuItemCollection
        {
            get
            {
                return this._menuItemsView.View;
            }
        }
        public string Day
        {
            get
            {
                return DateTime.Today.DayOfWeek.ToString() + ", " + DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        public Decimal DecSubtotal
        {
            set
            {
                dec_subtotal = value;
                OnPropertyChanged();
            }
            get { return dec_subtotal; }
        }

        public string StrSubtotal
        {
            set
            {
                str_subtotal = value;
                OnPropertyChanged();
            }
            get { return str_subtotal; }
        }
        public string SearchText { get { return _searchText; } set { _searchText = value; this._menuItemsView.View.Refresh(); OnPropertyChanged(); } }
        #endregion

        #region commands
        public ICommand OrderFeature_Command { get; set; }
        public ICommand RemoveItemFeature_Command { get; set; }
        public ICommand SortingFeature_Command { get; set; }
        public ICommand ShowDetailOrder_Command { get; set; }
        public ICommand FindDishes { get; set; }
        public ICommand ClearAllSelectedDishes { get; set; }
        public ICommand Inform_Chef_Of_OrderedDishes { get; set; }
        public ICommand SwitchCustomerTable { get; set; }
        public ICommand PayBillCommand { get; set; }
        #endregion

        #region methods
        private void LoadCombobox_2Items()
        {
            _comboBox_2Items.Add("Giá cao -> thấp");
            _comboBox_2Items.Add("Giá thấp -> cao");
            _comboBox_2Items.Add("A -> Z");
            _comboBox_2Items.Add("Z -> A");

            ComboBox_2Items = _comboBox_2Items;
        }

        private void OrderAnItem(string ID)
        {
            foreach (MenuItem item in _menuItems)
            {
                if (item.ID == ID)
                {
                    DecSubtotal += item.Price;
                    StrSubtotal = String.Format("{0:0,0 VND}", DecSubtotal);
                    SelectedMenuItem x = checkIfAnItemIsInOrderItems(ID);
                    if (x != null)
                    {
                        x.Quantity++;
                        return;
                    }
                    SelectedMenuItem s_item = new SelectedMenuItem(item.ID, item.FoodName, item.Price, 1);
                    SelectedItems.Add(s_item);
                    break;
                }
            }
        }
        private void RemoveAnItem(SelectedMenuItem x)
        {
            DecSubtotal -= x.Price;
            StrSubtotal = String.Format("{0:0,0 VND}", DecSubtotal);
            if (x.Quantity > 1)
            {
                x.Quantity--;
            }
            else
            {
                SelectedItems.Remove(x);
            }
        }

        public void SortMenuItems()
        {
            _menuItemsView.SortDescriptions.Clear();

            if (MyComboboxSelection == "Giá cao -> thấp")
            {
                _menuItemsView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Descending));
            }
            else if (MyComboboxSelection == "Giá thấp -> cao")
            {
                _menuItemsView.SortDescriptions.Add(new SortDescription("Price", ListSortDirection.Ascending));
            }
            else if (MyComboboxSelection == "A -> Z")
            {
                _menuItemsView.SortDescriptions.Add(new SortDescription("FoodName", ListSortDirection.Ascending));
            }
            else if (MyComboboxSelection == "Z -> A")
            {
                _menuItemsView.SortDescriptions.Add(new SortDescription("FoodName", ListSortDirection.Descending));
            }
        }
        #endregion

        #region complementary methods
        private SelectedMenuItem checkIfAnItemIsInOrderItems(string ID)
        {
            foreach (SelectedMenuItem item in _selectedItems)
            {
                if (item.ID == ID)
                {
                    return item;
                }
            }
            return null;
        }
        public void MenuItems_Filter(object sender, FilterEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                e.Accepted = true;
                return;
            }

            Models.MenuItem item = e.Item as Models.MenuItem;
            if (item.FoodName.RemoveDiacritics().ToLower().Contains(SearchText.RemoveDiacritics().ToLower()))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        #endregion
    }
}
