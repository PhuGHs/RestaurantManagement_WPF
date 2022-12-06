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

namespace QuanLyNhaHang.ViewModel
{
    public class ThongKeViewModel : BaseViewModel
    {
        public ThongKeViewModel()
        {
            SeriesCollection = new SeriesCollection();
            Formatter = value => value.ToString("G");
            DayMonthCheckingCommand = new RelayCommand<Months>((p) => true, (p) => DayMonthCheck());
            MonthYearCheckingCommand = new RelayCommand<Years>((p) => true, (p) => MonthYearCheck());
            LoadMonth();
            LoadYear();
        }
        #region Variables and Properties
        private ObservableCollection<Months> months = new ObservableCollection<Months>();
        public ObservableCollection<Months> Months
        {
            get { return months; }
            set
            {
                months = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<Years> years = new ObservableCollection<Years>();
        public ObservableCollection<Years> Years
        {
            get { return years; }
            set
            {
                years = value;
                OnPropertyChanged();
            }
        }


        private SeriesCollection seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return seriesCollection; }
            set { seriesCollection = value; }
        }
        private string[] labels;
        public string[] Labels
        {
            get { return labels; }
            set { labels = value; OnPropertyChanged(); }
        }
        public Func<double, string> Formatter { get; set; }
        private double dec_sumofprofit = 0;
        private string sumofprofit = "0 VND";
        private double dec_sumofpaid = 0;
        private string sumofpaid = "0 VND";
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

        private Months selectedMonth = new Months("-1");
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
        private Years selectedYear = new Years("-1");
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
        #endregion
        #region Commands
        public ICommand DayMonthCheckingCommand { get; set; }
        public ICommand MonthYearCheckingCommand { get; set; }

        #endregion
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
        public void DayMonthCheck()
        {
            SeriesCollection.Clear();
            ResetSum();
            if (int.Parse(SelectedYear.Year) == -1 || int.Parse(selectedMonth.Month) == -1) return;
            int NumofDay = DateTime.DaysInMonth(int.Parse(SelectedYear.Year), int.Parse(SelectedMonth.Month));
            double[] month = new double[NumofDay];
            string[] months = new string[NumofDay];
            for (int i = 0; i < month.Length; i++)
            {
                month[i] = i + 1;
                months[i] = "Ngày " + month[i].ToString();
            }
            Labels = months;
            Random rd = new Random(); //Khuc nay demo chart, sau xoa
            //Mang doanh thu
            double[] ProfitbyMonth = new double[NumofDay];
            for (int i = 0; i < NumofDay; i++)
            {
                //Lay ngay dang xet
                DateTime day = new DateTime(int.Parse(SelectedYear.Year), int.Parse(SelectedMonth.Month), i + 1);

                //Lay tat ca hoa don trong ngay


                //Tinh so tien thu duoc

                ProfitbyMonth[i] = rd.Next(50 - (2 - 1)) + 2; //Dang mac dinh, fix sau
                DecSumofProfit += ProfitbyMonth[i] * 1000000;
                SumofProfit = String.Format("{0:0,0 VND}", DecSumofProfit);
            }
            SeriesCollection.Add(new LineSeries
            {
                Title = "Thu",
                Values = new ChartValues<double>(ProfitbyMonth)
            });
            double[] PaidbyMonth = new double[NumofDay];
            for (int i = 0; i < NumofDay; i++)
            {
                //Lay ngay dang xet
                DateTime day = new DateTime(int.Parse(SelectedYear.Year), int.Parse(SelectedMonth.Month), i + 1);

                //Lay tat ca hoa don trong ngay


                //Tinh so tien thu duoc

                PaidbyMonth[i] = rd.Next(50 - (2 - 1)) + 2; //Dang mac dinh, fix sau
                DecSumofPaid += PaidbyMonth[i] * 1000000;
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
            Labels = new[] { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };
            Random rd = new Random(); //Khuc nay demo chart, sau xoa
            //Mang doanh thu
            double[] ProfitbyYear = new double[12];
            for (int i = 0; i < 12; i++)
            {
                //Lay ngay dau va cuoi cua thang
                DateTime dayBegin = new DateTime(int.Parse(SelectedYear.Year), i + 1, 1);
                int numOfDay = DateTime.DaysInMonth(int.Parse(SelectedYear.Year), i + 1);
                DateTime dayEnd = new DateTime(int.Parse(SelectedYear.Year), i + 1, numOfDay);

                //Lay tat ca hoa don trong thang


                //Tinh so tien thu duoc 
                ProfitbyYear[i] = rd.Next(500 - (50 - 1)) + 50;  //De mac dinh, fix sau
                DecSumofProfit += ProfitbyYear[i] * 1000000;
                SumofProfit = String.Format("{0:0,0 VND}", DecSumofProfit);
            }
            SeriesCollection.Add(new LineSeries
            {
                Title = "Thu",
                Values = new ChartValues<double>(ProfitbyYear)
            });
            double[] PaidbyYear = new double[12];
            for (int i = 0; i < 12; i++)
            {
                //Lay ngay dau va cuoi cua thang
                DateTime dayBegin = new DateTime(int.Parse(SelectedYear.Year), i + 1, 1);
                int numOfDay = DateTime.DaysInMonth(int.Parse(SelectedYear.Year), i + 1);
                DateTime dayEnd = new DateTime(int.Parse(SelectedYear.Year), i + 1, numOfDay);

                //Lay tat ca hoa don trong thang


                //Tinh so tien thu duoc 
                PaidbyYear[i] = rd.Next(500 - (50 - 1)) + 50;  //De mac dinh, fix sau
                DecSumofPaid += PaidbyYear[i] * 1000000;
                SumofPaid = String.Format("{0:0,0 VND}", DecSumofPaid);
            }
            SeriesCollection.Add(new LineSeries
            {
                Title = "Chi",
                Values = new ChartValues<double>(PaidbyYear)
            });
        }
    }
}