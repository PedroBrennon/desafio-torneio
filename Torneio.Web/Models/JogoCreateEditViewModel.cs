using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Torneio.Web.Models
{
    public class JogoCreateEditViewModel
    {
        public int Id { get; set; }

        public int NumPartida { get; set; }

        [Display(Name = "Data da Partida")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataDaPartida { get; set; }

        public string Chave { get; set; }
        public List<SelectListItem> Chaves { get; set; }

        public int TimeId1 { get; set; }
        public List<SelectListItem> NameTimeId1 { get; set; }

        public int TimeId2 { get; set; }
        public List<SelectListItem> NameTimeId2 { get; set; }

        [Display(Name = "Gols Time Casa")]
        public int GolsTimeId1 { get; set; }
        [Display(Name = "Gols Time Fora")]
        public int GolsTimeId2 { get; set; }

        public int TimeVencedor { get; set; }
        public List<SelectListItem> NameTimeVencedor { get; set; }

        [Display(Name = "Jogo terminou?")]
        public bool Terminou { get; set; }
    }
}