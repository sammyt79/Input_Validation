using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Exception_Handling
{
    [Serializable]
    class IDAlreadyExistsException : Exception
    {
        public IDAlreadyExistsException()
            : base(string.Format("ID already exists.")) { }
        public IDAlreadyExistsException(string message)
            : base(message) { }
    }
}
