using Fluent.Converters;
using LiveChartsCore.Geo;
using Nemo.DTO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SkiaSharp.HarfBuzz.SKShaper;
using static System.Net.WebRequestMethods;

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
												ptp.maHD IS NULL and ptp.tien is NULL
											GROUP BY
												ptp.maptp, qd.hesokhachnuocngoai, qd.tilephuthu, lp.gia, ptp.ngaythue, qd.sl_khachtoida
											order by ptp.maptp desc");

            conn.CloseConnection();
			if (result == null)
			{
				return null;
			}
            ObservableCollection<PhieuThuePhong> list = JArray.FromObject(result)
                                                              .ToObject<ObservableCollection<PhieuThuePhong>>();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].tienThue < 0) list[i].tienThue = 0;
			}

            return list;
        }
		public int[] getCountPhong()
		{
            var conn = new ConnectDB();
            conn.OpenConnection();
            var countPhong = conn.ExecuteQuery(@"SELECT
													SUM(CASE WHEN tinhtrang = 'Còn trống' THEN 1 ELSE 0 END) AS sophongcontrong,
													SUM(CASE WHEN tinhtrang = 'Đã thuê' THEN 1 ELSE 0 END) AS sophongdathue
												FROM
													phong;");

            conn.CloseConnection();

            object value1 = countPhong.Rows[0]["sophongdathue"];
            object value2 = countPhong.Rows[0]["sophongcontrong"];

            return new int[] { value1 != DBNull.Value ? Convert.ToInt32(value1) : 0, value2 != DBNull.Value ? Convert.ToInt32(value2) : 0 };
		}
		public void thanhToanPTP(int maptp, int mahd)
		{
            var conn = new ConnectDB();
            conn.OpenConnection();

			var result = conn.ExecuteQuery($@"SELECT
											CASE
												WHEN COUNT(DISTINCT CASE
													WHEN kn.makh IS NOT NULL THEN kn.passport
													ELSE NULL
													END) = 0 AND COUNT(ls.maPTP) < qd.sl_khachtoida THEN 
													(CURRENT_DATE - ptp.ngaythue + CASE WHEN CURRENT_DATE = ptp.ngaythue THEN 1 ELSE 0 END) * lp.gia
												WHEN COUNT(DISTINCT CASE
													WHEN kn.makh IS NOT NULL THEN kn.makh
													ELSE NULL
													END) > 0 AND COUNT(ls.maPTP) < qd.sl_khachtoida THEN 
													(CURRENT_DATE - ptp.ngaythue + CASE WHEN CURRENT_DATE = ptp.ngaythue THEN 1 ELSE 0 END) * lp.gia * qd.hesokhachnuocngoai
												WHEN COUNT(DISTINCT CASE
													WHEN kn.makh IS NOT NULL THEN kn.passport
													ELSE NULL
													END) = 0 AND COUNT(ls.maPTP) = qd.sl_khachtoida THEN 
													(CURRENT_DATE - ptp.ngaythue + CASE WHEN CURRENT_DATE = ptp.ngaythue THEN 1 ELSE 0 END) * lp.gia * (1 + qd.tilephuthu)
												ELSE (CURRENT_DATE - ptp.ngaythue + CASE WHEN CURRENT_DATE = ptp.ngaythue THEN 1 ELSE 0 END) * lp.gia * qd.hesokhachnuocngoai * (1 + qd.tilephuthu)
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
											ptp.maptp = {maptp}
										GROUP BY
											ptp.maptp, qd.hesokhachnuocngoai, qd.tilephuthu, lp.gia, ptp.ngaythue, qd.sl_khachtoida
										ORDER BY ptp.ngaythue DESC");
            float tienthue = 0;

            object value = result.Rows[0]["tienthue"];
            tienthue = value != DBNull.Value ? Convert.ToSingle(value) : 0;

            conn.ExecuteQuery($@"UPDATE phieuthuephong
								SET ngaytra = CURRENT_DATE, mahd = {mahd}, tien = {tienthue}
								WHERE maptp = {maptp};");

            conn.CloseConnection();
        }
		public float getTienThue(int maptp)
		{
            var conn = new ConnectDB();
            conn.OpenConnection();

            var result = conn.ExecuteQuery($@"SELECT
											CASE
												WHEN COUNT(DISTINCT CASE
													WHEN kn.makh IS NOT NULL THEN kn.passport
													ELSE NULL
													END) = 0 AND COUNT(ls.maPTP) < qd.sl_khachtoida THEN 
													(CURRENT_DATE - ptp.ngaythue + CASE WHEN CURRENT_DATE = ptp.ngaythue THEN 1 ELSE 0 END) * lp.gia
												WHEN COUNT(DISTINCT CASE
													WHEN kn.makh IS NOT NULL THEN kn.makh
													ELSE NULL
													END) > 0 AND COUNT(ls.maPTP) < qd.sl_khachtoida THEN 
													(CURRENT_DATE - ptp.ngaythue + CASE WHEN CURRENT_DATE = ptp.ngaythue THEN 1 ELSE 0 END) * lp.gia * qd.hesokhachnuocngoai
												WHEN COUNT(DISTINCT CASE
													WHEN kn.makh IS NOT NULL THEN kn.passport
													ELSE NULL
													END) = 0 AND COUNT(ls.maPTP) = qd.sl_khachtoida THEN 
													(CURRENT_DATE - ptp.ngaythue + CASE WHEN CURRENT_DATE = ptp.ngaythue THEN 1 ELSE 0 END) * lp.gia * (1 + qd.tilephuthu)
												ELSE (CURRENT_DATE - ptp.ngaythue + CASE WHEN CURRENT_DATE = ptp.ngaythue THEN 1 ELSE 0 END) * lp.gia * qd.hesokhachnuocngoai * (1 + qd.tilephuthu)
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
											ptp.maptp = {maptp}
										GROUP BY
											ptp.maptp, qd.hesokhachnuocngoai, qd.tilephuthu, lp.gia, ptp.ngaythue, qd.sl_khachtoida
										ORDER BY ptp.ngaythue DESC");
            float tienthue = 0;

            object value = result.Rows[0]["tienthue"];
            tienthue = value != DBNull.Value ? Convert.ToSingle(value) : 0;

            conn.CloseConnection();
			return tienthue;
        }
		public int addPTP(PhieuThuePhong ptp)
		{
            var conn = new ConnectDB();
			conn.OpenConnection();

			var check = conn.ExecuteQuery($@"select maptp
											from phieuthuephong
											where ngaythue = '{ptp.ngayThue}' and maphongthue = '{ptp.maPhongThue}'");

			if (check == null)
			{
				var conn2 = new ConnectDB();
				conn2.OpenConnection();
				var res = conn2.ExecuteQuery(@$"UPDATE phong
												SET tinhtrang = 
													CASE
														WHEN (
															SELECT COUNT(*)
															FROM lichsuthuephong lsp
															JOIN phieuthuephong ptp ON lsp.maptp = ptp.maptp
															WHERE ptp.maphongthue = {ptp.maPhongThue}
														) = (
															SELECT sl_khachtoida
															FROM quydinh
															WHERE maqd = (
																SELECT maqd
																FROM phieuthuephong
																WHERE maphong = {ptp.maPhongThue}
																LIMIT 1
															)
														) THEN CAST('Đã thuê' AS room_status)
														ELSE CAST('Đang xử lí' AS room_status)
													END
												WHERE maphong = {ptp.maPhongThue};

												INSERT INTO phieuthuephong (ngaythue, ngaytra, maphongthue, maqd)
												VALUES ('{ptp.ngayThue}', '{ptp.ngayTra}', {ptp.maPhongThue}, (SELECT maqd FROM quydinh ORDER BY maqd DESC LIMIT 1))
												RETURNING maptp;");
				conn2.CloseConnection();
                return Convert.ToInt32(res.Rows[0]["maptp"]);
            }
			else
			{
                conn.CloseConnection();
                return Convert.ToInt32(check.Rows[0]["maptp"]);
            }
        }
		public void huyPTP(int maptp)
		{
            var conn = new ConnectDB();
            conn.OpenConnection();

            var result = conn.ExecuteQuery(@$"update phieuthuephong
											set tien = -1 
											where maptp = {maptp}");
            conn.CloseConnection();
        }
		public int countSoKhachInPTP(int maptp)
		{
            var conn = new ConnectDB();
            conn.OpenConnection();

            var result = conn.ExecuteQuery(@$"SELECT COUNT(makh) AS SoLuongKhach
												FROM lichsuthuephong
												WHERE maptp = {maptp};");
            conn.CloseConnection();
            return Convert.ToInt32(result.Rows[0]["SoLuongKhach"]);
		}
		public void addLichSu(int maptp, int makh)
		{
            var conn = new ConnectDB();
            conn.OpenConnection();

            conn.ExecuteQuery(@$"INSERT INTO lichsuthuephong (maptp, makh)
								SELECT {maptp}, {makh}
								WHERE NOT EXISTS (
									SELECT 1
									FROM lichsuthuephong
									WHERE maptp = {maptp} AND makh = {makh});");
            conn.CloseConnection();
        }
		public int getMaPTP(string ngaythue, int maphong)
		{
            var conn = new ConnectDB();
            conn.OpenConnection();

            var result = conn.ExecuteQuery(@$"SELECT maptp
											FROM phieuthuephong
											WHERE ngaythue = '{ngaythue}' AND maphongthue = {maphong};");
            conn.CloseConnection();
            return Convert.ToInt32(result.Rows[0]["maptp"]);
		}
		public string getNgayTra(int maphong)
		{
            var conn = new ConnectDB();
            conn.OpenConnection();

            var result = conn.ExecuteQuery(@$"SELECT ptp.ngaytra	
												FROM
													phieuthuephong ptp	
												WHERE
													ptp.maHD IS NULL and ptp.maphongthue = {maphong} and tien is null");
            conn.CloseConnection();
            ObservableCollection<PhieuThuePhong> list = JArray.FromObject(result)
                                                              .ToObject<ObservableCollection<PhieuThuePhong>>();
            if (list.Count > 1)
			{
				return null; // trả về nhiều hơn 1 dòng --> phòng đã được đặt trước
			}
            return result.Rows[0]["ngaytra"].ToString();
        }
        public ObservableCollection<PhieuThuePhong> getListSelected(List<int> selectedList)
        {
            var conn = new ConnectDB();
            conn.OpenConnection();

            string query = $@"SELECT
								ptp.*,
								COUNT(ls.maPTP) AS sokhach,
								CAST(current_date - ngaythue AS INTEGER) AS songaythue,
								COUNT(DISTINCT CASE
										WHEN kn.makh IS NOT NULL THEN kn.passport
										ELSE NULL
									END) AS sokhachnuocngoai,
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
								ptp.maptp IN (";

            for (int i = 0; i < selectedList.Count; i++)
            {
                query += "'" + selectedList[i] + "'";
                if (i < selectedList.Count - 1)
                    query += ",";
            }

            query += @$")
							GROUP BY
								ptp.maptp, qd.hesokhachnuocngoai, qd.tilephuthu, lp.gia, ptp.ngaythue, qd.sl_khachtoida
							order by ptp.ngaythue desc";

            var result = conn.ExecuteQuery(query);

            conn.CloseConnection();
            ObservableCollection<PhieuThuePhong> list = JArray.FromObject(result)
                                                              .ToObject<ObservableCollection<PhieuThuePhong>>();

            for (int i = 0; i < list.Count(); i++)
            {
                list[i].STT = i + 1;
                if (list[i].sokhachnuocngoai > 0)
                {
                    list[i].ghiChu = $"{list[i].sokhachnuocngoai} khách nước ngoài";
                }
            }

            return list;
        }
		public void updateTinhTrangSauThanhToan(int maptp)
		{
            var conn = new ConnectDB();
            conn.OpenConnection();

            conn.ExecuteQuery(@$"UPDATE phong
								SET tinhtrang = 'Còn trống'
								WHERE tinhtrang = 'Đã thuê'
								AND maphong IN (
									SELECT maphongthue
									FROM phieuthuephong
									WHERE maptp = {maptp}
								)");
            conn.CloseConnection();
        }
		public bool isConflict(int maphong, string ngayThue, string ngayTra, int maptp = 0)
		{
			var conn = new ConnectDB();
			conn.OpenConnection();

			if (ngayTra == null || ngayThue == null) // check cho nhận phòng đặt trước, gia hạn thời gian trả phòng
			{
                var queryDate = ngayThue != null ? ngayThue : ngayTra;
                var result = conn.ExecuteQuery(@$"SELECT ptp.*
												FROM phieuthuephong ptp
												WHERE mahd IS NULL
													AND tien IS NULL
													AND maphongthue = {maphong}
													AND TO_DATE('{queryDate}', 'DD/MM/YYYY') BETWEEN ngaythue AND ngaytra;");
				if (result != null)
					return true;
				return false;
			}
			else // check cho thuê phòng
			{
				if (maptp == 0)
				{
					var result = conn.ExecuteQuery(@$"SELECT ptp.*
											FROM phieuthuephong ptp
											WHERE mahd IS NULL
												AND tien IS NULL
												AND maphongthue = {maphong}
												AND (ngaythue, ngaytra) OVERLAPS (TO_DATE('{ngayThue}', 'DD/MM/YYYY'), TO_DATE('{ngayTra}', 'DD/MM/YYYY'));");
					conn.CloseConnection();

					if (result != null)
						return true;
					return false;
				}
				else
				{
                    var result = conn.ExecuteQuery(@$"SELECT ptp.*
											FROM phieuthuephong ptp
											WHERE mahd IS NULL
												AND tien IS NULL
												AND maptp != {maptp}
												AND maphongthue = {maphong}
												AND (ngaythue, ngaytra) OVERLAPS (TO_DATE('{ngayThue}', 'DD/MM/YYYY'), TO_DATE('{ngayTra}', 'DD/MM/YYYY'));");
                    conn.CloseConnection();

                    if (result != null)
                        return true;
                    return false;
                }
			}
		}
		public void changeNgayThue(int maptp, string date)
		{
            var conn = new ConnectDB();
            conn.OpenConnection();

            conn.ExecuteQuery(@$"UPDATE phieuthuephong
								SET ngaythue = TO_DATE('{date}', 'DD/MM/YYYY')
								WHERE maptp = {maptp};");
            conn.CloseConnection();
        }
		public void changeNgayTra(int maptp, string date)
		{
            var conn = new ConnectDB();
            conn.OpenConnection();

            conn.ExecuteQuery(@$"UPDATE phieuthuephong
								SET ngaytra = TO_DATE('{date}', 'DD/MM/YYYY')
								WHERE maptp = {maptp};");
            conn.CloseConnection();
        }
    }
}
