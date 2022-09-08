using Codecool.CodecoolShop.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Codecool.CodecoolShop.Daos
{
    public interface IProductCategoryDao : IDao<ProductCategory>
    {
        public void SetFeatured(int categoryId, int featuredId);

        public ProductCategory GetBy(string name);

    }
}
