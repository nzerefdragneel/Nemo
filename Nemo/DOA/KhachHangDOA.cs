using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Nemo.DOA
{
    static class Program
    {
        /// <summary>The main entry point for your application.</summary>
        [STAThread]
        static void Main()
    {
        // Connection information
        string connInfo = string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                                        "localhost", 5432, "postgres", "12345678", "sample_db");

        // Connection
        NpgsqlConnection conn = null;

        try
        {
            // Initialization
            conn = new NpgsqlConnection(connInfo);

            // Open connection
            conn.Open();

            MessageBox.Show("Successfully connected to PostgreSQL.");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Failed to connect to PostgreSQL. Error: " + ex.Message);
        }
        finally
        {
            // Close connection
            if (null != conn)
            {
                conn.Close();

            }
        }
    }
}
