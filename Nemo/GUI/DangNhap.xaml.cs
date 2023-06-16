using Nemo.DAO;
using Nemo.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nemo.GUI
{
    /// <summary>
    /// Interaction logic for DangNhap.xaml
    /// </summary>
    public partial class DangNhap : Window
    {
        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public DangNhap()
        {
            InitializeComponent();
            var username = ConfigurationManager.AppSettings["LastUsername"];
            var password64 = ConfigurationManager.AppSettings["LastPassword"];
            var entropy64 = ConfigurationManager.AppSettings["Entropy"];
            var password = "";
            if (username != "" && password64 != "")
            {
                var entropy = Convert.FromBase64String(entropy64);
                var unprotectedBytes = ProtectedData.Unprotect(Convert.FromBase64String(password64), entropy, DataProtectionScope.CurrentUser);
                var base64String = Convert.ToBase64String(unprotectedBytes);
                password= Base64Decode(base64String);
            }
            TenTk_textbox.Text = username;
            Password_Box.Password = password;
        }
        private void KiemTraDangNhap(string username, string pwd)
        {
            var check = new DangNhapDAO();
            TaiKhoan tk = check.GetUserName(username);
            if (tk == null)
            {
                Warning_textblock.Text = "Tên đăng nhập không tôn tại!";
            }
            else
            {
                var entropy = Convert.FromBase64String(tk.Muoi) ;
                var unprotectedBytes = ProtectedData.Unprotect(Convert.FromBase64String(tk.MatKhau), entropy, DataProtectionScope.CurrentUser);
                var base64String = Convert.ToBase64String(unprotectedBytes);
                var password = Base64Decode(base64String);
                if (password == pwd)
                {
                    if (rememberMeCheckBox.IsChecked == true)
                    {

                        var config = ConfigurationManager.OpenExeConfiguration(
                            ConfigurationUserLevel.None);
                        config.AppSettings.Settings["LastUsername"].Value = username;
                        config.AppSettings.Settings["LastPassword"].Value = tk.MatKhau;
                        config.AppSettings.Settings["Entropy"].Value = tk.Muoi;
                        config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");

                    }
                    var screen = new MainWindow();
                    screen.Show();
                    this.Close();
                }else
                {
                    Warning_textblock.Text = "Mật khẩu không chính xác!";

                }
            }
        }
        private void DangNhap_Btn_Click(object sender, RoutedEventArgs e)
        {
            string username = TenTk_textbox.Text;
            string password = Password_Box.Password;
            KiemTraDangNhap(username,password);
          }

        private void DangKy_Btn_Click(object sender, RoutedEventArgs e)
        {
            var screen = new Dangky();
            screen.Show();
            this.Close();

        }
    }
}
