using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemo.DTO
{
    public class TaiKhoan
    {
        public TaiKhoan(string us, string pw, string s) { TenTK = us;MatKhau = pw;Muoi = s; }  
        public string TenTK { get; set; }
        public string MatKhau { get; set;}
        public string Muoi { get; set;}
        public string Quyen { get; set;}
    }
}
