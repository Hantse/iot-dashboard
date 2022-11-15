using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Interfaces
{
    public interface IRealtimeService
    {
        Task StartListen();
    }
}
