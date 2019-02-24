using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Torneio.Web.Models
{
    public class Chaves
    {
        public static string OITAVAS = "Oitavas";
        public static string QUARTAS = "Quartas";
        public static string SEMINIFINAL = "Semifinal";
        public static string FINAL = "Final";
        public static List<string> ChavesList = new List<string>() { OITAVAS, QUARTAS, SEMINIFINAL, FINAL };
    }
}