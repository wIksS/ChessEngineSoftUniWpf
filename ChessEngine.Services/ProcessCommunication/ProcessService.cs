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

            // Set UseShellExecute to false for redirection.
            process.StartInfo.UseShellExecute = false;

            // Redirect the standard output of the sort command.  
            // This stream is read asynchronously using an event handler.
            process.StartInfo.RedirectStandardOutput = true;
            var output = new StringBuilder("");

            // Set our event handler to asynchronously read the sort output.
            process.OutputDataReceived += new DataReceivedEventHandler(outputHandler);

            // Redirect standard input as well.  This stream
            // is used synchronously.
            process.StartInfo.RedirectStandardInput = true;
            process.Start();

            // Use a stream writer to synchronously write the sort input.
            StreamWriter writer = process.StandardInput;

            // Start the asynchronous read of the sort output stream.
            process.BeginOutputReadLine();
            return writer;
        }
    }
}
