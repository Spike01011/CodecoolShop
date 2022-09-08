using System;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;

namespace Codecool.CodecoolShop.Controllers
{
    public class SqlManager : Controller
    {
        private SqlManager()
        {

        }
        private static SqlManager _instance = null;
        public void EnsureConnectionSuccessful()
        {
            if (!testConnection())
            {
                throw new Exception("Connection failed");
            }
            throw new Exception("Connection success");
        }

        public string connectionString => ConfigurationManager.AppSettings["connectionString"];

        public static SqlManager GetInstance()
        {
            if (_instance == null) _instance = new SqlManager();
            return _instance;
        }

        public bool testConnection()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public void Add()
        {
            var firstName = "lol";
            var lastName = "Doe";
            var birthDate = new DateTime(1900, 5, 28);
            const string insertCommand =
                @"insert into author (first_name, last_name, birth_date) values (@first_name, @last_name, @birth_date) select scope_identity();";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    var cmd = new SqlCommand(insertCommand, connection);
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    cmd.Parameters.AddWithValue("@first_name", firstName);
                    cmd.Parameters.AddWithValue("@last_name", lastName);
                    cmd.Parameters.AddWithValue("@birth_date", birthDate);
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
}
