using QuanLyNhaHang.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace QuanLyNhaHang.Models
{
    public class MenuModel { }

    public class MenuItem : BaseViewModel
    {
        public MenuItem(string id = "", string foodName = "", decimal price = 0, BitmapImage foodImage = null, int cookingTime = 0)
        {
            this.id = id;
            this.foodName = foodName;
            this.price = price;
            this.foodImage = foodImage;
            this.cookingTime = cookingTime;
        }
        private string id;
        private string foodName;
        private decimal price;
        private BitmapImage foodImage;
        private int cookingTime;
        public string ID { get { return id; } set { id = value; } }

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

        public decimal Price
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

        public BitmapImage FoodImage
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
        public int CookingTime
        {
            get
            {
                return cookingTime;
            }
            set
            {
                cookingTime = value;
                OnPropertyChanged();
            }
        }
        public bool IsNullOrEmpty()
        {
            if (foodImage == null || id == "" || foodName == "" || cookingTime == 0 || price == 0)
            {
                return true;
            }
            return false;
        }

        public void Clear()
        {
            this.FoodName = "";
            this.Price = 0;
            this.CookingTime = 0;
            this.ID = "";
        }


    }
    public class SelectedMenuItem : BaseViewModel
    {
        public SelectedMenuItem(string ID, string foodName, Decimal price, int quantity)
        {
            _id = ID;
            _foodName = foodName;
            _price = price;
            _quantity = quantity;
        }
        #region attributes
        private string _id;
        private string _foodName;
        private Decimal _price;
        private int _quantity;
        #endregion
        #region properties
        public string ID { get { return _id; } set { _id = value; } }

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
