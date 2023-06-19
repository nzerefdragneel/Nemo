using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemo.DTO
{
    internal class ChiTietPhieuThuePhong
    {
        public int STT { get; set; }
        public string tenKH { get; set; }
        public int heSo { get; set; }
        public string loaiKhach { get; set; }
        public string dinhDanh { get; set; }
        public string diaChi { get; set; }
        public string ghiChu { get; set; }
        public double tiLePhuThu { get; set; }
        public int sl_khachtoida { get; set; }
    }
}