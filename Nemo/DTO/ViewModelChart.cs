using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemo.DTO
{
    public class ViewModelChart
    {
        public ObservableCollection<DoanhThu> Data{ set; get; }
        public ViewModelChart()
        {
            Data = new ObservableCollection<DoanhThu> {
                new DoanhThu {month="1",total = 100},
                new DoanhThu {month="2",total = 150},
                new DoanhThu {month="3",total = 100},
                new DoanhThu {month="4",total = 170 }
            };
        }
    }
}
