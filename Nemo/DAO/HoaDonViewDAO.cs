using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nemo.DTO;
using Npgsql;
using JsonConverter;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Data;

namespace Nemo.DAO
{
    internal class HoaDonViewDAO
    {
        public HoaDonViewDAO() { }
        public ObservableCollection<HoaDon> GetListHoaDon()
        {
            var conn = new ConnectDB();
            conn.OpenConnection();
            var result = conn.ExecuteQuery(@"select hd.mahd as mahoadon,
		                                            ptp.ngaytra as ngaythanhtoan,
		                                            kh.tenkh as khachhang,
		                                            count(ptp.maptp) as sophongthanhtoan,
		                                            sum(ptp.tien) as tongtien
                                            from hoadonthanhtoan hd
		                                            left join phieuthuephong ptp on hd.mahd = ptp.mahd
		                                            join khachhang kh on hd.makh = kh.makh
                                            group by mahoadon, ngaythanhtoan, khachhang
                                            order by ngaythanhtoan desc");
            conn.CloseConnection();
            ObservableCollection<HoaDon> list = JArray.FromObject(result).ToObject<ObservableCollection<HoaDon>>();
            for (int i = 0; i < list.Count(); i++)
            {
                list[i].stt = i + 1;
            }
            return list;
        }
        public ObservableCollection<HoaDon> GetListHoaDon(int sokhachquydinh, float phuthusokhach, float tilekhachnuocngoai)
        {
            var conn = new ConnectDB();
            conn.OpenConnection();
            var ptsk = phuthusokhach.ToString();
            var tlknn = tilekhachnuocngoai.ToString();
            var result = conn.ExecuteQuery(@$"select hd.mahd as mahoadon,
													ptp.ngaytra as ngaythanhtoan,
													kh.tenkh as khachhang,
													count(ptp.maptp) as sophongthanhtoan,
													t.tongtien
											from hoadonthanhtoan hd
													left join phieuthuephong ptp on hd.mahd = ptp.mahd
													join khachhang kh on hd.makh = kh.makh
													join
													(select hd.mahd, sum(ptp.tongtienptp) as tongtien
													from hoadonthanhtoan hd join
													(select ptp.maptp, ptp.mahd, 
													case
														when (k.sokhach < {sokhachquydinh}) and (k.kh_nuocngoai is null) then (ptp.ngaytra - ptp.ngaythue) * lp.gia
														when (k.sokhach < {sokhachquydinh}) and (k.kh_nuocngoai = 1) then (ptp.ngaytra - ptp.ngaythue) * lp.gia * {tlknn}
														when (k.sokhach = {sokhachquydinh}) and (k.kh_nuocngoai is null) then (ptp.ngaytra - ptp.ngaythue) * lp.gia * {phuthusokhach}
														else (ptp.ngaytra - ptp.ngaythue) * lp.gia * {ptsk} * {tlknn}
													end
													as tongtienptp
													from phieuthuephong ptp join phong p on ptp.maphongthue = p.maphong
															join loaiphong lp on p.maloaiphong = lp.maloaiphong
															join
													(select ptp2.maptp, ptp2.maphongthue as maphong, ptp4.kh_nuocngoai, count(lstp2.makh) as sokhach
													from phieuthuephong ptp2 
															join lichsuthuephong lstp2 on ptp2.maptp = lstp2.maptp
															left join
															(select distinct ptp3.maptp, ptp3.maphongthue as maphong,
																case
																	when kh_nuocngoai.makh is not null then 1
																	else 0
																end
																as kh_nuocngoai
															from phieuthuephong ptp3 join lichsuthuephong lstp3 on ptp3.maptp = lstp3.maptp
																					join kh_nuocngoai on lstp3.makh = kh_nuocngoai.makh
															where ptp3.mahd is not null) ptp4
															on ptp2.maptp = ptp4.maptp
															join phong p on ptp2.maphongthue = p.maphong
															join loaiphong lp on p.maloaiphong = lp.maloaiphong
													where ptp2.mahd is not null
													group by ptp2.maptp, ptp2.maphongthue, kh_nuocngoai) k on k.maptp = ptp.maptp
													where ptp.mahd is not null) ptp
													on hd.mahd = ptp.mahd
													group by hd.mahd) t on t.mahd = hd.mahd
											group by mahoadon, ngaythanhtoan, khachhang, tongtien
											order by ngaythanhtoan desc");
            conn.CloseConnection();
            ObservableCollection<HoaDon> list = JArray.FromObject(result).ToObject<ObservableCollection<HoaDon>>();
            for (int i = 0; i < list.Count(); i++)
            {
                list[i].stt = i + 1;
            }
            return list;
        }
		public int createHoaDon(int makh)
		{
            var conn = new ConnectDB();
            conn.OpenConnection();

            var result = conn.ExecuteQuery($@"INSERT INTO hoadonthanhtoan (makh) VALUES ({makh}) RETURNING mahd");
            int mahd = 0;

            object value = result.Rows[0]["mahd"];
            mahd = value != DBNull.Value ? Convert.ToInt32(value) : 0;

            conn.CloseConnection();
			return mahd;
        }
    }
}
