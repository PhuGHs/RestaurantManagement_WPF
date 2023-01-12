using Project;
using QuanLyNhaHang.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RestaurantManagement.ViewModel
{
    public class LoginWindowVM : BaseViewModel
    {
        private string strCon = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyNhaHang;Integrated Security=True";
        private SqlConnection sqlCon = null;
        public bool IsLoggedIn { get; set; }
        public string Role { get; set; }
        public ICommand CloseLoginCM { get; set; }
        public ICommand LoginCM { get; set; }
        public LoginWindowVM()
        {
            IsLoggedIn = false;
            CloseLoginCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (p == null) return;
                p.Close();
            });
            LoginCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Login(p);
                if (IsLoggedIn)
                {
                    p.Hide();
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    p.Close();
                    return;
                }
            });
            void Login(Window p)
            {
                if (p == null) return;
                IsLoggedIn = true;
            }
            void OpenConnect()
            {
                sqlCon = new SqlConnection(strCon);
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
            }

            void CloseConnect()
            {
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
            }
        }
    }
}
