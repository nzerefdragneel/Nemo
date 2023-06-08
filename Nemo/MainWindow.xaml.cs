using Nemo.DTO;
using Nemo.DOA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<Bill> items = new List<Bill>();

            items.Add(new Bill() { STT = 1, MaHD = 4, NgayTra = "01/05/2023", SoPhong = 1, KhachHang = "Nguyễn An", TongTien = 1100000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt"});
            items.Add(new Bill() { STT = 2, MaHD = 3, NgayTra = "01/04/2023", SoPhong = 3, KhachHang = "Minatozaki Sana", TongTien = 2800000, DiaChi = "Sóc Trăng", PhuongThuc = "Chuyển khoản" });
            items.Add(new Bill() { STT = 3, MaHD = 2, NgayTra = "01/03/2023", SoPhong = 2, KhachHang = "Sherlock Holmes", TongTien = 1550000, DiaChi = "Bình Dương", PhuongThuc = "Chuyển khoản" });
            items.Add(new Bill() { STT = 4, MaHD = 1, NgayTra = "01/02/2023", SoPhong = 1, KhachHang = "Trần Thanh Trúc", TongTien = 670000 });
            items.Add(new Bill() { STT = 1, MaHD = 4, NgayTra = "01/05/2023", SoPhong = 1, KhachHang = "Nguyễn An", TongTien = 1100000 });
            items.Add(new Bill() { STT = 2, MaHD = 3, NgayTra = "01/04/2023", SoPhong = 3, KhachHang = "Minatozaki Sana", TongTien = 2800000 });
            items.Add(new Bill() { STT = 3, MaHD = 2, NgayTra = "01/03/2023", SoPhong = 2, KhachHang = "Sherlock Holmes", TongTien = 1550000 });
            items.Add(new Bill() { STT = 4, MaHD = 1, NgayTra = "01/02/2023", SoPhong = 1, KhachHang = "Trần Thanh Trúc", TongTien = 670000 });
            items.Add(new Bill() { STT = 1, MaHD = 4, NgayTra = "01/05/2023", SoPhong = 1, KhachHang = "Nguyễn An", TongTien = 1100000 });
            items.Add(new Bill() { STT = 2, MaHD = 3, NgayTra = "01/04/2023", SoPhong = 3, KhachHang = "Minatozaki Sana", TongTien = 2800000 });
            items.Add(new Bill() { STT = 3, MaHD = 2, NgayTra = "01/03/2023", SoPhong = 2, KhachHang = "Sherlock Holmes", TongTien = 1550000 });
            items.Add(new Bill() { STT = 4, MaHD = 1, NgayTra = "01/02/2023", SoPhong = 1, KhachHang = "Trần Thanh Trúc", TongTien = 670000 });
            items.Add(new Bill() { STT = 1, MaHD = 4, NgayTra = "01/05/2023", SoPhong = 1, KhachHang = "Nguyễn An", TongTien = 1100000 });
            items.Add(new Bill() { STT = 2, MaHD = 3, NgayTra = "01/04/2023", SoPhong = 3, KhachHang = "Minatozaki Sana", TongTien = 2800000 });
            items.Add(new Bill() { STT = 3, MaHD = 2, NgayTra = "01/03/2023", SoPhong = 2, KhachHang = "Sherlock Holmes", TongTien = 1550000 });
            items.Add(new Bill() { STT = 4, MaHD = 1, NgayTra = "01/02/2023", SoPhong = 1, KhachHang = "Trần Thanh Trúc", TongTien = 670000 });
            items.Add(new Bill() { STT = 1, MaHD = 4, NgayTra = "01/05/2023", SoPhong = 1, KhachHang = "Nguyễn An", TongTien = 1100000 });
            items.Add(new Bill() { STT = 2, MaHD = 3, NgayTra = "01/04/2023", SoPhong = 3, KhachHang = "Minatozaki Sana", TongTien = 2800000 });
            items.Add(new Bill() { STT = 3, MaHD = 2, NgayTra = "01/03/2023", SoPhong = 2, KhachHang = "Sherlock Holmes", TongTien = 1550000 });
            items.Add(new Bill() { STT = 4, MaHD = 1, NgayTra = "01/02/2023", SoPhong = 1, KhachHang = "Trần Thanh Trúc", TongTien = 670000 });
            items.Add(new Bill() { STT = 1, MaHD = 4, NgayTra = "01/05/2023", SoPhong = 1, KhachHang = "Nguyễn An", TongTien = 1100000 });
            items.Add(new Bill() { STT = 2, MaHD = 3, NgayTra = "01/04/2023", SoPhong = 3, KhachHang = "Minatozaki Sana", TongTien = 2800000 });
            items.Add(new Bill() { STT = 3, MaHD = 2, NgayTra = "01/03/2023", SoPhong = 2, KhachHang = "Sherlock Holmes", TongTien = 1550000 });
            items.Add(new Bill() { STT = 4, MaHD = 1, NgayTra = "01/02/2023", SoPhong = 1, KhachHang = "Trần Thanh Trúc", TongTien = 670000 });

            //items.Add(new Bill() { STT = 1, MaPTP = 4, MaPhong = "A203", SoNgayThue = 2, SoKhach = 2, DonGia = 400000, TongTien = 800000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt"});
            //items.Add(new Bill() { STT = 2, MaPTP = 4, MaPhong = "A203", SoNgayThue = 2, SoKhach = 2, DonGia = 400000, TongTien = 800000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt" });
            //items.Add(new Bill() { STT = 3, MaPTP = 4, MaPhong = "A203", SoNgayThue = 2, SoKhach = 2, DonGia = 400000, TongTien = 800000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt" });
            //items.Add(new Bill() { STT = 4, MaPTP = 4, MaPhong = "A203", SoNgayThue = 2, SoKhach = 2, DonGia = 400000, TongTien = 800000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt" });
            //items.Add(new Bill() { STT = 5, MaPTP = 4, MaPhong = "A203", SoNgayThue = 2, SoKhach = 2, DonGia = 400000, TongTien = 800000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt" });
            //items.Add(new Bill() { STT = 6, MaPTP = 4, MaPhong = "A203", SoNgayThue = 2, SoKhach = 2, DonGia = 400000, TongTien = 800000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt" });
            //items.Add(new Bill() { STT = 7, MaPTP = 4, MaPhong = "A203", SoNgayThue = 2, SoKhach = 2, DonGia = 400000, TongTien = 800000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt" });
            //items.Add(new Bill() { STT = 8, MaPTP = 4, MaPhong = "A203", SoNgayThue = 2, SoKhach = 2, DonGia = 400000, TongTien = 800000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt" });
            //items.Add(new Bill() { STT = 9, MaPTP = 4, MaPhong = "A203", SoNgayThue = 2, SoKhach = 2, DonGia = 400000, TongTien = 800000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt" });
            //items.Add(new Bill() { STT = 10, MaPTP = 4, MaPhong = "A203", SoNgayThue = 2, SoKhach = 2, DonGia = 400000, TongTien = 800000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt" });
            //items.Add(new Bill() { STT = 11, MaPTP = 4, MaPhong = "A203", SoNgayThue = 2, SoKhach = 2, DonGia = 400000, TongTien = 800000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt" });
            //items.Add(new Bill() { STT = 12, MaPTP = 4, MaPhong = "A203", SoNgayThue = 2, SoKhach = 2, DonGia = 400000, TongTien = 800000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt" });
            //items.Add(new Bill() { STT = 13, MaPTP = 4, MaPhong = "A203", SoNgayThue = 2, SoKhach = 2, DonGia = 400000, TongTien = 800000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt" });
            //items.Add(new Bill() { STT = 14, MaPTP = 4, MaPhong = "A203", SoNgayThue = 2, SoKhach = 2, DonGia = 400000, TongTien = 800000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt" });
            //items.Add(new Bill() { STT = 15, MaPTP = 4, MaPhong = "A203", SoNgayThue = 2, SoKhach = 2, DonGia = 400000, TongTien = 800000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt" });
            //items.Add(new Bill() { STT = 16, MaPTP = 4, MaPhong = "A203", SoNgayThue = 2, SoKhach = 2, DonGia = 400000, TongTien = 800000, DiaChi = "TPHCM", PhuongThuc = "Tiền mặt" });

            lvBill.ItemsSource = items;
        }

        private class Bill
        {
            public int STT { get; set; }
            public int MaPTP { get; set; }
            public string MaPhong { get; set; }
            public int SoNgayThue { get; set; }
            public int SoKhach { get; set; }
            public long DonGia { get; set; }
            public int MaHD { get; set; }
            public string NgayTra { get; set; }
            public int SoPhong { get; set; }
            public string KhachHang { get; set; }
            public long TongTien { get; set; }
            public string DiaChi { get; set; }
            public string PhuongThuc { get; set; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //var KhachHang = new KhachHang()
            //{
               // KhachHang = "001",
                //TenKH = "Nguyễn Văn A",
                //DiaChi = "Hồ Chí Minh"
           // };
            //var conn = new KhachHangDOA();
            //conn.OpenConnection();
            //conn.ExecuteQuery("");
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
