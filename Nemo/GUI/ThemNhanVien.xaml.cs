using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Nemo.DAO;
namespace Nemo.GUI
{
    /// <summary>
    /// Interaction logic for Dangky.xaml
    /// </summary>
    public partial class ThemNhanVien : Window
    {
        public ThemNhanVien()
        {
            InitializeComponent();
        }

        private void Themnhanvien_Btn_Click(object sender, RoutedEventArgs e)
        {
            string username = TenTk_textbox.Text;
            string password = Password_Box.Password;
            string hoten = Hoten_textbox.Text;
            string ngayvaolam = Ngayvaolam_textbox.Text;
            var dangky = new DangkyDAO();
            if (username == "")
            {
                Warning_textblock.Text = "Bạn chưa nhập tên đăng nhập!";
            }
            else if (password == "")
            {
                Warning_textblock.Text = "Bạn chưa nhập mật khẩu!";
            }
            else if (hoten == "")
            {
                Warning_textblock.Text = "Bạn chưa nhập họ tên nhân viên!";
            }
            else if (ngayvaolam == "")
            {
                Warning_textblock.Text = "Bạn chưa nhập ngày vào làm!";
            }
            else if (!dangky.CheckUserName(username))
            {
                Warning_textblock.Text = "Tên đăng nhập đã tồn tại!";
            }
            else
            {
                var passwordInBytes = Encoding.UTF8.GetBytes(password);
                var entropy = new byte[20];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(entropy);
                }

                var cypherPass = ProtectedData.Protect(passwordInBytes, entropy, DataProtectionScope.CurrentUser);
                var cypherPass64 = Convert.ToBase64String(cypherPass);
                var entropy64 = Convert.ToBase64String(entropy);

                var tk = new DTO.TaiKhoan(username, cypherPass64, entropy64,hoten,ngayvaolam);
                dangky.ThemNhanVien(tk);
                MessageBox.Show("Thêm thành công!");
                DialogResult = true;
                this.Close();
            }
        }

        private void QuayLai_Btn_click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
