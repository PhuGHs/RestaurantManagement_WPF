using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Models
{
    public class Bep
    {
        private string _stt;
        public string stt { get => _stt; set => _stt = value; }
        private string _TenMon;
        public string TenMon { get => _TenMon; set => _TenMon = value; }
        private int _SoBan;
        public int SoBan { get => _SoBan; set => _SoBan = value; }
        private string _TinhTrang;
        public string TinhTrang { get => _TinhTrang; set => _TinhTrang = value; }
        public Bep(string stt, string tenMon, int soBan, string tinhTrang)
        {

            this.stt = stt;
            TenMon = tenMon;
            SoBan = soBan;
            TinhTrang = tinhTrang;
        }
    }
}
