using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Nemo.DTO
{
    class ChiTietHoaDonView : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<ChiTietHoaDon> ListChiTietHoaDon { get; set; }
        public ObservableCollection<ChiTietHoaDon> CurChiTietHoaDon { get; set; }
        public int mahoadon { get; set; }
        public DateTime ngaythanhtoan { get; set; }
        public float tongtien { get; set; }
        public string phuongthuc { get; set; }
        public string khachhang { get; set; }
        public string diachi { get; set; }
        //    public int PhongDaThue { get; set; }
        //    public int PhongTrong { get; set; }
        //    public int prevpage { get; set; } = 0;
        //    public int curpage { get; set; } = 0;
        //    public int nextpage { get; set; } = 0;
        //    public int perpage { get; set; }
        //    public int totalpage { get; set; } = 0;
        //    public int totalItems { get; set; } = 0;
        public ChiTietHoaDonView()
        {
            ListChiTietHoaDon = new ObservableCollection<ChiTietHoaDon>();
            CurChiTietHoaDon = new ObservableCollection<ChiTietHoaDon>();
            //curpage = 1;
            //perpage = 8;
            //totalpage = 0;
            //totalItems = 0;
            //prevpage = 0;
            //nextpage = 2;
        }
        //    public void UpdatePaging()
        //    {
        //        totalItems = ListPhong.Count;
        //        totalpage = (totalItems / perpage) +
        //            (totalItems % perpage == 0 ? 0 : 1);
        //        CurPhong = new ObservableCollection<Phong>(ListPhong
        //             .Skip((curpage - 1) * perpage)
        //             .Take(perpage).ToList());
        //    }


    }
}
