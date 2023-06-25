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
using LiveChartsCore.Measure;
using System.Windows;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Configuration;
using Nemo.GUI;

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

        DoanhThuView DoanhThuView = new DoanhThuView();
        MatDoView MatDoView = new MatDoView();
        QuyDinhView QuyDinhView = new QuyDinhView();
        ViewModelChart Chart = new ViewModelChart();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Chart;
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
            var username = ConfigurationManager.AppSettings["LastUsername"];
            Admin_textblock.Text = username;
            //if get quyèn
            DAtBtn.Visibility = Visibility.Collapsed;
            //update
            Get_HoaDon_View();
            Get_DoanhThu_View();
            Get_QuyDinh_View();
            
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
            var screen = new Suaphong(PhongView.CurPhong[index]);
            if (screen.ShowDialog() == true)
            {
                var phongmoi = (Phong)screen.PhongMoi.Clone();
                var conn = new PhongViewDAO();
                conn.Suaphong(phongmoi);
                Get_DanhMucPhong_View();
            }

            }
        public void Get_DanhMucPhong_View()
        {
            PhongViewDAO con = new PhongViewDAO();
            PhongView.ListPhong = con.GetListPhong();
            PhongView.Tinhtrangphong = con.GetListTinhTrang();
            PhongView.ResetCurlist();
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
            if (PhongView.curpage >0) { PhongView.curpage = PhongView.curpage - 1; }
            Page_DanhMucPhong_text.Text = PhongView.curpage.ToString();
            PhongView.UpdatePaging();
            ListView_DanhMucPhong.ItemsSource = PhongView.CurPhong;
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
                var phongmoi = (Phong)screen.PhongMoi.Clone();
                var conn = new PhongViewDAO();
                conn.ThemPhongMoi(phongmoi);
                Get_DanhMucPhong_View();
            }
            
        }

        private void Dangxuat_Btn_click(object sender, RoutedEventArgs e)
            {
            var screen = new DangNhap();
            screen.Show();
            this.Close();
            }

        private void Doimatkhau_btn_CLick(object sender, RoutedEventArgs e)
        {
            var screen = new Doimatkhau();
            screen.ShowDialog();

        }

        private void Timkiem_DanhMucPhong_TextChange(object sender, TextChangedEventArgs e)
        {
            var keyword = Timkiem_DanhMucPhong_Textbox.Text;
            var sub = new ObservableCollection<Phong>();
            foreach (var p in PhongView.ListPhong)
            {
                var maphong=p.maphong.ToString();
                var maloaiphong = p.maloaiphong.ToString();
                if (maphong.Contains(keyword))
                {
                    sub.Add(p);
                }else
                if (maloaiphong.Contains(keyword))
                {
                    sub.Add(p);
                }else
                if (p.tinhtrang.Contains(keyword))
                {
                    sub.Add(p);
                }
            }
            PhongView.CurListPhong = sub;
            PhongView.curpage = 1;
            Page_DanhMucPhong_text.Text = PhongView.curpage.ToString();
            PhongView.UpdatePaging();
            ListView_DanhMucPhong.ItemsSource = PhongView.CurPhong;

        }

        private void PhongThue_DanhMucPhong_Btn_Click(object sender, RoutedEventArgs e)
        {
            PhongViewDAO con = new PhongViewDAO();
            PhongView.CurListPhong = con.GetListPhongLoai("Đã thuê");
            PhongView.UpdatePaging();
            ListView_DanhMucPhong.ItemsSource = PhongView.CurPhong;
            Page_DanhMucPhong_text.Text = PhongView.curpage.ToString();
        }

        private void ConTrong_DanhMucPhong_Btn_Click(object sender, RoutedEventArgs e)
        {
            PhongViewDAO con = new PhongViewDAO();
            PhongView.CurListPhong = con.GetListPhongLoai("Còn trống");
            PhongView.UpdatePaging();
            ListView_DanhMucPhong.ItemsSource = PhongView.CurPhong;
            Page_DanhMucPhong_text.Text = PhongView.curpage.ToString();
        }

        private void Dangdoi_DanhMucPhong_Btn_Click(object sender, RoutedEventArgs e)
        {
            PhongViewDAO con = new PhongViewDAO();
            PhongView.CurListPhong = con.GetListPhongLoai("Đang đợi");
            PhongView.UpdatePaging();
            ListView_DanhMucPhong.ItemsSource = PhongView.CurPhong;
            Page_DanhMucPhong_text.Text = PhongView.curpage.ToString();
        }

        private void Dangsuachua_DanhMucPhong_Btn_Click(object sender, RoutedEventArgs e)
        {
            PhongViewDAO con = new PhongViewDAO();
            PhongView.CurListPhong = con.GetListPhongLoai("Đang sửa chữa");
            PhongView.UpdatePaging();
            ListView_DanhMucPhong.ItemsSource = PhongView.CurPhong;
            Page_DanhMucPhong_text.Text = PhongView.curpage.ToString();

            }

        public void Get_HoaDon_View()
        {
            var con = new HoaDonViewDAO();
            HoaDonView.ListHoaDon = con.GetListHoaDon();
            HoaDonView.ResetCurlist();
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

        }
        //Quy Dinh
        private void Get_QuyDinh_View()
        {
            QuyDinhView.getQuyDinh();
            Text_QuyDinh_ma.Text = QuyDinhView.quyDinh.maQD.ToString();
            NumOfTypes.Text = QuyDinhView.quyDinh.soLoaiPhong.ToString();
            NumOfCustomers.Text = QuyDinhView.quyDinh.soLuongKhachToiDa.ToString();
            PhuThu.Text = QuyDinhView.quyDinh.tiLePhuThu.ToString();
            HeSoKNN.Text = QuyDinhView.quyDinh.heSoKhachNN.ToString();
        }
        private void ChangeButton_QD(object sender, RoutedEventArgs e)
        {
            QuyDinhView.addQuyDinh(PhuThu.Text, HeSoKNN.Text, NumOfTypes.Text, NumOfCustomers.Text);
            MessageBox.Show("Thay đổi thành công");
            Get_QuyDinh_View();
            }

        private void SetQD(object sender, RoutedEventArgs e)
        {
            Get_QuyDinh_View();
        }
        //Bao cao
        //Doanh Thu
        private void Get_DoanhThu_View()
        {
            DoanhThuView.setListYearExist();
            Cbb_DoanhThu_year1.ItemsSource = DoanhThuView.listYearExist;
            Cbb_DoanhThu_year2.ItemsSource = DoanhThuView.listYearExist;
            Cbb_MatDo_Year1.ItemsSource = DoanhThuView.listYearExist;
            Cbb_MatDo_year2.ItemsSource = DoanhThuView.listYearExist;

        }

        private void Cbb_DoanhThu_year1_SelectionChanged(object sender, SelectionChangedEventArgs e)
                {
            var year = Cbb_DoanhThu_year1.SelectedItem;
            if (year == null) return;
            DoanhThuView.year1 = Convert.ToInt16(year);
            Cbb_DoanhThu_month.ItemsSource = DoanhThuView.getListMonthExist(DoanhThuView.year1);
        }

        private void Cbb_DoanhThu_month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var month = Cbb_DoanhThu_month.SelectedItem;
            if (month == null) return;
            DoanhThuView.month = Convert.ToInt16(month);
            DoanhThuView.settinhDoanhThuThangTheoLoai(DoanhThuView.year1, DoanhThuView.month);
            ListView_DoanhThu.ItemsSource = DoanhThuView.tinhDoanhThuThangTheoLoai;
            Text_tongdoanhthu.Text = DoanhThuView.tinhTongdoanhThuThang().ToString();
            Chart.Chart_DoanhThuThang =Chart.create_chart_DoanhThuThang(DoanhThuView.tinhDoanhThuThangTheoLoai);
            

            //DataContext = Chart;
        }
        private void Cbb_DoanhThu_year2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var year = Cbb_DoanhThu_year2.SelectedItem;
            if (year == null) return;
            DoanhThuView.year2 = Convert.ToInt16(year);
            Chart.create_chart_DoanhThuNam(DoanhThuView.year2);
        }

        private void Cbb_MatDo_Year1_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
            var year = Cbb_MatDo_Year1.SelectedItem;
            if (year == null) return;
            MatDoView.year = Convert.ToInt16(year);
            Cbb_MatDo_month.ItemsSource = DoanhThuView.getListMonthExist(MatDoView.year);
        }

        private void Cbb_MatDo_month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var month = Cbb_MatDo_month.SelectedItem;
            if (month == null) return;
            MatDoView.month = Convert.ToInt16(month);
            dataGrid_MatDo.ItemsSource = MatDoView.setMatDoThang();
        }

        private void Cbb_MatDo_year2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var year = Cbb_MatDo_year2.SelectedItem;
            if (year == null) return;
            MatDoView.year2 = Convert.ToInt16(year);
            Chart.create_chart_MatDoNam(MatDoView.year2);
            var x = Chart.Chart_MatDoNam;
        }


    }
}
