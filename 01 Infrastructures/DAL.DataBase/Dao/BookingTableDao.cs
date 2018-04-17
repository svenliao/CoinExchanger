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
    public class BookingTableDao
    {
        readonly string conStr = SQLiteHelper.SQLiteHelper.LocalDbConnectionString;

        /// <summary>
        ///  BookingTable
        /// </summary>
        /// <param name="BookingTable">BookingTable实体对象</param>
        public void Insert(BookingTable obj)
        {
            try
            {
                string sql = "insert into Booking(Pair, Coin, Currency,Asks,Bids,CreateTime,LastChangeTime) values(@Pair,@Coin, @Currency,@Asks,@Bids,datetime('now', 'localtime'),datetime('now', 'localtime'))";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@Asks", obj.Asks);
                    cmd.Parameters.AddWithValue("@Bids", obj.Bids);
                    cmd.Parameters.AddWithValue("@Coin", obj.Coin);
                    cmd.Parameters.AddWithValue("@Currency", obj.Currency);
                    cmd.Parameters.AddWithValue("@Pair", obj.Pair);

                    SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用BookingTable时，访问Insert时出错", ex);
            }
        }
        /// <summary>
        /// BookingTable
        /// </summary>
        /// <param name="BookingTable">BookingTable</param>
        /// <returns>状态代码</returns>
        public int Update(BookingTable obj)
        {
            try
            {
                string sql = "UPDATE Booking set Asks=@Asks, Bids=@Bids, Coin=@Coin,Currency=@Currency,Pair=@Pair,LastChangeTime=datetime('now', 'localtime') where id=@ID";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@ID", obj.ID);
                    cmd.Parameters.AddWithValue("@Asks", obj.Asks);
                    cmd.Parameters.AddWithValue("@Bids", obj.Bids);
                    cmd.Parameters.AddWithValue("@Coin", obj.Coin);
                    cmd.Parameters.AddWithValue("@Currency", obj.Currency);
                    cmd.Parameters.AddWithValue("@Pair", obj.Pair);

                    return SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用BookingTable时，访问Update时出错", ex);
            }
        }


        /// <summary>
        /// BookingTable
        /// </summary>
        /// <param name="BookingTable">BookingTable</param>
        /// <returns>状态代码</returns>
        public int Delete(BookingTable obj)
        {
            try
            {
                string sql = "Delect from Booking where id=@ID";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@ID", obj.ID);
                    
                    return SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用BookingTable 时，访问Delete时出错", ex);
            }
        }

        /// <summary>
        /// BookingTable
        /// </summary>
        /// <param name="BookingTable">BookingTable</param>
        /// <returns>状态代码</returns>
        public List<BookingTable> Select()
        {
            try
            {
                string sql = "select ID,Pair, Coin, Currency,Asks,Bids,CreateTime,LastChangeTime from Booking";

                var results = new List<BookingTable>();
                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    var reader = SQLiteHelper.SQLiteHelper.ExecuteReader(conStr, cmd);
                    while (reader.Read())
                    {
                        results.Add(new BookingTable()
                        {
                            ID = long.Parse(reader["id"].ToString()),
                            Pair = reader["Pair"].ToString(),
                            Asks = double.Parse(reader["Asks"].ToString()),
                            Bids = double.Parse(reader["Bids"].ToString()),
                            Coin = reader["Coin"].ToString(),
                            Currency=reader["Currency"].ToString(),
                            CreateTime = DateTime.Parse(reader["CreateTime"].ToString()),
                            LastChangeTime = DateTime.Parse(reader["LastChangeTime"].ToString()),
                        });
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("调用BookingTable 时，访问Select时出错", ex);
            }
        }

        /// <summary>
        /// BookingTable
        /// </summary>
        /// <param name="BookingTable">BookingTable</param>
        /// <returns>状态代码</returns>
        public List<BookingTable> Select(string Coin)
        {
            try
            {
                string sql = "select ID,Pair, Coin, Currency,Asks,Bids,CreateTime,LastChangeTime from Booking where coin=@coin";

                List<BookingTable> results = new List<BookingTable>();
                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@Coin", Coin);
                    var reader = SQLiteHelper.SQLiteHelper.ExecuteReader(conStr, cmd);
                    while (reader.Read())
                    {
                        results.Add(new BookingTable()
                        {
                            ID = long.Parse(reader["id"].ToString()),
                            Pair = reader["Pair"].ToString(),
                            Asks = double.Parse(reader["Asks"].ToString()),
                            Bids = double.Parse(reader["Bids"].ToString()),
                            Coin = reader["Coin"].ToString(),
                            Currency = reader["Currency"].ToString(),
                            CreateTime = DateTime.Parse(reader["CreateTime"].ToString()),
                            LastChangeTime = DateTime.Parse(reader["LastChangeTime"].ToString()),
                        });
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("调用BookingTable 时，访问Select时出错", ex);
            }
        }

        public void InsertOrUpdate(List<BookingTable> objs)
        {
            foreach (var obj in objs)
            {
                var list = Select(obj.Coin);
                if (list.Exists(o=>o.Currency==obj.Currency))
                {
                    var ex = list.Find(o => o.Currency == obj.Currency);
                    obj.ID = ex.ID;
                    obj.CreateTime = ex.CreateTime;
                    Update(obj);
                }
                else
                {
                    Insert(obj);
                }
            }
        }
    }
}
