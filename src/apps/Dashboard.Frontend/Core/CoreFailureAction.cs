using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Core
{
    public class CoreFailureAction
    {
        public CoreFailureAction(string error, bool isError)
        {
            Error = error;
            IsError = isError;
        }

        public string Error { get; }
        public bool IsError { get; }
    }
}
