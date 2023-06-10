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
using Microsoft.VisualBasic;
using System.Windows.Forms;
using ControlzEx.Controls;

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
            var KhachHang = new KhachHang()
            {
                MaKH = "001",
                TenKH = "Nguyễn Văn A",
                DiaChi = "Hồ Chí Minh"
            };
            /*var conn = new KhachHangDOA();
            conn.OpenConnection();
            conn.ExecuteQuery("");*/
            this.DataContext = danhSachPhieuThue;
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

        private void phieuThueBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void thanhToanBtn_Click(object sender, RoutedEventArgs e)
        {
            MyTabControl.SelectedItem = ThanhToanTabItem;
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

                MyTabControl.SelectedItem = ChiTietPhieuThueTabItem;
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
            MyTabControl.SelectedIndex = MyTabControl.SelectedIndex - 1;
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
    }
}
