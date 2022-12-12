using QuanLyNhaHang.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TinhTrangBan.Models
{
    public class TinhTrangBan { }
    public class NameOfColor : BaseViewModel
    {
        private string _color;
        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }       
    }
    public class SelectedMenuItems : BaseViewModel
    {
        #region variables
        private int _id;
        private string _foodName;
        private Decimal _price;
        private int _quantity;
        #endregion
        public SelectedMenuItems(int ID, string foodName, Decimal price, int quantity)
        {
            _id = ID;
            _foodName = foodName;
            _price = price;
            _quantity = quantity;
        }
        #region properties
        public int ID { get { return _id; } set { _id = value; } }

        public string FoodName
        {
            get { return _foodName; }
            set
            {
                if (_foodName != value)
                {
                    _foodName = value;
                    OnPropertyChanged("food name");
                }
            }
        }

        public Decimal Price
        {
            get { return _price; }
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged("price");
                }
            }
        }

        public string PriceVNDCurrency
        {
            get { return String.Format("{0:0,0 VND}", Price); }
        }
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged("quantity");
                }
            }
        }
        #endregion
    }
}
