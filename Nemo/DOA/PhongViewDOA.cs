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
        public List<Phong> GetListPhong()
        {
            var conn = new ConnectDB();
            conn.OpenConnection();
            var result=conn.ExecuteQuery("select * from Phong p left join loaiphong l on p.maloaiphong=l.maloaiphong");
            conn.CloseConnection();
            List<Phong> list = JArray.FromObject(result).ToObject<List<Phong>>();
            return list;
        }
    }
}
