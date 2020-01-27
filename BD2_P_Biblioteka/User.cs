using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2_P_Biblioteka
{
    class User
    {
        public Nullable<decimal> Id { get; set; }
        public string Email { get; set; }
        public Nullable<double> Fees { get; set; }

        public string levelAccess { get; set; }
    }
}
