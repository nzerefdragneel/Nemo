using Npgsql;
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

namespace Nemo.DOA
{
    public class KhachHangDOA
    {
        /// <summary>The main entry point for your application.</summary>
        private string connString; // Chuỗi kết nối tới database
        private NpgsqlConnection conn; // Đối tượng kết nối tới database
        /*
        public KhachHangDOA()
        {
            // Khởi tạo chuỗi kết nối
            connString = "Host=localhost;Username=postgres;Password=1;Database=nemo";

            // Khởi tạo đối tượng kết nối
            conn = new NpgsqlConnection(connString);
        }
        public void OpenConnection()
        {
            //conn.Open(); // Mở kết nối tới database
            Debug.WriteLine("test ok");
        }

        public void CloseConnection()
        {
            conn.Close(); // Đóng kết nối tới database
        }
        
        public NpgsqlDataReader ExecuteQuery(string query)
        {
            string que = "SELECT * FROM phong";
            NpgsqlCommand cmd = new NpgsqlCommand(que, conn); // Tạo đối tượng command để thực hiện truy vấn
            NpgsqlDataReader reader = cmd.ExecuteReader(); // Thực hiện truy vấn và trả về đối tượng reader để đọc kết quả

           return reader; // Trả về đối tượng reader
        }
        */

    }
}