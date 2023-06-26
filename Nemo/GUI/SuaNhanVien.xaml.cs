using Nemo.DAO;
using Nemo.DTO;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for SuaNhanVien.xaml
    /// </summary>
    public partial class SuaNhanVien : Window
    {
       
        public TaiKhoan TKmoi { get; set; }
        public List<string> ListTinhTrang { get; set; }
        public SuaNhanVien(TaiKhoan old)
        {
            InitializeComponent();
            TKmoi = (TaiKhoan)old.Clone();
            var con = new PhongViewDAO();
            ListTinhTrang = new List<string>() { "Đang làm việc", "Đã nghỉ việc" };
            this.DataContext = this;
            Tinhtrang_Cbb.ItemsSource = ListTinhTrang;
            Hoten_textbox.Text = TKmoi.hoten;
            TenTk_textbox.Text = TKmoi.tentk;
            Ngayvaolam_textbox.Text = TKmoi.ngayvaolam;
            Tinhtrang_Cbb.SelectedValue = TKmoi.tinhtrang;

        }
      


        private void QuayLai_Btn_click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void Suanhanvien_Btn_Click(object sender, RoutedEventArgs e)
        {
            //kiểm tra điều kiện
            var hoten = Hoten_textbox.Text;
            var matkhau = Password_Box.Password;
            var ngayvaolam = Ngayvaolam_textbox.Text;
            var tinhtrang = Tinhtrang_Cbb.SelectedValue.ToString();
            if (hoten=="")
            {
                MessageBox.Show("Họ tên không để trống", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (ngayvaolam=="")
            {
                MessageBox.Show("Ngày vào làm không để trống", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (tinhtrang == "")
            {
                MessageBox.Show("Tình trạng không để trống", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //gán giá trị
                TKmoi.hoten = hoten;
                TKmoi.ngayvaolam = ngayvaolam;
                TKmoi.tinhtrang = tinhtrang;
                if (matkhau != "")
                {
                    var passwordInBytes = Encoding.UTF8.GetBytes(matkhau);
                    var entropy = new byte[20];
                    using (var rng = new RNGCryptoServiceProvider())
                    {
                        rng.GetBytes(entropy);
                    }

                    var cypherPass = ProtectedData.Protect(passwordInBytes, entropy, DataProtectionScope.CurrentUser);
                    var cypherPass64 = Convert.ToBase64String(cypherPass);
                    var entropy64 = Convert.ToBase64String(entropy);
                    TKmoi.matkhau = cypherPass64;
                    TKmoi.muoi = entropy64;
                }


                var conn = new DangkyDAO();
                conn.Suanhanvien(TKmoi);
                DialogResult = true;
                this.Close();
            }
        }


        private void Xoa_Btn_click(object sender, RoutedEventArgs e)
        {
            var phong = new DangkyDAO();
            phong.Xoanhanvien(TKmoi);
            DialogResult = true;
            this.Close();
        }
    }
}
