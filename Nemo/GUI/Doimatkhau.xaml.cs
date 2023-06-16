using Nemo.DAO;
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

namespace Nemo.GUI
{
    /// <summary>
    /// Interaction logic for Doimatkhau.xaml
    /// </summary>
    public partial class Doimatkhau : Window
    {
        public Doimatkhau()
        {
            InitializeComponent();
        }
        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public string uncodepwd(string password64,string entropy64)
        {
            var entropy = Convert.FromBase64String(entropy64);
            var unprotectedBytes = ProtectedData.Unprotect(Convert.FromBase64String(password64), entropy, DataProtectionScope.CurrentUser);
            var base64String = Convert.ToBase64String(unprotectedBytes);
            var result  = Base64Decode(base64String);
            return result;
        }
        private void Doimatkhau_Btn_Click(object sender, RoutedEventArgs e)
        {
            string username = ConfigurationManager.AppSettings["LastUsername"];
            string entropyold= ConfigurationManager.AppSettings["Entropy"];
            string pwdold = ConfigurationManager.AppSettings["LastPassword"];
            

            string oldpassword = OldPassword_Box.Password;
            string newpassword = NewPassword_Box.Password;
            string confirmpwd = ConfirmPassword_Box.Password;
            var dangky = new DangkyDAO();
            if (oldpassword == "")
            {
                Warning_textblock.Text = "Bạn chưa nhập mật khẩu cũ!";
            }
            else if (newpassword == "")
            {
                Warning_textblock.Text = "Bạn chưa nhập mật khẩu mới!";
            }
            else if (confirmpwd == "")
            {
                Warning_textblock.Text = "Xác nhận mật khẩu không chính xác!";
            }
            else if (confirmpwd != newpassword)
            {
                Warning_textblock.Text = "Mật khẩu không khớp!";
            }
            else if (uncodepwd(pwdold, entropyold) != oldpassword)
            {
                Warning_textblock.Text = "Mật khẩu cũ không chính xác!";
            }
            else
            {
                var passwordInBytes = Encoding.UTF8.GetBytes(newpassword);
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
                dangky.DoiMatKhau(tk);
                DialogResult = true;
                this.Close();
                MessageBox.Show("Đổi mật khẩu thành công!");
            }
        }

        private void Huy_Btn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
