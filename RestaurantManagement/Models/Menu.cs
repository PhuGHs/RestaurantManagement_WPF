using QuanLyNhaHang.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Models
{
    public class MenuModel { }

    public class MenuItem : BaseViewModel
    {
        private int id;
        private string foodName;
        private Decimal price;
        private string foodImage;
        public int ID { get { return id; } set { id = value; } }

        public string FoodName
        {
            get { return foodName; }
            set
            {
                if (foodName != value)
                {
                    foodName = value;
                    OnPropertyChanged("food name");
                }
            }
        }

        public Decimal Price
        {
            get { return price; }
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged("price");
                }
            }
        }

        public string FoodImage
        {
            get { return foodImage; }
            set
            {
                if (foodImage != value)
                {
                    foodImage = value;
                    OnPropertyChanged("food image");
                }
            }
        }

        public string PriceVNDCurrency
        {
            get { return String.Format("{0:0,0 VND}", Price); }
        }


    }
    public class SelectedMenuItem : BaseViewModel
    {
        public SelectedMenuItem(int ID, string foodName, Decimal price, int quantity)
        {
            _id = ID;
            _foodName = foodName;
            _price = price;
            _quantity = quantity;
        }
        #region attributes
        private int _id;
        private string _foodName;
        private Decimal _price;
        private int _quantity;
        #endregion
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
