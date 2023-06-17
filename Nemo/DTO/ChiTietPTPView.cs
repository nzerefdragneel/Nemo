using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Nemo.DTO
{
    class ChiTietPTPView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<ChiTietPhieuThuePhong> listChiTietPTP { get; set; }
        public ObservableCollection<ChiTietPhieuThuePhong> curChiTietPTP { get; set; }
        public int maPTP { get; set; }
        public DateTime ngayThue { get; set; }
        public int maPhongThue { get; set; }
        public float tienThue { get; set; }
        public int prevpage { get; set; } = 0;
        public int curpage { get; set; } = 0;
        public int nextpage { get; set; } = 0;
        public int perpage { get; set; }
        public int totalpage { get; set; } = 0;
        public int totalItems { get; set; } = 0;
        public ChiTietPTPView()
        {
            listChiTietPTP = new ObservableCollection<ChiTietPhieuThuePhong>();
            curChiTietPTP = new ObservableCollection<ChiTietPhieuThuePhong>();
            curpage = 1;
            perpage = 13;
            totalpage = 0;
            totalItems = 0;
            prevpage = 0;
            nextpage = 2;
        }
        public void UpdatePaging()
        {
            totalItems = listChiTietPTP.Count;
            totalpage = (totalItems / perpage) +
                (totalItems % perpage == 0 ? 0 : 1);
            curChiTietPTP = new ObservableCollection<ChiTietPhieuThuePhong>(listChiTietPTP
                 .Skip((curpage - 1) * perpage)
                 .Take(perpage).ToList());
        }
    }
}
