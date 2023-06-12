using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcertPlatform.Models.EntityLayer;
using Npgsql;

namespace ConcertPlatform.Models.DataAccessLayer
{
    public class UserDAL
    {
        //create user function
        public static void CreateUser(User user)
        {
            NpgsqlConnection connection = DALHelper.Connection;
            connection.Open();

            string query = "INSERT INTO Users (username, password, isAdmin) VALUES (@username, @password, @isAdmin)";

            var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("username", user.Username);
            command.Parameters.AddWithValue("password", user.Password);
            command.Parameters.AddWithValue("isAdmin", user.Isadmin);

            command.ExecuteNonQuery();
        }

        public static void DeleteUser(User user)
        {
            NpgsqlConnection connection = DALHelper.Connection;
            connection.Open();

            string query = "DELETE FROM Users WHERE user_id = @userId";

            var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("userId", user.UserId);

            command.ExecuteNonQuery();
        }
    }
}
