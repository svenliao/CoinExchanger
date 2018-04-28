using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using DAL.DataBase.Dao;

namespace ExmoClient
{
    public delegate void JobNotifyEvent(object obj);
    public class NotifyIconJob : IJob
    {
        public static event JobNotifyEvent ExcuteEvent;

        public void Execute()
        {
            if (ExcuteEvent != null)
            {
                ExcuteEvent(null);
            }
        }
    }

    public class BalanceJob : IJob
    {
        public static event JobNotifyEvent ExcuteEvent;

        public void Execute()
        {
            var doa = new BalanceDao();
            var data = doa.Select();
            if (ExcuteEvent != null)
            {
                ExcuteEvent(data);
            }
        }
    }

    public class TickerJob : IJob
    {
        public static event JobNotifyEvent ExcuteEvent;

        public void Execute()
        {
            var doa = new TickerDao();
            var data = doa.Select();
            if (ExcuteEvent != null)
            {
                ExcuteEvent(data);
            }
        }
    }
       
}
