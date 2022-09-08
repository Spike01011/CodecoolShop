using System.Collections.Generic;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Daos;

public interface ICartDao
{


    public void Add(Product game);
    public void Remove(int id);

    public Product Get(int id);

    public IEnumerable<Product> GetAll();
    public void EmptyCart();

    public void CreateCart(int userId);

}