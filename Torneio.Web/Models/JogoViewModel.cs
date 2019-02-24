using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Torneio.Web.Models
{
    public class JogoViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Número da Partida")]
        public int NumPartida { get; set; }

        [Display(Name = "Dia do Jogo")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataDaPartida { get; set; }

        [Display(Name = "Chave")]
        public string Chave { get; set; }

        public int TimeId1 { get; set; }
        [Display(Name = "Time Casa")]
        public string NameTimeId1 { get; set; }

        public int TimeId2 { get; set; }
        [Display(Name = "Time Fora")]
        public string NameTimeId2 { get; set; }

        [Display(Name = "Gols Time Casa")]
        public int GolsTimeId1 { get; set; }
        [Display(Name = "Gols Time Fora")]
        public int GolsTimeId2 { get; set; }

        public int TimeVencedor { get; set; }
        [Display(Name = "Time Vencedor")]
        public string NameTimeVencedor { get; set; }

        [Display(Name = "Jogo terminou?")]
        public bool Terminou { get; set; }
    }
}