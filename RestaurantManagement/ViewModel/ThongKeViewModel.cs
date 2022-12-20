using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using RestaurantManagement.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace QuanLyNhaHang.ViewModel
{
    public class ThongKeViewModel : BaseViewModel
    {
        string connectstring = ConfigurationManager.ConnectionStrings["QuanLyNhaHang"].ConnectionString;
        public ThongKeViewModel()
        {
            SeriesCollection = new SeriesCollection();
            Formatter = value => value.ToString("G");
            DayMonthCheckingCommand = new RelayCommand<Months>((p) => true, (p) => DayMonthCheck());
            MonthYearCheckingCommand = new RelayCommand<Years>((p) => true, (p) => MonthYearCheck());
            LoadMonth();
            LoadYear();
        }
        #region Attributes
        private SeriesCollection seriesCollection;
        private ObservableCollection<Months> months = new ObservableCollection<Months>();
        private ObservableCollection<Years> years = new ObservableCollection<Years>();
        private Months selectedMonth = new Months("-1");
        private Years selectedYear = new Years("-1");

        private string[] labels;
        private double dec_sumofprofit = 0;
        private string sumofprofit = "0 VND";
        private double dec_sumofpaid = 0;
        private string sumofpaid = "0 VND";
        private string visibility = "hidden";

        #endregion
        #region Properties
        public SeriesCollection SeriesCollection
        {
            get { return seriesCollection; }
            set { seriesCollection = value; }
        }
        public ObservableCollection<Months> Months
        {
            get { return months; }
            set
            {
                months = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Years> Years
        {
            get { return years; }
            set
            {
                years = value;
                OnPropertyChanged();
            }
        }
        public Months SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                if (value != selectedMonth)
                {
                    selectedMonth = value;
                    OnPropertyChanged();
                }
            }
        }
        public Years SelectedYear
        {
            get { return selectedYear; }
            set
            {
                if (value != selectedYear)
                {
                    selectedYear = value;
                    OnPropertyChanged();
                }
            }
        }
        public string[] Labels
        {
            get { return labels; }
            set { labels = value; OnPropertyChanged(); }
        }
        public Func<double, string> Formatter { get; set; }
        public string SumofProfit
        {
            get { return sumofprofit; }
            set { sumofprofit = value; OnPropertyChanged(); }
        }
        public double DecSumofProfit
        {
            get { return dec_sumofprofit; }
            set { dec_sumofprofit = value; OnPropertyChanged(); }
        }
        public string SumofPaid
        {
            get { return sumofpaid; }
            set { sumofpaid = value; OnPropertyChanged(); }
        }
        public double DecSumofPaid
        {
            get { return dec_sumofpaid; }
            set { dec_sumofpaid = value; OnPropertyChanged(); }
        }
        public string Visibility
        {
            get { return visibility; }
            set { visibility = value; OnPropertyChanged(); }
        }
        #endregion
        #region Commands
        public ICommand DayMonthCheckingCommand { get; set; }
        public ICommand MonthYearCheckingCommand { get; set; }

        #endregion
        #region Methods
        public void LoadMonth()
        {
            months.Add(new Months("1"));
            months.Add(new Months("2"));
            months.Add(new Months("3"));
            months.Add(new Months("4"));
            months.Add(new Months("5"));
            months.Add(new Months("6"));
            months.Add(new Months("7"));
            months.Add(new Months("8"));
            months.Add(new Months("9"));
            months.Add(new Months("10"));
            months.Add(new Months("11"));
            months.Add(new Months("12"));

            Months = months;
        }
        public void LoadYear()
        {
            years.Add(new Years("2017"));
            years.Add(new Years("2018"));
            years.Add(new Years("2019"));
            years.Add(new Years("2020"));
            years.Add(new Years("2021"));
            years.Add(new Years("2022"));
            years.Add(new Years("2023"));
            years.Add(new Years("2024"));

            Years = years;
        }
        public void ResetSum()
        {
            DecSumofProfit = 0;
            DecSumofPaid = 0;
        }
        public double GetBillofDay(string day)
        {
            double d = 0;
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "Select SUM(TRIGIA) from HOADON where NgHD = @nghd";
                cmd.Parameters.AddWithValue("@nghd", day);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        d = reader.GetSqlMoney(0).ToDouble();
                    }
                    catch
                    {
                        d = 0;
                    }
                }
                con.Close();
                return d;
            }
        }
        public double GetBillofMonth(string month, string year)
        {
            double d = 0;
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "Select SUM(TRIGIA) from HOADON where MONTH(NgHD) = @month and YEAR(NgHD) = @year";
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        d = reader.GetSqlMoney(0).ToDouble();
                    }
                    catch
                    {
                        d = 0;
                    }
                }
                con.Close();
                return d;
            }
        }
        public double GetPaidofDay(string day)
        {
            double d = 0;
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "Select SUM(TONG) from (select DonGia * SoLuong as TONG from CHITIETNHAP where NgayNhap = @ngnh) as TONGGIA";
                cmd.Parameters.AddWithValue("@ngnh", day);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        d = reader.GetSqlMoney(0).ToDouble();
                    }
                    catch
                    {
                        d = 0;
                    }
                }
                con.Close();
                return d;
            }
        }
        public double GetPaidofMonth(string month, string year)
        {
            double d = 0;
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "Select SUM(TONG) from (select DonGia * SoLuong as TONG from CHITIETNHAP where MONTH(NgayNhap) = @month and YEAR(NgayNhap) = @year) as TONGGIA";
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@year", year);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        d = reader.GetSqlMoney(0).ToDouble();
                    }
                    catch
                    {
                        d = 0;
                    }
                }
                con.Close();
                return d;
            }
        }
        public void DayMonthCheck()
        {
            SeriesCollection.Clear();
            ResetSum();
            if (int.Parse(SelectedYear.Year) == -1 || int.Parse(selectedMonth.Month) == -1) return;
            
            int NumofDay = DateTime.DaysInMonth(int.Parse(SelectedYear.Year), int.Parse(SelectedMonth.Month));
            Visibility = "Visibility";

            double[] month = new double[NumofDay];
            string[] months = new string[NumofDay];
            for (int i = 0; i < month.Length; i++)
            {
                month[i] = i + 1;
                months[i] = "Ngày " + month[i].ToString();
            }
            Labels = months;
            
            //Mang doanh thu
            double[] ProfitbyMonth = new double[NumofDay];
            for (int i = 0; i < NumofDay; i++)
            {
                //Lay ngay dang xet
                DateTime day = new DateTime(int.Parse(SelectedYear.Year), int.Parse(SelectedMonth.Month), i + 1);

                //Tinh so tien thu duoc theo ngay cua thang
                ProfitbyMonth[i] = GetBillofDay(day.ToShortDateString()) / 1000000; 
                DecSumofProfit += GetBillofDay(day.ToShortDateString());
                SumofProfit = String.Format("{0:0,0 VND}", DecSumofProfit);
            }
            SeriesCollection.Add(new LineSeries
            {
                Title = "Thu",
                Values = new ChartValues<double>(ProfitbyMonth)
            });

            //Mang chi ra
            double[] PaidbyMonth = new double[NumofDay];
            for (int i = 0; i < NumofDay; i++)
            {
                //Lay ngay dang xet
                DateTime day = new DateTime(int.Parse(SelectedYear.Year), int.Parse(SelectedMonth.Month), i + 1);

                //Tinh so tien chi ra theo ngay cua thang
                PaidbyMonth[i] = GetPaidofDay(day.ToShortDateString()) / 1000000;
                DecSumofPaid += GetPaidofDay(day.ToShortDateString());
                SumofPaid = String.Format("{0:0,0 VND}", DecSumofPaid);
            }
            SeriesCollection.Add(new LineSeries
            {
                Title = "Chi",
                Values = new ChartValues<double>(PaidbyMonth)
            });
        }
        public void MonthYearCheck()
        {
            SeriesCollection.Clear();
            ResetSum();
            if (int.Parse(SelectedYear.Year) == -1 || int.Parse(selectedMonth.Month) == -1) return;

            Visibility = "Visibility";
            Labels = new[] { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };

            //Mang doanh thu
            double[] ProfitbyYear = new double[12];
            for (int i = 0; i < 12; i++)
            {
                //Tinh so tien thu duoc theo thang cua nam 

                ProfitbyYear[i] = GetBillofMonth(i.ToString(), SelectedYear.Year) / 1000000;  
                DecSumofProfit += GetBillofMonth(i.ToString(), SelectedYear.Year);
                SumofProfit = String.Format("{0:0,0 VND}", DecSumofProfit);
            }
            SeriesCollection.Add(new LineSeries
            {
                Title = "Thu",
                Values = new ChartValues<double>(ProfitbyYear)
            });

            //Mang chi ra
            double[] PaidbyYear = new double[12];
            for (int i = 0; i < 12; i++)
            {
                //Tinh so tien chi ra theo thang cua nam

                PaidbyYear[i] = GetPaidofMonth(i.ToString(), SelectedYear.Year) / 1000000; 
                DecSumofPaid += GetPaidofMonth(i.ToString(), SelectedYear.Year);
                SumofPaid = String.Format("{0:0,0 VND}", DecSumofPaid);
            }
            SeriesCollection.Add(new LineSeries
            {
                Title = "Chi",
                Values = new ChartValues<double>(PaidbyYear)
            });
        }
        #endregion
    }
}