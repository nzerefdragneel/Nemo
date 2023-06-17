using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nemo.DTO;
using System.Data;

namespace Nemo.DAO
{
    internal class QuyDinhDAO
    {
        public ConnectDB conn;
        public QuyDinhDAO()
        {
            conn = new ConnectDB();
            conn.OpenConnection();
        }
        ~QuyDinhDAO()
        {
            conn.CloseConnection(); 
        }
        public QuyDinh getQuyDinh()
        {
            var rs = conn.ExecuteQuery("select * from quydinh");
            var Qd = new QuyDinh();
            Qd.tiLePhuThu = Convert.ToSingle(rs.Rows[0]["tilephuthu"]);
            Qd.heSoKhachNN = Convert.ToSingle(rs.Rows[0]["hesokhachnuocngoai"]);
            Qd.soLuongKhachToiDa = Convert.ToInt32(rs.Rows[0]["sl_khachtoida"]);
            Qd.soLoaiPhong = Convert.ToInt32(rs.Rows[0]["sl_loaiphong"]);
            return Qd;
        }
        public void setQuyDinh(QuyDinh qd)
        {
            conn.ExecuteQuery($"UPDATE quydinh SET tilephuthu={qd.tiLePhuThu}, hesokhachnuocngoai={qd.heSoKhachNN}," +
                $" sl_khachtoida={qd.soLuongKhachToiDa}, sl_loaiphong={qd.soLoaiPhong}");
        }
    }
}
