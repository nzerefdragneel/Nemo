using Nemo.DAO;
using Nemo.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nemo.GUI
{
    /// <summary>
    /// Interaction logic for Suaphong.xaml
    /// </summary>
    public partial class Suaphong : Window
    {
        public Phong PhongMoi { get; set; }
        public DataTable ListLoaiPhong { get; set; }
        public List<string> ListTinhTrang { get; set; }
        public Suaphong(Phong old)
        {
            InitializeComponent();
            PhongMoi = (Phong)old.Clone();
            var con = new PhongViewDAO();
            ListLoaiPhong = con.GetListLoaiPhong();
            ListTinhTrang = new List<string>() { "Còn trống", "Đang đợi", "Đã thuê", "Đang sửa chữa" };
            this.DataContext = this;
            ChonLoaiPhong_Cbb.ItemsSource = ListLoaiPhong.DefaultView;
            ChonLoaiPhong_Cbb.DisplayMemberPath = "maloaiphong";
            ChonLoaiPhong_Cbb.SelectedValuePath = "maloaiphong";

            ChonLoaiPhong_Cbb.SelectedValue = PhongMoi.maloaiphong;

            Maphong_textbox.Text = PhongMoi.maphong.ToString();
            ChonTinhTrang_Cbb.SelectedValue = PhongMoi.tinhtrang;

        }


        private void QuayLai_Btn_click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void ThemPhong_Btn_Click(object sender, RoutedEventArgs e)
        {
            //kiểm tra điều kiện
            
            var loaiphong = ChonLoaiPhong_Cbb.SelectedValue.ToString();
            int loaiphongvalue = int.Parse(loaiphong);
            var number = Maphong_textbox.Text;
            int maphong;
            if (!int.TryParse(number, out maphong))
            {
                MessageBox.Show("Mã phòng là số nguyên", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                PhongMoi.maphong = maphong;
                PhongMoi.maloaiphong = loaiphongvalue;
                PhongMoi.tinhtrang = ChonTinhTrang_Cbb.SelectedValue.ToString();
                var conn = new PhongViewDAO();
                conn.Suaphong(PhongMoi);
                DialogResult = true;
                this.Close();
            }
        }

        private void Xoa_Btn_Click(object sender, RoutedEventArgs e)
        {
            var loaiphong = ChonLoaiPhong_Cbb.SelectedValue.ToString();
            int loaiphongvalue = int.Parse(loaiphong);
            var number = Maphong_textbox.Text;
            int maphong;
            if (!int.TryParse(number, out maphong))
            {
                MessageBox.Show("Mã phòng là số nguyên", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                PhongMoi.maphong = maphong;
                PhongMoi.maloaiphong = loaiphongvalue;
                PhongMoi.tinhtrang = ChonTinhTrang_Cbb.SelectedValue.ToString();
                var phong = new PhongViewDAO();
                phong.Xoaphong(PhongMoi);
                DialogResult = false;
                this.Close();
            }
        }
    }
}

