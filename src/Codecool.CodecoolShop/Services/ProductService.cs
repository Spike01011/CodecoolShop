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

        public ProductService(IProductDao productDao, IProductCategoryDao productCategoryDao, ISupplierDao supplierDao)
        {
            this.productDao = productDao;
            this.productCategoryDao = productCategoryDao;
            this.supplierDao = supplierDao;
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
    }
}
