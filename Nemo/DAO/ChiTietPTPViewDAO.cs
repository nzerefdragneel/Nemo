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
    class ChiTietPTPViewDAO
    {
        public ChiTietPTPViewDAO() { }
        public ObservableCollection<ChiTietPhieuThuePhong> GetListPTP(int maptp)
        {
            var conn = new ConnectDB();
            conn.OpenConnection();

            var result = conn.ExecuteQuery(@$"SELECT
                                                kh.tenkh,
                                                CASE
                                                    WHEN tn.makh IS NOT NULL THEN 'Trong nước'
                                                    WHEN kn.makh IS NOT NULL THEN 'Quốc tế'
                                                    ELSE ''
                                                END AS loaikhach,
                                                CASE
                                                    WHEN kn.makh IS NOT NULL THEN qd.hesokhachnuocngoai
                                                    ELSE 1
                                                END AS heso,
                                                CASE
                                                    WHEN kn.makh IS NOT NULL THEN kn.passport
                                                    ELSE tn.cccd
                                                END AS dinhdanh,
                                                kh.diachi
                                            FROM
                                                khachhang kh
                                                LEFT JOIN kh_trongnuoc tn ON kh.makh = tn.makh
                                                LEFT JOIN kh_nuocngoai kn ON kh.makh = kn.makh
                                                LEFT JOIN lichsuthuephong ls ON kh.makh = ls.makh
                                                LEFT JOIN phieuthuephong ptp ON ls.maptp = ptp.maptp
                                                CROSS JOIN (SELECT hesokhachnuocngoai FROM quydinh) qd
                                            WHERE
                                                ptp.maptp = {maptp}");
            conn.CloseConnection();
            ObservableCollection<ChiTietPhieuThuePhong> list = JArray.FromObject(result)
                                                              .ToObject<ObservableCollection<ChiTietPhieuThuePhong>>();
            for (int i = 0; i < list.Count(); i++)
            {
                list[i].STT = i + 1;
            }

            return list;
        }
    }
}
