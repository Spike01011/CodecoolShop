using System;
using System.Collections.Generic;
using Codecool.CodecoolShop.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Data.SqlClient;

namespace Codecool.CodecoolShop.Daos.Implementations
{
    class ProductCategoryDaoDB : IProductCategoryDao
    {
        private static ProductCategoryDaoDB instance;
        private static ProductDaoDB productDao = ProductDaoDB.GetInstance();
        private string _connectionString => ConfigurationManager.AppSettings["connectionString"];

        private ProductCategoryDaoDB()
        {

        }

        public static ProductCategoryDaoDB GetInstance()
        {
            if (instance == null)
            {
                instance = new ProductCategoryDaoDB();
            }

            return instance;
        }


        public void Add(ProductCategory item)
        {
            const string command =
                @"insert into product_categories (name, description, department) values (@name, @description, @department) select scope_identity();";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(command, connection);
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    cmd.Parameters.AddWithValue("@name", item.Name);
                    cmd.Parameters.AddWithValue("@description", item.Description);
                    cmd.Parameters.AddWithValue("@department", item.Department);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                throw new RuntimeWrappedException(e);
            }

            var newitem = GetBy(item.Name);
            item.Id = newitem.Id;
        }

        public void Remove(int id)
        {
            const string command = @"delete from product_categories where id=@id select scope_identity();";

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

        public ProductCategory Get(int id)
        {
            const string command = @"select * from product_categories where id=@id;";

            try
            {
                string name;
                string description;
                string department;
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(command, connection);
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    name = reader.GetString("name");
                    description = reader.GetString("description");
                    department = reader.GetString("department");
                    Product featuredProduct;
                    try
                    {
                        var featuredElemId = reader.GetInt32("featured_elem");
                        featuredProduct = productDao.Get(featuredElemId, new ProductCategory(){Id = id, Name = name, Department = department, Description = description});
                    }
                    catch (SqlNullValueException e)
                    {
                        featuredProduct = null;
                    }
                    connection.Close();
                    ProductCategory product = new ProductCategory() { Id = id, Name = name, Description = description, Department = department, FeaturedProduct = featuredProduct};
                    return product;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                throw new RuntimeWrappedException(e);
            }
        }

        public IEnumerable<ProductCategory> GetAll()
        {
            const string command = @"select * from product_categories;";
            try
            {
                var result = new List<ProductCategory>();
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
                        var department = reader.GetString("department");
                        Product featuredProduct;
                        try
                        {
                            var featuredElemId = reader.GetInt32("featured_elem");
                            featuredProduct = productDao.Get(featuredElemId);
                        }
                        catch (SqlNullValueException e)
                        {
                            featuredProduct = null;
                        }

                        ProductCategory product = new ProductCategory() { Id = id, Name = name, Description = description, Department = department, FeaturedProduct = featuredProduct};
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

        public void SetFeatured(int categoryId, int featuredId)
        {
            const string command = @"update product_categories set featured_elem = @featured_elem where id=@cat_id;";
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(command, connection);
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    cmd.Parameters.AddWithValue("featured_elem", featuredId);
                    cmd.Parameters.AddWithValue("@cat_id", categoryId);
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

        public ProductCategory GetBy(string name)
        {
            const string command = @"select * from product_categories where name=@name;";

            try
            {
                int id;
                string description;
                string department;
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(command, connection);
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    cmd.Parameters.AddWithValue("@name", name);
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    id = reader.GetInt32("id");
                    description = reader.GetString("description");
                    department = reader.GetString("department");
                    connection.Close();
                    ProductCategory product = new ProductCategory() { Id = id, Name = name, Description = description, Department = department };
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
