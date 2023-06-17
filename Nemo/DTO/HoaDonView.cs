using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Nemo.DTO
{
    class HoaDonView : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<HoaDon> ListHoaDon { get; set; }
        public ObservableCollection<HoaDon> CurHoaDon { get; set; }
        public ObservableCollection<HoaDon> CurListHoaDon { get; set; }
        public int prevpage { get; set; } = 0;
        public int curpage { get; set; } = 0;
        public int nextpage { get; set; } = 0;
        public int perpage { get; set; }
        public int totalpage { get; set; } = 0;
        public int totalItems { get; set; } = 0;
        public HoaDonView()
        {
            ListHoaDon = new ObservableCollection<HoaDon>();
            CurHoaDon = new ObservableCollection<HoaDon>();
            CurListHoaDon = new ObservableCollection<HoaDon>();
            curpage = 1;
            perpage = 16;
            totalpage = 0;
            totalItems = 0;
            prevpage = 0;
            nextpage = 2;
        }
        public void ResetCurlist()
        {
            CurListHoaDon = ListHoaDon;
        }
        public void UpdatePaging()
        {
            totalItems = ListHoaDon.Count;
            totalpage = (totalItems / perpage) +
                (totalItems % perpage == 0 ? 0 : 1);
            CurHoaDon = new ObservableCollection<HoaDon>(CurListHoaDon
                 .Skip((curpage - 1) * perpage)
                 .Take(perpage).ToList());
        }


    }
}
