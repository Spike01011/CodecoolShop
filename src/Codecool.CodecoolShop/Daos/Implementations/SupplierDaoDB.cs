using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using Codecool.CodecoolShop.Models;
using Microsoft.Data.SqlClient;
using System.Configuration;
namespace Codecool.CodecoolShop.Daos.Implementations
{
    public class SupplierDaoDB : ISupplierDao
    {
        private static SupplierDaoDB instance;
        private string _connectionString => ConfigurationManager.AppSettings["connectionString"];

        private SupplierDaoDB()
        {
        }

        public static SupplierDaoDB GetInstance()
        {
            if (instance == null)
            {
                instance = new SupplierDaoDB();
            }

            return instance;
        }

        public void Add(Supplier item)
        {
            const string command =
                @"insert into suppliers (name, description) values (@name, @description) select scope_identity();";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(command, connection);
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    cmd.Parameters.AddWithValue("@name", item.Name);
                    cmd.Parameters.AddWithValue("@description", item.Description);
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
            const string command = @"delete from suppliers where id=@id select scope_identity();";

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

        public Supplier Get(int id)
        {
            const string command = @"select * from suppliers where id=@id;";

            try
            {
                string name;
                string description;
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(command, connection);
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    name = reader.GetString("name");
                    description = reader.GetString("description");
                    connection.Close();
                    Supplier product = new Supplier() { Id = id, Name = name, Description = description};
                    return product;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                throw new RuntimeWrappedException(e);
            }
        }

        public IEnumerable<Supplier> GetAll()
        {
            const string command = @"select * from suppliers;";
            try
            {
                var result = new List<Supplier>();
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
                        Supplier product = new Supplier() { Id = id, Name = name, Description = description};
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

        public Supplier GetBy(string name)
        {
            const string command = @"select * from suppliers where name=@name;";

            try
            {
                int id;
                string description;
                using (var connection = new SqlConnection(_connectionString))
                {
                    var cmd = new SqlCommand(command, connection);
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    cmd.Parameters.AddWithValue("@name", name);
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    id = reader.GetInt32("id");
                    description = reader.GetString("description");
                    connection.Close();
                    Supplier product = new Supplier() { Id = id, Name = name, Description = description };
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
