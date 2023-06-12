using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Nemo.DTO
{
    class PhongView: INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<Phong> ListPhong { get; set; }
        public ObservableCollection<Phong> CurPhong { get; set; }
        public int PhongDaThue { get; set; }
        public int PhongTrong { get; set; }
        public int prevpage { get; set; } = 0;
        public int curpage { get; set; } = 0;
        public int nextpage { get; set; } = 0;
        public int perpage { get; set; }
        public int totalpage { get; set; } = 0;
        public int totalItems { get; set; } = 0;
        public PhongView()
        {
            ListPhong = new ObservableCollection<Phong>();
            CurPhong = new ObservableCollection<Phong>();
            curpage = 1;           
            perpage = 5;
            totalpage = 0;
            totalItems = 0;
            prevpage = 0;
            nextpage = 2;
        }
        public void UpdatePaging()
        {
            totalItems = ListPhong.Count;
            totalpage = (totalItems / perpage) +
                (totalItems % perpage == 0 ? 0 : 1);
            CurPhong = new ObservableCollection<Phong>(ListPhong
                 .Skip((curpage - 1) * perpage)
                 .Take(perpage).ToList());
        }
       

    }
}
