using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Final_Semester__NT106_
{
    [Serializable]
    class ThongTin
    { 
        public string Ten { get; set; }
        public long MSSV { get; set; }
        public string Lop { get; set; }
        public string Khoa { get; set; }
        public override string ToString()
        {
            return Ten + "\n" + MSSV + "\n" + Lop + "\n" + Khoa ;

        }
    }
}
