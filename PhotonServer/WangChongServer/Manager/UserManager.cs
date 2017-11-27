using System.Collections.Generic;
using MyGameServer.Model;
using NHibernate;
using NHibernate.Criterion;

namespace MyGameServer.Manager
{
    class UserManager : IUserManager
    {

        public void Add(User user)
        {
            //也可使用成一个事务
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())//事务的开始
                {
                    //进行操作
                    session.Save(user);
                    transaction.Commit();//事物的提交
                }
            }
        }

        public ICollection<User> GetAllUsers()
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                //Restrictions.Eq()表示添加查询条件
                // criteria.UniqueResult<User>();得到唯一的结果，返回的是User对象
                IList<User> users = session.CreateCriteria(typeof(User)).List<User>();


                return users;
            }
        }

        public User GetById(int id)//查询条件不会更改数据所以不需要使用事务
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                //进行操作
                User user = session.Get<User>(id);//删除数据
                return user;
            }
        }

        public User GetByUsername(string username)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                //Restrictions.Eq()表示添加查询条件
                // criteria.UniqueResult<User>();得到唯一的结果，返回的是User对象
                User user = session.CreateCriteria(typeof(User)).Add(Restrictions.Eq("Username", username)).UniqueResult<User>();//创建一个配置文件
                return user;
            }

        }

        public void Remove(User user)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())//事务的开始
                {
                    //进行操作
                    session.Delete(user);//删除数据
                    transaction.Commit();//事物的提交
                }
            }
        }

        public void Update(User user)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())//事务的开始
                {
                    //进行操作
                    session.Update(user);//更新数据
                    transaction.Commit();//事物的提交
                }
            }
        }

        public bool VerifyUser(string username, string password)
        {
            using (ISession session = NhibernateHelper.OpenSession())
            {
                IList<User> users = session.CreateCriteria(typeof(User)).List<User>();
                User user = session.CreateCriteria(typeof(User))
                          .Add(Restrictions.Eq("Username", username))       // User类里面主键的映射
                          .Add(Restrictions.Eq("Password", password))       // User类里面主键的映射
                          .UniqueResult<User>();
                if (user == null) return false;
                return true;
            }
        }
    }
}