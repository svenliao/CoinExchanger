using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using Unity;
using FluentScheduler;
using Domain.Common.Ioc;
using Domain.Common.DataBaseModels;
using Domain.Kraken;
using Domain.Exchange.Rate;
using API.Exchange;
using API.Kraken;
using Kraken.EUR2USD;

namespace KrakenClient
{
    public partial class MainForm : Form
    {
        private AppHost appHost;
        public string SelectedCoin { get; private set; }

        private double USDRate { get; set; }
        private double EURRate { get; set; }

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

            ExchangeRateJob.ExcuteEvent += RefreshExchangeRate;
            NotifyIconJob.ExcuteEvent += RefreshNotifyIcon;
            BalanceJob.ExcuteEvent += RefreshBalance;
            TickerJob.ExcuteEvent += RefreshTicker;
            BookingJob.ExcuteEvent += RefreshBooking;

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
                    lvi.SubItems.Add((eurTicker.Ask*EURRate).ToString("F2"));
                    lvi.SubItems.Add((usdTicker.Bid*USDRate).ToString("F2"));

                    double profit = (usdTicker.Bid * USDRate - eurTicker.Ask * EURRate) / (eurTicker.Ask * EURRate);
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

        public void RefreshBooking(object obj)
        {
            var tables = obj as List<BookingTable>;

            var usdBooking = tables.Find(b => b.Coin == SelectedCoin && b.Currency == "USD");
            var eurBooking = tables.Find(b => b.Coin == SelectedCoin && b.Currency == "EUR");

            var asks = JsonConvert.Import(eurBooking.Asks) as JsonArray;
            this.listView3.BeginInvoke(new MethodInvoker(delegate ()
            {
                double sum = 0;

                this.listView3.BeginUpdate();
                this.listView3.Items.Clear();
                foreach (JsonArray a in asks)
                {
                    double price = double.Parse(a[0].ToString());

                    double volume = double.Parse(a[1].ToString());

                    sum = sum + (price * volume*EURRate);

                    ListViewItem lvi = new ListViewItem();

                    lvi.SubItems.Add(sum.ToString("F0"));
                    lvi.SubItems.Add(volume.ToString("F2"));
                    lvi.SubItems.Add((price* EURRate).ToString("F5"));

                    this.listView3.Items.Add(lvi);
                }
                this.listView3.EndUpdate();
            }));

            var bids = JsonConvert.Import(usdBooking.Bids) as JsonArray;
            this.listView4.BeginInvoke(new MethodInvoker(delegate ()
            {
                double sum = 0;

                this.listView4.BeginUpdate();
                this.listView4.Items.Clear();
                foreach (JsonArray b in bids)
                {
                    double price = double.Parse(b[0].ToString());

                    double volume = double.Parse(b[1].ToString());

                    sum = sum + (price * volume*USDRate);

                    ListViewItem lvi = new ListViewItem();
                    lvi.SubItems.Add((price*USDRate).ToString("F5"));
                    lvi.SubItems.Add(volume.ToString("F2"));
                    lvi.SubItems.Add(sum.ToString("F0"));
                                      
                    this.listView4.Items.Add(lvi);
                }
                this.listView4.EndUpdate();
            }));
        }

        private void RefreshExchangeRate(object obj)
        {
            List<ExchangeRateTable> rates=obj as List<ExchangeRateTable>;

            USDRate = rates.Find(r => r.Source == "USD").Quotes;
            EURRate = rates.Find(r => r.Source == "EUR").Quotes;
        }
        #endregion

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            ListView view = sender as ListView;
            if (view.SelectedItems.Count > 0)
            {
                ListViewItem item = view.SelectedItems[0];
                if (item != null)
                {
                    this.SelectedCoin = item.SubItems[1].Text;

                    var doa = new DAL.DataBase.Dao.BookingTableDao();
                    var data = doa.Select();
                    RefreshBooking(data);
                }
            }
        }
    }
}
