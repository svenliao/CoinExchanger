using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.DataBase.Dao;
using Domain.Common.Ioc;
using Exmo.Kraken.RUB2EUR;
using Domain.Exmo;
using Domain.Kraken;
using Domain.Common.DataBaseModels;
using API.Kraken;
using API.Exmo;
using Unity;
using FluentScheduler;

namespace ExmoClient
{
    public partial class MainForm : Form
    {
        private AppHost appHost;
        public MainForm()
        {
            InitializeComponent();
            ServiceInit();
        }

        #region Service
        private void ServiceInit()
        {
            IocFactory.Default.RegisterType<IKrakenRepertory, KrakenRepertory>();
            IocFactory.Default.RegisterType<IExmoRepertory, ExmoRepertory>();
            appHost = new AppHost();
            appHost.Init();

            BalanceJob.ExcuteEvent += RefreshBalance;
            TickerJob.ExcuteEvent += RefreshTicker;

            JobManager.Initialize(new JobsRegistry());
        }
        #endregion

        #region 【界面刷新功能】
        private void RefreshTicker(object obj)
        {
            List<TickerTable> tables = obj as List<TickerTable>;
            var coinList = tables.Where(t=>t.Platform.Name=="Kraken").Select(t => t.Coin).Distinct();

            this.dgvTicker.BeginInvoke(new MethodInvoker(delegate ()
            {
                double rate = double.Parse(this.txbRate.Text);
                this.dgvTicker.Rows.Clear();
                foreach (var coin in coinList)
                {
                    var rubTicker = tables.Find(t => t.Coin == coin && t.Currency == "RUB"&& t.Platform.Name == "Exmo");
                    var eurTicker = tables.Find(t => t.Coin == coin && t.Currency == "EUR"&& t.Platform.Name == "Kraken");
                    if (rubTicker != null && eurTicker != null)
                    {
                        string ask = (rubTicker.Ask / rate).ToString("f2");
                        string bid = (eurTicker.Bid).ToString("f2");
                        double save = (eurTicker.Bid-rubTicker.Ask / rate) * 100 / (rubTicker.Ask / rate);

                        this.dgvTicker.Rows.Add(coin, ask, bid, 5.35, (save - 5.35).ToString("f2"), "卖出/买入");
                    }                
                }
            }));
        }

        private void RefreshBalance(object obj)
        {
            List<BalanceTable> tables = obj as List<BalanceTable>;

            foreach (var table in tables.Where(t=>t.Platform.Name=="Kraken"))
            {
                switch (table.Coin)
                {                  
                    case "EUR":
                        this.tlsEur.Text = table.Amount.ToString("f2");
                        break;
                    case "BTC":
                        this.tlsKBTC.Text = table.Amount.ToString("f4");
                        break;
                    case "XRP":
                        this.tlsKXRP.Text = table.Amount.ToString("f4");
                        break;
                    case "ETH":
                        this.tlsKETH.Text = table.Amount.ToString("f4");
                        break;                   
                }
            }

            foreach (var table in tables.Where(t => t.Platform.Name == "Exmo"))
            {
                switch (table.Coin)
                {
                    case "RUB":
                        this.tlsRUB.Text = table.Amount.ToString("f2");
                        break;
                    case "BTC":
                        this.tlsEBTC.Text = table.Amount.ToString("f4");
                        break;
                    case "XRP":
                        this.tlsEXRP.Text = table.Amount.ToString("f4");
                        break;
                    case "ETH":
                        this.tlsEETH.Text = table.Amount.ToString("f4");
                        break;
                }
            }
        }
        #endregion
    }
}
