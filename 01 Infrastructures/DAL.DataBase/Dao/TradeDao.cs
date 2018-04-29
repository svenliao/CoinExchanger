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
    public class TradeDao
    {
        readonly string conStr = SQLiteHelper.SQLiteHelper.LocalDbConnectionString;

        /// <summary>
        ///  TradeTable
        /// </summary>
        /// <param name="TradeTable">TradeTable实体对象</param>
        public void Insert(TradeTable obj)
        {
            try
            {
                string sql = "insert into Trades(TransactionID,Executed, PlatformID,OrderTxid,Status,AvgPrice,Volume,Cost,Fee,CreateTime,LastChangeTime) " +
                    "values(@TransactionID,@Executed, @PlatformID,@OrderTxid,@Status,@AvgPrice,@Volume,@Cost,@Fee,datetime('now', 'localtime'),datetime('now', 'localtime'))";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@PlatformID", obj.Platform.ID);
                    cmd.Parameters.AddWithValue("@TransactionID", obj.TransactionID);
                    cmd.Parameters.AddWithValue("@Executed", obj.Executed);
                    cmd.Parameters.AddWithValue("@OrderTxid", obj.OrderTxid);
                    cmd.Parameters.AddWithValue("@Status", obj.Status);
                    cmd.Parameters.AddWithValue("@AvgPrice", obj.AvgPrice);
                    cmd.Parameters.AddWithValue("@Volume", obj.Volume);
                    cmd.Parameters.AddWithValue("@Cost", obj.Cost);
                    cmd.Parameters.AddWithValue("@Fee", obj.Fee);

                    SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用TradeTable时，访问Insert时出错", ex);
            }
        }
        /// <summary>
        /// TradeTable
        /// </summary>
        /// <param name="TradeTable">TradeTable实体对象</param>
        /// <returns>状态代码</returns>
        public int Update(TradeTable obj)
        {
            try
            {
                string sql = "UPDATE Trades set PlatformID=@PlatformID,OrderTxid=@OrderTxid, TransactionID=@TransactionID, Executed=@Executed,Status=@Status,AvgPrice=@AvgPrice,Volume=@Volume,Cost=@Cost," +
                    "Fee=@Fee,LastChangeTime=datetime('now', 'localtime') where id=@ID";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@ID", obj.ID);
                    cmd.Parameters.AddWithValue("@PlatformID", obj.Platform.ID);
                    cmd.Parameters.AddWithValue("@TransactionID", obj.TransactionID);
                    cmd.Parameters.AddWithValue("@Executed", obj.Executed);
                    cmd.Parameters.AddWithValue("@OrderTxid", obj.OrderTxid);
                    cmd.Parameters.AddWithValue("@Status", obj.Status);
                    cmd.Parameters.AddWithValue("@AvgPrice", obj.AvgPrice);
                    cmd.Parameters.AddWithValue("@Volume", obj.Volume);
                    cmd.Parameters.AddWithValue("@Cost", obj.Cost);
                    cmd.Parameters.AddWithValue("@Fee", obj.Fee);

                    return SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用TradeTable 时，访问Update时出错", ex);
            }
        }


        /// <summary>
        /// TradeTable
        /// </summary>
        /// <param name="TradeTable">TradeTable实体对象</param>
        /// <returns>状态代码</returns>
        public int Delete(TradeTable obj)
        {
            try
            {
                string sql = "delete from Trades where id=@ID";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@ID", obj.ID);
                    
                    return SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用TradeTable 时，访问Delete时出错", ex);
            }
        }

        /// <summary>
        /// TradeTable
        /// </summary>
        /// <param name="TradeTable">TradeTable实体对象</param>
        /// <returns>状态代码</returns>
        public List<TradeTable> Select()
        {
            try
            {
                string sql = "select Trades.ID,TransactionID,Executed, PlatformID,Name1,Url,OrderTxid,Status,AvgPrice,Volume,Cost,Fee,Trades.CreateTime,Trades.LastChangeTime" +
                    " from Trades,Platform where Trades.PlatformID=Platform.ID";

                var results = new List<TradeTable>();
                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    var reader = SQLiteHelper.SQLiteHelper.ExecuteReader(conStr, cmd);
                    while (reader.Read())
                    {
                        results.Add(new TradeTable()
                        {
                            ID = long.Parse(reader["id"].ToString()),
                            OrderTxid = reader["OrderTxid"].ToString(),
                            TransactionID = reader["TransactionID"].ToString(),
                            Executed =DateTime.Parse(reader["Executed"].ToString()),
                            Status = reader["Status"].ToString(),
                            AvgPrice = double.Parse(reader["AvgPrice"].ToString()),
                            Cost = double.Parse(reader["Cost"].ToString()),
                            Fee = double.Parse(reader["Fee"].ToString()),
                            CreateTime = DateTime.Parse(reader["CreateTime"].ToString()),
                            LastChangeTime = DateTime.Parse(reader["LastChangeTime"].ToString()),                         
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
                throw new Exception("调用TradeTable 时，访问Select时出错", ex);
            }
        }

        /// <summary>
        /// TradeTable
        /// </summary>
        /// <param name="TradeTable">OrderTable实体对象</param>
        /// <returns>状态代码</returns>
        public List<TradeTable> Select(string OrderTxid)
        {
            try
            {
                string sql = "select Trades.ID,TransactionID,Executed, PlatformID,Name1,Url,OrderTxid,Status,AvgPrice,Volume,Cost,Fee,Trades.CreateTime,Trades.LastChangeTime" +
                    " from Trades,Platform where Trades.PlatformID=Platform.ID and OrderTxid=@OrderTxid";

                var results = new List<TradeTable>();
                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@OrderTxid", OrderTxid);
                    var reader = SQLiteHelper.SQLiteHelper.ExecuteReader(conStr, cmd);
                    while (reader.Read())
                    {
                        results.Add(new TradeTable()
                        {
                            ID = long.Parse(reader["id"].ToString()),
                            OrderTxid = reader["OrderTxid"].ToString(),
                            TransactionID = reader["TransactionID"].ToString(),
                            Executed = DateTime.Parse(reader["Executed"].ToString()),
                            Status = reader["Status"].ToString(),
                            AvgPrice = double.Parse(reader["AvgPrice"].ToString()),
                            Cost = double.Parse(reader["Cost"].ToString()),
                            Fee = double.Parse(reader["Fee"].ToString()),
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
                throw new Exception("调用TradeTable 时，访问Select时出错", ex);
            }
        }

        public void InsertOrUpdate(List<TradeTable> objs)
        {
            foreach (var obj in objs)
            {
                var list = Select(obj.OrderTxid);
                if (list!=null&&list.Exists(t=>t.TransactionID==obj.TransactionID))
                {
                    var o = list.Find(t => t.TransactionID == obj.TransactionID);
                    obj.ID = o.ID;
                    obj.CreateTime = o.CreateTime;
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
