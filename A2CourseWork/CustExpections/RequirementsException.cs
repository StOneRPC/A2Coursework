﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2CourseWork.CustExpections
{
    class RequirementsException : Exception
    {
        public RequirementsException() { }

        public RequirementsException(string message) : base(message) { }
    }
}
