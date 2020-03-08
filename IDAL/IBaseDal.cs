using Common;
using Common.AttributeExt;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IDAL
{
    public interface IBaseDal
    {
        public void InsertData<T>(T t) where T : BaseModel;
        /// <summary>
        /// 约束是为了正确的调用 才可以int Id
        /// 单个查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T Find<T>(int Id) where T : BaseModel;

        public List<T> FindAll<T>() where T : BaseModel;

    }
}
