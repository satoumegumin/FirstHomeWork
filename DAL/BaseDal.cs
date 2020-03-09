using Common;
using Common.AttributeExt;
using IDAL;
using Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DAL
{
    public class BaseDal:IBaseDal
    {
        //public void InsertData<T>(T t) where T : BaseModel
        //{
        //    Type type = typeof(T);
        //    var propArray = type.GetProperties().Where(t => t.Name != "Id");
        //    string colString = string.Join(',', propArray.Select(t => "`" + t.GetColName() + "`"));
        //    string paraString = string.Join(',',propArray.Select(t => "@" + t.GetColName()));
        //    var parameterArray = propArray.Select(p=>new MySqlParameter($"@{p.GetColName()}",p.GetValue(t))).ToArray();
        //    string sql = $"INSERT INTO `{type.Name}` ({colString}) VALUES ({paraString})";
        //    using (MySqlConnection conn = new MySqlConnection(StaticConstant.MysqlConn))
        //    {
        //        MySqlCommand command = new MySqlCommand(sql, conn);
        //        command.Parameters.AddRange(parameterArray);
        //        conn.Open();
        //        int iResult = command.ExecuteNonQuery();
        //        if (iResult==0)
        //        {
        //            throw new Exception("添加数据失败");
        //        }
        //    }
        //}
        /// <summary>
        /// 约束是为了正确的调用 才可以int Id
        /// 单个查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        //public T Find<T>(int Id) where T : BaseModel
        //{
        //    Type type = typeof(T);
        //    string colString =string.Join(',', type.GetProperties().Select(t=>t.GetColName()));
        //    string tableName = type.Name;
        //    string sql = $"SELECT {colString} FROM `{tableName}` WHERE Id={Id}";
        //    T model = null;// (T)Activator.CreateInstance(type);
        //    using (MySqlConnection conn = new MySqlConnection(StaticConstant.MysqlConn))
        //    {
        //        MySqlCommand command = new MySqlCommand(sql,conn);
        //        conn.Open();
        //        MySqlDataReader reader = command.ExecuteReader();
        //        model= this.ReaderToList<T>(reader).FirstOrDefault();
        //    }
        //    return model;
        //}

        public List<T> FindAll<T>() where T : BaseModel
        {
            Type type = typeof(T);
            string colString = string.Join(',', type.GetProperties().Select(t => t.GetColName()));
            string tableName = type.Name;
            string sql = $"SELECT {colString} FROM `{tableName}` WHERE 1";
            List<T> list = new List<T>();
            using (MySqlConnection conn = new MySqlConnection(StaticConstant.MysqlConn))
            {
                MySqlCommand command = new MySqlCommand(sql, conn);
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();
                list = this.ReaderToList<T>(reader);
            }
            return list;
        }

        #region 基于委托的封装 多个方法重复对数据库的访问 通过委托解耦 去掉重复代码
        public void InsertData<T>(T t) where T : BaseModel
        {
            Type type = typeof(T);
            var propArray = type.GetProperties().Where(t => t.Name != "Id");
            string colString = string.Join(',', propArray.Select(t => "`" + t.GetColName() + "`"));
            string paraString = string.Join(',', propArray.Select(t => "@" + t.GetColName()));
            var parameterArray = propArray.Select(p => new MySqlParameter($"@{p.GetColName()}", p.GetValue(t))).ToArray();
            string sql = $"INSERT INTO `{type.Name}` ({colString}) VALUES ({paraString})";

            Func<MySqlCommand, int> func = new Func<MySqlCommand, int>(command => {
                command.Parameters.AddRange(parameterArray);
                //conn.Open();
                int iResult = command.ExecuteNonQuery();
                if (iResult == 0)
                {
                    throw new Exception("添加数据失败");
                }
                return iResult;
            });
            ExcuteSql<int>(sql,func);
        }

        public T Find<T>(int Id) where T : BaseModel
        {
            Type type = typeof(T);
            string colString = string.Join(',', type.GetProperties().Select(t => t.GetColName()));
            string tableName = type.Name;
            string sql = $"SELECT {colString} FROM `{tableName}` WHERE Id={Id}";
            Func<MySqlCommand, T> func = new Func<MySqlCommand, T>(command =>
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    T model = this.ReaderToList<T>(reader).FirstOrDefault();
                    return model;
                }                               
            });
            T model= ExcuteSql<T>(sql,func);         
            return model;
        }

        public T ExcuteSql<T>(string sql,Func<MySqlCommand,T>func)
        {
            using (MySqlConnection conn = new MySqlConnection(StaticConstant.MysqlConn))
            {
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    conn.Open();

                    MySqlTransaction mySqlTransaction = conn.BeginTransaction();
                    command.Transaction = mySqlTransaction;
                    try
                    {                        
                        T result = func.Invoke(command);
                        mySqlTransaction.Commit();//提交事物
                        return result;
                    }
                    catch (Exception ex)
                    {
                        mySqlTransaction.Rollback();//事物回滚
                        throw;
                    }
                }                    
            }
        }


        #endregion

        #region private 转换模型
        private List<T> ReaderToList<T>(MySqlDataReader reader) where T : BaseModel
        {
            Type type = typeof(T);
            List<T> list = new List<T>();
            while (reader.Read())
            {
                T model =  (T)Activator.CreateInstance(type);
                foreach (PropertyInfo prop in type.GetProperties())
                {
                    object value = reader[prop.GetColName()];
                    if (value is DBNull)
                    {
                        value = null;
                    }                    
                    prop.SetValue(model, value);
                }
                list.Add(model);
            }
            return list;
        } 
        #endregion
    }
}
