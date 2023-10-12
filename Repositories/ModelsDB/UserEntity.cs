using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ModelsDB
{
    public class UserEntity:IEntity
    {
        public int Id { get; set; }
        public string UserLastName { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string Password { get; set; }
    }
}
