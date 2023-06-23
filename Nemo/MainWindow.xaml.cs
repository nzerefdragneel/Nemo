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
using CheckBox = System.Windows.Controls.CheckBox;
using Button = System.Windows.Controls.Button;
using ListViewItem = System.Windows.Controls.ListViewItem;
using System.Globalization;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Nemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        PhongView PhongView = new PhongView();
        PhieuThuePhongView PTPView = new PhieuThuePhongView();
        ChiTietPTPView ChiTietPTPView = new ChiTietPTPView();

        PhieuThuePhongView PTPView_ThanhToan = new PhieuThuePhongView();
        List<int> selectedMaPTPList = new List<int>();

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
                ChiTietPTPView.ngayTra = selectedData.ngayTra;

                DateTime date_ngayThue = DateTime.Parse(selectedData.ngayThue);
                DateTime currentDate = DateTime.Now;
                bool reserve = false;
                if (DateTime.Compare(currentDate, date_ngayThue) < 0)
                    reserve = true;

                TabControl_PhieuThuePhong.SelectedItem = ChiTietPhieuThueTabItem;
                UpdateChiTietPhieuThueData(ChiTietPTPView, reserve);
            }
        }
        private void UpdateChiTietPhieuThueData(ChiTietPTPView CT, bool reserve = false)
        {
            var con = new ChiTietPTPViewDAO();
            MaPTPTextBlock.Text = MaPTPTextBlock_HD.Text =  CT.maPTP.ToString();
            MaPhongTextBlock.Text = MaPhongTextBlock_HD.Text = CT.maPhongThue.ToString();
            NgayTaoTextBlock.Text = NgayTaoTextBlock_HD.Text = CT.ngayThue;
            NgayTraTextBlock.Text = CT.ngayTra;
            TienThueTextBlock.Text = TienThueTextBlock_HD.Text = CT.tienThue.ToString("N0") + " VNĐ";

            if (reserve == true)
            {
                DatTruocTextBlock.Visibility = Visibility;
                NdungTienTextBlock.Visibility = Visibility.Collapsed;
                thanhToanBtn.Visibility = Visibility.Collapsed;
                NhanPhong_PTP.Visibility = Visibility;
                HuyDatTruocPhong_PTP.Visibility = Visibility;
            }
            else
            {
                DatTruocTextBlock.Visibility = Visibility.Collapsed;
                NdungTienTextBlock.Visibility = Visibility;
                thanhToanBtn.Visibility = Visibility;
                NhanPhong_PTP.Visibility = Visibility.Collapsed;
                HuyDatTruocPhong_PTP.Visibility = Visibility.Collapsed;
            }

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
            if (TabControl_PhieuThuePhong.SelectedItem == ThanhToanTabItem || TabControl_PhieuThuePhong.SelectedItem == ChiTietPhieuThueTabItem)
            {
                TabControl_PhieuThuePhong.SelectedIndex -= 1;
            }
            else
            {
                if (TabControl_PhieuThuePhong.SelectedItem == ThanhToanNhieuTabItem || TabControl_PhieuThuePhong.SelectedItem == TaoPTPTabItem)
                {
                    TabControl_PhieuThuePhong.SelectedIndex = 0;
                }
            }

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
                conn_ptp.updateTinhTrangSauThanhToan(int.Parse(maptp));

                Get_PhieuThuePhong_View();
                DinhDanhTextBox.Text = string.Empty;
                TabControl_PhieuThuePhong.SelectedIndex = 0;
            }
        }

        private void ThemKHBtn_PTP_Click(object sender, RoutedEventArgs e)
        {
            /*ComboBoxItem selectedComboBoxItem = (ComboBoxItem)inputLoaiPhong_PTP.SelectedItem;
            string loaiPhong = selectedComboBoxItem.Content.ToString();

            string soPhong = inputSoPhong_PTP.Text;
            string ngayThue = inputNgayThue_PTP.Text;
            string ngayTra = inputNgayTra_PTP.Text;

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
            if (int.TryParse(soPhong, out soPhongInt)) //check số phòng hợp lệ
            {
                string tinhtrang = conn_phong.checkTinhTrang(soPhongInt, loaiPhongInt);
                if (tinhtrang == "Còn trống")
                {
                    string dateFormat = "dd/MM/yyyy";
                    DateTime date;

                    if (string.IsNullOrEmpty(ngayThue) || 
                        DateTime.TryParseExact(ngayThue, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date) == false)
                    {
                        errorNgayThue_PTP.Visibility = Visibility;
                        errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(ngayTra) ||
                            DateTime.TryParseExact(ngayTra, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date) == false ||
                            DateTime.ParseExact(ngayThue, "dd/MM/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(ngayTra, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                        {
                            errorNgayTra_PTP.Visibility = Visibility;
                            errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(tenKH) ||
                                string.IsNullOrEmpty(loaiKH) ||
                                string.IsNullOrEmpty(diaChi) ||
                                string.IsNullOrEmpty(loaiDD) ||
                                string.IsNullOrEmpty(soDD)) //check thông tin khách hàng trống
                            {
                                errorThieuThongTin_PTP.Visibility = Visibility;
                                errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                                errorNgayThue_PTP.Visibility = Visibility.Collapsed;
                                errorNgayTra_PTP.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                string ngayThueFormatted = DateTime.ParseExact(ngayThue, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                                string ngayTraFormatted = DateTime.ParseExact(ngayTra, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

                                int makh_moiThem = conn_kh.addKhachHang(kh);
                                var ptp = new PhieuThuePhong()
                                {
                                    ngayThue = ngayThueFormatted,
                                    ngayTra = ngayTraFormatted,
                                    maPhongThue = Convert.ToInt32(soPhong)
                                };

                                *//*System.Windows.MessageBox.Show(ptp.maPhongThue.ToString() + " + " + ptp.ngayThue.ToString());*//*

                                int maptp_new = conn_ptp.addPTP(ptp);
                                DateTime date_ngayThue = DateTime.ParseExact(ngayThue, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                DateTime currentDate = DateTime.Now;
                                bool reserve = false;
                                if (currentDate < date_ngayThue)
                                    reserve = true;

                                conn_ptp.addLichSu(maptp_new, makh_moiThem);
                                conn_phong.updateTinhTrang(soPhongInt, maptp_new, false, reserve);
                                int soKhach = conn_ptp.countSoKhachInPTP(maptp_new);

                                errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                                errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;
                                errorNgayThue_PTP.Visibility = Visibility.Collapsed;
                                errorNgayTra_PTP.Visibility = Visibility.Collapsed;
                                soKhachDaThem_PTP.Text = $"Số khách đã thêm: {soKhach}";
                            }
                        }
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
                        if (tinhtrang == "Đang xử lí")
                        {
                            string ngayThueFormatted = DateTime.ParseExact(ngayThue, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                            string ngayTraFormatted = DateTime.ParseExact(ngayTra, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

                            int maptp = conn_ptp.getMaPTP(ngayThueFormatted, soPhongInt);
                            int makh_moiThem = conn_kh.addKhachHang(kh);

                            conn_ptp.addLichSu(maptp, makh_moiThem);
                            conn_phong.updateTinhTrang(soPhongInt, maptp);
                            bool checkFull = conn_phong.checkFull(soPhongInt);
                            if (checkFull == true)
                            {
                                errorSoPhong_PTP.Text = "*Phòng đã đầy";
                                errorSoPhong_PTP.Visibility = Visibility;
                            }
                            int soKhach = conn_ptp.countSoKhachInPTP(maptp);

                            errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                            errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;
                            errorNgayThue_PTP.Visibility = Visibility.Collapsed;
                            errorNgayTra_PTP.Visibility = Visibility.Collapsed;

                            soKhachDaThem_PTP.Text = $"Số khách đã thêm: {soKhach}";
                        }
                        else
                        {
                            if (tinhtrang == "Đã thuê")
                            {
                                //thông báo đầy với đặt trực tiếp
                                if (DateTime.Now.Date == DateTime.ParseExact(ngayThue, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                                {
                                    errorSoPhong_PTP.Text = "*Phòng đã đầy";
                                    errorSoPhong_PTP.Visibility = Visibility;

                                    errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;
                                    errorNgayThue_PTP.Visibility = Visibility.Collapsed;
                                    errorNgayTra_PTP.Visibility = Visibility.Collapsed;
                                }
                                else
                                {
                                    // check đặt trước
                                    string ngayTraDuKien = conn_ptp.getNgayTra(soPhongInt);
                                    if (ngayTraDuKien != null) // phòng chưa cho đặt trước
                                    {
                                        DateTime date_ngayTraDuKien = DateTime.Parse(ngayTraDuKien);
                                        DateTime date_ngayThue = DateTime.ParseExact(ngayThue, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        int comparison = DateTime.Compare(date_ngayTraDuKien, date_ngayThue);

                                        if (comparison < 0) // được đặt trước
                                        {
                                            string dateFormat = "dd/MM/yyyy";
                                            DateTime date;

                                            if (string.IsNullOrEmpty(ngayThue) ||
                                                DateTime.TryParseExact(ngayThue, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date) == false)
                                            {
                                                errorNgayThue_PTP.Visibility = Visibility;
                                                errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(ngayTra) ||
                                                    DateTime.TryParseExact(ngayTra, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date) == false ||
                                                    DateTime.ParseExact(ngayThue, "dd/MM/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(ngayTra, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                                                {
                                                    errorNgayTra_PTP.Visibility = Visibility;
                                                    errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                                                }
                                                else
                                                {
                                                    if (string.IsNullOrEmpty(tenKH) ||
                                                        string.IsNullOrEmpty(loaiKH) ||
                                                        string.IsNullOrEmpty(diaChi) ||
                                                        string.IsNullOrEmpty(loaiDD) ||
                                                        string.IsNullOrEmpty(soDD)) //check thông tin khách hàng trống
                                                    {
                                                        errorThieuThongTin_PTP.Visibility = Visibility;
                                                        errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                                                    }
                                                    else
                                                    {
                                                        string ngayThueFormatted = DateTime.ParseExact(ngayThue, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                                                        string ngayTraFormatted = DateTime.ParseExact(ngayTra, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

                                                        int makh_moiThem = conn_kh.addKhachHang(kh);
                                                        var ptp = new PhieuThuePhong()
                                                        {
                                                            ngayThue = ngayThueFormatted,
                                                            ngayTra = ngayTraFormatted,
                                                            maPhongThue = Convert.ToInt32(soPhong)
                                                        };

                                                        int maptp_new = conn_ptp.addPTP(ptp);

                                                        conn_ptp.addLichSu(maptp_new, makh_moiThem);
                                                        conn_phong.updateTinhTrang(soPhongInt, maptp_new);
                                                        int soKhach = conn_ptp.countSoKhachInPTP(maptp_new);

                                                        errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                                                        errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;
                                                        errorNgayThue_PTP.Visibility = Visibility.Collapsed;
                                                        errorNgayTra_PTP.Visibility = Visibility.Collapsed;
                                                        soKhachDaThem_PTP.Text = $"Số khách đã thêm: {soKhach}";
                                                    }

                                                }
                                            }
                                        }
                                        else
                                        {
                                            errorSoPhong_PTP.Text = "*Phòng đã bận";
                                            errorSoPhong_PTP.Visibility = Visibility;

                                            errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;
                                            errorNgayThue_PTP.Visibility = Visibility.Collapsed;
                                            errorNgayTra_PTP.Visibility = Visibility.Collapsed;
                                        }
                                    }
                                    else
                                    {
                                        errorSoPhong_PTP.Text = "*Phòng đã bận";
                                        errorSoPhong_PTP.Visibility = Visibility;

                                        errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;
                                        errorNgayThue_PTP.Visibility = Visibility.Collapsed;
                                        errorNgayTra_PTP.Visibility = Visibility.Collapsed;
                                    }
                                }
                            }
                            else
                            {
                                errorSoPhong_PTP.Text = "*Phòng đã bận";
                                errorSoPhong_PTP.Visibility = Visibility;

                                errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;
                                errorNgayThue_PTP.Visibility = Visibility.Collapsed;
                                errorNgayTra_PTP.Visibility = Visibility.Collapsed;
                            }
                        }
                    }
                }
            }
            else
            {
                errorSoPhong_PTP.Text = "*Phòng không tồn tại";
                errorSoPhong_PTP.Visibility = Visibility;
            }*/
            string ngayThue = inputNgayThue_PTP.Text;
            string ngayTra = inputNgayTra_PTP.Text;

            string dateFormat = "dd/MM/yyyy";
            DateTime date;

            // check input ngày thuê và ngày trả hợp lệ
            if (string.IsNullOrEmpty(ngayThue) ||
                        DateTime.TryParseExact(ngayThue, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date) == false)
            {
                errorNgayThue_PTP.Visibility = Visibility;
            }
            else
            {
                if (string.IsNullOrEmpty(ngayTra) ||
                    DateTime.TryParseExact(ngayTra, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date) == false ||
                    DateTime.ParseExact(ngayThue, "dd/MM/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(ngayTra, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    errorNgayThue_PTP.Visibility = Visibility.Collapsed;
                    errorNgayTra_PTP.Visibility = Visibility;
                }
                else
                {
                    errorNgayTra_PTP.Visibility = Visibility.Collapsed;
                    errorNgayThue_PTP.Visibility = Visibility.Collapsed;

                    DateTime ngayThueDate = DateTime.ParseExact(ngayThue, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime ngayTraDate = DateTime.ParseExact(ngayTra, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime currentDate = DateTime.Now.Date;

                    string tinhHuong = "";
                    if (currentDate == ngayThueDate)
                    {
                        tinhHuong = "Thuê trực tiếp";
                    }
                    else
                    {
                        if (currentDate < ngayThueDate)
                        {
                            tinhHuong = "Đặt phòng";
                        }
                    }
                    ComboBoxItem selectedComboBoxItem = (ComboBoxItem)inputLoaiPhong_PTP.SelectedItem;
                    string loaiPhong = selectedComboBoxItem.Content.ToString();

                    string soPhong = inputSoPhong_PTP.Text;
                    DateTime dt = DateTime.ParseExact(ngayThue, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string ngayThueFormatted = dt.ToString("yyyy-MM-dd");
                    string ngayTraFormatted = DateTime.ParseExact(ngayTra, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

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
                    switch (tinhHuong)
                    {
                        case "Thuê trực tiếp":
                            // Xử lý khi khách hàng thuê trực tiếp tại khách sạn
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
                                            ngayTra = ngayTraFormatted,
                                            maPhongThue = Convert.ToInt32(soPhong)
                                        };

                                        int maptp_new = conn_ptp.addPTP(ptp);

                                        conn_ptp.addLichSu(maptp_new, makh_moiThem);
                                        conn_phong.updateTinhTrang(soPhongInt, maptp_new);
                                        int soKhach = conn_ptp.countSoKhachInPTP(maptp_new);

                                        errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                                        errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;
                                        inputTenKhach_PTP.Text = string.Empty;
                                        inputDiaChi_PTP.Text = string.Empty;
                                        inputSoDinhDanh_PTP.Text = string.Empty;
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
                                        if (tinhtrang == "Đang xử lí")
                                        {
                                            int maptp = conn_ptp.getMaPTP(ngayThueFormatted, soPhongInt);
                                            int makh_moiThem = conn_kh.addKhachHang(kh);

                                            conn_ptp.addLichSu(maptp, makh_moiThem);
                                            conn_phong.updateTinhTrang(soPhongInt, maptp);
                                            bool checkFull = conn_phong.checkFull(soPhongInt);
                                            if (checkFull == true)
                                            {
                                                errorSoPhong_PTP.Text = "*Phòng đã đầy";
                                                errorSoPhong_PTP.Visibility = Visibility;
                                            }
                                            int soKhach = conn_ptp.countSoKhachInPTP(maptp);

                                            errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                                            errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;
                                            inputTenKhach_PTP.Text = string.Empty;
                                            inputDiaChi_PTP.Text = string.Empty;
                                            inputSoDinhDanh_PTP.Text = string.Empty;
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
                            break;

                        case "Đặt phòng":
                            // Xử lý khi khách hàng đang đặt phòng
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
                                            ngayTra = ngayTraFormatted,
                                            maPhongThue = Convert.ToInt32(soPhong)
                                        };

                                        int maptp_new = conn_ptp.addPTP(ptp);

                                        conn_ptp.addLichSu(maptp_new, makh_moiThem);
                                        conn_phong.updateTinhTrang(soPhongInt, maptp_new);
                                        int soKhach = conn_ptp.countSoKhachInPTP(maptp_new);

                                        errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                                        errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;
                                        inputTenKhach_PTP.Text = string.Empty;
                                        inputDiaChi_PTP.Text = string.Empty;
                                        inputSoDinhDanh_PTP.Text = string.Empty;
                                        soKhachDaThem_PTP.Text = $"Số khách đã thêm: {soKhach}";
                                    }
                                }
                                else
                                {
                                    if (tinhtrang == "Đã thuê")
                                    {
                                        string ngayTraDuKien = conn_ptp.getNgayTra(soPhongInt);
                                        DateTime ngayTraDuKienDate = DateTime.Parse(ngayTraDuKien);

                                        if (ngayThueDate > ngayTraDuKienDate) // được đặt trước
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
                                                    ngayTra = ngayTraFormatted,
                                                    maPhongThue = Convert.ToInt32(soPhong)
                                                };

                                                int maptp_new = conn_ptp.addPTP(ptp);

                                                conn_ptp.addLichSu(maptp_new, makh_moiThem);
                                                conn_phong.updateTinhTrang(soPhongInt, maptp_new);
                                                int soKhach = conn_ptp.countSoKhachInPTP(maptp_new);

                                                errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                                                errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;
                                                inputTenKhach_PTP.Text = string.Empty;
                                                inputDiaChi_PTP.Text = string.Empty;
                                                inputSoDinhDanh_PTP.Text = string.Empty;
                                                soKhachDaThem_PTP.Text = $"Số khách đã thêm: {soKhach}";
                                            }
                                        }
                                        else
                                        {
                                            errorSoPhong_PTP.Text = "*Phòng đã đầy";
                                            errorSoPhong_PTP.Visibility = Visibility;
                                            errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;
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
                                            if (tinhtrang == "Đang xử lí")
                                            {
                                                int maptp = conn_ptp.getMaPTP(ngayThueFormatted, soPhongInt);
                                                int makh_moiThem = conn_kh.addKhachHang(kh);

                                                conn_ptp.addLichSu(maptp, makh_moiThem);
                                                conn_phong.updateTinhTrang(soPhongInt, maptp, true, true);
                                                bool checkFull = conn_phong.checkFull(soPhongInt);
                                                if (checkFull == true)
                                                {
                                                    errorSoPhong_PTP.Text = "*Phòng đã đầy";
                                                    errorSoPhong_PTP.Visibility = Visibility;
                                                }
                                                int soKhach = conn_ptp.countSoKhachInPTP(maptp);

                                                errorSoPhong_PTP.Visibility = Visibility.Collapsed;
                                                errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;
                                                inputTenKhach_PTP.Text = string.Empty;
                                                inputDiaChi_PTP.Text = string.Empty;
                                                inputSoDinhDanh_PTP.Text = string.Empty;
                                                soKhachDaThem_PTP.Text = $"Số khách đã thêm: {soKhach}";
                                            }
                                            else
                                            {
                                                errorSoPhong_PTP.Text = "*Phòng đang bận";
                                                errorSoPhong_PTP.Visibility = Visibility;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                errorSoPhong_PTP.Text = "*Phòng không tồn tại";
                                errorSoPhong_PTP.Visibility = Visibility;
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            
        }
        private void DoneBtn_PTP_Click(object sender, RoutedEventArgs e)
        {
            errorSoPhong_PTP.Visibility = Visibility.Collapsed;
            errorThieuThongTin_PTP.Visibility = Visibility.Collapsed;

            string soPhong = inputSoPhong_PTP.Text;
            int soPhongInt;
            int.TryParse(soPhong, out soPhongInt);

            string ngayThue = inputNgayThue_PTP.Text;
            DateTime date_ngayThue = DateTime.ParseExact(ngayThue, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime currentDate = DateTime.Now;

            System.Windows.MessageBox.Show(ngayThue + "$$$" + currentDate.ToString());
            bool reserve = false;
            if (currentDate < date_ngayThue)
                reserve = true;

            var conn_phong = new PhongViewDAO();
            if (reserve == true)// đặt phòng trước
            {
                ComboBoxItem selectedComboBoxItem = (ComboBoxItem)inputLoaiPhong_PTP.SelectedItem;
                selectedComboBoxItem = (ComboBoxItem)inputLoaiKhach_PTP.SelectedItem;
                string loaiPhong = selectedComboBoxItem.Content.ToString();
                int loaiPhongInt;
                if (loaiPhong == "Loại A")
                    loaiPhongInt = 1;
                else
                {
                    if (loaiPhong == "Loại B")
                        loaiPhongInt = 2;
                    else loaiPhongInt = 3;
                }

                string tinhTrangPhong = conn_phong.checkTinhTrang(soPhongInt, loaiPhongInt);
                if (tinhTrangPhong == "Đang xử lí")
                {
                    conn_phong.updateTinhTrang(soPhongInt, 0, true, true);
                }
            }
            else // đặt phòng trực tiếp
            {
                conn_phong.updateTinhTrang(soPhongInt, 0, true);
            }

            TabControl_PhieuThuePhong.SelectedIndex = 0;
            inputSoPhong_PTP.Text = string.Empty;
            inputTenKhach_PTP.Text = string.Empty;
            inputDiaChi_PTP.Text = string.Empty;
            inputSoDinhDanh_PTP.Text = string.Empty;
            inputNgayTra_PTP.Text = string.Empty;
            soKhachDaThem_PTP.Text = "Số khách đã thêm: 0";
            Get_PhieuThuePhong_View();
        }
        /*private void DisableCheckBoxForZeroTienThue()
        {
            // Lặp qua các mục trong ListView của bạn
            foreach (var item in ListView_PhieuThuePhong.Items)
            {
                float tienThue = (float)item.tienThue;

                var checkBox = FindCheckBoxForItem(item);

                if (tienThue == 0)
                {
                    if (checkBox != null)
                    {
                        checkBox.IsEnabled = false;
                    }
                }
            }
        }
        private CheckBox FindCheckBoxForItem(object item)
        {
            // Tìm ListViewItem chứa mục
            ListViewItem listViewItem = ListView_PhieuThuePhong.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;

            if (listViewItem != null)
            {
                // Tìm CheckBox trong ListViewItem
                CheckBox checkBox = FindVisualChild<CheckBox>(listViewItem);

                return checkBox;
            }

            return null;
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is T result)
                {
                    return result;
                }
                else
                {
                    T descendant = FindVisualChild<T>(child);
                    if (descendant != null)
                    {
                        return descendant;
                    }
                }
            }
            return null;
        }*/
        private void ItemCheckBox_PTP_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            string maPTP = checkBox.CommandParameter.ToString();
            
            selectedMaPTPList.Add(Convert.ToInt32(maPTP));
            
            ThanhToanOut_PTP.Visibility = Visibility;

        }
        private void ItemCheckBox_PTP_UnChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            string maPTP = checkBox.CommandParameter.ToString();
            selectedMaPTPList.Remove(Convert.ToInt32(maPTP));
            if (selectedMaPTPList.Count == 0)
                ThanhToanOut_PTP.Visibility = Visibility.Collapsed;
        }

        private void ThanhToanOut_PTP_Click(object sender, RoutedEventArgs e)
        {
            TabControl_PhieuThuePhong.SelectedItem = ThanhToanNhieuTabItem;
            var con = new PhieuThuePhongViewDAO();
            PTPView_ThanhToan.listPTP = con.getListSelected(selectedMaPTPList);
            float tongTien = 0;
            for (int i = 0; i < PTPView_ThanhToan.listPTP.Count; i++)
            {
                tongTien += PTPView_ThanhToan.listPTP[i].tienThue;
            }

            MaPTPText_ThanhToanNhieu_Block_HD.Text = tongTien.ToString("N0") + " VNĐ";
            soPhongTT_ThanhToanNhieu_TextBlock_HD.Text = PTPView_ThanhToan.listPTP.Count.ToString();

            PTPView_ThanhToan.UpdatePaging();
            ListView_ChiTietPTP_ThanhToanNhieu.ItemsSource = PTPView_ThanhToan.curPTP;
        }

        private void ListView_ChiTietPTP_ThanhToanNhieu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedData = (PhieuThuePhong)ListView_ChiTietPTP_ThanhToanNhieu.SelectedItem;

            if (selectedData != null)
            {
                ChiTietPTPView.maPTP = selectedData.maPTP;
                ChiTietPTPView.maPhongThue = selectedData.maPhongThue;
                ChiTietPTPView.ngayThue = selectedData.ngayThue;
                ChiTietPTPView.tienThue = selectedData.tienThue;

                TabControl_PhieuThuePhong.SelectedItem = ChiTietPhieuThueTabItem;
                thanhToanBtn.Visibility = Visibility.Collapsed;
                UpdateChiTietPhieuThueData(ChiTietPTPView);
            }
        }

        private void lapHoaDonBtn_ThanhToanNhieu_Click(object sender, RoutedEventArgs e)
        {
            string dinhDanh = DinhDanh_ThanhToanNhieu_TextBox.Text;

            var conn_kh = new KhachHangViewDAO();
            var kh = new KhachHang();
            kh = conn_kh.getMaKhachHangByDinhDanh(dinhDanh);
            if (kh == null)
            {
                errorDinhDanh_ThanhToanNhieu.Visibility = Visibility;
            }
            else
            {
                errorDinhDanh_ThanhToanNhieu.Visibility = Visibility.Collapsed;
                var conn_hd = new HoaDonViewDAO();
                int mahd = conn_hd.createHoaDon(int.Parse(kh.MaKH));

                var conn_ptp = new PhieuThuePhongViewDAO();
                for (int i = 0; i < selectedMaPTPList.Count; i++)
                {
                    conn_ptp.thanhToanPTP(selectedMaPTPList[i], mahd);
                }
                Get_PhieuThuePhong_View();
                TabControl_PhieuThuePhong.SelectedIndex = 0;
            }
        }

        private void TimKiem_PTP_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = TimKiem_PTP_TextBox.Text;
            
            var sub = new ObservableCollection<PhieuThuePhong>();
            foreach (var ptp in PTPView.listPTP)
            {
                var maptp = ptp.maPTP.ToString();
                var ngayThue = ptp.ngayThue;
                var maPhongThue = ptp.maPhongThue.ToString();

                if (maptp.Contains(keyword))
                {
                    sub.Add(ptp);
                }
                else
                if (maPhongThue.Contains(keyword))
                {
                    sub.Add(ptp);
                }
                else
                if (ngayThue.Contains(keyword) && keyword.Contains('/'))
                {
                    sub.Add(ptp);
                }
            }
            
            PTPView.curPTP = sub;
            PTPView.curpage = 1;

            PTPView.UpdatePaging();
            ListView_PhieuThuePhong.ItemsSource = sub;
            Page_PhieuThuePhong_text.Text = PTPView.curpage.ToString();
        }

        private void NhanPhong_PTP_Click(object sender, RoutedEventArgs e)
        {
            DatTruocTextBlock.Visibility = Visibility.Collapsed;
            NdungTienTextBlock.Visibility = Visibility;
            thanhToanBtn.Visibility = Visibility;
            NhanPhong_PTP.Visibility = Visibility.Collapsed;
            HuyDatTruocPhong_PTP.Visibility = Visibility.Collapsed;

            var conn_phong = new PhongViewDAO();
            string maphong = MaPhongTextBlock.Text;
            conn_phong.updateTinhTrang(Convert.ToInt32(maphong), 0, true);
        }

        private void HuyDatTruocPhong_PTP_Click(object sender, RoutedEventArgs e)
        {
            var conn = new PhieuThuePhongViewDAO();
            string maptp = MaPTPTextBlock.Text;
            conn.huyPTP(Convert.ToInt32(maptp));

            //cập nhật tình trạng
            var conn_phong = new PhongViewDAO();
            string maphong = MaPhongTextBlock.Text;
            conn_phong.updateTinhTrang(Convert.ToInt32(maphong), 0, false, false, true);
            Get_PhieuThuePhong_View();
            TabControl_PhieuThuePhong.SelectedIndex = 0;
        }
    }
}
