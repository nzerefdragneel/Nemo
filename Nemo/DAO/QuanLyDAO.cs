using Nemo.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemo.DAO
{
    class QuanLyDAO
    {
        public ConnectDB conn;
        public QuanLyDAO()
        {
            conn = new ConnectDB();
        }

        public ObservableCollection<TaiKhoan> GetListNhanVien()
        {
            var result = conn.ExecuteQuery("select * from taikhoan ");
            if (result == null) return null;
            ObservableCollection<TaiKhoan> list = JArray.FromObject(result).ToObject<ObservableCollection<TaiKhoan>>();
            return list;
        }
    }
}
