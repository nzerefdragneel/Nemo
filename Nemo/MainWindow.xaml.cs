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
            if (PhongView.curpage>1) { PhongView.curpage = PhongView.curpage - 1; }
            Page_DanhMucPhong_text.Text = PhongView.curpage.ToString();
            PhongView.UpdatePaging();
            ListView_DanhMucPhong.ItemsSource = PhongView.CurPhong;
        }
    }
}
