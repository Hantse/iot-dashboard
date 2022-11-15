using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Core
{
    public class CoreState
    {
        public CoreState(bool loadInProgress, string error = null, bool isError = false)
        {
            LoadInProgress = loadInProgress;
            Error = error;
            IsError = isError;
        }

        public bool LoadInProgress { get; }
        public string Error { get; }
        public bool IsError { get; }
    }
}
