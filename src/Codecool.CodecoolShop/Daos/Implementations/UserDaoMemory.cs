using Codecool.CodecoolShop.Models;
using System.Collections.Generic;
using System.Linq;

namespace Codecool.CodecoolShop.Daos.Implementations;

public class UserDaoMemory : IUserDao
{
    public static List<User> Users = new List<User>();

    public UserDaoMemory()
    {
    }
    public void Add(User user)
    {
        User newUser = new User(user.UserName.ToLower(), user.Password);
        if (Users.Count > 0)
        {
            newUser.Id = Users.Max(x => x.Id) + 1;
        }
        else
        {
            newUser.Id = 1;
        }
        Users.Add(newUser);
    }

    public List<string> GetAllNames()
    {
        List<string> usernames = new List<string>();
        foreach (var user in Users)
        {
            usernames.Add(user.UserName.ToLower());
        }
        return usernames;
    }

    public User GetBy(string name)
    {
        foreach (var user in Users)
        {
            if (user.UserName.ToLower() == name.ToLower()) return user;
        }

        return null;
    }
}