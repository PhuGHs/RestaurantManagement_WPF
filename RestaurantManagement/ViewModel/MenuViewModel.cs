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
            _menuItems = new ObservableCollection<MenuItem>();
            _selectedItems = new ObservableCollection<SelectedMenuItem>();
            LoadMenuItems();
        }

        #region attributes
        private ObservableCollection<MenuItem> _menuItems;
        private ObservableCollection<SelectedMenuItem> _selectedItems;
        #endregion

        #region properties
        public ObservableCollection<MenuItem> MenuItems { get { return _menuItems; } set { _menuItems = value; } }
        public ObservableCollection<SelectedMenuItem> SelectedItems { get { return _selectedItems; } set { _selectedItems = value; } }
        #endregion

        #region commands
        public ICommand OrderFeature { get; set; }  
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
