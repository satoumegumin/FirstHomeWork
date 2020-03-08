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
        public void InsertData<T>(T t) where T : BaseModel
        {
            Type type = typeof(T);
            var propArray = type.GetProperties().Where(t => t.Name != "Id");
            string colString = string.Join(',', propArray.Select(t => "`" + t.GetColName() + "`"));
            string paraString = string.Join(',',propArray.Select(t => "@" + t.GetColName()));
            var parameterArray = propArray.Select(p=>new MySqlParameter($"@{p.GetColName()}",p.GetValue(t))).ToArray();
            string sql = $"INSERT INTO `{type.Name}` ({colString}) VALUES ({paraString})";
            using (MySqlConnection conn = new MySqlConnection(StaticConstant.MysqlConn))
            {
                MySqlCommand command = new MySqlCommand(sql, conn);
                command.Parameters.AddRange(parameterArray);
                conn.Open();
                int iResult = command.ExecuteNonQuery();
                if (iResult==0)
                {
                    throw new Exception("添加数据失败");
                }
            }
        }
        /// <summary>
        /// 约束是为了正确的调用 才可以int Id
        /// 单个查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T Find<T>(int Id) where T : BaseModel
        {
            Type type = typeof(T);
            string colString =string.Join(',', type.GetProperties().Select(t=>t.GetColName()));
            string tableName = type.Name;
            string sql = $"SELECT {colString} FROM `{tableName}` WHERE Id={Id}";
            T model = null;// (T)Activator.CreateInstance(type);
            using (MySqlConnection conn = new MySqlConnection(StaticConstant.MysqlConn))
            {
                MySqlCommand command = new MySqlCommand(sql,conn);
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();
                model= this.ReaderToList<T>(reader).FirstOrDefault();
            }
            return model;
        }

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

        #region private 转换模型
        private List<T> ReaderToList<T>(MySqlDataReader reader) where T : BaseModel
        {
            Type type = typeof(T);
            List<T> list = new List<T>();
            while (reader.Read())
            {
                T model = null;// (T)Activator.CreateInstance(type);
                foreach (PropertyInfo prop in type.GetProperties())
                {
                    object value = reader[prop.GetColName()];
                    if (value is DBNull)
                    {
                        value = null;
                    }
                    prop.SetValue(model, reader[prop.GetColName()]);
                }
                list.Add(model);
            }
            return list;
        } 
        #endregion
    }
}
