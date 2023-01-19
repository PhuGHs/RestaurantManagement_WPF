using QuanLyNhaHang.DataProvider;
using QuanLyNhaHang.Models;
using QuanLyNhaHang.State.Navigator;
using RestaurantManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using RestaurantManagement.View;
using System.Windows.Media.Imaging;
using Project;

namespace QuanLyNhaHang.ViewModel
{

    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            
            LoadWindowCommand = new RelayCommand<Window>((p) => true, (p) =>
            {
                if(p == null)
                {
                    return;
                }
                p.Hide();
                LoginWindow window = new LoginWindow();
                window.ShowDialog();
                var loginVM = window.DataContext as LoginWindowVM;
                if(loginVM == null)
                {
                    return;
                }
                if(loginVM.IsLoggedIn)
                {
                    Navigator = new Navigator(loginVM.Role);
                    CaiDatViewModel = new CaiDatViewModel(loginVM.MaNV, loginVM.UserName, loginVM.Password, loginVM.Role);
                    p.Show();
                }
                else
                {
                    p.Close();
                }
            });

            LogOutCommand = new RelayCommand<Window>((p) => true, (p) =>
            {
                if (p == null)
                {
                    return;
                }
                System.Windows.Forms.Application.Restart();
                p.Close();
            });
            HeaderViewModel = new HeaderViewModel();
        }
        CaiDatViewModel caiDatViewModel;
        HeaderViewModel headerViewModel;
        Navigator navigator;
        public ICommand LoadWindowCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public CaiDatViewModel CaiDatViewModel
        {
            get { return caiDatViewModel; }
            set { caiDatViewModel = value; OnPropertyChanged(); }
        }

        public HeaderViewModel HeaderViewModel
        {
            get { return headerViewModel; }
            set { headerViewModel = value; OnPropertyChanged(); }
        }

        public Navigator Navigator
        {
            get { return navigator; }
            set { navigator = value; OnPropertyChanged(); }
        }
    }
}
