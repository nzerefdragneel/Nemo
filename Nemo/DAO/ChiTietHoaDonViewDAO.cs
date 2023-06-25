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
    internal class ChiTietHoaDonViewDAO
    {
        public ChiTietHoaDonViewDAO() { }
        public ObservableCollection<ChiTietHoaDon> GetListChiTietHoaDon(int mahd, int sokhachquydinh)
        {
            var conn = new ConnectDB();
            conn.OpenConnection();
            var result = conn.ExecuteQuery(@$"select distinct ptp.maptp, ptp.maphongthue as maphong, ptp.ngaytra - ptp.ngaythue as songaythue, dk.sokhach, lp.gia as dongia,
											ptp.tien as tongtien,
											case
												when dk.sokhach < {sokhachquydinh} and dk.kh_nuocngoai is null then null
												when dk.sokhach = {sokhachquydinh} and dk.kh_nuocngoai is null then 'Phụ thu số khách'
												when dk.sokhach < {sokhachquydinh} and dk.kh_nuocngoai = 1 then 'Khách nước ngoài'
												else 'Phụ thu số khách + Khách nước ngoài'
											end
											as ghichu
											from phieuthuephong ptp 
												join phong p on ptp.maphongthue = p.maphong
												join lichsuthuephong lstp on ptp.maptp = lstp.maptp	
												join loaiphong lp on p.maloaiphong = lp.maloaiphong
												join quydinh qd on ptp.maqd = qd.maqd
												join (select ptp2.maptp, ptp4.kh_nuocngoai, count(lstp2.makh) as sokhach
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
														group by ptp2.maptp, ptp2.maphongthue, kh_nuocngoai) dk
														on dk.maptp = ptp.maptp
											where ptp.mahd = {mahd}
											order by maptp asc");
            conn.CloseConnection();
			if (result == null) return null;
            ObservableCollection<ChiTietHoaDon> list = JArray.FromObject(result).ToObject<ObservableCollection<ChiTietHoaDon>>();
            for (int i = 0; i < list.Count(); i++)
            {
                list[i].stt = i + 1;
            }
            return list;
        }
    }
}
