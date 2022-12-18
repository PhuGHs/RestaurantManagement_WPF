using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyNhaHang.Models;
using System.Windows.Input;
using QuanLyNhaHang.DataProvider;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace QuanLyNhaHang.ViewModel
{
    public class CaiDatViewModel : BaseViewModel
    {
        public CaiDatViewModel()
        {
            NhanVien = CaiDatDP.Flag.GetCurrentEmployee();
            CaiDatDP.Flag.LoadProfileImage(NhanVien);
            UpdateInfoCommand = new RelayCommand<object>((p) => true, (p) =>
            {
                CaiDatDP.Flag.UpdateInfo(NhanVien.HoTen, NhanVien.DiaChi, NhanVien.SDT, NhanVien.NgaySinh, NhanVien.MaNV);
            });
            ChangeProfileImage = new RelayCommand<object>((p) => true, (p) =>
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Title = "Thay ảnh đại diện";
                open.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";

                if (open.ShowDialog() == DialogResult.OK)
                {
                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.CacheOption = BitmapCacheOption.OnLoad;
                    bmi.UriSource = new Uri(open.FileName);
                    bmi.EndInit();
                    NhanVien.AnhDaiDien = bmi;
                }
                CaiDatDP.Flag.ChangeProfileImage_SaveToDB(NhanVien);
            });
            CurrentPasswordChangedCommand = new RelayCommand<PasswordBox>((p) => true, (p) =>
            {
                CurrentPassword = p.Password;
            });
            NewPasswordChangedCommand = new RelayCommand<PasswordBox>((p) => true, (p) =>
            {
                NewPassword = p.Password;   
            });
            ConfirmPasswordChangedCommand = new RelayCommand<PasswordBox>((p) => true, (p) =>
            {
                ConfirmPassword = p.Password;   
            });
            ChangePassword = new RelayCommand<object>((p) => true, (p) =>
            {
                if(OverallPasswordValidation())
                {
                    CaiDatDP.Flag.ChangePassword(NewPassword);
                    ConfirmPassword = "";
                    NewPassword = "";
                    CurrentPassword = "";
                }
                else
                {
                    MyMessageBox msb = new MyMessageBox("Mật khẩu sai!");
                    msb.Show();
                }
            });
        }
        #region attributes
        private NhanVien _nhanVien;
        private string _currentPassword;
        private string _newPassword;
        private string _confirmPassword;
        #endregion
        #region variables
        public NhanVien NhanVien { get { return _nhanVien; } set { _nhanVien = value; OnPropertyChanged(); } }
        public string LoaiNhanVien_Str
        {
            get
            {
                if (_nhanVien.Fulltime)
                {
                    return "Full-time";
                }
                return "Part-time";
            }
            set
            {
                if (value == "Full-time")
                {
                    _nhanVien.Fulltime = true;
                }
                else
                {
                    _nhanVien.Fulltime = false;
                }
            }
        }
        public string CurrentPassword
        {
            get => _currentPassword;
            set
            {
                _currentPassword = value; OnPropertyChanged();
            }
        }
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value; OnPropertyChanged();
            }
        }
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value; OnPropertyChanged();
            }
        }
        #endregion
        #region commands
        public ICommand UpdateInfoCommand { get; set; } 
        public ICommand ChangeProfileImage { get; set; }
        public ICommand ChangePassword { get; set; }
        public ICommand CurrentPasswordChangedCommand { get; set; }
        public ICommand NewPasswordChangedCommand { get; set; }
        public ICommand ConfirmPasswordChangedCommand { get; set; }
        #endregion

        #region complementary functions
        public bool CheckIfCurrentPasswordIsMatch()
        {
            return CurrentPassword == NhanVien.MatKhau;
        }
        public bool CheckConfirmPassword()
        {
            return ConfirmPassword == NewPassword;
        }
        public bool OverallPasswordValidation()
        {
            return CheckIfCurrentPasswordIsMatch() && CheckConfirmPassword();
        }
        #endregion
    }
}
