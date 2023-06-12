using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcertPlatform.Models.DataAccessLayer
{
    public class DALHelper
    {
        private static readonly string connectionString = "Host=localhost;Database=ConcertDB;Username=postgres;Password=123";

        public static NpgsqlConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        } 
    }
}
