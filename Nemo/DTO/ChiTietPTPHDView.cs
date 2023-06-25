using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Nemo.DTO
{
    class ChiTietPTPHDView : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<ChiTietPTPHD> ListChiTietPTPHD { get; set; }
        public ObservableCollection<ChiTietPTPHD> CurChiTietPTPHD { get; set; }
        public int maptp { get; set; }
        public int maphong { get; set; }
        public DateTime ngaythue { get; set; }
        public DateTime ngaytra { get; set; }
        public string ghichu { get; set; }
        public int prevpage { get; set; } = 0;
        public int curpage { get; set; } = 0;
        public int nextpage { get; set; } = 0;
        public int perpage { get; set; }
        public int totalpage { get; set; } = 0;
        public int totalItems { get; set; } = 0;
        public ChiTietPTPHDView()
        {
            ListChiTietPTPHD = new ObservableCollection<ChiTietPTPHD>();
            CurChiTietPTPHD = new ObservableCollection<ChiTietPTPHD>();
            curpage = 1;
            perpage = 13;
            totalpage = 0;
            totalItems = 0;
            prevpage = 0;
            nextpage = 2;
        }
        public void UpdatePaging()
        {
            totalItems = ListChiTietPTPHD.Count;
            totalpage = (totalItems / perpage) +
                (totalItems % perpage == 0 ? 0 : 1);
            CurChiTietPTPHD = new ObservableCollection<ChiTietPTPHD>(ListChiTietPTPHD
                 .Skip((curpage - 1) * perpage)
                 .Take(perpage).ToList());
        }


    }
}
