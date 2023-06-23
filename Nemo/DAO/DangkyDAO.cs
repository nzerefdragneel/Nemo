﻿using Microsoft.VisualBasic;
using Nemo.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

namespace Nemo.DAO
{
    internal class DangkyDAO
    {
        public ConnectDB conn;
        public DangkyDAO() {
            conn = new ConnectDB();
        }
        public bool CheckUserName(string username)
        {
            if (username == "") return false;
            var query = "select * from Taikhoan where tentk=\'"+username+"\'";
            var result = conn.ExecuteQuery(query);
            if (result!= null) return false;
            return true;
        }
        public void Themtk(TaiKhoan tk)
        {
            var query = "INSERT INTO TaiKhoan (TenTK, MatKhau, Muoi) VALUES (\'"+tk.TenTK+"\',\'"+ tk.MatKhau+"\',\'"+ tk.Muoi+"\')";
            conn.UpdateQuery(query);

        }
        public void DoiMatKhau(TaiKhoan tk)
        {
            var query = "UPDATE TaiKhoan SET MatKhau =\'" + tk.MatKhau + "\',Muoi=\'" + tk.Muoi + "\' where TenTK=\'" + tk.TenTK+"\'";
            conn.UpdateQuery(query);

        }
    }
}