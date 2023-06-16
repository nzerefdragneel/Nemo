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

            if (confirmpwd == password) {
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
                var dangky = new DangkyDAO();
                var tk = new DTO.TaiKhoan(username, cypherPass64, entropy64);
                dangky.Themtk(tk);
                var screen = new DangNhap();
                screen.Show();
                this.Close();
            }
        
                
            }
    }
}
