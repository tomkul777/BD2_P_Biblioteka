using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2_P_Biblioteka
{
    class BorrowedBook
    {
        public Nullable<decimal> Id { get; set; }
        public Nullable<decimal> IdUser { get; set; }
        public Nullable<decimal> IdBook { get; set; }
        public string Autor { get; set; }
        public string Tytul { get; set; }
        public string DataWypozyczenia { get; set; }
        public string DataOdbioru { get; set; }
        public string DataOddania { get; set; }
        public Nullable<decimal> IloscPrzedluzen { get; set; }
    }
}
