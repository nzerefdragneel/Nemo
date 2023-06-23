using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nemo.DTO
{
    public class Phong:ICloneable
    {
        public int maphong { get; set; } 
        public string tinhtrang { get; set; }
        public int maloaiphong { get; set; }
        public int soluong { get; set; }
        public float gia { get; set; }
        public string tuychon { get; set; }
        public int songaythue { get; set; } = 0;


        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
