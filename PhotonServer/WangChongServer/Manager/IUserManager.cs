using System.Collections.Generic;
using MyGameServer.Model;


namespace MyGameServer.Manager
{
    interface IUserManager
    {
        void Add(User ser);
        void Update(User user);//更新数据
        void Remove(User user); //删除数据
        User GetById(int id); //根据ID获取数据
        User GetByUsername(string username); //根据username获取数据
        ICollection<User> GetAllUsers();  //获取所有数据
        bool VerifyUser(string username, string password);//验证用户密码

    }
}