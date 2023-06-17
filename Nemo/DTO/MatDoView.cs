using Nemo.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemo.DTO
{
    public class MatDoView
    {
        public int year { get; set; }
        public int month { get; set; }
        public int year2 { get; set; }
        public ObservableCollection<Phong> setMatDoThang()
        {
            var con = new MatDoDAO();
            var con2 = new PhongViewDAO();
            var phong = con2.GetListPhong();
            DataTable data = con.getPhongThue();
            foreach (DataRow row in data.Rows)
            {
                DateTime ngaythue = row.Field<DateTime>("ngaythue");
                DateTime ngaytra = row.Field<DateTime>("ngaytra");
                int maphong = row.Field<int>("maphong");
                if (ngaythue.Year == year && ngaytra.Year == year)
                {
                    if (ngaythue.Month != month && ngaytra.Month != month)
                    {
                        continue;
                    }
                    else if (ngaythue.Month == month && ngaytra.Month != month)
                    {
                        ngaytra = new DateTime(year, month, 1).AddMonths(1);
                    }
                    else if (ngaytra.Month == month && ngaythue.Month != month)
                    {
                        ngaythue = new DateTime(year, month, 1);
                    }
                }
                else if (ngaythue.Year == year && ngaytra.Year > year && ngaythue.Month == 12)
                {
                    ngaytra = new DateTime(year + 1, 1, 1);
                }
                else if (ngaythue.Year < year && ngaytra.Year == year && ngaytra.Month == 1)
                {
                    ngaythue = new DateTime(year, 1, 1);
                }
                Phong p = phong.FirstOrDefault(p => p.maphong == maphong);
                p.songaythue += (ngaytra - ngaythue).Days;
            }
            Debug.WriteLine("hi");
            foreach (var p in phong)
            {
                Debug.WriteLine($"{p.maphong} {p.songaythue}");
            }
            return phong;
        }
        public ObservableCollection<Phong> setMatDoNam(int year){
            var con = new MatDoDAO();
            var con2 = new PhongViewDAO();
            var phong = con2.GetListPhong();
            DataTable data = con.getPhongThue();
            foreach (DataRow row in data.Rows)
            {
                DateTime ngaythue = row.Field<DateTime>("ngaythue");
                DateTime ngaytra = row.Field<DateTime>("ngaytra");
                int maphong = row.Field<int>("maphong");
                if (ngaythue.Year != year && ngaytra.Year != year)
                {
                    continue;
                }
                else if (ngaythue.Year == year && ngaytra.Year > year && ngaythue.Month == 12)
                {
                    ngaytra = new DateTime(year + 1, 1, 1);
                }
                else if (ngaythue.Year < year && ngaytra.Year == year && ngaytra.Month == 1)
                {
                    ngaythue = new DateTime(year, 1, 1);
                }
                Phong p = phong.FirstOrDefault(p => p.maphong == maphong);
                p.songaythue += (ngaytra - ngaythue).Days;
            }
            return phong;
        }
            
    }
}
