using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Nemo.DTO
{
    class PhieuThuePhongView : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<PhieuThuePhong> listPTP { get; set; }
        public ObservableCollection<PhieuThuePhong> curPTP { get; set; }
        public int PhongDaThue { get; set; }
        public int PhongTrong { get; set; }
        public int prevpage { get; set; } = 0;
        public int curpage { get; set; } = 0;
        public int nextpage { get; set; } = 0;
        public int perpage { get; set; }
        public int totalpage { get; set; } = 0;
        public int totalItems { get; set; } = 0;
        public PhieuThuePhongView()
        {
            listPTP = new ObservableCollection<PhieuThuePhong>();
            curPTP = new ObservableCollection<PhieuThuePhong>();
            curpage = 1;
            perpage = 8;
            totalpage = 0;
            totalItems = 0;
            prevpage = 0;
            nextpage = 2;
        }
        public void UpdatePaging()
        {
            if (listPTP == null) return;
            totalItems = listPTP.Count;
            totalpage = (totalItems / perpage) +
                (totalItems % perpage == 0 ? 0 : 1);
            curPTP = new ObservableCollection<PhieuThuePhong>(listPTP.Skip((curpage - 1) * perpage)
                .Take(perpage)
                .ToList());
        }
    }
}
