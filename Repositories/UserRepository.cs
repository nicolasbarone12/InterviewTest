using Infraestructure;
using Infraestructure.Errors;
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
    public class UserRepository : IRepository
    {
        public bool Delete(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IEntity> GetByFilters(string filterExpression)
        {
            string queryLogin = $"Select UserId, UserCode, UserLastName, UserName from Users{filterExpression} ";
            ICollection<IEntity> users = new List<IEntity>();
            try
            {
                var dbHelper = new DbHelper();
                DataTable dt = dbHelper.EjecutarQuery(System.Data.CommandType.Text, queryLogin);

                if (dt is not null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        users.Add(new UserEntity()
                        {
                            Id = int.Parse(row["UserId"].ToString()),
                            UserCode = row["UserCode"].ToString(),
                            UserLastName = row["UserLastName"].ToString(),
                            UserName = row["UserName"].ToString()
                        });
                    }
                }

            }
            catch(Exception ex)
            {
                throw;
            }

            return users;
        }

        public IEntity GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(IEntity entity)
        {
            int result = -1;
            try
            {
                var dbHelper = new DbHelper();//DbHelper.GetInstance();
                UserEntity user = (UserEntity)entity;
                string query = $"Insert INTO Users (UserName,UserLastName,Pass,UserCode) Values ('{user.UserName}', '{user.UserLastName}', '{user.Password}', '{user.UserCode}')";

                result = dbHelper.ExecuteNonQuery(System.Data.CommandType.Text, query);
                
            }
            catch(Exception e)
            {
                throw;
            }

            return result;
        }

        public bool Update(IEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
