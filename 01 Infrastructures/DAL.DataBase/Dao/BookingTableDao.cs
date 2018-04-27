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
                string sql = "insert into Booking(Pair,PlatformID, Coin, Currency,Asks,Bids,CreateTime,LastChangeTime) values(@Pair,@PlatformID,@Coin, @Currency,@Asks,@Bids,datetime('now', 'localtime'),datetime('now', 'localtime'))";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@Asks", obj.Asks);
                    cmd.Parameters.AddWithValue("@Bids", obj.Bids);
                    cmd.Parameters.AddWithValue("@PlatformID", obj.Platform.ID);
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
                string sql = "UPDATE Booking set Asks=@Asks,PlatformID=@PlatformID, Bids=@Bids, Coin=@Coin,Currency=@Currency,Pair=@Pair,LastChangeTime=datetime('now', 'localtime') where id=@ID";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@ID", obj.ID);
                    cmd.Parameters.AddWithValue("@PlatformID", obj.Platform.ID);
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
                string sql = "select Booking.ID,PlatformID,Name1,Url,Pair, Coin, Currency,Asks,Bids,Booking.CreateTime,Booking.LastChangeTime from Booking,Platform where Booking.PlatformID=Platform.ID";

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
                            Asks = reader["Asks"].ToString(),
                            Bids = reader["Bids"].ToString(),
                            Coin = reader["Coin"].ToString(),
                            Currency=reader["Currency"].ToString(),
                            CreateTime = DateTime.Parse(reader["CreateTime"].ToString()),
                            LastChangeTime = DateTime.Parse(reader["LastChangeTime"].ToString()),
                            Platform = new PlatformTable()
                            {
                                ID = long.Parse(reader["PlatformID"].ToString()),
                                Name = reader["Name1"].ToString(),
                                Url = reader["Url"].ToString()
                            }
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
                string sql = "select Booking.ID,PlatformID,Name1,Url,Pair, Coin, Currency,Asks,Bids,Booking.CreateTime,Booking.LastChangeTime from Booking,Platform where Booking.PlatformID=Platform.ID and Coin=@Coin";

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
                            Asks = reader["Asks"].ToString(),
                            Bids = reader["Bids"].ToString(),
                            Coin = reader["Coin"].ToString(),
                            Currency = reader["Currency"].ToString(),
                            CreateTime = DateTime.Parse(reader["CreateTime"].ToString()),
                            LastChangeTime = DateTime.Parse(reader["LastChangeTime"].ToString()),
                            Platform = new PlatformTable()
                            {
                                ID = long.Parse(reader["PlatformID"].ToString()),
                                Name = reader["Name1"].ToString(),
                                Url = reader["Url"].ToString()
                            }
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
                if (list.Exists(o=>o.Currency==obj.Currency && o.Platform.ID == obj.Platform.ID))
                {
                    var ex = list.Find(o => o.Currency == obj.Currency && o.Platform.ID == obj.Platform.ID);
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
