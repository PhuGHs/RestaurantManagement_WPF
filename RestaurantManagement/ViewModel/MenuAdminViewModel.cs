using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using QuanLyNhaHang.DataProvider;
using QuanLyNhaHang.Models;
using System.Windows.Data;
using System.ComponentModel;
using Diacritics.Extensions;

namespace QuanLyNhaHang.ViewModel
{
    public class MenuAdminViewModel : BaseViewModel
    {
        public MenuAdminViewModel()
        {
            _menuitems = MenuDP.Flag.ConvertToCollection();
            _menuItemsView = new CollectionViewSource();
            _menuItemsView.Source = MenuItems;
            _menuItemsView.Filter += MenuItems_Filter;
            AddItem = new Models.MenuItem();
            AddItem.FoodImage = converting("pack://application:,,,/images/menu_default_image.jpg");
            EditView = Visibility.Visible;
            AddView = Visibility.Collapsed;
            //Command executes
            AddDishes_Command = new RelayCommand<object>((p) => true, (p) =>
            {
                AddView = Visibility.Visible;
                EditView = Visibility.Collapsed;
            });
            SwitchToEditView_Command = new RelayCommand<object>((p) => true, (p) =>
            {
                AddView = Visibility.Collapsed;
                EditView = Visibility.Visible;
            });
            RemoveDish_Command = new RelayCommand<object>((p) => true, (p) =>
            {
                MyMessageBox msb = new MyMessageBox($"Bạn có muốn xoá món này?\n Món có mã là {MenuItem.ID} sẽ mất \n sau khi bạn ấn nút xoá ", true);
                msb.ShowDialog();
                if (msb.ACCEPT() == true)
                {
                    MenuDP.Flag.RemoveDish(MenuItem.ID);
                    MenuItems.Remove(MenuItem);
                    MyMessageBox msb2 = new MyMessageBox("Xoá thành công!");
                    msb2.Show();
                }
            });
            AddDish_Command = new RelayCommand<object>((p) => true, (p) =>
            {
                try
                {
                    if (AddItem.IsNullOrEmpty())
                    {
                        throw new ArgumentNullException("Hãy điền đầy đủ thông tin");
                    }
                    else
                    {
                        MenuItems.Add(AddItem);
                        MenuDP.Flag.AddDish(AddItem);
                        AddItem.Clear();
                        MyMessageBox msb = new MyMessageBox("Thêm thành công!");
                        msb.Show();
                    }
                }
                catch (Exception ex)
                {
                    MyMessageBox msb = new MyMessageBox(ex.Message);
                    msb.Show();
                }
            });
            AddImage_Command = new RelayCommand<object>((p) => true, (p) =>
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.CacheOption = BitmapCacheOption.OnLoad;
                    bmi.UriSource = new Uri(op.FileName);
                    bmi.EndInit();
                    AddItem.FoodImage = bmi;
                }
            });
            SaveChanges_Command = new RelayCommand<object>((p) => true, (p) => {
                try
                {
                    if (MenuItem.IsNullOrEmpty())
                    {
                        throw new ArgumentNullException("Hãy điền đầy đủ thông tin!");
                    }
                    else
                    {
                        MenuDP.Flag.EditDishInfo(MenuItem);
                        MyMessageBox msb = new MyMessageBox("Sửa thành công!");
                        msb.Show();
                    }
                }
                catch (Exception ex)
                {
                    MyMessageBox msb = new MyMessageBox(ex.Message);
                    msb.Show();
                }
            });
        }
        #region attributes
        private ObservableCollection<Models.MenuItem> _menuitems;
        private string _filterText;
        private CollectionViewSource _menuItemsView;
        private Models.MenuItem _menuitem;
        private Visibility editView;
        private Visibility addView;
        private Models.MenuItem addItem;
        #endregion
        #region properties
        public ObservableCollection<Models.MenuItem> MenuItems { get { return _menuitems; } set { _menuitems = value; OnPropertyChanged(); } }
        public Models.MenuItem MenuItem { get { return _menuitem; } set { _menuitem = value; OnPropertyChanged(); } }
        public Models.MenuItem AddItem { get { return addItem; } set { addItem = value; OnPropertyChanged(); } }
        public Visibility EditView { get { return editView; } set { editView = value; OnPropertyChanged(); } }
        public Visibility AddView { get { return addView; } set { addView = value; OnPropertyChanged(); } }
        public string FilterText { get { return _filterText; } set { _filterText = value; this._menuItemsView.View.Refresh(); OnPropertyChanged(); } }
        public ICollectionView MenuItemCollection
        {
            get
            {
                return this._menuItemsView.View;
            }
        }
        #endregion
        #region commands
        public ICommand AddDishes_Command { get; set; }
        public ICommand AddDish_Command { get; set; }
        public ICommand SwitchToEditView_Command { get; set; }
        public ICommand RemoveDish_Command { get; set; }
        public ICommand AddImage_Command { get; set; }
        public ICommand SaveChanges_Command { get; set; }
        public ICommand DiscardChanges_Command { get; set; }
        #endregion
        #region complementary functions
        public BitmapImage converting(string ur)
        {
            BitmapImage bmi = new BitmapImage();
            bmi.BeginInit();
            bmi.CacheOption = BitmapCacheOption.OnLoad;
            bmi.UriSource = new Uri(ur);
            bmi.EndInit();

            return bmi;
        }
        public void MenuItems_Filter(object sender, FilterEventArgs e)
        {
            if (string.IsNullOrEmpty(FilterText))
            {
                e.Accepted = true;
                return;
            }

            Models.MenuItem item = e.Item as Models.MenuItem;
            if (item.FoodName.RemoveDiacritics().ToLower().Contains(FilterText.RemoveDiacritics().ToLower()))
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
