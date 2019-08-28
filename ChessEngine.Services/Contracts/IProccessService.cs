using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Services.ProcessCommunication.Contracts
{
    public interface IProccessService
    {
        StreamWriter SpawnProcess(string filePath, DataReceivedEventHandler handler);
    }
}
