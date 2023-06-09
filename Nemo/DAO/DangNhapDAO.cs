﻿using Nemo.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemo.DAO
{
    public class DangNhapDAO
    {
        public ConnectDB conn;
        public DangNhapDAO()
        {
            conn = new ConnectDB();
        }
        public TaiKhoan GetUserName(string username)
        {
            if (username == "") return null;
            var query = @$"select tentk, matkhau,muoi from taikhoan where tentk='{username}'";
            var result = conn.ExecuteQuery(query);
            if (result == null) return null;
            ObservableCollection<TaiKhoan> list = JArray.FromObject(result).ToObject<ObservableCollection<TaiKhoan>>();
            if (result != null) return list[0];
            return null;
        }

      
    }
}
