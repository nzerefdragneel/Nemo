using Nemo.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemo.DTO
{
    public class QuyDinhView:INotifyPropertyChanged
    {
        public QuyDinh quyDinh{get;set;}

        public event PropertyChangedEventHandler? PropertyChanged;

        public void getQuyDinh()
        {
            var con = new QuyDinhDAO();
            quyDinh= con.getQuyDinh();
        }
        public void addQuyDinh(string tile, string heso,string soloai,string sokhach)
        {
            var con = new QuyDinhDAO();
            quyDinh.tiLePhuThu = Convert.ToSingle(tile);
            quyDinh.heSoKhachNN = Convert.ToSingle(heso);
            quyDinh.soLoaiPhong = Convert.ToInt16(soloai);
            quyDinh.soLuongKhachToiDa = Convert.ToInt16(sokhach);
            con.addQuyDinh(quyDinh);
        }
    }
}
