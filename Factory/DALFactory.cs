using IDAL;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Factory
{
    public static class DALFactory
    {
        static DALFactory()
        {
            Assembly assembly = Assembly.Load("DAL");
            DalType = assembly.GetType("DAL.BaseDal");

        }
        public static Type DalType { get; set; }

        public static IBaseDal CreateInstance()
        {
            return (IBaseDal)Activator.CreateInstance(DalType);
        }

    }
}
