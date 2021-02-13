using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeDownloaderDotNet.Models.Enums
{
    public enum FileState
    {
        Playing = 1,
        Stopped = 2,
        Finished = 3,
        InQueue = 4,
        Corrupted = 5,
    }
}
