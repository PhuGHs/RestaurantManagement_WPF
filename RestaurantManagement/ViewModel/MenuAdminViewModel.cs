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

namespace QuanLyNhaHang.ViewModel
{
    public class MenuAdminViewModel : BaseViewModel
    {
        public MenuAdminViewModel()
        {
            _menuitems = MenuDP.Flag.ConvertToCollection();
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
                EditView= Visibility.Visible;
            });
            RemoveDish_Command = new RelayCommand<System.Windows.Controls.ListView>((p) => true, (p) =>
            {
                if(p.SelectedItems.Count > 0)
                {
                    p.Items.Remove(p.SelectedItems[0]);
                }
            });
            AddDish_Command = new RelayCommand<object>((p) => true, (p) =>
            {
                try
                {
                    if (AddItem == null)
                    {
                        throw new ArgumentNullException("Hãy điền đầy đủ các thông tin!");
                    }
                    else
                    {
                        MenuDP.Flag.AddDish(AddItem);
                    }
                }catch (Exception ex)
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
        }
        #region attributes
        private ObservableCollection<Models.MenuItem> _menuitems;
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
        #endregion
        #region commands
        public ICommand AddDishes_Command { get; set; }
        public ICommand AddDish_Command { get; set; }   
        public ICommand SwitchToEditView_Command { get; set; }
        public ICommand RemoveDish_Command { get; set; }
        public ICommand AddImage_Command { get; set; }
        #endregion
    }
}
