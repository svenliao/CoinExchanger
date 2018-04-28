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
using API.Kraken;
using API.Exmo;
using Unity;

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
        }
        #endregion
    }
}
