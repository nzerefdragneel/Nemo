using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;

namespace Nemo.DTO
{
    class PhongView: INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<Phong> ListPhong { get; set; }
        public ObservableCollection<Phong> CurListPhong { get; set; }
        public ObservableCollection<Phong> CurPhong { get; set; }
        public ObservableCollection<Tinhtrangphong> Tinhtrangphong { get; set; }
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
            Tinhtrangphong = new ObservableCollection<Tinhtrangphong>();
            ListPhong = new ObservableCollection<Phong>();
            CurPhong = new ObservableCollection<Phong>();
            CurListPhong = new ObservableCollection<Phong>();
            curpage = 1;           
            perpage = 8;
            totalpage = 0;
            totalItems = 0;
            prevpage = 0;
            nextpage = 2;
        }
        public void ResetCurlist()
        {
            CurListPhong = ListPhong;
        }
        public void UpdatePaging()
        {
            if (CurListPhong == null)
            {
                CurPhong = null;
                return;
            }
            totalItems = CurListPhong.Count;
            totalpage = (totalItems / perpage) +
                (totalItems % perpage == 0 ? 0 : 1);
            CurPhong = new ObservableCollection<Phong>(CurListPhong
                 .Skip((curpage - 1) * perpage)
                 .Take(perpage).ToList());
        }
       

    }
}
