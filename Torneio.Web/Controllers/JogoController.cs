using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Torneio.Web.ConnectionApi;
using Torneio.Web.Models;

namespace Torneio.Web.Controllers
{
    public class JogoController : Controller
    {
        private ConnectionApiHelper connectionApi = new ConnectionApiHelper();

        private bool VerificaTimeVencedor(JogoCreateEditViewModel jogo)
        {
            if (jogo.GolsTimeId1 > jogo.GolsTimeId2)
            {
                jogo.TimeVencedor = jogo.TimeId1;
                return true;
            } else if (jogo.GolsTimeId1 < jogo.GolsTimeId2)
            {
                jogo.TimeVencedor = jogo.TimeId2;
                return true;
            } else
            {
                return false;
            }
        }

        private JogoViewModel VerificaJogos(JogoCreateEditViewModel jogo2, List<JogoViewModel> jogos, string chave)
        {
            var jogo1 = new JogoViewModel();

            if (jogo2.NumPartida % 2 == 0)
            {
                jogo1 = jogos.Find(j => j.Chave.ToUpper().Equals(chave.ToUpper()) && j.NumPartida == jogo2.NumPartida - 1);

                return jogo1;
            }
            else
            {
                jogo1 = jogos.Find(j => j.Chave.ToUpper().Equals(chave.ToUpper()) && j.NumPartida == jogo2.NumPartida + 1);

                return jogo1;
            }
        }

        public async Task ProximaFase(JogoCreateEditViewModel jogo2)
        {
            var response = await connectionApi.GetAllJogos();
            var jogos = (List<JogoViewModel>)await response.Content.ReadAsAsync<IEnumerable<JogoViewModel>>();
            var jogo1 = new JogoViewModel();

            if (jogo2.Chave.ToUpper().Equals(Chaves.OITAVAS.ToUpper()))
            {
                jogo1 = VerificaJogos(jogo2, jogos, Chaves.OITAVAS);

                if (jogo1.Terminou.Equals(true))
                {
                    var numPartidasQuartas = jogos.Where(j => j.Chave.ToUpper().Equals(Chaves.QUARTAS.ToUpper())).Count();

                    var novoJogo = new JogoCreateEditViewModel()
                    {
                        DataDaPartida = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("pt-BR"))),
                        NumPartida = numPartidasQuartas + 1,
                        Chave = Chaves.QUARTAS,
                        TimeId1 = jogo1.TimeVencedor,
                        TimeId2 = jogo2.TimeVencedor
                    };

                    await connectionApi.InsertJogo(novoJogo);

                    return;
                }
            }
            else if (jogo2.Chave.ToUpper().Equals(Chaves.QUARTAS.ToUpper()))
            {
                jogo1 = VerificaJogos(jogo2, jogos, Chaves.QUARTAS);

                if (jogo1.Terminou.Equals(true))
                {
                    var numPartidasQuartas = jogos.Where(j => j.Chave.ToUpper().Equals(Chaves.SEMINIFINAL.ToUpper())).Count();

                    var novoJogo = new JogoCreateEditViewModel()
                    {
                        DataDaPartida = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("pt-BR"))),
                        NumPartida = numPartidasQuartas + 1,
                        Chave = Chaves.SEMINIFINAL,
                        TimeId1 = jogo1.TimeVencedor,
                        TimeId2 = jogo2.TimeVencedor
                    };

                    await connectionApi.InsertJogo(novoJogo);

                    return;
                }
            }
            else if (jogo2.Chave.ToUpper().Equals(Chaves.SEMINIFINAL.ToUpper()))
            {
                jogo1 = VerificaJogos(jogo2, jogos, Chaves.SEMINIFINAL);

                if (jogo1.Terminou.Equals(true))
                {
                    var numPartidasQuartas = jogos.Where(j => j.Chave.ToUpper().Equals(Chaves.FINAL.ToUpper())).Count();

                    var novoJogo = new JogoCreateEditViewModel()
                    {
                        DataDaPartida = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("pt-BR"))),
                        NumPartida = numPartidasQuartas + 1,
                        Chave = Chaves.FINAL,
                        TimeId1 = jogo1.TimeVencedor,
                        TimeId2 = jogo2.TimeVencedor
                    };

                    await connectionApi.InsertJogo(novoJogo);

                    return;
                }
            }
            else if (jogo2.Chave.ToUpper().Equals(Chaves.FINAL.ToUpper()))
            {

            }
            else
            {
                return;
            }
        }

        // GET: Jogo
        public async Task<ActionResult> Index()
        {
            var response = await connectionApi.GetAllJogos();

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<IEnumerable<JogoViewModel>>();
                model.OrderByDescending(jogo => jogo.DataDaPartida);

                return View(model);
            }

            return View(new List<JogoViewModel>());
        }

        // GET: Jogo/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var response = await connectionApi.GetJogoById(id);

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<JogoViewModel>();

                return View(model);
            }
            return View();
        }

        // GET: Jogo/Create
        public async Task<ActionResult> Create()
        {
            var responseJogos = await connectionApi.GetAllJogos();
            var jogos = await responseJogos.Content.ReadAsAsync<IEnumerable<JogoViewModel>>();
            jogos.Where(j => j.Chave.ToUpper() == Chaves.OITAVAS.ToUpper());

            if (jogos.Where(j => j.Chave.ToUpper() == Chaves.OITAVAS.ToUpper()).Count() == 8)
            {
                return View("Error");
            }
            else
            {
                var response = await connectionApi.GetAllTimes();

                if (response.IsSuccessStatusCode)
                {
                    var modelTimes = await response.Content.ReadAsAsync<IEnumerable<TimeViewModel>>();
                    var model = new JogoCreateEditViewModel();
                    var selectList1 = new List<SelectListItem>();
                    var selectList2 = new List<SelectListItem>();
                    var selectListChaves = new List<SelectListItem>();

                    foreach (var ch in Chaves.ChavesList)
                    {
                        if (ch.ToString().Equals(Chaves.OITAVAS))
                        {
                            var select = new SelectListItem
                            {
                                Value = ch.ToString(),
                                Text = ch.ToString(),
                                Selected = ch.ToString() == model.Chave
                            };
                            selectListChaves.Add(select);
                        }
                    }
                    model.Chaves = selectListChaves;

                    foreach (var t in modelTimes)
                    {
                        var time1 = new SelectListItem
                        {
                            Value = ((int)t.Id).ToString(),
                            Text = t.Nome,
                            Selected = t.Id == model.TimeId1
                        };
                        var time2 = new SelectListItem
                        {
                            Value = ((int)t.Id).ToString(),
                            Text = t.Nome,
                            Selected = t.Id == model.TimeId2
                        };
                        selectList1.Add(time1);
                        selectList2.Add(time2);
                    }
                    model.NameTimeId1 = selectList1;
                    model.NameTimeId2 = selectList2;

                    return View(model);
                }

                return View();
            }
        }

        // POST: Jogo/Create
        [HttpPost]
        public async Task<ActionResult> Create(JogoCreateEditViewModel jogo)
        {
            try
            {
                var response = await connectionApi.GetAllJogos();
                var jogos = (List<JogoViewModel>)await response.Content.ReadAsAsync<IEnumerable<JogoViewModel>>();

                var numPartidas = jogos.Where(j => j.Chave.ToUpper().Equals(Chaves.OITAVAS.ToUpper())).Count();
                jogo.NumPartida = numPartidas + 1;
                jogo.DataDaPartida = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("pt-BR")));

                await connectionApi.InsertJogo(jogo);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Jogo/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var responseTimes = await connectionApi.GetAllTimes();
            var response = await connectionApi.GetJogoById(id);

            if (response.IsSuccessStatusCode && responseTimes.IsSuccessStatusCode)
            {
                var modelTimes = await responseTimes.Content.ReadAsAsync<IEnumerable<TimeViewModel>>();
                var modelJogoView = await response.Content.ReadAsAsync<JogoViewModel>();

                var model = new JogoCreateEditViewModel()
                {
                    NumPartida = modelJogoView.NumPartida,
                    DataDaPartida = modelJogoView.DataDaPartida,
                    Chave = modelJogoView.Chave,
                    TimeId1 = modelJogoView.TimeId1,
                    TimeId2 = modelJogoView.TimeId2,
                    GolsTimeId1 = modelJogoView.GolsTimeId1,
                    GolsTimeId2 = modelJogoView.GolsTimeId2,
                    TimeVencedor = modelJogoView.TimeVencedor,
                    Terminou = modelJogoView.Terminou
                };
                var selectListChaves = new List<SelectListItem>();

                var select = new SelectListItem
                {
                    Value = model.Chave.ToString(),
                    Text = model.Chave.ToString(),
                    Selected = model.Chave.ToString() == model.Chave
                };
                selectListChaves.Add(select);
                model.Chaves = selectListChaves;

                return View(model);
            }

            return View();
        }

        // POST: Jogo/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, JogoCreateEditViewModel jogo)
        {
            try
            {
                if (jogo.Terminou.Equals(true) && VerificaTimeVencedor(jogo))
                {
                    await ProximaFase(jogo);

                    await connectionApi.UpdateJogo(id, jogo);

                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Jogo/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var response = await connectionApi.GetJogoById(id);

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<JogoViewModel>();

                return View(model);
            }

            return View();
        }

        // POST: Jogo/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, JogoViewModel jogo)
        {
            try
            {
                await connectionApi.DeleteJogo(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Jogo/Oitavas
        public async Task<ActionResult> Oitavas()
        {
            var response = await connectionApi.GetAllJogos();

            if (response.IsSuccessStatusCode)
            {
                List<JogoViewModel> oitavas = new List<JogoViewModel>();
                var model = await response.Content.ReadAsAsync<IEnumerable<JogoViewModel>>();

                foreach (var m in model)
                {
                    if (m.Chave.ToUpper().Equals(Chaves.OITAVAS.ToUpper()))
                    {
                        oitavas.Add(m);
                    }
                }

                return View(oitavas);
            }

            return View(new List<JogoViewModel>());
        }

        // GET: Jogo/Quartas
        public async Task<ActionResult> Quartas()
        {
            var response = await connectionApi.GetAllJogos();

            if (response.IsSuccessStatusCode)
            {
                List<JogoViewModel> chaves = new List<JogoViewModel>();
                var model = await response.Content.ReadAsAsync<IEnumerable<JogoViewModel>>();

                foreach (var m in model)
                {
                    if (m.Chave.ToUpper().Equals(Chaves.QUARTAS.ToUpper()))
                    {
                        chaves.Add(m);
                    }
                }

                return View(chaves);
            }

            return View(new List<JogoViewModel>());
        }

        // GET: Jogo/Seminifinal
        public async Task<ActionResult> Semifinal()
        {
            var response = await connectionApi.GetAllJogos();

            if (response.IsSuccessStatusCode)
            {
                List<JogoViewModel> chaves = new List<JogoViewModel>();
                var model = await response.Content.ReadAsAsync<IEnumerable<JogoViewModel>>();

                foreach (var m in model)
                {
                    if (m.Chave.ToUpper().Equals(Chaves.SEMINIFINAL.ToUpper()))
                    {
                        chaves.Add(m);
                    }
                }

                return View(chaves);
            }

            return View(new List<JogoViewModel>());
        }

        // GET: Jogo/Final
        public async Task<ActionResult> Final()
        {
            var response = await connectionApi.GetAllJogos();

            if (response.IsSuccessStatusCode)
            {
                JogoViewModel chaves = new JogoViewModel();
                var model = await response.Content.ReadAsAsync<IEnumerable<JogoViewModel>>();

                foreach (var m in model)
                {
                    if (m.Chave.ToUpper().Equals(Chaves.FINAL.ToUpper()))
                    {
                        chaves = m;
                    }
                }

                return View(chaves);
            }

            return View(new List<JogoViewModel>());
        }
    }
}
