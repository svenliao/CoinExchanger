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
    public class PlatformDao
    {
        readonly string conStr = SQLiteHelper.SQLiteHelper.LocalDbConnectionString;

        /// <summary>
        ///  PlatformTable
        /// </summary>
        /// <param name="PlatformTable">PlatformTable实体对象</param>
        public void Insert(PlatformTable obj)
        {
            try
            {
                string sql = "insert into Platform(Name1,Url,CreateTime,LastChangeTime) values(@Name,@Url,datetime('now', 'localtime'),datetime('now', 'localtime'))";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@Name", obj.Name);
                    cmd.Parameters.AddWithValue("@Url", obj.Url);
                    
                    SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用PlatformTable时，访问Insert时出错", ex);
            }
        }
        /// <summary>
        /// PlatformTable
        /// </summary>
        /// <param name="PlatformTable">PlatformTable</param>
        /// <returns>状态代码</returns>
        public int Update(PlatformTable obj)
        {
            try
            {
                string sql = "UPDATE Platform set Name1=@Name,Url=@Url,LastChangeTime=datetime('now', 'localtime') where id=@ID";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@ID", obj.ID);
                    cmd.Parameters.AddWithValue("@Name", obj.Name);
                    cmd.Parameters.AddWithValue("@Url", obj.Url);

                    return SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用PlatformTable时，访问Update时出错", ex);
            }
        }


        /// <summary>
        /// PlatformTable
        /// </summary>
        /// <param name="PlatformTable">PlatformTable</param>
        /// <returns>状态代码</returns>
        public int Delete(PlatformTable obj)
        {
            try
            {
                string sql = "Delect from Platform where id=@ID";

                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@ID", obj.ID);
                    
                    return SQLiteHelper.SQLiteHelper.ExecuteNonQuery(conStr, cmd);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("调用PlatformTable 时，访问Delete时出错", ex);
            }
        }

        /// <summary>
        /// PlatformTable
        /// </summary>
        /// <param name="PlatformTable">PlatformTable</param>
        /// <returns>状态代码</returns>
        public List<PlatformTable> Select()
        {
            try
            {
                string sql = "select ID,Name1,Url,CreateTime,LastChangeTime from Platform";

                var results = new List<PlatformTable>();
                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    var reader = SQLiteHelper.SQLiteHelper.ExecuteReader(conStr, cmd);
                    while (reader.Read())
                    {
                        results.Add(new PlatformTable()
                        {
                            ID = long.Parse(reader["id"].ToString()),
                            Name = reader["Name1"].ToString(),
                            Url = reader["Url"].ToString(),                          
                            CreateTime = DateTime.Parse(reader["CreateTime"].ToString()),
                            LastChangeTime = DateTime.Parse(reader["LastChangeTime"].ToString()),                          
                        });
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("PlatformTable 时，访问Select时出错", ex);
            }
        }

        /// <summary>
        /// PlatformTable
        /// </summary>
        /// <param name="PlatformTable">PlatformTable</param>
        /// <returns>状态代码</returns>
        public List<PlatformTable> Select(string name)
        {
            try
            {
                string sql = "select ID,Name1,Url,CreateTime,LastChangeTime from Platform where name1=@name";

                List<PlatformTable> results = new List<PlatformTable>();
                using (SQLiteCommand cmd = new SQLiteCommand(sql))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    var reader = SQLiteHelper.SQLiteHelper.ExecuteReader(conStr, cmd);
                    while (reader.Read())
                    {
                        results.Add(new PlatformTable()
                        {
                            ID = long.Parse(reader["id"].ToString()),
                            Name = reader["Name1"].ToString(),
                            Url = reader["Url"].ToString(),
                            CreateTime = DateTime.Parse(reader["CreateTime"].ToString()),
                            LastChangeTime = DateTime.Parse(reader["LastChangeTime"].ToString()),
                        });
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new Exception("调用PlatformTable 时，访问Select时出错", ex);
            }
        }

        public void InsertOrUpdate(List<PlatformTable> objs)
        {
            foreach (var obj in objs)
            {
                var list = Select(obj.Name);
                if (list!=null&&list.Any())
                {
                    var ex = list.Find(o => o.Name == obj.Name);
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
