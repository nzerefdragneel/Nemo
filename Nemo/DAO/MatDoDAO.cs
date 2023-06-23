using Nemo.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace Nemo.DAO
{
    public class MatDoDAO
    {
        public ConnectDB conn;
        public MatDoDAO() 
        { 
            conn = new ConnectDB();
            conn.OpenConnection();
        }
        ~MatDoDAO()
        {
            conn.CloseConnection();
        }
        public DataTable getPhongThue()
        {
            var rs = conn.ExecuteQuery("select * from phieuthuephong p join phong l on p.maphongthue = l.maphong");
            return rs;
        }
    }
}
