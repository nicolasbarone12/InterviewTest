﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Errors
{
    public class BaseException:SystemException
    {
        public BaseException(string? message) : base(message)
        {
        }
    }
}
