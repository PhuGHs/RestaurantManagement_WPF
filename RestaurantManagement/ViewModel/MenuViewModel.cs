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
using QuanLyNhaHang.State.Navigator;

namespace QuanLyNhaHang.ViewModel
{
    public class MenuViewModel : BaseViewModel, INavigator
    {
        public MenuViewModel()
        {
            //LoadMenu
            _menuItems = MenuDP.Flag.ConvertToCollection();
            //Command actions
            OrderFeature_Command = new RelayCommand<MenuItem>((p) => true, (p) => OrderAnItem(p.ID));
            RemoveItemFeature_Command = new RelayCommand<SelectedMenuItem>((p) => true, (p) => RemoveAnItem(p));
            ClearAllSelectedDishes = new RelayCommand<object>((p) => true, (p) => {
                if (SelectedItems.Count > 0)
                {
                    MyMessageBox msb = new MyMessageBox("Bạn có muốn xoá tất cả \n   những món đã chọn?", true);
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
                if (MyComboboxSelection == "Giá cao -> thấp")
                {
                    MenuItems = new ObservableCollection<MenuItem>(MenuItems.OrderByDescending(i => i.Price));
                }
                else if (MyComboboxSelection == "Giá thấp -> cao")
                {
                    MenuItems = new ObservableCollection<MenuItem>(MenuItems.OrderBy(i => i.Price));
                }
                else if (MyComboboxSelection == "A -> Z")
                {
                    MenuItems = new ObservableCollection<MenuItem>(MenuItems.OrderBy((i) => i.FoodName));
                }
                else if (MyComboboxSelection == "Z -> A")
                {
                    MenuItems = new ObservableCollection<MenuItem>(MenuItems.OrderByDescending(i => i.FoodName));
                }
            });
            ShowDetailOrder_Command = new RelayCommand<object>((p) => true, (p) =>
            {
                OrderWindow OrderWin = new OrderWindow();
                OrderWin.ShowDialog();
            });
            FindDishes = new RelayCommand<object>((p) => true, (p) => searchMealItems(SearchText));
            Inform_Chef_Of_OrderedDishes = new RelayCommand<object>((p) => true, (p) =>
            {
                foreach (SelectedMenuItem orderDish in SelectedItems)
                {
                    MenuDP.Flag.InformChef(orderDish.ID, 9, orderDish.Quantity);
                }
                MyMessageBox msb = new MyMessageBox("Đã báo chế biến thành công!");
                msb.Show();
                SelectedItems.Clear();
            });
            SwitchCustomerTable = new RelayCommand<object>((p) => true, (p) =>
            {
                SelectViewModelCommand.Execute(p);
            });
            PayBillCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                MenuDP.Flag.PayABill(9);
                MyMessageBox msb = new MyMessageBox("Thanh toán thành công!");
            });
            _selectedItems = new ObservableCollection<SelectedMenuItem>();
            _comboBox_2Items = new ObservableCollection<string>();
            LoadCombobox_2Items();
        }
          
        #region attributes
        private ObservableCollection<MenuItem> _menuItems;
        private ObservableCollection<SelectedMenuItem> _selectedItems;
        private ObservableCollection<string> _comboBox_2Items;
        private BaseViewModel _currentViewModel;
        private string _currentTitle;
        private string myComboboxSelection = "A -> Z";
        private decimal dec_subtotal = 0;
        private string str_subtotal = "0 VND";
        private string _searchText;
        #endregion

        #region properties
        public ObservableCollection<MenuItem> MenuItems { get { return _menuItems; } set { _menuItems = value; OnPropertyChanged(); } }
        public ObservableCollection<SelectedMenuItem> SelectedItems { get { return _selectedItems; } set { _selectedItems = value; } }
        public ObservableCollection<string> ComboBox_2Items { get { return _comboBox_2Items; } set { _comboBox_2Items = value; } }
        public string MyComboboxSelection { get { return myComboboxSelection; } set { myComboboxSelection = value; } }
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
                if (value != dec_subtotal)
                {
                    dec_subtotal = value;
                    OnPropertyChanged();
                }
            }
            get { return dec_subtotal; }
        }

        public string StrSubtotal
        {
            set
            {
                if (value != str_subtotal)
                {
                    str_subtotal = value;
                    OnPropertyChanged();
                }
            }
            get { return str_subtotal; }
        }
        public string SearchText { get { return _searchText; } set { _searchText = value; OnPropertyChanged(); } }
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
        public BaseViewModel CurrentViewModel { get => _currentViewModel; set { _currentViewModel = value; OnPropertyChanged(nameof(_currentViewModel)); } }
        public string CurrentTitle { get => _currentTitle; set { _currentTitle = value; OnPropertyChanged(nameof(_currentTitle)); } }

        public ICommand SelectViewModelCommand => new SelectViewModelCommand(this, this);
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
        public void searchMealItems(string keyword)
        {
            if (String.IsNullOrEmpty(keyword))
            {
                MenuItems = MenuDP.Flag.ConvertToCollection();
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    ObservableCollection<MenuItem> relatingItems = new ObservableCollection<MenuItem>();
                    foreach (MenuItem x in MenuItems)
                    {
                        if (x.FoodName.RemoveDiacritics().ToLower().Contains(keyword.RemoveDiacritics().ToLower()))
                        {
                            relatingItems.Add(x);
                        }
                    }
                    return relatingItems;
                }).ContinueWith(task =>
                {
                    MenuItems.Clear();
                    foreach (MenuItem result in task.Result)
                    {
                        MenuItems.Add(result);
                    }
                }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
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
        
        #endregion
    }
}
