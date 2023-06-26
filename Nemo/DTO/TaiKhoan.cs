using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemo.DTO
{
    public class TaiKhoan:ICloneable
    {
        public TaiKhoan()
        {  }
        public TaiKhoan(string us, string pw, string s)
        { tentk = us; matkhau = pw; muoi = s; }
        public TaiKhoan(string us, string pw, string s, string ht, string ngay)
        { tentk = us; matkhau = pw; muoi = s; hoten = ht; ngayvaolam = ngay; }
        public string tentk { get; set; }
        public string hoten { get; set; }
        public string matkhau { get; set;}
        public string muoi { get; set;}
        public string ? quyen { get; set;}
        public string ngayvaolam { get; set; }
        public string tinhtrang { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }

    }
}
