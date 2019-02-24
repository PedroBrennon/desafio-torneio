using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Torneio.API.Models
{
    public class JogoBindingModel
    {
        public int Id { get; set; }
        public int NumPartida { get; set; }
        public DateTime DataDaPartida { get; set; }
        public string Chave { get; set; }
        public int TimeId1 { get; set; }
        public int TimeId2 { get; set; }
        public string NameTimeId1 { get; set; }
        public string NameTimeId2 { get; set; }
        public int GolsTimeId1 { get; set; }
        public int GolsTimeId2 { get; set; }
        public int TimeVencedor { get; set; }
        public string NameTimeVencedor { get; set; }
        public bool Terminou { get; set; }
    }
}