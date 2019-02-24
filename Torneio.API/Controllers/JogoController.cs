using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Torneio.API.Models;

namespace Torneio.API.Controllers
{
    public class JogoController : ApiController
    {
        private SqlConnection sqlConnection;

        private void SqlConnection()
        {
            string configManager = ConfigurationManager.ConnectionStrings["db"].ToString();
            sqlConnection = new SqlConnection(configManager);
        }

        // GET: api/Jogo
        public IEnumerable<JogoBindingModel> Get()
        {
            SqlConnection();
            List<JogoBindingModel> jogos = new List<JogoBindingModel>();

            using (SqlCommand sqlCommand = new SqlCommand("GetAllJogos", sqlConnection))
            {
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    if (sqlDataReader.HasRows)
                    {
                        JogoBindingModel jogo = new JogoBindingModel()
                        {
                            Id = Convert.ToInt32(sqlDataReader["Id"]),
                            NumPartida = Convert.ToInt32(sqlDataReader["Partida"]),
                            DataDaPartida = Convert.ToDateTime(sqlDataReader["DataDaPartida"]),
                            Chave = Convert.ToString(sqlDataReader["Chave"]),
                            GolsTimeId1 = Convert.ToInt32(sqlDataReader["GolsTimeId1"]),
                            GolsTimeId2 = Convert.ToInt32(sqlDataReader["GolsTimeId2"]),
                            TimeId1 = Convert.ToInt32(sqlDataReader["TimeId1"]),
                            TimeId2 = Convert.ToInt32(sqlDataReader["TimeId2"]),
                            TimeVencedor = Convert.ToInt32(sqlDataReader["TimeVencedor"]),
                            NameTimeId1 = Convert.ToString(sqlDataReader["NameTimeId1"]),
                            NameTimeId2 = Convert.ToString(sqlDataReader["NameTimeId2"]),
                            NameTimeVencedor = Convert.ToString(sqlDataReader["NameTimeVencedor"]),
                            Terminou = Convert.ToBoolean(sqlDataReader["Terminou"])
                        };
                        jogos.Add(jogo);
                    }
                }
                sqlConnection.Close();
            }

            return jogos;
        }

        // GET: api/Jogo/5
        public JogoBindingModel Get(int id)
        {
            SqlConnection();
            JogoBindingModel Jogo = new JogoBindingModel();

            using (SqlCommand sqlCommand = new SqlCommand("GetJogoById", sqlConnection))
            {
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    Jogo = new JogoBindingModel()
                    {
                        Id = Convert.ToInt32(sqlDataReader["Id"]),
                        NumPartida = Convert.ToInt32(sqlDataReader["Partida"]),
                        DataDaPartida = Convert.ToDateTime(sqlDataReader["DataDaPartida"]),
                        Chave = Convert.ToString(sqlDataReader["Chave"]),
                        GolsTimeId1 = Convert.ToInt32(sqlDataReader["GolsTimeId1"]),
                        GolsTimeId2 = Convert.ToInt32(sqlDataReader["GolsTimeId2"]),
                        TimeId1 = Convert.ToInt32(sqlDataReader["TimeId1"]),
                        TimeId2 = Convert.ToInt32(sqlDataReader["TimeId2"]),
                        TimeVencedor = Convert.ToInt32(sqlDataReader["TimeVencedor"]),
                        NameTimeId1 = Convert.ToString(sqlDataReader["NameTimeId1"]),
                        NameTimeId2 = Convert.ToString(sqlDataReader["NameTimeId2"]),
                        NameTimeVencedor = Convert.ToString(sqlDataReader["NameTimeVencedor"]),
                        Terminou = Convert.ToBoolean(sqlDataReader["Terminou"])
                    };
                }
                sqlConnection.Close();
            }

            return Jogo;
        }

        // POST: api/Jogo
        public IHttpActionResult Post(JogoBindingModel Jogo)
        {
            if (Jogo.TimeId1 != Jogo.TimeId2)
            {
                SqlConnection();

                using (SqlCommand sqlCommand = new SqlCommand("InsertJogo", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Partida", Jogo.NumPartida);
                    sqlCommand.Parameters.AddWithValue("@DataDaPartida", Jogo.DataDaPartida);
                    sqlCommand.Parameters.AddWithValue("@Chave", Jogo.Chave);
                    sqlCommand.Parameters.AddWithValue("@GolsTimeId1", Jogo.GolsTimeId1);
                    sqlCommand.Parameters.AddWithValue("@GolsTimeId2", Jogo.GolsTimeId2);
                    sqlCommand.Parameters.AddWithValue("@TimeId1", Jogo.TimeId1);
                    sqlCommand.Parameters.AddWithValue("@TimeId2", Jogo.TimeId2);
                    sqlCommand.Parameters.AddWithValue("@TimeVencedor", Jogo.TimeVencedor);
                    sqlCommand.Parameters.AddWithValue("@Terminou", Jogo.Terminou);

                    sqlConnection.Open();
                    int execute = sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close();

                return Ok(Jogo);
            }
            else
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro."));
            }
        }

        // PUT: api/Jogo/5
        public IHttpActionResult Put(int id, JogoBindingModel Jogo)
        {
            if (Jogo.TimeId1 != Jogo.TimeId2) { 
                SqlConnection();

                using (SqlCommand sqlCommand = new SqlCommand("UpdateJogo", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Id", id);
                    sqlCommand.Parameters.AddWithValue("@Partida", Jogo.NumPartida);
                    sqlCommand.Parameters.AddWithValue("@DataDaPartida", Jogo.DataDaPartida);
                    sqlCommand.Parameters.AddWithValue("@Chave", Jogo.Chave);
                    sqlCommand.Parameters.AddWithValue("@GolsTimeId1", Jogo.GolsTimeId1);
                    sqlCommand.Parameters.AddWithValue("@GolsTimeId2", Jogo.GolsTimeId2);
                    sqlCommand.Parameters.AddWithValue("@TimeId1", Jogo.TimeId1);
                    sqlCommand.Parameters.AddWithValue("@TimeId2", Jogo.TimeId2);
                    sqlCommand.Parameters.AddWithValue("@TimeVencedor", Jogo.TimeVencedor);
                    sqlCommand.Parameters.AddWithValue("@Terminou", Jogo.Terminou);

                    sqlConnection.Open();
                    int execute = sqlCommand.ExecuteNonQuery();
                }
                sqlConnection.Close();

                return Ok(Jogo);
            }
            else
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro."));
            }
        }

        // DELETE: api/Jogo/5
        public IHttpActionResult Delete(int id)
        {
            SqlConnection();

            using (SqlCommand sqlCommand = new SqlCommand("DeleteJogo", sqlConnection))
            {
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", id);

                sqlConnection.Open();
                int execute = sqlCommand.ExecuteNonQuery();
            }
            sqlConnection.Close();

            return Ok();
        }
    }
}
