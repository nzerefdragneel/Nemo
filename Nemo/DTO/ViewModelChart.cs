using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LiveCharts;

namespace Nemo.DTO;
/*
 */
public class ViewModelChart:INotifyPropertyChanged
{
    public IEnumerable<ISeries> Chart_DoanhThuThang { get; set; }
    private IEnumerable<ISeries> _chartDoanhThuThang;


    public event PropertyChangedEventHandler PropertyChanged;



    public Axis[] XAxes { get; set; }
    public IEnumerable<ISeries> Chart_DoanhThuNam { get; set; }
    public IEnumerable<ISeries> Chart_MatDoNam { get; set; }
    public Axis[] YAxes { get; set; }

    public IEnumerable<ISeries> create_chart_DoanhThuThang(ObservableCollection<DoanhThu> dt)
    {
        List<ISeries> list = new List<ISeries>();
        foreach(DoanhThu d in dt) { 
            list.Add(new PieSeries<double> { Values = new double[] { d.doanhthutheomaloai }, Name = $"Loại {d.maloai}" }); 
        }
        return list;
    }
    public void create_chart_DoanhThuNam(int year)
    {
        var con = new DoanhThuView();
        List<DoanhThu> data = con.tinhDoanhThuThang(year);
        List<double> doanhthu = new List<double>();
        List<string> name = new List<string>();    
        foreach(var d in data)
        {
            doanhthu.Add(d.total);
            name.Add($"Tháng {d.month}");
        }
        Chart_DoanhThuNam = new ISeries[] {
            new ColumnSeries<double>
            {
             Name= "Doanh Thu",
            Values = doanhthu.ToArray(),//
            }
        };
        XAxes = new Axis[]
{
            new Axis
            {
                Labels = name.ToArray(),
            }
        };
    }
    public void create_chart_MatDoNam(int year)
    {
        var con = new MatDoView();
        var data = con.setMatDoNam(year);
        List<double> matdo = new List<double>();
        List<string> name = new List<string>();
        foreach (var d in data)
        {
            matdo.Add(d.songaythue);
            name.Add($"Phòng {d.maphong}");
        }
        Chart_MatDoNam = new ISeries[] {
            new RowSeries<double>
            {
            Name = "Số ngày cho thuê",
            Values = matdo.ToArray(),
            }
        };
        YAxes = new Axis[]
        {
            new Axis
            {
                Labels = name.ToArray(),
            }
        };
    }

}