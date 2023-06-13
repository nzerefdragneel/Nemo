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
using System.Windows.Shapes;
using Nemo.DTO;
using Nemo.DAO;
using System.Data;
using System.Diagnostics;
using System.Xml.Linq;

namespace Nemo
{
    /// <summary>
    /// Interaction logic for ThemPhong.xaml
    /// </summary>
    public partial class ThemPhong : Window
    {
        public Phong PhongMoi { get; set; }
        public DataTable ListLoaiPhong { get; set; }
        public ThemPhong()
        {
            InitializeComponent();
            PhongMoi = new Phong();
            var con = new PhongViewDAO();
            ListLoaiPhong = con.GetListLoaiPhong();
            ChonLoaiPhong_Cbb.ItemsSource =ListLoaiPhong.DefaultView;
            ChonLoaiPhong_Cbb.DisplayMemberPath = "maloaiphong";
        }
        

        private void QuayLai_Btn_click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(ListLoaiPhong);
            DialogResult = false;
            this.Close();
        }

        private void ThemPhong_Btn_Click(object sender, RoutedEventArgs e)
        {
            //kiểm tra điều kiện
            DialogResult = true;
            this.Close();
        }
    }
  
    }

