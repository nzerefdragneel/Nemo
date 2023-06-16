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

namespace Nemo.DAO
{
    internal class PhongViewDAO
    {
        public ConnectDB conn;
        public PhongViewDAO() {
            conn = new ConnectDB();
            conn.OpenConnection();
        }
       ~PhongViewDAO()
        {
            conn.CloseConnection();
        }
        public ObservableCollection<Phong> GetListPhong()
        {
            var result=conn.ExecuteQuery("select * from Phong p left join loaiphong l on p.maloaiphong=l.maloaiphong");
            ObservableCollection<Phong> list = JArray.FromObject(result).ToObject<ObservableCollection<Phong>>();
            for (int i=0;i<list.Count();i++)
            {
                if (list[i].tinhtrang == "Còn trống")
                    list[i].tuychon = "Thuê phòng";
                else list[i].tuychon = "Chính sửa";
            }
            return list;
        }
        public DataTable GetListLoaiPhong()
        {
            var result = conn.ExecuteQuery("Select maloaiphong from loaiphong");
            return result;
        }
        public void ThemPhongMoi(Phong phongmoi)
        {
            var query = "INSERT INTO phong VALUES ("+phongmoi.maphong+ ",\'Còn trống\'," + phongmoi.maloaiphong+")";

            conn.UpdateQuery(query);

        }
    }
}
