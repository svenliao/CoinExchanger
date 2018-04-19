using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Applications.Server;
using Unity;
using FluentScheduler;
using Domain.Common.Ioc;
using Domain.Common.DataBaseModels;
using Domain.Kraken;
using Domain.Exchange.Rate;
using API.Exchange;
using API.Kraken;

namespace KrakenClient
{
    public partial class MainForm : Form
    {
        private AppHost appHost;
        public string SelectedCoin { get; private set; }

        private double usdRate { get; set; }
        private double eurRate { get; set; }

        public MainForm()
        {
            InitializeComponent();
            ServiceInit();
        }

        #region Service
        private void ServiceInit()
        {           
            IocFactory.Default.RegisterType<IKrakenRepertory, KrakenRepertory>();
            IocFactory.Default.RegisterType<IExchangeRepertory, ExchangeRepertory>();

            SelectedCoin = "BTC";
            appHost = new AppHost();
            appHost.Start();


            NotifyIconJob.ExcuteEvent += RefreshNotifyIcon;
            BalanceJob.ExcuteEvent += RefreshBalance;
            TickerJob.ExcuteEvent += RefreshTicker;
            ExchangeRateJob.ExcuteEvent += RefreshExchangeRate;
            JobManager.Initialize(new JobsRegistry());
        }
     
        #endregion

        #region Refresh
        public void RefreshNotifyIcon(object obj)
        {
            int index = int.Parse(this.notifyIcon1.Tag.ToString()) % 4;
            this.notifyIcon1.Tag = index + 1;
            switch (index)
            {
                case 0:
                    this.notifyIcon1.Icon = global::KrakenClient.Properties.Resources.bitbug_favicon__3_;
                    break;
                case 1:
                    this.notifyIcon1.Icon = global::KrakenClient.Properties.Resources.bitbug_favicon__5_;
                    break;
                case 2:
                    this.notifyIcon1.Icon = global::KrakenClient.Properties.Resources.bitbug_favicon__4_;
                    break;
                case 3:
                    this.notifyIcon1.Icon = global::KrakenClient.Properties.Resources.bitbug_favicon__6_;
                    break;
            }
        }

        public void RefreshTicker(object obj)
        {
            List<TickerTable> tables = obj as List<TickerTable>;
            var coinList = tables.Select(t => t.Coin).Distinct();

            this.listView1.BeginInvoke(new MethodInvoker(delegate ()
            {
                this.listView1.BeginUpdate();
                this.listView1.Items.Clear();
                foreach (var coin in coinList)
                {
                    var usdTicker = tables.Find(t => t.Coin == coin && t.Currency == "USD");
                    var eurTicker = tables.Find(t => t.Coin == coin && t.Currency == "EUR");

                    ListViewItem lvi = new ListViewItem();

                    lvi.SubItems.Add(coin);
                    lvi.SubItems.Add((eurTicker.Ask*eurRate).ToString("F2"));
                    lvi.SubItems.Add((usdTicker.Bid*usdRate).ToString("F2"));

                    double profit = (usdTicker.Bid * usdRate - eurTicker.Ask * eurRate) / (eurTicker.Ask * eurRate);
                    lvi.SubItems.Add((profit * 100).ToString("F4"));
                    lvi.SubItems.Add("-0.35");
                    lvi.SubItems.Add((profit * 100 - 0.35).ToString("F4"));

                    this.listView1.Items.Add(lvi);
                }
                this.listView1.EndUpdate();
            }));
        }

        public void RefreshBalance(object obj)
        {
            List<BalanceTable> tables = obj as List<BalanceTable>;

            foreach (var table in tables)
            {
                switch (table.Coin)
                {
                    case "USD":
                        this.tslUSD.Text = table.Amount.ToString("f2");
                        break;
                    case "EUR":
                        this.tslEUR.Text = table.Amount.ToString("f2");
                        break;
                    case "BTC":
                        this.tslBTC.Text = table.Amount.ToString("f4");
                        break;
                    case "ETH":
                        this.tslETH.Text = table.Amount.ToString("f4");
                        break;
                    case "XRP":
                        this.tslXRP.Text = table.Amount.ToString("f4");
                        break;
                    case "BCH":
                        this.tslBCH.Text = table.Amount.ToString("f4");
                        break;
                    case "ETC":
                        this.tslETC.Text = table.Amount.ToString("f4");
                        break;
                    case "EOS":
                        this.tslEOS.Text = table.Amount.ToString("f4");
                        break;
                    case "LTC":
                        this.tslLTC.Text = table.Amount.ToString("f4");
                        break;
                    case "XLM":
                        this.tslXLM.Text = table.Amount.ToString("f4");
                        break;
                    case "XMR":
                        this.tslXMR.Text = table.Amount.ToString("f4");
                        break;
                }
            }
        }

        public void RefreshBooking(List<BookingTable> tableList)
        {

        }

        private void RefreshExchangeRate(object obj)
        {
            List<ExchangeRateTable> rates=obj as List<ExchangeRateTable>;

            usdRate = rates.Find(r => r.Source == "USD").Quotes;
            eurRate = rates.Find(r => r.Source == "EUR").Quotes;
        }
        #endregion

    }
}
