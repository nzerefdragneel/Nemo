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
    internal class DangkyDAO
    {
        public ConnectDB conn;
        public DangkyDAO() {
            conn = new ConnectDB();
            conn.OpenConnection();
        }
        ~DangkyDAO()
        {
            conn.CloseConnection();
        }
        public List<TaiKhoan> GetUserbyUserName(string username)
        {
            var result = conn.ExecuteQuery("select * from Phong p left join loaiphong l on p.maloaiphong=l.maloaiphong");
            List<TaiKhoan> list = JArray.FromObject(result).ToObject<List<TaiKhoan>>();
            return list;
        }
        public void Themtk(TaiKhoan tk)
        {
            var query = "INSERT INTO TaiKhoan (TenTK, MatKhau, Muoi) VALUES (\'"+tk.TenTK+"\',\'"+ tk.MatKhau+"\',\'"+ tk.Muoi+"\')";

            conn.UpdateQuery(query);

        }
    }
}
