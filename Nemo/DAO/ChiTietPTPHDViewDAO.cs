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
    internal class ChiTietPTPHDViewDAO
    {
        public ChiTietPTPHDViewDAO() { }
        public ObservableCollection<ChiTietPTPHD> GetListChiTietPTPHD(int maptp)
        {
            var conn = new ConnectDB();
            conn.OpenConnection();
            var result = conn.ExecuteQuery($@"select kh.tenkh,
                                                case
	                                                when tn.cccd is null then 'Nước ngoài'
	                                                else 'Nội địa'
                                                end
                                                as loaikhach,
                                                case
	                                                when tn.cccd is null then nn.passport
	                                                else tn.cccd
                                                end
                                                as dinhdanh,
                                                kh.diachi
                                                from lichsuthuephong ls join khachhang kh on ls.makh = kh.makh
						                                                left join kh_trongnuoc tn on ls.makh = tn.makh
						                                                left join kh_nuocngoai nn on ls.makh = nn.makh
                                                where maptp = {maptp}");
            conn.CloseConnection();
            ObservableCollection<ChiTietPTPHD> list = JArray.FromObject(result).ToObject<ObservableCollection<ChiTietPTPHD>>();
            for (int i = 0; i < list.Count(); i++)
            {
                list[i].stt = i + 1;
            }
            return list;
        }
        public DataTable GetNgayThueNgayTra(int maptp)
        {
            var conn = new ConnectDB();
            conn.OpenConnection();
            var result = conn.ExecuteQuery($"select ngaythue, ngaytra from phieuthuephong where maptp = {maptp}");
            conn.CloseConnection();
            return result;
        }
    }
}
