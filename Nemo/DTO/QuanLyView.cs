using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemo.DTO
{
    class QuanLyView : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<TaiKhoan> ListNhanVien { get; set; }
        public ObservableCollection<TaiKhoan> CurListNhanVien { get; set; }
        public ObservableCollection<TaiKhoan> CurNhanVien { get; set; }
        public int prevpage { get; set; } = 0;
        public int curpage { get; set; } = 0;
        public int nextpage { get; set; } = 0;
        public int perpage { get; set; }
        public int totalpage { get; set; } = 0;
        public int totalItems { get; set; } = 0;
        public QuanLyView()
        {
            ListNhanVien = new ObservableCollection<TaiKhoan>();
            CurListNhanVien = new ObservableCollection<TaiKhoan>();
            CurNhanVien = new ObservableCollection<TaiKhoan>();
            curpage = 1;
            perpage = 6;
            totalpage = 0;
            totalItems = 0;
            prevpage = 0;
            nextpage = 2;
        }
        public void ResetCurlist()
        {
            CurListNhanVien = ListNhanVien;
        }
        public void UpdatePaging()
        {
            if (CurListNhanVien == null)
            {
                CurNhanVien = null;
                return;
            }
            totalItems = CurListNhanVien.Count;
            totalpage = (totalItems / perpage) +
                (totalItems % perpage == 0 ? 0 : 1);
            CurNhanVien = new ObservableCollection<TaiKhoan>(CurListNhanVien
                 .Skip((curpage - 1) * perpage)
                 .Take(perpage).ToList());
        }


    }
}
