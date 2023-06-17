using Nemo.DTO;
using Nemo.DAO;
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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using ControlzEx.Controls;
using Button = System.Windows.Controls.Button;
using ListViewItem = System.Windows.Controls.ListViewItem;

namespace Nemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PhongView PhongView = new PhongView();
        HoaDonView HoaDonView = new HoaDonView();
        ChiTietHoaDonView ChiTietHoaDonView = new ChiTietHoaDonView();
        ChiTietPTPHDView ChiTietPTPHDView = new ChiTietPTPHDView();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelChart();
        }

        class PhieuThue
        {
            public int MaPT { get; set; }
            public DateTime NgayTao { get; set; }
            public string Phong { get; set; }
            public int SoKhach { get; set; }
            public float TienThue { get; set; }
        }

        List<PhieuThue> danhSachPhieuThue = new List<PhieuThue>()
        {
            new PhieuThue() { MaPT = 1, NgayTao = new DateTime(2023, 6, 9), Phong = "Phòng A", SoKhach = 2, TienThue = 100 },
            new PhieuThue() { MaPT = 2, NgayTao = new DateTime(2023, 6, 10), Phong = "Phòng B", SoKhach = 3, TienThue = 150 },
        };
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //load phòng
            Get_DanhMucPhong_View();
            Get_HoaDon_View();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            PopupMenu.IsOpen = !PopupMenu.IsOpen;
        }

        
        private void ChangeButton_QD(object sender, RoutedEventArgs e)
        {
            NumOfTypes.IsEnabled = true;
            NumOfTypes.Focus();
            NumOfCustomers.IsEnabled = true;
            PhuThu.IsEnabled = true;
            HeSoKNN.IsEnabled = true;

        }

        private void SetQD(object sender, RoutedEventArgs e)
        {
            NumOfTypes.IsEnabled = false;
            NumOfCustomers.IsEnabled = false;
            PhuThu.IsEnabled = false;
            HeSoKNN.IsEnabled = false;
        }

        private void TuyChon_Btn_Click(object sender, RoutedEventArgs e)
        {
            //get index of sender button
            Button button = sender as Button;
            DependencyObject parent = VisualTreeHelper.GetParent(button);
            while (!(parent is ListViewItem))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            ListViewItem item = parent as ListViewItem;
            int index = ListView_DanhMucPhong.Items.IndexOf(item.DataContext);
            if (PhongView.ListPhong[index].tuychon=="Chỉnh sửa")
            {
                Debug.WriteLine("Chỉnh sửa");
            }
            else
            {
                Debug.WriteLine("Thuê Phòng");
            }
        }
        public void Get_DanhMucPhong_View()
        {
            var con = new PhongViewDAO();
            PhongView.ListPhong = con.GetListPhong();
            PhongView.UpdatePaging();
            ListView_DanhMucPhong.ItemsSource = PhongView.CurPhong;
            Page_DanhMucPhong_text.Text = PhongView.curpage.ToString();

        }
        private void Nextpage_DanhMucPhong_Btn_Click(object sender, RoutedEventArgs e)
        {
            if  (PhongView.curpage < PhongView.totalpage) { PhongView.curpage = PhongView.curpage + 1; }
            Page_DanhMucPhong_text.Text = PhongView.curpage.ToString();
            PhongView.UpdatePaging();
            ListView_DanhMucPhong.ItemsSource = PhongView.CurPhong;
        }

        private void Prevpage_DanhMucPhong_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void phieuThueBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void thanhToanBtn_Click(object sender, RoutedEventArgs e)
        {
            //MyTabControl.SelectedItem = ThanhToanTabItem;
        }

        private void PhieuThuePhong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedData = (PhieuThue)MyListView.SelectedItem;

            if (selectedData != null)
            {
                int maPTP = selectedData.MaPT;
                string maPhong = selectedData.Phong;
                DateTime ngaytao = selectedData.NgayTao;
                float tienThue = selectedData.TienThue;
                int soKhach = selectedData.SoKhach;

               // MyTabControl.SelectedItem = ChiTietPhieuThueTabItem;
                UpdateChiTietPhieuThueData(maPTP, maPhong, soKhach, ngaytao, tienThue);
            }
        }
        private void UpdateChiTietPhieuThueData(int maPTP, string maPhong, int soKhach, DateTime ngayTao, float tienThue)
        {
            MaPTPTextBlock.Text = MaPTPTextBlock_HD.Text =  maPTP.ToString();
            MaPhongTextBlock.Text = MaPhongTextBlock_HD.Text = maPhong;
            /*SoKhachTextBlock.Text = soKhach.ToString();*/
            NgayTaoTextBlock.Text = NgayTaoTextBlock_HD.Text = ngayTao.ToString();
            TienThueTextBlock.Text = TienThueTextBlock_HD.Text = tienThue.ToString();
        }

        private void TabControl_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
           MyTabControl.SelectedIndex =1;
            ChiTietPhieuThueTabItem.Visibility = Visibility.Collapsed ;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedItem != null)
            {
                ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
                comboBox.Text = selectedItem.Content.ToString();
                Console.WriteLine("Selected item: {0}", selectedItem.Content);
            }
        }

        private void TaoPTPBtn_Click(object sender, RoutedEventArgs e)
        {
           MyTabControl.SelectedItem = TaoPTPTabItem;
        }

        private void Themhong_Btn_click(object sender, RoutedEventArgs e)
        {
            var screen = new ThemPhong();
            if (screen.ShowDialog() == true)
            {
                // var sv = (Book)screen.NewBook.Clone();
                // sv.Img = new BitmapImage(new Uri(_imgList[0], UriKind.Relative));
                Debug.WriteLine("mở oke");
            }
            else
            {
                Title = "KHONG CO DU LIEU";
            }
        }

        public void Get_HoaDon_View()
        {
            var con = new HoaDonViewDAO();
            HoaDonView.ListHoaDon = con.GetListHoaDon();
            HoaDonView.UpdatePaging();
            ListView_HoaDon.ItemsSource = HoaDonView.CurHoaDon;
            Page_HoaDon_text.Text = HoaDonView.curpage.ToString();
        }

        private void Nextpage_HoaDon_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (HoaDonView.curpage < HoaDonView.totalpage) { HoaDonView.curpage = HoaDonView.curpage + 1; }
            Page_HoaDon_text.Text = HoaDonView.curpage.ToString();
            HoaDonView.UpdatePaging();
            ListView_HoaDon.ItemsSource = HoaDonView.CurHoaDon;
        }

        private void Prevpage_HoaDon_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (HoaDonView.curpage > 1) { HoaDonView.curpage = HoaDonView.curpage - 1; }
            Page_HoaDon_text.Text = HoaDonView.curpage.ToString();
            HoaDonView.UpdatePaging();
            ListView_HoaDon.ItemsSource = HoaDonView.CurHoaDon;
        }

        private void HoaDon_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer_HoaDon.ScrollToVerticalOffset(ScrollViewer_HoaDon.VerticalOffset - e.Delta / 3);
        }

        private void HoaDon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = (HoaDon) ListView_HoaDon.SelectedItem;
            if (row != null)
            {
                ChiTietHoaDonView.mahoadon = row.mahoadon;
                ChiTietHoaDonView.ngaythanhtoan = row.ngaythanhtoan;
                ChiTietHoaDonView.khachhang = row.khachhang;
                ChiTietHoaDonView.tongtien = row.tongtien;
                ChiTietHoaDonView.sophongthanhtoan = row.sophongthanhtoan;
                Get_ChiTietHoaDon_View(ChiTietHoaDonView.mahoadon, 3, (float)1.25, (float)1.5);
                TabControl_HoaDon.SelectedIndex = 1;
            }
            ListView_HoaDon.SelectedItem = null;
        }

        private void BackBtn_ChiTietHoaDon_Click(object sender, RoutedEventArgs e)
        {
            TabControl_HoaDon.SelectedIndex = 0;
        }

        public void Get_ChiTietHoaDon_View(int mahd, int sokhachquydinh, float phuthusokhach, float tilekhachnuocngoai)
        {
            var con = new ChiTietHoaDonViewDAO();

            TextBlock_MaHoaDon_ChiTietHoaDon.Text = ChiTietHoaDonView.mahoadon.ToString();
            TextBlock_NgayThanhToan_ChiTietHoaDon.Text = ChiTietHoaDonView.ngaythanhtoan.ToString("dd'/'MM'/'yyyy");
            TextBlock_KhachHang_ChiTietHoaDon.Text = ChiTietHoaDonView.khachhang;
            TextBlock_TongTien_ChiTietHoaDon.Text = ChiTietHoaDonView.tongtien.ToString("N0");
            TextBlock_SoPhongThanhToan_ChiTietHoaDon.Text = ChiTietHoaDonView.sophongthanhtoan.ToString();

            ChiTietHoaDonView.ListChiTietHoaDon = con.GetListChiTietHoaDon(mahd, sokhachquydinh);
            ChiTietHoaDonView.UpdatePaging();
            ListView_ChiTietHoaDon.ItemsSource = ChiTietHoaDonView.CurChiTietHoaDon;
            Page_ChiTietHoaDon_text.Text = ChiTietHoaDonView.curpage.ToString();
        }
        private void Nextpage_ChiTietHoaDon_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (ChiTietHoaDonView.curpage < ChiTietHoaDonView.totalpage) { ChiTietHoaDonView.curpage = ChiTietHoaDonView.curpage + 1; }
            Page_ChiTietHoaDon_text.Text = ChiTietHoaDonView.curpage.ToString();
            ChiTietHoaDonView.UpdatePaging();
            ListView_ChiTietHoaDon.ItemsSource = ChiTietHoaDonView.CurChiTietHoaDon;
        }

        private void Prevpage_ChiTietHoaDon_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (ChiTietHoaDonView.curpage > 1) { ChiTietHoaDonView.curpage = ChiTietHoaDonView.curpage - 1; }
            Page_ChiTietHoaDon_text.Text = ChiTietHoaDonView.curpage.ToString();
            ChiTietHoaDonView.UpdatePaging();
            ListView_ChiTietHoaDon.ItemsSource = ChiTietHoaDonView.CurChiTietHoaDon;
        }

        private void ChiTietHoaDon_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer_ChiTietHoaDon.ScrollToVerticalOffset(ScrollViewer_ChiTietHoaDon.VerticalOffset - e.Delta / 3);
        }

        private void ChiTietHoaDon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var row = (ChiTietHoaDon)ListView_ChiTietHoaDon.SelectedItem;
            if (row != null)
            {
                ChiTietPTPHDView.maptp = row.maptp;
                ChiTietPTPHDView.maphong = row.maphong;
                ChiTietPTPHDView.ghichu = row.ghichu;
                Get_ChiTietPTP_HD_View(ChiTietPTPHDView.maptp);
                TabControl_HoaDon.SelectedIndex = 2;
            }
            ListView_ChiTietHoaDon.SelectedItem = null;
        }

        private void BackBtn_ChiTietPTP_HD_Click(object sender, RoutedEventArgs e)
        {
            TabControl_HoaDon.SelectedIndex = 1;
        }

        public void Get_ChiTietPTP_HD_View(int maptp)
        {
            var con = new ChiTietPTPHDViewDAO();
            var dt_NgayThueNgayTra = con.GetNgayThueNgayTra(maptp);

            ChiTietPTPHDView.ngaythue = (DateTime)dt_NgayThueNgayTra.Rows[0]["ngaythue"];
            ChiTietPTPHDView.ngaytra = (DateTime)dt_NgayThueNgayTra.Rows[0]["ngaytra"];

            TextBlock_MaPTP_ChiTietPTP_HD.Text = ChiTietPTPHDView.maptp.ToString();
            TextBlock_MaPhong_ChiTietPTP_HD.Text = ChiTietPTPHDView.maphong.ToString();
            TextBlock_NgayThue_ChiTietPTP_HD.Text = ChiTietPTPHDView.ngaythue.ToString("dd'/'MM'/'yyyy");
            TextBlock_NgayTra_ChiTietPTP_HD.Text = ChiTietPTPHDView.ngaytra.ToString("dd'/'MM'/'yyyy");
            TextBlock_GhiChu_ChiTietPTP_HD.Text = ChiTietPTPHDView.ghichu;

            ChiTietPTPHDView.ListChiTietPTPHD = con.GetListChiTietPTPHD(maptp);
            ChiTietPTPHDView.UpdatePaging();
            ListView_ChiTietPTP_HD.ItemsSource = ChiTietPTPHDView.CurChiTietPTPHD;
            Page_ChiTietPTP_HD_text.Text = ChiTietPTPHDView.curpage.ToString();
        }
        private void Nextpage_ChiTietPTP_HD_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (ChiTietPTPHDView.curpage < ChiTietPTPHDView.totalpage) { ChiTietPTPHDView.curpage = ChiTietPTPHDView.curpage + 1; }
            Page_ChiTietPTP_HD_text.Text = ChiTietPTPHDView.curpage.ToString();
            ChiTietPTPHDView.UpdatePaging();
            ListView_ChiTietPTP_HD.ItemsSource = ChiTietPTPHDView.CurChiTietPTPHD;
        }

        private void Prevpage_ChiTietPTP_HD_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (ChiTietPTPHDView.curpage > 1) { ChiTietPTPHDView.curpage = ChiTietPTPHDView.curpage - 1; }
            Page_ChiTietPTP_HD_text.Text = ChiTietPTPHDView.curpage.ToString();
            ChiTietPTPHDView.UpdatePaging();
            ListView_ChiTietPTP_HD.ItemsSource = ChiTietPTPHDView.CurChiTietPTPHD;
        }

        private void ChiTietPTP_HD_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer_ChiTietPTP_HD.ScrollToVerticalOffset(ScrollViewer_ChiTietPTP_HD.VerticalOffset - e.Delta / 3);
        }
    }
}
