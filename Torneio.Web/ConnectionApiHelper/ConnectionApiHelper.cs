using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Torneio.Web.Models;

namespace Torneio.Web.ConnectionApi
{
    public class ConnectionApiHelper
    {
        private readonly HttpClient httpClient;

        public ConnectionApiHelper()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:64335/")
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
            httpClient.DefaultRequestHeaders.Accept.Add(mediaType);
        }

        #region Time
        public async Task<HttpResponseMessage> GetAllTimes()
        {
            return await httpClient.GetAsync("api/Time");
        }

        public async Task<HttpResponseMessage> GetTimeById(int id)
        {
            return await httpClient.GetAsync($"api/Time/{id}");
        }

        public async Task<HttpResponseMessage> UpdateTime(int id, TimeViewModel model)
        {
            return await httpClient.PutAsJsonAsync($"api/Time/{id}", model);
        }

        public async Task<HttpResponseMessage> InsertTime(TimeViewModel model)
        {
            return await httpClient.PostAsJsonAsync("api/Time", model);
        }

        public async Task<HttpResponseMessage> DeleteTime(int id)
        {
            return await httpClient.DeleteAsync($"api/Time/{id}");
        }
        #endregion

        #region Jogo
        public async Task<HttpResponseMessage> GetAllJogos()
        {
            return await httpClient.GetAsync("api/Jogo");
        }

        public async Task<HttpResponseMessage> GetJogoById(int id)
        {
            return await httpClient.GetAsync($"api/Jogo/{id}");
        }

        public async Task<HttpResponseMessage> UpdateJogo(int id, JogoCreateEditViewModel model)
        {
            return await httpClient.PutAsJsonAsync($"api/Jogo/{id}", model);
        }

        public async Task<HttpResponseMessage> InsertJogo(JogoCreateEditViewModel model)
        {
            return await httpClient.PostAsJsonAsync("api/Jogo", model);
        }

        public async Task<HttpResponseMessage> DeleteJogo(int id)
        {
            return await httpClient.DeleteAsync($"api/Jogo/{id}");
        }
        #endregion
    }
}