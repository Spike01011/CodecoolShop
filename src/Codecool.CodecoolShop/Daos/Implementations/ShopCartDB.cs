using System;
using Codecool.CodecoolShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Configuration;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Data.SqlClient;
using Exception = System.Exception;

namespace Codecool.CodecoolShop.Daos.Implementations;

public class ShopCartDB : ICartDao
{
    private static ShopCartDB instance;
    private string _connectionString => ConfigurationManager.AppSettings["connectionString"];
    private static SupplierDaoDB supplierDao = SupplierDaoDB.GetInstance();
    private static ProductCategoryDaoDB categoryDao = ProductCategoryDaoDB.GetInstance();
    private static ProductDaoDB productDao = ProductDaoDB.GetInstance();
    private ShopCartDB()
    {
    }

    public static ShopCartDB GetInstance()
    {
        if (instance == null)
        {
            instance = new ShopCartDB();
        }

        return instance;
    }

    public void Add(Product item)
    {
        string items = "";
        const string command =
            @"select items from cart_order where user_id=@user_id;";
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed) connection.Open();
                var cmd = new SqlCommand(command, connection);
                int userId = MyGlobals.Id.Value;
                string productsIds;
                cmd.Parameters.AddWithValue("@user_id", userId);
                try
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    productsIds = reader.GetString("items");
                    items = productsIds + $"{item.Id}, ";
                    connection.Close();
                }
                catch (InvalidOperationException e)
                {
                    items = $"{item.Id}, ";
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            throw new RuntimeWrappedException(e);
        }

        RewriteItems(items);
    }

    public void Remove(int itemId)
    {
        string items = "";
        const string command = @"select items from cart_order where user_id=@user_id;";

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(command, connection);
                if (connection.State == ConnectionState.Closed) connection.Open();
                var userId = MyGlobals.Id.Value;
                cmd.Parameters.AddWithValue("@user_id", userId);
                var reader = cmd.ExecuteReader();
                reader.Read();
                var productsIds = reader.GetString("items");
                productsIds = productsIds.TrimEnd();
                productsIds = productsIds.TrimEnd(',');
                var products = productsIds.Split(", ").ToList();
                var productsClone = new List<string>();
                productsClone.AddRange(products);
                for (int i = 0; i < productsClone.Count; i++)
                {
                    if (Convert.ToInt32(products[i]) == itemId)
                    {
                        products.RemoveAt(i);
                        break;
                    }
                }

                foreach (var item in products)
                {
                    items += $"{item}, ";
                }
                connection.Close();
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            throw new RuntimeWrappedException(e);
        }

        RewriteItems(items);
    }

    public Product Get(int itemId)
    {
        const string command = @"select items from cart_order where user_id=@user_id;";

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(command, connection);
                if (connection.State == ConnectionState.Closed) connection.Open();
                var userId = MyGlobals.Id.Value;
                cmd.Parameters.AddWithValue("@user_id", userId);
                var reader = cmd.ExecuteReader();
                reader.Read();
                var productsIds = reader.GetString("items");
                productsIds = productsIds.TrimEnd();
                productsIds = productsIds.TrimEnd(',');
                var productsIdsList = productsIds.Split(", ");
                var products = new List<Product>();
                connection.Close();
                if (productsIdsList.Contains($", {itemId},")) return productDao.Get(itemId);
                throw new RuntimeWrappedException("No such game in cart");
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
        const string command = @"select items from cart_order where user_id=@user_id;";
        try
        {
            List<Product> result = new List<Product>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(command, connection);
                if (MyGlobals.Id == null)
                {
                    return new List<Product>();
                }
                var userId = MyGlobals.Id.Value;
                if (connection.State == ConnectionState.Closed) connection.Open();
                cmd.Parameters.AddWithValue("@user_id", userId);
                var reader = cmd.ExecuteReader();
                if (reader == null) return result;
                reader.Read();
                var productIds = reader.GetString("items");
                productIds = productIds.TrimEnd();
                productIds = productIds.TrimEnd(',');
                if (productIds == null || productIds == "") return result;
                foreach (var itemId in productIds.Split(", "))
                {
                    var product = productDao.Get(Convert.ToInt32(itemId));
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

    public void EmptyCart()
    {
        string items = "";
        const string command = @"update cart_order set items='' where user_id=@user_id select scope_identity();";

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(command, connection);
                if (connection.State == ConnectionState.Closed) connection.Open();
                var userId = MyGlobals.Id.Value;
                cmd.Parameters.AddWithValue("@user_id", userId);
                var reader = cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            throw new RuntimeWrappedException(e);
        }
    }

    private void RewriteItems(string items)
    {
        const string command2 = @"update cart_order set items = @items where user_id=@user_id;";
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(command2, connection);
                if (connection.State == ConnectionState.Closed) connection.Open();
                var userId = MyGlobals.Id.Value;
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@items", items);
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

    public void CreateCart(int userId)
    {
        const string command = @"insert into cart_order (items, user_id) values (@items, @user_id) select scope_identity()";
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(command, connection);
                if (connection.State == ConnectionState.Closed) connection.Open();
                cmd.Parameters.AddWithValue("@items", "");
                cmd.Parameters.AddWithValue("@user_id", userId);
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

}