﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKModel
{
    public static class MageKnightDataDB
    {
        public static SqlConnection GetConnection()
        {
            // If necessary, change the following connection string
            // so it works for your system
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\johns\OneDrive\Documents\GitHub\MageKnight\MKModel\MageKnightData.mdf;Integrated Security=True";
            SqlConnection connection =
            new SqlConnection(connectionString);
            return connection;
        }
    }

    public static class MKUserDataDB
    {
        public static SqlConnection GetConnection()
        {
            // If necessary, change the following connection string
            // so it works for your system
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\johns\OneDrive\Documents\GitHub\MageKnight\MKModel\MKUserData.mdf;Integrated Security=True";
            SqlConnection connection =
            new SqlConnection(connectionString);
            return connection;
        }
    }
}
