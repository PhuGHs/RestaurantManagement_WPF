using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Menu.Models;
using QuanLyNhaHang.View;

namespace QuanLyNhaHang.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        public MenuViewModel()
        {
            OrderFeature_Command = new RelayCommand<MenuItem>((p) => true, (p) => OrderAnItem(p.ID));
            RemoveItemFeature_Command = new RelayCommand<SelectedMenuItem>((p) => true, (p) => RemoveAnItem(p));
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
            _menuItems = new ObservableCollection<MenuItem>();
            _selectedItems = new ObservableCollection<SelectedMenuItem>();
            _comboBox_2Items = new ObservableCollection<string>();
            LoadMenuItems();
            LoadCombobox_2Items();
        }

        #region attributes
        private ObservableCollection<MenuItem> _menuItems;
        private ObservableCollection<SelectedMenuItem> _selectedItems;
        private ObservableCollection<string> _comboBox_2Items;
        private string myComboboxSelection = "A -> Z";
        private decimal dec_subtotal = 0;
        private string str_subtotal = "0 VND";
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
                if(value != str_subtotal)
                {
                    str_subtotal = value;
                    OnPropertyChanged();
                }
            }
            get { return str_subtotal; }
        }
        #endregion

        #region commands
        public ICommand OrderFeature_Command { get; set; }  
        public ICommand RemoveItemFeature_Command { get; set; }
        public ICommand SortingFeature_Command { get; set; }
        public ICommand ShowDetailOrder_Command { get; set; }
        #endregion

        #region methods
        private void LoadMenuItems()
        {
            _menuItems.Add(new MenuItem { FoodImage = "pack://application:,,,/images/menuitem_1.png", ID = 1, FoodName = "Phở Bò", Price = 45000 });
            _menuItems.Add(new MenuItem { FoodImage = "pack://application:,,,/images/menuitem_2.png", ID = 2, FoodName = "Canh Chua", Price = 20000 });
            _menuItems.Add(new MenuItem { FoodImage = "pack://application:,,,/images/menuitem_3.png", ID = 3, FoodName = "Lẩu Hải Sản", Price = 140000 });
            _menuItems.Add(new MenuItem { FoodImage = "pack://application:,,,/images/menuitem_4.jpg", ID = 4, FoodName = "Mì tôm", Price = 10000 });
            _menuItems.Add(new MenuItem { FoodImage = "pack://application:,,,/images/menuitem_5.jpg", ID = 5, FoodName = "Hàu nướng", Price = 200000 });
            _menuItems.Add(new MenuItem { FoodImage = "pack://application:,,,/images/menuitem_6.jpg", ID = 6, FoodName = "Cá Ngừ", Price = 1000000 });
            _menuItems.Add(new MenuItem { FoodImage = "pack://application:,,,/images/menuitem_6.jpg", ID = 7, FoodName = "Cá Ngừ Đại Dương", Price = 1000000 });
            _menuItems.Add(new MenuItem { FoodImage = "pack://application:,,,/images/menuitem_6.jpg", ID = 8, FoodName = "Cá Ngừ Đại Dương", Price = 1000000 });
            _menuItems.Add(new MenuItem { FoodImage = "pack://application:,,,/images/menuitem_6.jpg", ID = 9, FoodName = "Cá Ngừ Đại Dương", Price = 1000000 });
            _menuItems.Add(new MenuItem { FoodImage = "pack://application:,,,/images/menuitem_6.jpg", ID = 10, FoodName = "Cá Ngừ Đại Dương", Price = 1000000 });
            _menuItems.Add(new MenuItem { FoodImage = "pack://application:,,,/images/menuitem_6.jpg", ID = 11, FoodName = "Cá Ngừ Đại Dương", Price = 1000000 });
            _menuItems.Add(new MenuItem { FoodImage = "pack://application:,,,/images/menuitem_6.jpg", ID = 12, FoodName = "Cá Ngừ Đại Dương", Price = 1000000 });
            _menuItems.Add(new MenuItem { FoodImage = "pack://application:,,,/images/menuitem_6.jpg", ID = 13, FoodName = "Cá Ngừ Đại Dương", Price = 1000000 });

            MenuItems = _menuItems;
        }
        private void LoadCombobox_2Items()
        {
            _comboBox_2Items.Add("Giá cao -> thấp");
            _comboBox_2Items.Add("Giá thấp -> cao");
            _comboBox_2Items.Add("A -> Z");
            _comboBox_2Items.Add("Z -> A");

            ComboBox_2Items = _comboBox_2Items;
        }

        private void OrderAnItem(int ID)
        {
            foreach(MenuItem item in _menuItems)
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
        #endregion

        #region complementary methods
        private SelectedMenuItem checkIfAnItemIsInOrderItems(int ID)
        {
            foreach(SelectedMenuItem item in _selectedItems)
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
