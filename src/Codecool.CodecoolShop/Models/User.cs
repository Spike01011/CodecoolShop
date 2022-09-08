using System.Runtime.InteropServices.ComTypes;

namespace Codecool.CodecoolShop.Models;

public class User
{
    public User(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}