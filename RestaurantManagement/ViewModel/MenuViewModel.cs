using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Menu.Models;

namespace QuanLyNhaHang.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        public MenuViewModel()
        {
            OrderFeature = new RelayCommand<MenuItem>((p) => true, (p) => OrderAnItem(p.ID));
            RemoveItemFeature = new RelayCommand<SelectedMenuItem>((p) => true, (p) => RemoveAnItem(p));
            _menuItems = new ObservableCollection<MenuItem>();
            _selectedItems = new ObservableCollection<SelectedMenuItem>();
            LoadMenuItems();
        }

        #region attributes
        private ObservableCollection<MenuItem> _menuItems;
        private ObservableCollection<SelectedMenuItem> _selectedItems;
        private decimal dec_subtotal = 0;
        private string str_subtotal = "0 VND";
        #endregion

        #region properties
        public ObservableCollection<MenuItem> MenuItems { get { return _menuItems; } set { _menuItems = value; } }
        public ObservableCollection<SelectedMenuItem> SelectedItems { get { return _selectedItems; } set { _selectedItems = value; } }
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
        public ICommand OrderFeature { get; set; }  
        public ICommand RemoveItemFeature { get; set; }
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
