using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nemo.DTO;
using Npgsql;
using JsonConverter;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Data;
using Microsoft.VisualBasic;

namespace Nemo.DAO
{
    internal class PhongViewDAO
    {
        public ConnectDB conn;
        public PhongViewDAO() {
            conn = new ConnectDB();
        }
      
        public ObservableCollection<Phong> GetListPhong()
        {
            var result=conn.ExecuteQuery("select * from Phong p left join loaiphong l on p.maloaiphong=l.maloaiphong order by p.maphong asc");
            if (result == null) return null;
            ObservableCollection<Phong> list = JArray.FromObject(result).ToObject<ObservableCollection<Phong>>();
            return list;
        }
        public ObservableCollection<Phong> GetListPhongLoai(string loai)
        {
            var result = conn.ExecuteQuery(@$"select * from Phong p left join loaiphong l on p.maloaiphong=l.maloaiphong where p.tinhtrang=E'{loai}'");
            if (result == null) return null;
            ObservableCollection<Phong> list = JArray.FromObject(result).ToObject<ObservableCollection<Phong>>();
            return list;
        }
        public DataTable GetListLoaiPhong()
        {
            var result = conn.ExecuteQuery("Select maloaiphong from loaiphong");
            return result;
        }
        public bool Kiemtramaphong(int maphong)
        {
            var result = conn.ExecuteQuery(@$"select * from Phong where maphong={maphong}");
            if (result == null) return true;
            return false;
        }
        public ObservableCollection<Tinhtrangphong> GetListTinhTrang()
        {
            var result = conn.ExecuteQuery("select p.tinhtrang, count(*) from Phong p group by p.tinhtrang");
            if (result == null) return null;
            ObservableCollection<Tinhtrangphong> list = JArray.FromObject(result).ToObject<ObservableCollection<Tinhtrangphong>>();
            return list;
        }
        public void ThemPhongMoi(Phong phongmoi)
        {
            var query = @$"INSERT INTO phong VALUES ({phongmoi.maphong},'Còn trống',{phongmoi.maloaiphong})";
            conn.UpdateQuery(query);

        }
        public void Suaphong(Phong phongmoi)
        {
            var query = @$"UPDATE phong SET tinhtrang='{phongmoi.tinhtrang}',maloaiphong={phongmoi.maloaiphong}
                            where maphong={phongmoi.maphong}";
            conn.UpdateQuery(query);

        }
    }
}
