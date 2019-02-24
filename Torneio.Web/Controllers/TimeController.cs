using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Torneio.Web.ConnectionApi;
using Torneio.Web.Models;

namespace Torneio.Web.Controllers
{
    public class TimeController : Controller
    {
        private ConnectionApiHelper connectionApi = new ConnectionApiHelper();

        // GET: Time
        public async Task<ActionResult> Index()
        {
            var response = await connectionApi.GetAllTimes();

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<IEnumerable<TimeViewModel>>();

                return View(model);
            }

            return View(new List<TimeViewModel>());
        }

        // GET: Time/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var response = await connectionApi.GetTimeById(id);

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<TimeViewModel>();

                return View(model);
            }
            return View();
        }

        // GET: Time/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Time/Create
        [HttpPost]
        public async Task<ActionResult> Create(TimeViewModel Time)
        {
            try
            {
                await connectionApi.InsertTime(Time);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Time/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var response = await connectionApi.GetTimeById(id);

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<TimeViewModel>();

                return View(model);
            }

            return View();
        }

        // POST: Time/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, TimeViewModel Time)
        {
            try
            {
                await connectionApi.UpdateTime(id, Time);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Time/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var response = await connectionApi.GetTimeById(id);

            if (response.IsSuccessStatusCode)
            {
                var model = await response.Content.ReadAsAsync<TimeViewModel>();

                return View(model);
            }

            return View();
        }

        // POST: Time/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, TimeViewModel Time)
        {
            try
            {
                await connectionApi.DeleteTime(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
