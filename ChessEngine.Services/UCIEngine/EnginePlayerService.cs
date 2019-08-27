using ChessEngine.Services.ProcessCommunication.Contracts;
using ChessEngine.Services.UCIEngine.Contracts;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChessEngine.Services.UCIEngine
{
    public class EnginePlayerService : IEnginePlayerService
    {
        private readonly IProccessService process;
        private StringBuilder moves;
        private StreamWriter writer;
        private string engineMove;
        private EventWaitHandle waitHandle = new ManualResetEvent(false);
        private int waitingTime;

        public EnginePlayerService(IProccessService process)
        {
            this.process = process;
            this.moves = new StringBuilder();
            engineMove = null;
        }

        public void InitPlayer(int waitTimePerMove)
        {
            waitingTime = waitTimePerMove;
            writer = process.SpawnProcess(@"C:\Users\Sadmin\Desktop\GitClone\ChessEngineSoftUniWpf\stockfish-10-win\Windows\stockfish_10_x64.exe", HandleOutputReceive);
            writer.WriteLine("uci");
            writer.WriteLine("ucinewgame");
        }


        public async Task<string> PlayMove(string move)
        {
            moves.Append(move + " ");
            writer.WriteLine("position startpos moves " + moves);
            writer.WriteLine("go infinite");

            await Task.Run(()=> { Thread.Sleep(waitingTime); });
            writer.WriteLine("stop");

            waitHandle.WaitOne();
            waitHandle.Reset();

            moves.Append(engineMove + " ");

            return engineMove;
        }

        private void HandleOutputReceive(object sender, DataReceivedEventArgs e)
        {
            if (e.Data.IndexOf("bestmove") > -1)
            {

                engineMove = e.Data.Split()[1];
                waitHandle.Set();
            }
        }
    }
}
