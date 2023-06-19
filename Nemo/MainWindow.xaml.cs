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
using System.Globalization;
using static System.Net.WebRequestMethods;

namespace Nemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PhongView PhongView = new PhongView();
        PhieuThuePhongView PTPView = new PhieuThuePhongView();
        ChiTietPTPView ChiTietPTPView = new ChiTietPTPView();
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelChart();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //load phòng
            Get_PhieuThuePhong_View();
            Get_DanhMucPhong_View();
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

        public void Get_PhieuThuePhong_View()
        {
            var con = new PhieuThuePhongViewDAO();
            PTPView.listPTP = con.GetListPTP();
            PTPView.PhongDaThue = con.getCountPhong()[0];
            PTPView.PhongTrong = con.getCountPhong()[1];
            PTPView.UpdatePaging();
            ListView_PhieuThuePhong.ItemsSource = PTPView.curPTP;
            Page_PhieuThuePhong_text.Text = PTPView.curpage.ToString();

            soPhongConTrong_PTP.Text = $"Số phòng còn trống\n{PTPView.PhongTrong}";
            soPhongDaThue_PTP.Text = $"Số phòng đã thuê\n{PTPView.PhongDaThue}";
        }
        private void Nextpage_PhieuThuePhong_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (PTPView.curpage < PTPView.totalpage) { PTPView.curpage = PTPView.curpage + 1; }
            Page_PhieuThuePhong_text.Text = PTPView.curpage.ToString();
            PTPView.UpdatePaging();
            ListView_PhieuThuePhong.ItemsSource = PTPView.curPTP;
        }

        private void Prevpage_PhieuThuePhong_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (PTPView.curpage > 1) { PTPView.curpage = PTPView.curpage - 1; }
            Page_PhieuThuePhong_text.Text = PTPView.curpage.ToString();
            PTPView.UpdatePaging();
            ListView_PhieuThuePhong.ItemsSource = PTPView.curPTP;
        }

        private void phieuThueBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void thanhToanBtn_Click(object sender, RoutedEventArgs e)
        {
            //MyTabControl.SelectedItem = ThanhToanTabItem;
        }

        private void ListView_PhieuThuePhong_Click(object sender, SelectionChangedEventArgs e)
        {
            var selectedData = (PhieuThuePhong)ListView_PhieuThuePhong.SelectedItem;

            if (selectedData != null)
            {
                ChiTietPTPView.maPTP = selectedData.maPTP;
                ChiTietPTPView.maPhongThue = selectedData.maPhongThue;
                ChiTietPTPView.ngayThue = selectedData.ngayThue;
                ChiTietPTPView.tienThue = selectedData.tienThue;

                TabControl_PhieuThuePhong.SelectedItem = ChiTietPhieuThueTabItem;
                UpdateChiTietPhieuThueData(ChiTietPTPView);
            }
        }
        private void UpdateChiTietPhieuThueData(ChiTietPTPView CT)
        {
            var con = new ChiTietPTPViewDAO();
            MaPTPTextBlock.Text = MaPTPTextBlock_HD.Text =  CT.maPTP.ToString();
            MaPhongTextBlock.Text = MaPhongTextBlock_HD.Text = CT.maPhongThue.ToString();
            NgayTaoTextBlock.Text = NgayTaoTextBlock_HD.Text = CT.ngayThue;
            TienThueTextBlock.Text = TienThueTextBlock_HD.Text = CT.tienThue.ToString("N0") + " VNĐ";

            CT.listChiTietPTP = con.GetListPTP(CT.maPTP);
            CT.UpdatePaging();
            ListView_ChiTietPTP.ItemsSource = CT.curChiTietPTP;
            ListView_ChiTietPTP_HoaDon.ItemsSource = CT.curChiTietPTP;
        }

        private void TabControl_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BackButton_PhieuThuePhong_Click(object sender, RoutedEventArgs e)
        {
            TabControl_PhieuThuePhong.SelectedIndex = 0;
            ChiTietPhieuThueTabItem.Visibility = Visibility.Collapsed;
        }

        private void ThanhToanBtn_PTP_Click(object sender, RoutedEventArgs e)
        {
            TabControl_PhieuThuePhong.SelectedItem = ThanhToanTabItem;
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

        private void TabControl_PhieuThuePhong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TaoPTPBtn_PTP_Click(object sender, RoutedEventArgs e)
        {
            TabControl_PhieuThuePhong.SelectedItem = TaoPTPTabItem;
            DateTime currentDate = DateTime.Now;
            inputNgayThue_PTP.Text = currentDate.ToString("dd'/'MM'/'yyyy");
        }

        private void lapHoaDonBtn_PTP_Click(object sender, RoutedEventArgs e)
        {
            string dinhDanh = DinhDanhTextBox.Text;
            string maptp = MaPTPTextBlock_HD.Text;

            var conn_kh = new KhachHangViewDAO();
            var kh = new KhachHang();
            kh = conn_kh.getMaKhachHangByDinhDanh(dinhDanh);
            if (kh == null)
            {
                errorDinhDanh.Visibility = Visibility;
            }
            else
            {
                errorDinhDanh.Visibility = Visibility.Collapsed;
                var conn_hd = new HoaDonViewDAO();
                int mahd = conn_hd.createHoaDon(int.Parse(kh.MaKH));

                var conn_ptp = new PhieuThuePhongViewDAO();
                conn_ptp.thanhToanPTP(int.Parse(maptp), mahd);

                Get_PhieuThuePhong_View();
                TabControl_PhieuThuePhong.SelectedIndex = 0;
            }
        }

        private void ThemKHBtn_PTP_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = (ComboBoxItem)inputLoaiPhong_PTP.SelectedItem;
            string loaiPhong = selectedComboBoxItem.Content.ToString();

            string soPhong = inputSoPhong_PTP.Text;
            string ngayThue = inputNgayThue_PTP.Text;
            DateTime dt = DateTime.ParseExact(ngayThue, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string ngayThueFormatted = dt.ToString("yyyy-MM-dd");

            string tenKH = inputTenKhach_PTP.Text;
            selectedComboBoxItem = (ComboBoxItem)inputLoaiKhach_PTP.SelectedItem;
            string loaiKH = selectedComboBoxItem.Content.ToString();

            string diaChi = inputDiaChi_PTP.Text;
            selectedComboBoxItem = (ComboBoxItem)inputLoaiDinhDanh_PTP.SelectedItem;
            string loaiDD = selectedComboBoxItem.Content.ToString();
            string soDD = inputSoDinhDanh_PTP.Text;

            var kh = new KhachHang()
            {
                TenKH = tenKH,
                loaiKH = loaiKH,
                DiaChi = diaChi,
                dinhDanh = soDD,
            };
            int loaiPhongInt;
            if (loaiPhong == "Loại A")
                loaiPhongInt = 1;
            else
            {
                if (loaiPhong == "Loại B")
                    loaiPhongInt = 2;
                else loaiPhongInt = 3;
            }

            var conn_kh = new KhachHangViewDAO();
            var conn_phong = new PhongViewDAO();
            var conn_ptp = new PhieuThuePhongViewDAO();

            int soPhongInt;
            if (int.TryParse(soPhong, out soPhongInt))
            {
                string tinhtrang = conn_phong.checkTinhTrang(soPhongInt, loaiPhongInt);
                if (tinhtrang == "Còn trống")
                {
                    if (string.IsNullOrEmpty(tenKH) || 
                        string.IsNullOrEmpty(loaiKH) || 
                        string.IsNullOrEmpty(diaChi) || 
                        string.IsNullOrEmpty(loaiDD) || 
                        string.IsNullOrEmpty(soDD))
                    {
                        errorThieuThongTin_PTP.Visibility = Visibility; 
                        errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        int makh_moiThem = conn_kh.addKhachHang(kh);
                        var ptp = new PhieuThuePhong()
                        {
                            ngayThue = ngayThueFormatted,
                            maPhongThue = Convert.ToInt32(soPhong)
                        };

                        /*System.Windows.MessageBox.Show(ptp.maPhongThue.ToString() + " + " + ptp.ngayThue.ToString());*/

                        int maptp_new = conn_ptp.addPTP(ptp);

                        conn_ptp.addLichSu(maptp_new, makh_moiThem);
                        conn_phong.updateTinhTrang(soPhongInt, maptp_new);
                        int soKhach = conn_ptp.countSoKhachInPTP(maptp_new);

                        errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                        errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;
                        soKhachDaThem_PTP.Text = $"Số khách đã thêm: {soKhach}";
                    }
                }
                else
                {
                    if (tinhtrang == null)
                    {
                        errorSoPhong_PTP.Text = "*Phòng không tồn tại";
                        errorSoPhong_PTP.Visibility = Visibility;
                    }
                    else
                    {
                        if (tinhtrang == "Đang đợi")
                        {
                            int maptp = conn_ptp.getMaPTP(ngayThueFormatted, soPhongInt);
                            int makh_moiThem = conn_kh.addKhachHang(kh);

                            conn_ptp.addLichSu(maptp, makh_moiThem);
                            conn_phong.updateTinhTrang(soPhongInt, maptp);
                            bool checkFull = conn_phong.checkFull(soPhongInt);
                            if(checkFull == true)
                            {
                                errorSoPhong_PTP.Text = "*Phòng đã đầy";
                                errorSoPhong_PTP.Visibility = Visibility;
                            }
                            int soKhach = conn_ptp.countSoKhachInPTP(maptp);

                            errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                            errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;
                            soKhachDaThem_PTP.Text = $"Số khách đã thêm: {soKhach}";
                        }
                        else
                        {
                            errorSoPhong_PTP.Text = "*Phòng đã đầy";
                            errorSoPhong_PTP.Visibility = Visibility;
                        }
                    }
                }
            }
            else
            {
                errorSoPhong_PTP.Text = "*Phòng không tồn tại";
                errorSoPhong_PTP.Visibility = Visibility;
            }
        }

        private void DoneBtn_PTP_Click(object sender, RoutedEventArgs e)
        {
            errorSoPhong_PTP.Visibility = Visibility.Collapsed;
            errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;

            string soPhong = inputSoPhong_PTP.Text;
            int soPhongInt;
            int.TryParse(soPhong, out soPhongInt);

            var conn_phong = new PhongViewDAO();
            conn_phong.updateTinhTrang(soPhongInt, 0, true);

            TabControl_PhieuThuePhong.SelectedIndex = 0;
        }
    }
}
