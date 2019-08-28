using ChessEngine.Services.ProcessCommunication.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.Services.ProcessCommunication
{
    public class ProcessService : IProccessService
    {
        public StreamWriter SpawnProcess(string filePath, DataReceivedEventHandler outputHandler)
        {
            Process process;
            process = new Process();
            process.StartInfo.FileName = filePath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            var output = new StringBuilder("");
            process.OutputDataReceived += new DataReceivedEventHandler(outputHandler);
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
            StreamWriter writer = process.StandardInput;
            process.BeginOutputReadLine();
            return writer;
        }
    }
}
