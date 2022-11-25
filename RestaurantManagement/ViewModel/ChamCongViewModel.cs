using QuanLyNhaHang.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyNhaHang.ViewModel
{
    public class ChamCongViewModel : BaseViewModel
    {
        public ICommand CloseCM { get; set; }
        public ChamCongViewModel()
        {
            CloseCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (p == null) return;
                p.Close();
            });
        }
    }
}
