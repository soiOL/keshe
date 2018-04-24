using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test;

namespace test1
{
    //发送的数据类型
    public class InfoStruct : BaseStruct
    {
        public int Intensity { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public bool FanOk { get; set; }
        public bool TahOk { get; set; }
        public bool IsOk { get; set; }
        public string Date { get; set; }
        public string BaseBoard { get; set; }
    }

   
}
