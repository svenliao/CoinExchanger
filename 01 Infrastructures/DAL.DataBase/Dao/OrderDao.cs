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
    public class OrderDao
    {
        readonly string conStr = SQLiteHelper.SQLiteHelper.LocalDbConnectionString;

        /// <summary>
        ///  OrderTable
        /// </summary>
        /// <param name="OrderTable">OrderTable实体对象</param>
        public void Insert(OrderTable obj)
        {
            try
            {
                string sql = "insert into Orders(UID,PlatformID,OrderTxid, Type1,LimitType,Status,Pair,Coin,Currency,Volume,LimitPrice,Price,Opened,Closed,CreateTime,LastChangeTime) " +
                    " values(@UID,@PlatformID,@OrderTxid, @Type,@LimitType,@Status,@Pair,@Coin,@Currency,@Volume,@LimitPrice,@Price,@Opened,@Closed,datetime('now', 'localtime'),datetime('now', 'localtime'))";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@UID", obj.UID);
                    cmd.Parameters.AddWithValue("@PlatformID", obj.Platform.ID);
                    cmd.Parameters.AddWithValue("@OrderTxid", obj.OrderTxid);
                    cmd.Parameters.AddWithValue("@Type", obj.Type);
                    cmd.Parameters.AddWithValue("@LimitType", obj.LimitType);
                    cmd.Parameters.AddWithValue("@Status", obj.Status);
                    cmd.Parameters.AddWithValue("@Pair", obj.Pair);
                    cmd.Parameters.AddWithValue("@Coin", obj.Coin);
                    cmd.Parameters.AddWithValue("@Currency", obj.Currency);
                    cmd.Parameters.AddWithValue("@Volume", obj.Volume);
                    cmd.Parameters.AddWithValue("@LimitPrice", obj.LimitPrice);
                    cmd.Parameters.AddWithValue("@Price", obj.Price);
                    cmd.Parameters.AddWithValue("@Opened", obj.Opened);
                    cmd.Parameters.AddWithValue("@Closed", obj.Closed);

                    SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用OrderTable时，访问Insert时出错", ex);
            }
        }
        /// <summary>
        /// 修改OrderTable
        /// </summary>
        /// <param name="OrderTable">OrderTable实体对象</param>
        /// <returns>状态代码</returns>
        public int Update(OrderTable obj)
        {
            try
            {
                string sql = "UPDATE Orders set UID=@UID,PlatformID=@PlatformID,OrderTxid=@OrderTxid, Type1=@Type1, LimitType=@LimitType,Status=@Status,Pair=@Pair,Coin=@Coin,Currency=@Currency," +
                    " Volume=@Volume,LimitPrice=@LimitPrice,Price=@Price,Opened=@Opened,Closed=@Closed,LastChangeTime=datetime('now', 'localtime') where id=@ID";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@ID", obj.ID);
                    cmd.Parameters.AddWithValue("@UID", obj.UID);
                    cmd.Parameters.AddWithValue("@PlatformID", obj.Platform.ID);
                    cmd.Parameters.AddWithValue("@OrderTxid", obj.OrderTxid);
                    cmd.Parameters.AddWithValue("@Type1", obj.Type);
                    cmd.Parameters.AddWithValue("@LimitType", obj.LimitType);
                    cmd.Parameters.AddWithValue("@Status", obj.Status);
                    cmd.Parameters.AddWithValue("@Pair", obj.Pair);
                    cmd.Parameters.AddWithValue("@Coin", obj.Coin);
                    cmd.Parameters.AddWithValue("@Currency", obj.Currency);
                    cmd.Parameters.AddWithValue("@Volume", obj.Volume);
                    cmd.Parameters.AddWithValue("@LimitPrice", obj.LimitPrice);
                    cmd.Parameters.AddWithValue("@Price", obj.Price);
                    cmd.Parameters.AddWithValue("@Opened", obj.Opened);
                    cmd.Parameters.AddWithValue("@Closed", obj.Closed);

                    return SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用OrderTable 时，访问Update时出错", ex);
            }
        }


        /// <summary>
        /// OrderTable
        /// </summary>
        /// <param name="OrderTable">OrderTable实体对象</param>
        /// <returns>状态代码</returns>
        public int Delete(OrderTable obj)
        {
            try
            {
                string sql = "delete from Orders where id=@ID";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@ID", obj.ID);
                    
                    return SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用OrderTable 时，访问Delete时出错", ex);
            }
        }

        /// <summary>
        /// OrderTable
        /// </summary>
        /// <param name="OrderTable">OrderTable实体对象</param>
        /// <returns>状态代码</returns>
        public List<OrderTable> Select()
        {
            try
            {
                string sql = "select Orders.ID,UID,PlatformID,Name1,Url,OrderTxid, Type1,LimitType,Status,Pair,Coin,Currency,Volume,LimitPrice,Price,Opened,Closed,Orders.CreateTime,Orders.LastChangeTime" +
                    " from Orders,Platform where Orders.PlatformID=Platform.ID";

                var results = new List<OrderTable>();
                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    var reader = SQLiteHelper.SQLiteHelper.ExecuteReader(conStr, cmd);
                    while (reader.Read())
                    {
                        results.Add(new OrderTable()
                        {
                            ID = long.Parse(reader["id"].ToString()),
                            UID = reader["UID"].ToString(),
                            OrderTxid = reader["OrderTxid"].ToString(),
                            Type = reader["Type1"].ToString(),
                            LimitType = reader["LimitType"].ToString(),
                            Status = reader["Status"].ToString(),
                            Pair = reader["Pair"].ToString(),
                            Currency = reader["Currency"].ToString(),
                            Volume =double.Parse(reader["Volume"].ToString()),
                            LimitPrice = double.Parse(reader["LimitPrice"].ToString()),
                            Price = double.Parse(reader["Price"].ToString()),
                            CreateTime = DateTime.Parse(reader["CreateTime"].ToString()),
                            LastChangeTime = DateTime.Parse(reader["LastChangeTime"].ToString()),
                            Opened = DateTime.Parse(reader["Opened"].ToString()),
                            Closed = DateTime.Parse(reader["Closed"].ToString()),
                            Platform=new PlatformTable()
                            {
                                ID= long.Parse(reader["PlatformID"].ToString()),
                                Name= reader["Name1"].ToString(),
                                Url = reader["Url"].ToString()
                            }
                        });
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("调用OrderTable 时，访问Select时出错", ex);
            }
        }

        /// <summary>
        /// OrderTable
        /// </summary>
        /// <param name="OrderTable">OrderTable实体对象</param>
        /// <returns>状态代码</returns>
        public string SelectTxid()
        {
            try
            {
                string sql = "select OrderTxid from Orders order by opened desc limit 1 offset 0";
                var result = "";
                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    var reader = SQLiteHelper.SQLiteHelper.ExecuteReader(conStr, cmd);
                    while (reader.Read())
                    {

                        result = reader["OrderTxid"].ToString();
                           
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("调用OrderTable 时，访问Select时出错", ex);
            }
        }

        /// <summary>
        /// OrderTable
        /// </summary>
        /// <param name="OrderTable">OrderTable实体对象</param>
        /// <returns>状态代码</returns>
        public OrderTable Select(string OrderTxid)
        {
            try
            {
                string sql = "select Orders.ID,UID,PlatformID,Name1,Url,OrderTxid, Type1,LimitType,Status,Pair,Coin,Currency,Volume,LimitPrice,Price,Opened,Closed,Orders.CreateTime,Orders.LastChangeTime" +
                    " from Orders,Platform where Orders.PlatformID=Platform.ID and OrderTxid=@OrderTxid";

                OrderTable result = null;
                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@OrderTxid", OrderTxid);
                    var reader = SQLiteHelper.SQLiteHelper.ExecuteReader(conStr, cmd);
                    while (reader.Read())
                    {
                        result=new OrderTable()
                        {
                            ID = long.Parse(reader["id"].ToString()),
                            UID = reader["UID"].ToString(),
                            OrderTxid = reader["OrderTxid"].ToString(),
                            Type = reader["Type1"].ToString(),
                            LimitType = reader["LimitType"].ToString(),
                            Status = reader["Status"].ToString(),
                            Pair = reader["Pair"].ToString(),
                            Currency = reader["Currency"].ToString(),
                            Volume = double.Parse(reader["Volume"].ToString()),
                            LimitPrice = double.Parse(reader["LimitPrice"].ToString()),
                            Price = double.Parse(reader["Price"].ToString()),
                            CreateTime = DateTime.Parse(reader["CreateTime"].ToString()),
                            LastChangeTime = DateTime.Parse(reader["LastChangeTime"].ToString()),
                            Opened = DateTime.Parse(reader["Opened"].ToString()),
                            Closed = DateTime.Parse(reader["Closed"].ToString()),
                            Platform = new PlatformTable()
                            {
                                ID = long.Parse(reader["PlatformID"].ToString()),
                                Name = reader["Name1"].ToString(),
                                Url = reader["Url"].ToString()
                            }
                        };
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("调用OrderTable 时，访问Select时出错", ex);
            }
        }

        public void InsertOrUpdate(List<OrderTable> objs)
        {
            foreach (var obj in objs)
            {
                var list = Select(obj.OrderTxid);
                if (list!=null)
                {
                    obj.ID = list.ID;
                    obj.CreateTime = list.CreateTime;
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
