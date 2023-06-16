using System;
using System.Collections.Generic;
using System.Configuration;
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
using Nemo.DAO;
namespace Nemo.GUI
{
    /// <summary>
    /// Interaction logic for Dangky.xaml
    /// </summary>
    public partial class Dangky : Window
    {
        public Dangky()
        {
            InitializeComponent();
        }

        private void DangKy_Btn_Click(object sender, RoutedEventArgs e)
        {
            string username = TenTk_textbox.Text;
            string password = Password_Box.Password;
            string confirmpwd = ConfirmPassword_Box.Password;
            var dangky = new DangkyDAO();
            if (username == "")
            {
                Warning_textblock.Text = "Bạn chưa nhập tên đăng nhập!";
            }
            else if (password == "")
            {
                Warning_textblock.Text = "Bạn chưa nhập mật khẩu!";
            }
            else if(confirmpwd=="")
            {
                Warning_textblock.Text = "Bạn chưa xác nhận mật khẩu!";
            }
            else if(confirmpwd != password)
            {
                Warning_textblock.Text = "Mật khẩu không khớp!";
            }
            else if (!dangky.CheckUserName(username))
            {
                Warning_textblock.Text = "Tên đăng nhập đã tồn tại!";
            }
            else  {
                var passwordInBytes = Encoding.UTF8.GetBytes(password);
                var entropy = new byte[20];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(entropy);
                }

                var cypherPass = ProtectedData.Protect(passwordInBytes, entropy, DataProtectionScope.CurrentUser);
                var cypherPass64 = Convert.ToBase64String(cypherPass);
                var entropy64 = Convert.ToBase64String(entropy);

                var config = ConfigurationManager.OpenExeConfiguration(
                    ConfigurationUserLevel.None);
                config.AppSettings.Settings["LastUsername"].Value = username;
                config.AppSettings.Settings["LastPassword"].Value = cypherPass64;
                config.AppSettings.Settings["Entropy"].Value = entropy64;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
               
                var tk = new DTO.TaiKhoan(username, cypherPass64, entropy64);
                dangky.Themtk(tk);
                var screen = new DangNhap();
                screen.Show();
                this.Close();
            }
        
                
            }

        private void DangNhap_Btn_Click(object sender, RoutedEventArgs e)
        {
            var screen = new DangNhap();
            screen.Show();
            this.Close();
        }
    }
}
