using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Codecool.CodecoolShop.Models
{
    public class Product : BaseModel
    {
        public string Currency { get; set; }
        public decimal DefaultPrice { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public Supplier Supplier { get; set; }

        public string imgURL;

        public Product()
        {
            imgURL = $"img/{Name}.jpg";
        }
        public string MiniDescription => CreateMiniDescription();
        public void SetProductCategory(ProductCategory productCategory)
        {
            ProductCategory = productCategory;
            ProductCategory.Products.Add(this);
        }

        private string CreateMiniDescription(int max= 200)
        {
            if (this.Description.Length >= max)
            {
                string result;
                var tempResult = this.Description.Substring(0, max).Split(" ").ToList();
                tempResult.RemoveAt(tempResult.Count - 1);
                result = string.Join(" ", tempResult);
                return result + "...";
            }

            return Description;
        }

        public override bool Equals(object obj)
        {
            Product newObj;
            if (obj == null) return false;
            if (obj is Product)
            {
                newObj = (Product)obj;
                return newObj.Id == this.Id;
            }

            return false;
        }
    }
}
