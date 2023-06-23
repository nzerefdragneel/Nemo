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
            var rs = conn.ExecuteQuery("SELECT * FROM quydinh WHERE maqd = (SELECT MAX(maqd) FROM quydinh);");
            var Qd = new QuyDinh();
            DataRow r = rs.Rows[0];
            Qd.maQD = Convert.ToInt16(r["maqd"]);
            Qd.tiLePhuThu = Convert.ToSingle(r["tilephuthu"]);
            Qd.heSoKhachNN = Convert.ToSingle(r["hesokhachnuocngoai"]);
            Qd.soLuongKhachToiDa = Convert.ToInt16(r["sl_khachtoida"]);
            Qd.soLoaiPhong = Convert.ToInt16(r["sl_loaiphong"]);
            return Qd;
        }
        public void addQuyDinh(QuyDinh qd)
        {
            conn.ExecuteQuery($@"INSERT INTO quydinh(
	                            tilephuthu, hesokhachnuocngoai, sl_khachtoida, sl_loaiphong)
	                            VALUES ({qd.tiLePhuThu}, {qd.heSoKhachNN}, {qd.soLuongKhachToiDa}, {qd.soLoaiPhong});");
        }
    }
}
