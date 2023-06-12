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

namespace Nemo.DOA
{
    internal class PhongViewDOA
    {
        public PhongViewDOA() { }
        public ObservableCollection<Phong> GetListPhong()
        {
            var conn = new ConnectDB();
            conn.OpenConnection();
            var result=conn.ExecuteQuery("select * from Phong p left join loaiphong l on p.maloaiphong=l.maloaiphong");
            conn.CloseConnection();
            ObservableCollection<Phong> list = JArray.FromObject(result).ToObject<ObservableCollection<Phong>>();
            for (int i=0;i<list.Count();i++)
            {
                if (list[i].tinhtrang == "Còn trống")
                    list[i].tuychon = "Thuê phòng";
                else list[i].tuychon = "Chính sửa";
            }
            return list;
        }
    }
}
