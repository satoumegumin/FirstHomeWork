using Common.AttributeExt;
using Factory;
using IDAL;
using Model;
using System;

namespace FIrstHomeWork
{
    class Program
    {
        static void Main(string[] args)
        {
            IBaseDal baseDal = DALFactory.CreateInstance();
            Company company = baseDal.Find<Company>(1);
            Company c = new Company() { Id=1,Name="haha"};
            bool b= c.Validate();
            //UserInfo userInfo = new UserInfo() {  Account="4324", CompanyId=1, CompanyName="丰田啊到",CreateTime=DateTime.Now, CreatorId=1, Email="3252",LastLoginTime=DateTime.Now, LastModifierID=1, LastModifyTime=DateTime.Now, Name="ewang", Password="dsada", Status=0};
            //baseDal.InsertData<UserInfo>(userInfo);
            UserInfo u = baseDal.Find<UserInfo>(1);
            Console.WriteLine(company.Name+"的"+u.Name);
            Console.WriteLine("Hello World!");
        }
    }
}
