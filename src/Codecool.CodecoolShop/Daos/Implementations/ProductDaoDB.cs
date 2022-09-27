using System;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Codecool.CodecoolShop.Controllers;
using Codecool.CodecoolShop.Models;
using Microsoft.Data.SqlClient;

namespace Codecool.CodecoolShop.Daos.Implementations
{
    public class ProductDaoDB : IProductDao
    {
        private static ProductDaoDB instance;
        private static ProductCategoryDaoDB categoryDao = ProductCategoryDaoDB.GetInstance();
        private static SupplierDaoDB supplierDao = SupplierDaoDB.GetInstance();
        private string _connectionString => ConfigurationManager.AppSettings["connectionString"];

        private ProductDaoDB()
        {

        }

        public static ProductDaoDB GetInstance()
        {
            if (instance == null)
            {
                instance = new ProductDaoDB();
            }

            return instance;
        }

        public void Add(Product item)
        {
            const string command =
                @"insert into products (name, description, default_price, currency, supplier, category, image) values (@name, @description, @default_price, @currency, @supplier, @category, @image) select scope_identity();";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(command, connection);
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    cmd.Parameters.AddWithValue("@name", item.Name);
                    cmd.Parameters.AddWithValue("@description", item.Description);
                    cmd.Parameters.AddWithValue("@default_price", item.DefaultPrice);
                    cmd.Parameters.AddWithValue("@currency", item.Currency);
                    cmd.Parameters.AddWithValue("@supplier", item.Supplier.Id);
                    cmd.Parameters.AddWithValue("@category", item.ProductCategory.Id);
                    cmd.Parameters.AddWithValue("@image", item.imgURL);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                throw new RuntimeWrappedException(e);
            }

            var newItem = GetBy(item.Name);
            item.Id = newItem.Id;
        }

        public void Remove(int id)
        {
            const string command = @"delete from products where id=@id select scope_identity();";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(command, connection);
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                throw new RuntimeWrappedException(e);
            }
        }

        public Product Get(int id)
        {
            const string command = @"select * from products where id=@id;";

            try
            {
                string name;
                string description;
                decimal defaultPrice;
                string currency;
                int supplierId;
                int categoryId;
                Supplier supplier;
                ProductCategory category;
                string image;
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(command, connection);
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    name = reader.GetString("name");
                    description = reader.GetString("description");
                    defaultPrice = reader.GetDecimal("default_price");
                    currency = reader.GetString("currency");
                    supplierId = reader.GetInt32("supplier");
                    categoryId = reader.GetInt32("category");
                    image = reader.GetString("image");
                    supplier = supplierDao.Get(supplierId);
                    category = categoryDao.Get(categoryId);
                    connection.Close();
                    Product product = new Product() { Id = id, Name = name, Description = description, DefaultPrice = defaultPrice, Currency = currency, imgURL = image, Supplier = supplier, ProductCategory = category };
                    return product;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                throw new RuntimeWrappedException(e);
            }
        }
        public Product Get(int id, ProductCategory category=null)
        {
            const string command = @"select * from products where id=@id;";

            try
            {
                string name;
                string description;
                decimal defaultPrice;
                string currency;
                int supplierId;
                int categoryId;
                Supplier supplier;
                string image;
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(command, connection);
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    name = reader.GetString("name");
                    description = reader.GetString("description");
                    defaultPrice = reader.GetDecimal("default_price");
                    currency = reader.GetString("currency");
                    supplierId = reader.GetInt32("supplier");
                    categoryId = reader.GetInt32("category");
                    image = reader.GetString("image");
                    supplier = supplierDao.Get(supplierId);
                    if (category == null)
                    {
                        category = categoryDao.Get(categoryId);
                    }
                    else
                    {
                        category.FeaturedProduct = new Product() { Id = id, Name = name, Description = description, ProductCategory = category, Currency = currency, imgURL = image, Supplier = supplier, DefaultPrice = defaultPrice};
                    }
                    connection.Close();
                    Product product = new Product() { Id = id, Name = name, Description = description, DefaultPrice = defaultPrice, Currency = currency, imgURL = image, Supplier = supplier, ProductCategory = category };
                    return product;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                throw new RuntimeWrappedException(e);
            }
        }

        public IEnumerable<Product> GetAll()
        {
            const string command = @"select * from products;";
            try
            {
                var result = new List<Product>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(command, connection);
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    var reader = cmd.ExecuteReader();
                    if (!reader.HasRows) return result;

                    while (reader.Read())
                    {
                        var id = reader.GetInt32("id");
                        var name = reader.GetString("name");
                        var description = reader.GetString("description");
                        var defaultPrice = reader.GetDecimal("default_price");
                        var currency = reader.GetString("currency");
                        var supplierId = reader.GetInt32("supplier");
                        var categoryId = reader.GetInt32("category");
                        var image = reader.GetString("image");
                        var supplier = supplierDao.Get(supplierId);
                        var category = categoryDao.Get(categoryId);
                        Product product = new Product() { Id = id, Name = name, Description = description, DefaultPrice = defaultPrice, Currency = currency, imgURL = image, Supplier = supplier, ProductCategory = category };
                        result.Add(product);
                    }
                    connection.Close();
                }

                return result;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                throw new RuntimeWrappedException(e);
            }
        }

        public IEnumerable<Product> GetBy(Supplier supplier)
        {
            const string command = @"select * from products where supplier = @supplier_id";
            try
            {
                var result = new List<Product>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(command, connection);
                    cmd.Parameters.AddWithValue("@supplier_id", supplier.Id);
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    var reader = cmd.ExecuteReader();
                    if (!reader.HasRows) return result;

                    while (reader.Read())
                    {
                        var id = reader.GetInt32("id");
                        var name = reader.GetString("name");
                        var description = reader.GetString("description");
                        var defaultPrice = reader.GetDecimal("default_price");
                        var currency = reader.GetString("currency");
                        var categoryId = reader.GetInt32("category");
                        var image = reader.GetString("image");
                        var category = categoryDao.Get(categoryId);
                        Product product = new Product() { Id = id, Name = name, Description = description, DefaultPrice = defaultPrice, Currency = currency, imgURL = image, Supplier = supplier, ProductCategory = category };
                        result.Add(product);
                    }
                    connection.Close();
                }

                return result;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                throw new RuntimeWrappedException(e);
            }
        }

        public IEnumerable<Product> GetBy(ProductCategory productCategory)
        {
            const string command = @"select * from products where category = @category_id";
            try
            {
                var result = new List<Product>();
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(command, connection);
                    cmd.Parameters.AddWithValue("@category_id", productCategory.Id);
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    var reader = cmd.ExecuteReader();
                    if (!reader.HasRows) return result;

                    while (reader.Read())
                    {
                        var id = reader.GetInt32("id");
                        var name = reader.GetString("name");
                        var description = reader.GetString("description");
                        var defaultPrice = reader.GetDecimal("default_price");
                        var currency = reader.GetString("currency");
                        var supplierId = reader.GetInt32("supplier");
                        var image = reader.GetString("image");
                        var supplier = supplierDao.Get(supplierId);
                        Product product = new Product() { Id = id, Name = name, Description = description, DefaultPrice = defaultPrice, Currency = currency, imgURL = image, Supplier = supplier, ProductCategory = productCategory };
                        result.Add(product);
                    }
                    connection.Close();
                }

                return result;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                throw new RuntimeWrappedException(e);
            }
        }

        public Product GetBy(string name)
        {
            const string command = @"select * from products where name = @name;";

            try
            {
                int id;
                string description;
                decimal defaultPrice;
                string currency;
                int supplierId;
                int categoryId;
                Supplier supplier;
                ProductCategory category;
                string image;
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(command, connection);
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    cmd.Parameters.AddWithValue("@name", name);
                    var reader = cmd.ExecuteReader();
                    if (!reader.Read()) return null;
                    id = reader.GetInt32("id");
                    name = reader.GetString("name");
                    description = reader.GetString("description");
                    defaultPrice = reader.GetDecimal("default_price");
                    currency = reader.GetString("currency");
                    supplierId = reader.GetInt32("supplier");
                    categoryId = reader.GetInt32("category");
                    image = reader.GetString("image");
                    supplier = supplierDao.Get(supplierId);
                    category = categoryDao.Get(categoryId);
                    connection.Close();
                    Product product = new Product() { Id = id, Name = name, Description = description, DefaultPrice = defaultPrice, Currency = currency, imgURL = image, Supplier = supplier, ProductCategory = category };
                    return product;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                throw new RuntimeWrappedException(e);
            }
        }
    }
}
