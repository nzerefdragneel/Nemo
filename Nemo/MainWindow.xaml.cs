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
            var con = new PhongViewDOA();
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
        //    MyTabControl.SelectedIndex = MyTabControl.SelectedIndex - 1;
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
           // MyTabControl.SelectedItem = TaoPTPTabItem;
        }
    }
}
