using System.Collections.Generic;

namespace Codecool.CodecoolShop.Models
{
    public class ProductCategory : BaseModel
    {
        public List<Product> Products { get; set; }
        public string Department { get; set; }

        public Product FeaturedProduct { get; set; }

        public override string ToString()
        {
            return new string($"Id: {Id} Name: {Name} Description: {Description}");
        }
    }
}
