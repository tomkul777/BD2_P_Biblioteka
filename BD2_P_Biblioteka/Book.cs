using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD2_P_Biblioteka
{
    class Book
    {
        public Nullable<decimal> Id { get; set; }
        public string Autor { get; set; }
        public string Tytul { get; set; }
        public string Wydawnictwo { get; set; }
        public Nullable<decimal> RokWydania { get; set; }
        public string Opis { get; set; }

        public Nullable<decimal> Ilosc { get; set; }

        public Nullable<decimal> IdKoszyk { get; set; }
    }
}
