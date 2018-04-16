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
    public class AccountDao
    {
        readonly string conStr = SQLiteHelper.SQLiteHelper.LocalDbConnectionString;

        /// <summary>
        ///  AccountTable
        /// </summary>
        /// <param name="AccountTable">AccountTable实体对象</param>
        public void Insert(AccountTable obj)
        {
            try
            {
                string sql = "insert into Account(UID, ApiVersion, Secret,Key,CreateTime,LastChangeTime) values(@UID,@ApiVersion, @Secret,@Key,datetime('now', 'localtime'),datetime('now', 'localtime'))";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@UID", obj.UID);
                    cmd.Parameters.AddWithValue("@ApiVersion", obj.ApiVersion);
                    cmd.Parameters.AddWithValue("@Secret", obj.Secret);
                    cmd.Parameters.AddWithValue("@Key", obj.Key);

                    SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用AccountTable时，访问Insert时出错", ex);
            }
        }
        /// <summary>
        /// 修改AccountTable
        /// </summary>
        /// <param name="AccountTable">AccountTable实体对象</param>
        /// <returns>状态代码</returns>
        public int Update(AccountTable obj)
        {
            try
            {
                string sql = "UPDATE Account set UID=@UID, ApiVersion=@ApiVersion, Secret=@Secret,Key=@Key,LastChangeTime=datetime('now', 'localtime') where id=@ID";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@ID", obj.ID);
                    cmd.Parameters.AddWithValue("@UID", obj.UID);
                    cmd.Parameters.AddWithValue("@ApiVersion", obj.ApiVersion);
                    cmd.Parameters.AddWithValue("@Secret", obj.Secret);
                    cmd.Parameters.AddWithValue("@Key", obj.Key);

                   return SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用AccountTable 时，访问Update时出错", ex);
            }
        }


        /// <summary>
        /// AccountTable
        /// </summary>
        /// <param name="AccountTable">AccountTable实体对象</param>
        /// <returns>状态代码</returns>
        public int Delete(AccountTable obj)
        {
            try
            {
                string sql = "Delect from Account where id=@ID";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@ID", obj.ID);
                    
                    return SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用AccountTable 时，访问Delete时出错", ex);
            }
        }

        /// <summary>
        /// AccountTable
        /// </summary>
        /// <param name="AccountTable">AccountTable实体对象</param>
        /// <returns>状态代码</returns>
        public List<AccountTable> Select()
        {
            try
            {
                string sql = "select ID,UID, ApiVersion, Secret,Key,CreateTime,LastChangeTime from Account";

                var results = new List<AccountTable>();
                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    var reader = SQLiteHelper.SQLiteHelper.ExecuteReader(conStr, cmd);
                    while (reader.Read())
                    {
                        results.Add(new AccountTable()
                        {
                            ID = long.Parse(reader["id"].ToString()),
                            UID = reader["UID"].ToString(),
                            ApiVersion = int.Parse(reader["ApiVersion"].ToString()),
                            Secret = reader["Secret"].ToString(),
                            Key = reader["Key"].ToString(),
                            CreateTime = DateTime.Parse(reader["CreateTime"].ToString()),
                            LastChangeTime = DateTime.Parse(reader["LastChangeTime"].ToString()),
                        });
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("调用AccountTable 时，访问Select时出错", ex);
            }
        }

        /// <summary>
        /// AccountTable
        /// </summary>
        /// <param name="AccountTable">AccountTable实体对象</param>
        /// <returns>状态代码</returns>
        public AccountTable Select(string uid)
        {
            try
            {
                string sql = "select ID,UID, ApiVersion, Secret,Key,CreateTime,LastChangeTime from Account where uid=@uid";

                AccountTable result = null;
                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@uid", uid);
                    var reader = SQLiteHelper.SQLiteHelper.ExecuteReader(conStr, cmd);
                    while (reader.Read())
                    {
                        result=new AccountTable()
                        {
                            ID = long.Parse(reader["id"].ToString()),
                            UID = reader["UID"].ToString(),
                            ApiVersion = int.Parse(reader["ApiVersion"].ToString()),
                            Secret = reader["Secret"].ToString(),
                            Key = reader["Key"].ToString(),
                            CreateTime = DateTime.Parse(reader["CreateTime"].ToString()),
                            LastChangeTime = DateTime.Parse(reader["LastChangeTime"].ToString()),

                        };
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("调用AccountTable 时，访问Select时出错", ex);
            }
        }

        public void InsertOrUpdate(List<AccountTable> objs)
        {
            foreach (var obj in objs)
            {
                var list = Select(obj.UID);
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
