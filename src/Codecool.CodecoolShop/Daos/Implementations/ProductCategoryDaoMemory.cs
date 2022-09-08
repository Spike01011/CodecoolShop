using System.Collections.Generic;
using System.Linq;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Daos.Implementations
{
    class ProductCategoryDaoMemory : IProductCategoryDao
    {
        private List<ProductCategory> data = new List<ProductCategory>();
        private static ProductCategoryDaoMemory instance = null;
        private IProductDao productDao = ProductDaoMemory.GetInstance();

        private ProductCategoryDaoMemory()
        {
        }

        public static ProductCategoryDaoMemory GetInstance()
        {
            if (instance == null)
            {
                instance = new ProductCategoryDaoMemory();
            }

            return instance;
        }

        public void Add(ProductCategory item)
        {
            item.Id = data.Count + 1;
            data.Add(item);
        }

        public void Remove(int id)
        {
            data.Remove(this.Get(id));
        }

        public ProductCategory Get(int id)
        {
            return data.Find(x => x.Id == id);
        }

        public IEnumerable<ProductCategory> GetAll()
        {
            return data;
        }

        public void SetFeatured(int categoryId, int featuredId)
        {
            ProductCategory category = data.Where(x => x.Id == categoryId).First();
            if (category != null)
            {
                category.FeaturedProduct = productDao.Get(featuredId);
            }
        }

        public ProductCategory GetBy(string name)
        {
            return data.Find(x => x.Name == name);
        }
    }
}
