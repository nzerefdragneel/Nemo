using Nemo.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Diagnostics;
using System.Windows.Markup;
using System.ComponentModel;

namespace Nemo.DTO
{
    internal class DoanhThuView : INotifyPropertyChanged
    {
        public ObservableCollection<int> listYearExist { set; get; }
        public ObservableCollection<DoanhThu> tinhDoanhThuThangTheoLoai { set; get; }
        public int year1 { set; get; }  

        public int year2 { set; get; }
        public int month { set;get; }
        public DoanhThuView() { 
            listYearExist = new ObservableCollection<int>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void setListYearExist()
        {
            var con = new DoanhThuDAO();
            listYearExist = con.listYear();
        }
        public ObservableCollection<int> getListMonthExist(int year)
        {
            var list = new ObservableCollection<int>();
            DateTime now = DateTime.Now;
            list = (now.Year == year) ? new ObservableCollection<int>(Enumerable.Range(1, now.Month).ToArray()) : new ObservableCollection<int>(Enumerable.Range(1, 12).ToArray());
            return list;
        }
        public double tinhTienThue(DateTime ngaythue, DateTime ngaytra,int checkknn, double gia,int sokhach)
        {
            var con = new QuyDinhDAO();
            QuyDinh qd = con.getQuyDinh();
            double rs = 0;
            rs += (ngaytra - ngaythue).Days * gia;
            if (sokhach > qd.soLuongKhachToiDa) rs *= qd.tiLePhuThu;
            if (checkknn==1) rs *= qd.heSoKhachNN;
            return rs;
        }
        public double doanhThuThang(DataTable data,int year,int month)
        {
            double rs = 0;
            foreach (DataRow row in data.Rows)
            {
                DateTime ngaythue = row.Field<DateTime>("ngaythue");
                DateTime ngaytra = row.Field<DateTime>("ngaytra");
                int checkknn = row.Field<int>("knn");
                double gia = row.Field<double>("gia");
                int sokhach = row.Field<int>("sokhach");
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

                rs += tinhTienThue(ngaythue, ngaytra, checkknn, gia, sokhach);
            }
            return rs;
        }
        public List<DoanhThu> tinhDoanhThuThang(int year)
        {
            var rs = new List<DoanhThu>();
            var con = new DoanhThuDAO();
            var data = con.doanhThu();
            DateTime now = DateTime.Now;
            if(now.Year == year)
            {
                for(int i = 1; i <= now.Month;i++)
                {
                    rs.Add(new DoanhThu() {year=year,month=i,total=0});
                }
            }
            else
            {
                for (int i = 1; i <= 12; i++)
                {
                    rs.Add(new DoanhThu() { year = year, month = i, total = 0 });
                }
            }
            foreach (DoanhThu d in rs)
            {
                rs[d.month - 1].total += doanhThuThang(data, year, d.month);
            }
            return rs;
        }
        public void settinhDoanhThuThangTheoLoai(int year,int month)
        {
            var con = new PhongViewDAO();
            var con1 = new DoanhThuDAO();
            var data = con1.doanhThu();
            DataTable listmaphong = con.GetListLoaiPhong();
            var rs = new ObservableCollection<DoanhThu>();
            foreach(DataRow maphong in listmaphong.Rows)
            {
                rs.Add(new DoanhThu { maloai = Convert.ToInt32(maphong["maloaiphong"]), doanhthutheomaloai = 0 });
            }
            foreach (DataRow row in data.Rows)
            {
                DateTime ngaythue = row.Field<DateTime>("ngaythue");
                DateTime ngaytra = row.Field<DateTime>("ngaytra");
                int maloai = row.Field<int>("maloaiphong");
                double gia = row.Field<double>("gia");
                int sokhach = row.Field<int>("sokhach");
                int checkknn = row.Field<int>("knn");
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
                DoanhThu d = rs.FirstOrDefault(d => d.maloai == maloai);
                d.doanhthutheomaloai += tinhTienThue(ngaythue, ngaytra, checkknn, gia, sokhach);
            }
            tinhDoanhThuThangTheoLoai = rs;
        }
        public double tinhTongdoanhThuThang()
        {
            double rs = 0;
            foreach (var x in tinhDoanhThuThangTheoLoai) rs += x.doanhthutheomaloai;
            return rs;
        }

    }
}
