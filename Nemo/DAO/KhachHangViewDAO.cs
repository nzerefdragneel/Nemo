using Nemo.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;

namespace Nemo.DAO
{
    internal class KhachHangViewDAO
    {
        public KhachHangViewDAO() { }
        public KhachHang getMaKhachHangByDinhDanh(string dinhDanh)
        {
            var conn = new ConnectDB();
            conn.OpenConnection();

            var result = conn.ExecuteQuery($@"SELECT kh.*, nn.passport, tn.cccd
                                            FROM khachhang kh
                                            LEFT JOIN kh_nuocngoai nn ON kh.makh = nn.makh
                                            LEFT JOIN kh_trongnuoc tn ON kh.makh = tn.makh
                                            WHERE nn.passport = '{dinhDanh}'
                                               OR tn.cccd = '{dinhDanh}';");

            conn.CloseConnection();
            if (result == null)
                return null;

            ObservableCollection<KhachHang> list = JArray.FromObject(result)
                                                              .ToObject<ObservableCollection<KhachHang>>();
            return list[0];
        }
        public int addKhachHang(KhachHang kh)
        {
            var conn = new ConnectDB();
            conn.OpenConnection();

            if (kh.loaiKH == "Khách trong nước")
            {
                var conn2 = new ConnectDB();
                conn2.OpenConnection();
                var check = conn2.ExecuteQuery(@$"select makh
                                                from kh_trongnuoc
                                                where cccd = '{kh.dinhDanh}'");
                if (check == null)
                {
                    var res = conn.ExecuteQuery($@"INSERT INTO khachhang (tenkh, diachi)
                                                VALUES ('{kh.TenKH}', '{kh.DiaChi}');

                                                INSERT INTO kh_trongnuoc (makh, cccd)
                                                SELECT makh, '{kh.dinhDanh}'
                                                FROM khachhang
                                                WHERE makh = (SELECT makh from khachhang order by makh desc limit 1)
                                                RETURNING makh;");
                    conn.CloseConnection();
                    return Convert.ToInt32(res.Rows[0]["makh"]);
                }
                else
                {
                    return Convert.ToInt32(check.Rows[0]["makh"]);
                }
            }
            else
            {
                var conn2 = new ConnectDB();
                conn2.OpenConnection();
                var check = conn2.ExecuteQuery(@$"select makh
                                                from kh_nuocngoai
                                                where passport = '{kh.dinhDanh}'");
                if (check == null)
                {
                    var res = conn.ExecuteQuery($@"INSERT INTO khachhang (tenkh, diachi)
                                                VALUES ('{kh.TenKH}', '{kh.DiaChi}');

                                                INSERT INTO kh_nuocngoai (makh, passport)
                                                SELECT makh, '{kh.dinhDanh}'
                                                FROM khachhang
                                                WHERE makh = (SELECT makh from khachhang order by makh desc limit 1)
                                                RETURNING makh;");
                    conn.CloseConnection();
                    return Convert.ToInt32(res.Rows[0]["makh"]);
                }
                else
                {
                    return Convert.ToInt32(check.Rows[0]["makh"]);
                }
            }
        }
    }
}
