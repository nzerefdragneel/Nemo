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
    }
}
