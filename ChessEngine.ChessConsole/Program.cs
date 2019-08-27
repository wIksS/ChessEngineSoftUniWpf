using ChessEngine.Services.ProcessCommunication;
using ChessEngine.Services.ProcessCommunication.Contracts;
using ChessEngine.Services.UCIEngine;
using ChessEngine.Services.UCIEngine.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.ChessConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            IProccessService processService = new ProcessService();
            IEnginePlayerService service = new EnginePlayerService(processService);

            service.InitPlayer(500);
            while (true)
            {
                var move = Console.ReadLine();
                Console.WriteLine(service.PlayMove(move).Result);
            }
        }
    }
}
