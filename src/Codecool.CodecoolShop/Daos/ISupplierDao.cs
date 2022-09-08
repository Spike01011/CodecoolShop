using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Daos
{
    public interface ISupplierDao : IDao<Supplier>
    {
        Supplier Get(int id);
        public Supplier GetBy(string name);
    }
}
