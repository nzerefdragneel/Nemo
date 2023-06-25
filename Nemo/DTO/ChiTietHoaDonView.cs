using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Nemo.DTO
{
    class ChiTietHoaDonView : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<ChiTietHoaDon> ListChiTietHoaDon { get; set; }
        public ObservableCollection<ChiTietHoaDon> CurChiTietHoaDon { get; set; }
        public int mahoadon { get; set; }
        public DateTime? ngaythanhtoan { get; set; }
        public float? tongtien { get; set; }
        public string khachhang { get; set; }
        public int sophongthanhtoan { get; set; }
        public int prevpage { get; set; } = 0;
        public int curpage { get; set; } = 0;
        public int nextpage { get; set; } = 0;
        public int perpage { get; set; }
        public int totalpage { get; set; } = 0;
        public int totalItems { get; set; } = 0;
        public ChiTietHoaDonView()
        {
            ListChiTietHoaDon = new ObservableCollection<ChiTietHoaDon>();
            CurChiTietHoaDon = new ObservableCollection<ChiTietHoaDon>();
            curpage = 1;
            perpage = 13;
            totalpage = 0;
            totalItems = 0;
            prevpage = 0;
            nextpage = 2;
        }
        public void UpdatePaging()
        {
            if (ListChiTietHoaDon == null) return;
            totalItems = ListChiTietHoaDon.Count;
            totalpage = (totalItems / perpage) +
                (totalItems % perpage == 0 ? 0 : 1);
            CurChiTietHoaDon = new ObservableCollection<ChiTietHoaDon>(ListChiTietHoaDon
                 .Skip((curpage - 1) * perpage)
                 .Take(perpage).ToList());
        }


    }
}
