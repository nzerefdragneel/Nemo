using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Nemo.DTO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Nemo.DAO
{
    internal class DoanhThuDAO
    {
        public ConnectDB conn;
        public DoanhThuDAO()
        {
            conn = new ConnectDB();
            conn.OpenConnection();
        }
        ~DoanhThuDAO()
        {
            conn.CloseConnection();
        }
        public ObservableCollection<int> listYear()
        {
            var rs = conn.ExecuteQuery("SELECT DISTINCT EXTRACT(YEAR FROM ngaythue) AS year FROM phieuthuephong;");
            var list = new ObservableCollection<int>();
            foreach(DataRow row in rs.Rows)
            {
                list.Add(Convert.ToInt16(row["year"]));
            }
            return list;
        }
        public DataTable doanhThu()
        {
            var rs = conn.ExecuteQuery(@"SELECT pt.ngaythue, pt.ngaytra, pt.maphongthue, h.makh, p.maloaiphong, l.gia,
                                        MAX(CASE WHEN kh.makh IS NOT NULL THEN 1 ELSE 0 END) AS knn,CAST(COUNT(pt.maptp) AS INTEGER) as sokhach 
                                        FROM phieuthuephong pt
                                        JOIN hoadonthanhtoan h ON pt.mahd = h.mahd
                                        JOIN phong p ON pt.maphongthue = p.maphong
                                        JOIN loaiphong l ON l.maloaiphong = p.maloaiphong
                                        JOIN lichsuthuephong ls ON ls.maptp = pt.maptp
                                        LEFT JOIN kh_nuocngoai kh ON kh.makh = ls.makh
                                        GROUP BY pt.ngaythue, pt.ngaytra, pt.maphongthue, h.makh, p.maloaiphong, l.gia");
            return rs;
        }
    }
}
