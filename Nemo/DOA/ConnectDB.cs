﻿using Npgsql;
using System;
using Nemo.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Diagnostics;
using System.Data;

namespace Nemo.DOA
{
    public class ConnectDB
    {
        /// <summary>The main entry point for your application.</summary>
        private string connString; // Chuỗi kết nối tới database
        private NpgsqlConnection conn; // Đối tượng kết nối tới database
        public ConnectDB()
        {
            // Khởi tạo chuỗi kết nối
            connString = "Server=localhost;Port=5432;Username=postgres;Password=1;Database=postgres";

            // Khởi tạo đối tượng kết nối
            conn = new NpgsqlConnection(connString);
        }
        public void OpenConnection()
        {
            conn.Open(); 
        }

        public void CloseConnection()
        {
            conn.Close(); // Đóng kết nối tới database
        }
        public DataTable ExecuteQuery(string query)
        {
            
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn); // Tạo đối tượng command để thực hiện truy vấn
            NpgsqlDataReader reader = cmd.ExecuteReader(); // Thực hiện truy vấn và trả về đối tượng reader để đọc kết quả
            if (reader.HasRows)
            {
                DataTable dt=new DataTable();
                dt.Load(reader);
                return dt;
            }
            return null;
            // Trả về đối tượng reader
        }


    }
}