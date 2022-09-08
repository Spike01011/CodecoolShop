using System.Collections.Generic;
using Codecool.CodecoolShop.Models;
using Microsoft.Identity.Client;

namespace Codecool.CodecoolShop.Daos;

public interface IUserDao
{
    public User GetBy(string name);
    public List<string> GetAllNames();
    public void Add(User user);

}