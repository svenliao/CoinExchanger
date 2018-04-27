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
    public class BalanceDao
    {
        readonly string conStr = SQLiteHelper.SQLiteHelper.LocalDbConnectionString;

        /// <summary>
        ///  BalanceTable
        /// </summary>
        /// <param name="BalanceTable">BalanceTable实体对象</param>
        public void Insert(BalanceTable obj)
        {
            try
            {
                string sql = "insert into Balance(UID,PlatformID, Coin, Amount,CreateTime,LastChangeTime) values(@UID,@PlatformID,@Coin, @Amount,datetime('now', 'localtime'),datetime('now', 'localtime'))";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@UID", obj.UID);
                    cmd.Parameters.AddWithValue("@PlatformID", obj.Platform.ID);
                    cmd.Parameters.AddWithValue("@Coin", obj.Coin);
                    cmd.Parameters.AddWithValue("@Amount", obj.Amount);

                    SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用BalanceTable时，访问Insert时出错", ex);
            }
        }
        /// <summary>
        /// 修改BalanceTable
        /// </summary>
        /// <param name="BalanceTable">BalanceTable实体对象</param>
        /// <returns>状态代码</returns>
        public int Update(BalanceTable obj)
        {
            try
            {
                string sql = "UPDATE Balance set UID=@UID,PlatformID=@PlatformID, Coin=@Coin, Amount=@Amount,LastChangeTime=datetime('now', 'localtime') where id=@ID";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@ID", obj.ID);
                    cmd.Parameters.AddWithValue("@UID", obj.UID);
                    cmd.Parameters.AddWithValue("@PlatformID", obj.Platform.ID);
                    cmd.Parameters.AddWithValue("@Coin", obj.Coin);
                    cmd.Parameters.AddWithValue("@Amount", obj.Amount);

                    return SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用BalanceTable时，访问Update时出错", ex);
            }
        }


        /// <summary>
        /// BalanceTable
        /// </summary>
        /// <param name="BalanceTable">BalanceTable实体对象</param>
        /// <returns>状态代码</returns>
        public int Delete(BalanceTable obj)
        {
            try
            {
                string sql = "Delect from Balance where id=@ID";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@ID", obj.ID);
                    
                    return SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用BalanceTable 时，访问Delete时出错", ex);
            }
        }

        /// <summary>
        /// BalanceTable
        /// </summary>
        /// <param name="BalanceTable">BalanceTable实体对象</param>
        /// <returns>状态代码</returns>
        public List<BalanceTable> Select()
        {
            try
            {
                string sql = "select Balance.ID,UID,PlatformID,Name1,Url,Coin, Amount,Balance.CreateTime,Balance.LastChangeTime from Balance,Platform where Balance.PlatformID=Platform.ID";

                var results = new List<BalanceTable>();
                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    var reader = SQLiteHelper.SQLiteHelper.ExecuteReader(conStr, cmd);
                    while (reader.Read())
                    {
                        results.Add(new BalanceTable()
                        {
                            ID = long.Parse(reader["id"].ToString()),
                            UID = reader["UID"].ToString(),
                            Amount = double.Parse(reader["Amount"].ToString()),
                            Coin = reader["Coin"].ToString(),                          
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
                throw new Exception("调用BalanceTable 时，访问Select时出错", ex);
            }
        }

        /// <summary>
        /// BalanceTable
        /// </summary>
        /// <param name="BalanceTable">BalanceTable实体对象</param>
        /// <returns>状态代码</returns>
        public List<BalanceTable> Select(string uid)
        {
            try
            {
                string sql = "select Balance.ID,UID,PlatformID,Name1,Url, Coin, Amount,Balance.CreateTime,Balance.LastChangeTime from Balance,Platform where Balance.PlatformID=Platform.ID and uid=@uid";

                List<BalanceTable> results = new List<BalanceTable>();
                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@uid", uid);
                    var reader = SQLiteHelper.SQLiteHelper.ExecuteReader(conStr, cmd);
                    while (reader.Read())
                    {
                        results.Add(new BalanceTable()
                        {
                            ID = long.Parse(reader["id"].ToString()),
                            UID = reader["UID"].ToString(),
                            Amount = double.Parse(reader["Amount"].ToString()),
                            Coin = reader["Coin"].ToString(),
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
                throw new Exception("调用BalanceTable 时，访问Select时出错", ex);
            }
        }

        public void InsertOrUpdate(List<BalanceTable> objs)
        {
            foreach (var obj in objs)
            {
                var list = Select(obj.UID);
                if (list.Exists(o=>o.Coin==obj.Coin&&o.Platform.ID==obj.Platform.ID))
                {
                    var ex = list.Find(o => o.Coin == obj.Coin && o.Platform.ID == obj.Platform.ID);
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
