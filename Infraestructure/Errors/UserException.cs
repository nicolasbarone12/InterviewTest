using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Errors
{
    public class UserException : BaseException
    {
        public UserException(string? message) : base(message)
        {
        }
    }
}
