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
using LiveChartsCore.Geo;
using static System.Net.WebRequestMethods;

namespace Nemo.DAO
{
    internal class PhongViewDAO
    {
        public ConnectDB conn;
        public PhongViewDAO() {
            conn = new ConnectDB();
        }
      
        public ObservableCollection<Phong> GetListPhong()
        {
            var result=conn.ExecuteQuery("select * from Phong p left join loaiphong l on p.maloaiphong=l.maloaiphong order by p.maphong asc");
            if (result == null) return null;
            ObservableCollection<Phong> list = JArray.FromObject(result).ToObject<ObservableCollection<Phong>>();
            return list;
        }
        public ObservableCollection<Phong> GetListPhongLoai(string loai)
        {
            var result = conn.ExecuteQuery(@$"select * from Phong p left join loaiphong l on p.maloaiphong=l.maloaiphong where p.tinhtrang=E'{loai}'");
            if (result == null) return null;
            ObservableCollection<Phong> list = JArray.FromObject(result).ToObject<ObservableCollection<Phong>>();
            return list;
        }
        public DataTable GetListLoaiPhong()
        {
            var result = conn.ExecuteQuery("Select maloaiphong from loaiphong");
            return result;
        }
        public string checkTinhTrang(int maphong, int malp = 0)
        {
            if (malp == 0)
            {
                var conn = new ConnectDB();
                conn.OpenConnection();
                var result = conn.ExecuteQuery($"select tinhtrang from phong where maphong = '{maphong}'");
                conn.CloseConnection();

                if (result == null)
                {
                    return null;
                }

                object value = result.Rows[0]["tinhtrang"];
                return value.ToString();
            }
            else
            {
                var conn = new ConnectDB();
                conn.OpenConnection();
                var result = conn.ExecuteQuery($"select tinhtrang from phong where maphong = '{maphong}' and maloaiphong = {malp}");
                conn.CloseConnection();

                if (result == null)
                {
                    return null;
                }

                object value = result.Rows[0]["tinhtrang"];
                return value.ToString();
            }
        }
        public void updateTinhTrang(int maphong, int maptp, bool done = false, bool reserve = false, bool empty = false)
        {
            var conn = new ConnectDB();
            conn.OpenConnection();

            if (empty == true)
            {
                conn.ExecuteQuery(@$"update phong
									set tinhtrang = 'Còn trống'
									where maphong = {maphong}");
                return;
            }
            else
            {
                if (done == true)
                {
                    if (reserve == false)
                    {
                        conn.ExecuteQuery(@$"update phong
									set tinhtrang = 'Đã thuê'
									where maphong = {maphong}");
                        return;
                    }
                    else
                    {
                        conn.ExecuteQuery(@$"update phong
									set tinhtrang = 'Đang đợi'
									where maphong = {maphong}");
                        return;
                    }
                }
                else
                {
                    if (reserve == false)
                    {
                        conn.ExecuteQuery(@$"UPDATE phong
								SET tinhtrang = 
									CASE
										WHEN (
											SELECT COUNT(*)
											FROM lichsuthuephong lsp
											JOIN phieuthuephong ptp ON lsp.maptp = ptp.maptp
											WHERE ptp.maptp = {maptp}
										) = (
											SELECT sl_khachtoida
											FROM quydinh
											WHERE maqd = (
												SELECT maqd
												FROM phieuthuephong
												WHERE maptp = {maptp}
												LIMIT 1
											)
										) THEN CAST('Đã thuê' AS room_status)
										ELSE CAST('Đang xử lí' AS room_status)
									END
								WHERE maphong = {maphong};");
                    }
                    else
                    {
                        conn.ExecuteQuery(@$"UPDATE phong
								SET tinhtrang = 
									CASE
										WHEN (
											SELECT COUNT(*)
											FROM lichsuthuephong lsp
											JOIN phieuthuephong ptp ON lsp.maptp = ptp.maptp
											WHERE ptp.maptp = {maptp}
										) = (
											SELECT sl_khachtoida
											FROM quydinh
											WHERE maqd = (
												SELECT maqd
												FROM phieuthuephong
												WHERE maptp = {maptp}
												LIMIT 1
											)
										) THEN CAST('Đang đợi' AS room_status)
										ELSE CAST('Đang xử lí' AS room_status)
									END
								WHERE maphong = {maphong};");

                    }
                }
            }
            conn.CloseConnection();
        }
        public bool checkFull(int maphong)
        {
            var conn = new ConnectDB();
            conn.OpenConnection();

            var result = conn.ExecuteQuery(@$"select tinhtrang
                                                from phong
                                                where maphong = {maphong}");
            conn.CloseConnection();
            if (result.Rows[0]["tinhtrang"].ToString() == "Đã thuê")
                return true;
            return false;
        }
        public string isReserveAfterward(int maphong)
        {
            var conn = new ConnectDB();
            conn.OpenConnection();

            var result = conn.ExecuteQuery(@$"SELECT ptp.ngaythue
                                            FROM phieuthuephong ptp
                                            WHERE mahd IS NULL
                                                AND tien IS NULL
                                                AND maphongthue = {maphong}
                                                AND ngaythue = (
                                                SELECT MAX(ngaythue)
                                                FROM phieuthuephong
                                                WHERE mahd IS NULL
                                                    AND tien IS NULL
                                                    AND maphongthue = {maphong});");
            conn.CloseConnection();

            return result.Rows[0]["ngaythue"].ToString();
        }
        public string isReservePrevious(int maphong)
        {
            var conn = new ConnectDB();
            conn.OpenConnection();

            var result = conn.ExecuteQuery(@$"SELECT ptp.*
                                            FROM phieuthuephong ptp
                                            WHERE mahd IS NULL
                                                AND tien IS NULL
                                                AND maphongthue = {maphong}
                                                AND ngaythue = (
                                                SELECT MIN(ngaythue)
                                                FROM phieuthuephong
                                                WHERE mahd IS NULL
                                                    AND tien IS NULL
                                                    AND maphongthue = {maphong});");
            conn.CloseConnection();

            return result.Rows[0]["ngaythue"].ToString();
        }
    }
}
