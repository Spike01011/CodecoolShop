using System;
using Codecool.CodecoolShop.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;

namespace Codecool.CodecoolShop.Daos.Implementations;

public class UserDaoDB : IUserDao
{
    private string _connectionString => ConfigurationManager.AppSettings["connectionString"];
    private static ShopCartDB shopCartDB = ShopCartDB.GetInstance();
    public void Add(User item)
    {
        const string command =
            @"insert into users (name, password) values (@name, @password) select scope_identity();";

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(command, connection);
                if (connection.State == ConnectionState.Closed) connection.Open();
                cmd.Parameters.AddWithValue("@name", item.UserName.ToLower());
                cmd.Parameters.AddWithValue("@password", item.Password);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            throw new RuntimeWrappedException(e);
        }

        var newItem = this.GetBy(item.UserName);
        shopCartDB.CreateCart(newItem.Id);
        item.Id = newItem.Id;
    }

    public List<string> GetAllNames()
    {
        const string command = @"select name from users;";
        try
        {
            var result = new List<string>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(command, connection);
                if (connection.State == ConnectionState.Closed) connection.Open();
                var reader = cmd.ExecuteReader();
                if (!reader.HasRows) return result;

                while (reader.Read())
                {
                    string name = reader.GetString("name");
                    result.Add(name);
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

    public User GetBy(string name)
    {
        const string command = @"select * from users where name=@name;";

        try
        {
            int id;
            string password;
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(command, connection);
                if (connection.State == ConnectionState.Closed) connection.Open();
                cmd.Parameters.AddWithValue("@name", name.ToLower());
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    id = reader.GetInt32("id");
                    password = reader.GetString("password");
                    connection.Close();
                }
                else
                {
                    connection.Close();
                    return null;
                }
                connection.Close();
                User user = new User(name.ToLower(), password) { Id = id};
                return user;
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            throw new RuntimeWrappedException(e);
        }
    }
}