using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Data.Common;
using Domain.Common.DataBaseModels;

namespace DAL.DataBase.Dao
{
    public class ExchangeRateDao
    {
        private string con = SQLiteHelper.SQLiteHelper.LocalDbConnectionString;

        public void Insert(ExchangeRateTable obj)
        {
            string sql = "insert into ExchangeRate(Source, Currency, Quotes,CreateTime,LastChangeTime) values(@Source,@Currency, @Quotes,datetime('now', 'localtime'),datetime('now', 'localtime'))";

            using (SQLiteCommand cmd = new SQLiteCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Source", obj.Source);
                cmd.Parameters.AddWithValue("@Currency", obj.Currency);
                cmd.Parameters.AddWithValue("@Quotes", obj.Quotes);

                SQLiteHelper.SQLiteHelper.ExecuteNonQuery(con, cmd);
            }
        }

        public ExchangeRateTable Select(long id)
        {
            string sql = "select ID,Source, Currency, Quotes,CreateTime,LastChangeTime from ExchangeRate where id=@id";

            ExchangeRateTable result = null;
            using (SQLiteCommand cmd = new SQLiteCommand(sql))
            {
                cmd.Parameters.AddWithValue("@id", id);

                var reader = SQLiteHelper.SQLiteHelper.ExecuteReader(con, cmd);
                while (reader.Read())
                {
                    result = new ExchangeRateTable()
                    {
                        ID = long.Parse(reader["id"].ToString()),
                        Currency = reader["Currency"].ToString(),
                        Quotes = double.Parse(reader["Quotes"].ToString()),
                        Source = reader["Source"].ToString(),
                        CreateTime = DateTime.Parse(reader["CreateTime"].ToString()),
                        LastChangeTime = DateTime.Parse(reader["LastChangeTime"].ToString()),
                    };
                }
            }

            return result;
        }

        public List<ExchangeRateTable> Select(string Source)
        {
            string sql = "select ID,Source, Currency, Quotes,CreateTime,LastChangeTime from ExchangeRate where Source=@Source";

            var results = new List<ExchangeRateTable>();
            using (SQLiteCommand cmd = new SQLiteCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Source", Source);

                var reader = SQLiteHelper.SQLiteHelper.ExecuteReader(con, cmd);
                while (reader.Read())
                {
                    results.Add(new ExchangeRateTable()
                    {
                        ID = long.Parse(reader["id"].ToString()),
                        Currency = reader["Currency"].ToString(),
                        Quotes = double.Parse(reader["Quotes"].ToString()),
                        Source = reader["Source"].ToString(),
                        CreateTime = DateTime.Parse(reader["CreateTime"].ToString()),
                        LastChangeTime = DateTime.Parse(reader["LastChangeTime"].ToString()),
                    });
                }
            }

            return results;
        }

        public List<ExchangeRateTable> Select()
        {
            string sql = "select ID,Source, Currency, Quotes,CreateTime,LastChangeTime from ExchangeRate";

            var results = new List<ExchangeRateTable>();
            using (SQLiteCommand cmd = new SQLiteCommand(sql))
            {
                var reader = SQLiteHelper.SQLiteHelper.ExecuteReader(con, cmd);
                while (reader.Read())
                {
                    results.Add(new ExchangeRateTable()
                    {
                        ID = long.Parse(reader["id"].ToString()),
                        Currency = reader["Currency"].ToString(),
                        Quotes = double.Parse(reader["Quotes"].ToString()),
                        Source = reader["Source"].ToString(),
                        CreateTime = DateTime.Parse(reader["CreateTime"].ToString()),
                        LastChangeTime = DateTime.Parse(reader["LastChangeTime"].ToString()),
                    });
                }
            }

            return results;
        }

        public void Update(ExchangeRateTable obj)
        {
            string sql = "UPDATE ExchangeRate set Source=@Source, Currency=@Currency, Quotes=@Quotes,LastChangeTime=datetime('now', 'localtime') where id=@ID";

            using (SQLiteCommand cmd = new SQLiteCommand(sql))
            {
                cmd.Parameters.AddWithValue("@ID", obj.ID);
                cmd.Parameters.AddWithValue("@Source", obj.Source);
                cmd.Parameters.AddWithValue("@Currency", obj.Currency);
                cmd.Parameters.AddWithValue("@Quotes", obj.Quotes);

                SQLiteHelper.SQLiteHelper.ExecuteNonQuery(con, cmd);
            }
        }

        public void InsertOrUpdate(List<ExchangeRateTable> objs)
        {
            foreach (var obj in objs)
            {
                var lists = Select(obj.Source);
                if (lists.Exists(o => o.Currency == obj.Currency))
                {
                    var exchange = lists.Find(o => o.Currency == obj.Currency);
                    obj.ID = exchange.ID;
                    obj.CreateTime = exchange.CreateTime;
                    Update(exchange);
                }
                else
                {
                    Insert(obj);
                }
            }
        }
    }
}
