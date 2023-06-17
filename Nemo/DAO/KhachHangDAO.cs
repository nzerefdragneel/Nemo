using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemo.DAO
{
    public class KhachHangDAO
    {
        public ConnectDB conn;
        public KhachHangDAO() 
        {
            conn = new ConnectDB();
            conn.OpenConnection();
        }
        ~KhachHangDAO()
        {
            conn.CloseConnection();
        }
        public bool checkKhachNuocNgoai(int makh)
        {
            var rs = conn.ExecuteQuery($"select * from khachhang k join kh_nuocngoai n on k.makh = n.makh where k.makh={makh}");
            if (rs == null) return false;
            return true;
        }
    }
}
