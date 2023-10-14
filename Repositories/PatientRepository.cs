using Infraestructure;
using Repositories.Interfaces;
using Repositories.ModelsDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PatientRepository : IRepository
    {
        public bool Delete(IEntity entity)
        {
            PatientEntity patient = (PatientEntity)entity;

            var dbHelper = new DbHelper();
            
            string query = $"DELETE FROM Patient where PatientId={patient.PatientId}";

            bool result = dbHelper.ExecuteNonQuery(CommandType.Text, query) > 0;

            return result;
        }

        public IEnumerable<IEntity> GetAll()
        {
            ICollection<PatientEntity> result = new List<PatientEntity>();
            bool pendingData = true;
            int currentMinId = 0;
            while (pendingData)
            {
                DbHelper dbHelper = new DbHelper();

                string query = $"SELECT Top ( 500000) PatientId, PatientName, PatientLastName, DateOfBirth, Sickness FROM Patient Where PatientId > {currentMinId}";

                DataTable dt = dbHelper.EjecutarQuery(System.Data.CommandType.Text, query);
                
                if (dt is not null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {

                        result.Add(new PatientEntity()
                        {
                            DateOfBirth = DateTime.Parse(row["DateOfBirth"].ToString()),
                            LastName = row["PatientLastName"].ToString(),
                            PatientId = int.Parse(row["PatientId"].ToString()),
                            Name = row["PatientName"].ToString(),
                            Sickness = row["Sickness"].ToString(),

                        });
                    }
                    currentMinId = int.Parse(dt.Rows[dt.Rows.Count - 1]["PatientId"].ToString());
                }
                else
                    pendingData = false;
            }
           
            return result;
        }

        public IEnumerable<IEntity> GetByFilters(string filterExpression)
        {
            ICollection<PatientEntity> result = new List<PatientEntity>();
            
            
            DbHelper dbHelper = new DbHelper();

            string query = $"SELECT  PatientId, PatientName, PatientLastName, DateOfBirth, Sickness FROM Patient {filterExpression}";

            DataTable dt = dbHelper.EjecutarQuery(System.Data.CommandType.Text, query);

            if (dt is not null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    result.Add(new PatientEntity()
                    {
                        DateOfBirth = DateTime.Parse(row["DateOfBirth"].ToString()),
                        LastName = row["PatientLastName"].ToString(),
                        PatientId = int.Parse(row["PatientId"].ToString()),
                        Name = row["PatientName"].ToString(),
                        Sickness = row["Sickness"].ToString(),

                    });
                }
                    
            }
              
            

            return result;
        }

        public IEntity GetById(int id)
        {
            var dbHelper = new DbHelper();
            string query = $"SELECT PatientId, PatientName, PatientLastName, DateOfBirth, Sickness FROM Patient Where PatientId = {id}";
            DataTable dt = dbHelper.EjecutarQuery(System.Data.CommandType.Text, query);
            IEntity patient = null;

            if (dt is not null && dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                patient = new PatientEntity()
                {
                    DateOfBirth = DateTime.Parse(row["DateOfBirth"].ToString()),
                    LastName = row["PatientLastName"].ToString(),
                    PatientId = int.Parse(row["PatientId"].ToString()),
                    Name = row["PatientName"].ToString(),
                    Sickness = row["Sickness"].ToString(),
                };
            }

            return patient;
        }

        public int Insert(IEntity entity)
        {
            PatientEntity patient = (PatientEntity)entity;

            var dbHelper = new DbHelper();
            string format = "yyyy-MM-dd";
            string query = $"INSERT INTO Patient (PatientName, PatientLastName, DateOfBirth, Sickness) VALUES ('{patient.Name}', '{patient.LastName}', '{patient.DateOfBirth.ToString(format)}', '{patient.Sickness}')";

            int result = dbHelper.ExecuteNonQuery(CommandType.Text, query);

            return result;
        }

        public bool Update(IEntity entity)
        {
            PatientEntity patient = (PatientEntity)entity;

            var dbHelper = new DbHelper();
            string format = "yyyy-MM-dd";
            string query = $"UPDATE Patient SET PatientName='{patient.Name}', PatientLastName = '{patient.LastName}', DateOfBirth = '{patient.DateOfBirth.ToString(format)}', Sickness='{patient.Sickness}' Where PatientId = {patient.PatientId}";

            bool result = (dbHelper.ExecuteNonQuery(CommandType.Text, query) > 0);

            return result;
        }
    }
}
