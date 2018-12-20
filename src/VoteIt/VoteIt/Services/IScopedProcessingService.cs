using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VoteIt.Services
{
    internal interface IScopedProcessingService
    {
        void DoWork();
    }
}
