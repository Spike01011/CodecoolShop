using System.Collections.Generic;
using System.Linq;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Services
{
    public class ProductService
    {
        private readonly IProductDao productDao;
        private readonly IProductCategoryDao productCategoryDao;
        private readonly ISupplierDao supplierDao;
        private readonly ShopCart shopCartDao;

        public ProductService(IProductDao productDao, IProductCategoryDao productCategoryDao, ISupplierDao supplierDao, ShopCart shopCartdao)
        {
            this.productDao = productDao;
            this.productCategoryDao = productCategoryDao;
            this.supplierDao = supplierDao;
            this.shopCartDao = shopCartdao;
        }

        public ProductCategory GetProductCategory(int categoryId)
        {
            return this.productCategoryDao.Get(categoryId);
        }

        public IEnumerable<Product> GetProductsForCategory(int categoryId)
        {
            ProductCategory category = this.productCategoryDao.Get(categoryId);
            return this.productDao.GetBy(category);
        }

        public IEnumerable<ProductCategory> GetCategories()
        {
            return this.productCategoryDao.GetAll();
        }

        public IEnumerable<Product> GetProducts()
        {
            return productDao.GetAll();
        }

        public IEnumerable<Product> GetProductBySupplier(int id)
        {
            var supplier = supplierDao.Get(id);
            return productDao.GetBy(supplier);
        }

        public IEnumerable<Supplier> GetSuppliers()
        {
            return supplierDao.GetAll();
        }

        public IEnumerable<Product> GetByDevAndGenre(int devId, int catId)
        {
            var cat = productCategoryDao.Get(catId);
            var dev = supplierDao.Get(devId);

            return productDao.GetBy(dev).Intersect(productDao.GetBy(cat));
        }


        public IEnumerable<Product> GetShoppingCartItems()
        {
            return shopCartDao.GetAll();
        }

        public Product GetProduct(int id)
        {
            return productDao.Get(id);
        }

        public void AddToCart(int id)
        {
            var game = GetProduct(id);
            shopCartDao.Add(game);
        }

        public void RemoveFromCart(int id)
        {
            shopCartDao.Remove(id);
        }

        public List<Product> GetCartProducts()
        {
            return shopCartDao.GetAll().ToList();
        }

        public void EmptyCart()
        {
            shopCartDao.EmptyCart();
        }



    }
}
