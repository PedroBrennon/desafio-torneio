using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using Torneio.API.Models;

namespace Torneio.API.Controllers
{
    public class TimeController : ApiController
    {
        private SqlConnection sqlConnection;

        private void SqlConnection()
        {
            string configManager = ConfigurationManager.ConnectionStrings["db"].ToString();
            sqlConnection = new SqlConnection(configManager);
        }

        // GET: api/Time
        public IEnumerable<TimeBindingModel> Get()
        {
            SqlConnection();
            List<TimeBindingModel> times = new List<TimeBindingModel>();

            using (SqlCommand sqlCommand = new SqlCommand("GetAllTimes", sqlConnection))
            {
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    if (sqlDataReader.HasRows)
                    {
                        TimeBindingModel time = new TimeBindingModel()
                        {
                            Id = Convert.ToInt32(sqlDataReader["Id"]),
                            Nome  = Convert.ToString(sqlDataReader["Name"])
                        };
                        times.Add(time);
                    }
                }
                sqlConnection.Close();
            }

            return times;
        }

        // GET: api/Time/5
        public TimeBindingModel Get(int id)
        {
            SqlConnection();
            TimeBindingModel Time = new TimeBindingModel();

            using (SqlCommand sqlCommand = new SqlCommand("GetTimeById", sqlConnection))
            {
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    Time = new TimeBindingModel()
                    {
                        Id = Convert.ToInt32(sqlDataReader["Id"]),
                        Nome = Convert.ToString(sqlDataReader["Name"])
                    };
                }
                sqlConnection.Close();
            }

            return Time;
        }

        // POST: api/Time
        public IHttpActionResult Post(TimeBindingModel Time)
        {
            SqlConnection();

            using (SqlCommand sqlCommand = new SqlCommand("InsertTime", sqlConnection))
            {
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Name", Time.Nome);

                sqlConnection.Open();
                int execute = sqlCommand.ExecuteNonQuery();
            }
            sqlConnection.Close();

            return Ok(Time);
        }

        // PUT: api/Time/5
        public IHttpActionResult Put(int id, TimeBindingModel Time)
        {
            SqlConnection();

            using (SqlCommand sqlCommand = new SqlCommand("UpdateTime", sqlConnection))
            {
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlCommand.Parameters.AddWithValue("@Name", Time.Nome);

                sqlConnection.Open();
                int execute = sqlCommand.ExecuteNonQuery();
            }
            sqlConnection.Close();

            return Ok(Time);
        }

        // DELETE: api/Time/5
        public IHttpActionResult Delete(int id)
        {
            SqlConnection();

            using (SqlCommand sqlCommand = new SqlCommand("DeleteTime", sqlConnection))
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
