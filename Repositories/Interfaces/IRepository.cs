using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IRepository
    {
        public int Insert(IEntity entity);

        public bool Update(IEntity entity);

        public bool Delete(IEntity entity);

        public IEnumerable<IEntity> GetAll();

        public IEntity GetById(int id);
        public IEnumerable<IEntity> GetByFilters(string filterExpression);
    }
}
