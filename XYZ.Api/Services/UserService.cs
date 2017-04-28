using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XYZ.Api.Models;

namespace XYZ.Api.Services
{
    public class UserService
    {
        private static readonly List<User> _users = new List<User>();
        private static readonly object lockObj = new object();

        public List<User> GetUsers()
        {
            return _users;
        }

        public int AddUser(User user)
        {
            lock (lockObj)
            {
                if (!_users.Any(x =>
                    x.Name
                    .ToUpper()
                        .Equals(user.Name.ToUpper())))
                {
                    var newId = _users.Count + 1;
                    user.Id = newId;
                    _users.Add(user);
                    return newId;
                }
                return -1;
            }
        }

        public User Get(int id)
        {
            lock (lockObj)
            {
                return _users.FirstOrDefault(x => x.Id == id);
            }
        }

        public bool Update(int id, int points)
        {
            lock (lockObj)
            {
                var index = _users.FindIndex(x => x.Id == id);
                if (index>=0)
                {
                    _users[index].Points = points;
                    return true;
                }
                return false;
            }
        }

        



    }
}