using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using TestAnimatedGraph.Model;

namespace TestAnimatedGraph.View
{
    /// <summary>
    /// Логика взаимодействия для BarChartView.xaml
    /// </summary>
    public partial class BarChartView : UserControl
    {
        private readonly DispatcherTimer timer, BrokerTimer;
        private readonly Balance Balance = new Balance();
        private readonly Errors Errors = new Errors();
        private readonly SQLFunctions SQLFunctions = new SQLFunctions();
        public SeriesCollection SeriesCollection { get; set; }
        public ColumnSeries BuyColumnSeries { get; set; }
        public ColumnSeries SellingColumnSeries { get; set; }

        readonly Broker VirtualBroker = new Broker();

        private readonly List<Shares> SharesList = new List<Shares>();
        public BarChartView()
        {
            InitializeComponent();
            SQLFunctions.Init();
            SQLFunctions.Link();
            Balance.BalanceValue = 1000;
            SeriesCollection = new SeriesCollection();
            BuyColumnSeries = new ColumnSeries
            {
                Title = "Покупка"
            };
            SellingColumnSeries = new ColumnSeries
            {
                Title = "Продажа"
            };
            SeriesCollection.Add(BuyColumnSeries);
            SeriesCollection.Add(SellingColumnSeries);
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(8)
            };
            BrokerTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(8)
            };
            timer.Tick += new EventHandler(Time_tick);
            BrokerTimer.Tick += new EventHandler(Broker_Time_tick);
            FillingShareValues(SharesList);
            Update();
            timer.Start();
            BrokerTimer.Start();
            
        }
        private void Broker_Time_tick(object sender, EventArgs e)
        {
            Random random = new Random();
            int result = random.Next(0, 2);
            switch (result)
            {
                case 0:
                    VirtualBroker.CountOfSharesForOperation = random.Next(1, 3);
                    SQLFunctions.InsertOperationValue("Брокер", "Покупка");
                    VirtualBroker.Buy(Convert.ToInt32(BuyValueTextBlock.Text), VirtualBroker.CountOfSharesForOperation);
                    break;
                case 1:
                   if(VirtualBroker.BrokerBalance.CountOfShares > 0)
                    {
                        VirtualBroker.CountOfSharesForOperation = random.Next(1, VirtualBroker.BrokerBalance.CountOfShares);
                        SQLFunctions.InsertOperationValue("Брокер", "Продажа");
                        VirtualBroker.Sale(Convert.ToInt32(SellingValueTextBlock.Text), VirtualBroker.CountOfSharesForOperation);
                        break;
                    }
                    break;
            }
            UpdateParameters();
        }

        private void Time_tick(object sender, EventArgs e)
        {
            FillingShareValues(SharesList);
            Update();
        }
        private void FillingShareValues(List<Shares> shares)
        {
            Random random = new Random();
            double buyValue = Convert.ToDouble(random.Next(0, 100));
            double sellingValue = Convert.ToDouble(random.Next(Convert.ToInt32(buyValue), 100));
            SQLFunctions.InsertSaleAndCostValues(sellingValue, buyValue);
            shares.Add(new Shares()
            {
                SellingValue = sellingValue,
                BuyValue = buyValue
            });
        }
        private void Update()
        {
            ChartValues<double> buyValues = new ChartValues<double>();
            ChartValues<double> saleValues = new ChartValues<double>();
            Shares lastShare = null;
            for (int i = 0; i < SharesList.Count; i++)
            {
                buyValues.Add(SharesList[i].BuyValue);
                saleValues.Add(SharesList[i].SellingValue);
                if(i == SharesList.Count - 1)
                {
                    lastShare = SharesList[i];
                }
            }
            BuyColumnSeries.Values = buyValues;
            SellingColumnSeries.Values = saleValues;
            BuyValueTextBlock.Text = lastShare.BuyValue.ToString();
            SellingValueTextBlock.Text = lastShare.SellingValue.ToString();
            UpdateParameters();
            DataContext = this;

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(SharesList.Count > 0)
            {
                Buy();
                UpdateParameters();
            }

        }
        private void Buy()
        {
            try
            {
                if (Balance.BalanceValue - (Convert.ToInt32(BuyValueTextBlock.Text) * Convert.ToInt32(SharesCountTextBox.Text)) > 0)
                {
                    SQLFunctions.InsertOperationValue("Пользователь", "Покупка");
                    Balance.CountOfShares += Convert.ToInt32(SharesCountTextBox.Text);
                    Balance.BalanceValue -= Convert.ToInt32(BuyValueTextBlock.Text) * Convert.ToInt32(SharesCountTextBox.Text);
                }
                else
                {
                    Errors.ShowMessage("Недостаточно средств", MessageBoxImage.Error);
                }
            }
            catch
            {
                Errors.ShowMessage("Введите количество акций", MessageBoxImage.Information);
            }


        }
        private void Sale()
        {
            try
            {
                if (Balance.CountOfShares - Convert.ToInt32(SharesCountTextBox.Text) >= 0)
                {
                    SQLFunctions.InsertOperationValue("Пользователь", "Продажа");
                    Balance.CountOfShares -= Convert.ToInt32(SharesCountTextBox.Text);
                    Balance.BalanceValue += Convert.ToInt32(SellingValueTextBlock.Text) * Convert.ToInt32(SharesCountTextBox.Text);
                }
                else
                {
                    Errors.ShowMessage("Недостаточно акций", MessageBoxImage.Error);
                }
            }
            catch
            {
                Errors.ShowMessage("Введите количество акций", MessageBoxImage.Information);

            }


        }
        private void UpdateParameters()
        {
            BalanceValueTextBlock.DataContext = new Balance()
            {
                BalanceValue = Balance.BalanceValue
            };
            BrokerBalanceValueTextBlock.DataContext = new Balance()
            {
                BalanceValue = VirtualBroker.BrokerBalance.BalanceValue
            };
            CountOfSharesTextBlock.DataContext = new Balance()
            {
                CountOfShares = Balance.CountOfShares
            };
            BrokerCountOfSharesTextBlock.DataContext = new Balance()
            {
                CountOfShares = VirtualBroker.BrokerBalance.CountOfShares
            };
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            if (SharesList.Count > 0)
            {
                Sale();
                UpdateParameters();
            }

        }
    }
}
