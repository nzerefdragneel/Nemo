using Nemo.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemo.DAO
{
    class PhieuThuePhongViewDAO
    {
        public PhieuThuePhongViewDAO() { }
        public ObservableCollection<PhieuThuePhong> GetListPTP()
        {
            var conn = new ConnectDB();
            conn.OpenConnection();

            var result = conn.ExecuteQuery(@"SELECT
												ptp.*,
												COUNT(ls.maPTP) AS sokhach,
												COUNT(DISTINCT CASE
														WHEN kn.makh IS NOT NULL THEN kn.passport
														ELSE NULL
													END) AS sokhachnuocngoai,
												qd.hesokhachnuocngoai,
												qd.tilephuthu,
												lp.gia,
												CASE
													WHEN COUNT(DISTINCT CASE
														WHEN kn.makh IS NOT NULL THEN kn.passport
														ELSE NULL
														END) = 0 AND COUNT(ls.maPTP) < qd.sl_khachtoida THEN (CURRENT_DATE - ptp.ngaythue) * lp.gia
													WHEN COUNT(DISTINCT CASE
															WHEN kn.makh IS NOT NULL THEN kn.makh
															ELSE NULL
														END) > 0 AND COUNT(ls.maPTP) < qd.sl_khachtoida THEN 
															(CURRENT_DATE - ptp.ngaythue) * lp.gia * qd.hesokhachnuocngoai
													WHEN COUNT(DISTINCT CASE
														WHEN kn.makh IS NOT NULL THEN kn.passport
														ELSE NULL
														END) = 0 AND COUNT(ls.maPTP) = qd.sl_khachtoida THEN (CURRENT_DATE - ptp.ngaythue) * lp.gia * (1 + qd.tilephuthu)
													ELSE (CURRENT_DATE - ptp.ngaythue) * lp.gia * qd.hesokhachnuocngoai * (1 + qd.tilephuthu)
												END AS tienthue
											FROM
												phieuthuephong ptp
												LEFT JOIN lichsuthuephong ls ON ptp.maptp = ls.maptp
												LEFT JOIN khachhang kh ON ls.makh = kh.makh
												LEFT JOIN kh_trongnuoc tn ON kh.makh = tn.makh
												LEFT JOIN kh_nuocngoai kn ON kh.makh = kn.makh
												JOIN phong ph ON ph.maphong = ptp.maphongthue
												JOIN loaiphong lp ON lp.maloaiphong = ph.maloaiphong
												JOIN quydinh qd ON qd.maqd = ptp.maqd
											WHERE
												ptp.maHD IS NULL
											GROUP BY
												ptp.maptp, qd.hesokhachnuocngoai, qd.tilephuthu, lp.gia, ptp.ngaythue, qd.sl_khachtoida
											order by ptp.ngaythue desc");

            conn.CloseConnection();
            ObservableCollection<PhieuThuePhong> list = JArray.FromObject(result)
                                                              .ToObject<ObservableCollection<PhieuThuePhong>>();

            return list;
        }
    }
}
