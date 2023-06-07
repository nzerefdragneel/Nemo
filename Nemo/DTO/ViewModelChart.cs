using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Nemo.DTO;
/*
 */
public partial class ViewModelChart
{
    public ViewModelChart()
    {

        DoanhThuThang = new ISeries[]
        {
           new PieSeries<double> { Values = new double[] { 100 }, Name ="Loai A" },
           new PieSeries<double> { Values = new double[] { 2000 }, Name = "Loai B" },
           new PieSeries<double> { Values = new double[] { 180 }, Name = "Loai C" }
        };

        
        DoanhThuNam = new ISeries[] {
            new ColumnSeries<double>
            {
            Values = new double[] {100,200,80,500}
            }
        };
        XAxes =new Axis[] 
        {
        new Axis
        {
            Labels = new string[] { "Thang 1", "Thang 2", "Thang 3","Thang 4" },
            LabelsRotation = 0,
            SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
            SeparatorsAtCenter = false,
            TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
            TicksAtCenter = true
        }
    };
}

    public IEnumerable<ISeries> DoanhThuThang { get; set; }
    public Axis[] XAxes { get; set; }
public IEnumerable<ISeries> DoanhThuNam { get; set; }
 
}