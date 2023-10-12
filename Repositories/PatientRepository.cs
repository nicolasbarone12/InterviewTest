using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PatientRepository : IRepository
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
            throw new NotImplementedException();
        }

        public IEntity GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(IEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
