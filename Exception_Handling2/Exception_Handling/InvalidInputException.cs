using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Exception_Handling
{
    [Serializable]
    class InvalidInputException : Exception
    {
         public InvalidInputException()
            : base(string.Format("Please change invalid input(s).")) { }
         public InvalidInputException(string message)
            : base(message) { }
    }
}
