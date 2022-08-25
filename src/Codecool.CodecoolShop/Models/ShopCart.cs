using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Codecool.CodecoolShop.Models;

public class ShopCart
{
    private static ShopCart instance = null;
    private List<Product> cart = new List<Product>();
    private decimal totalCost => cart.Sum(x => x.DefaultPrice);

    private ShopCart()
    {

    }

    public static ShopCart GetInstance()
    {
        if (instance == null)
        {
            instance = new ShopCart();
        }

        return instance;
    }

    public void Add(Product game)
    {
        game.Id = cart.Count + 1;
        cart.Add(game);
    }

    public void Remove(int id)
    {
        cart.Remove(Get(id));
    }

    public Product Get(int id)
    {
        return cart.Find(x => x.Id == id);
    }

    public IEnumerable<Product> GetAll()
    {
        return cart;
    }

    public void EmptyCart()
    {
        cart.Clear();
    }

}