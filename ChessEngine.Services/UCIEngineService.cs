using ChessEngine.Data;
using ChessEngine.Services.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessEngine.Services
{
    public class UCIListener : INotifyPropertyChanged
    {
        private string bestMove;
        public string BestMove { get => bestMove; set { bestMove = value; NotifyPropertyChanged(); } }
        private string ponder;
        public string PonderMove { get => ponder; set { ponder = value; NotifyPropertyChanged(); } }

        private int cpScore;
        public int CurrentCPScore {
            get { return cpScore; }
            set {
                cpScore = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class UCIEngineService : IUCIEngineService
    {
        private readonly IProccessService process;
        private StringBuilder moves;
        private StreamWriter writer;
        private string bestMove;
        private string ponder;
        private EventWaitHandle waitHandle = new ManualResetEvent(false);
        private int breakAfterMiliseconds;
        private int currentCPScore;

        List<UCIListener> listeners;

        public void AddListener(UCIListener newListener)
        {
            listeners.Add(newListener);
        }

        public UCIEngineService(IProccessService process)
        {
            listeners = new List<UCIListener>();
            this.process = process;
            this.moves = new StringBuilder();
            this.bestMove = null;
            this.currentCPScore = 0;
        }

        public void Init(int breakAfterMiliseconds)
        {
            this.breakAfterMiliseconds = breakAfterMiliseconds;
            string path = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName, "ChessEngine.Services/Engines/stockfish64.exe");
            path = Path.GetFullPath(path);
            writer = process.SpawnProcess(path, HandleOutputReceive);
            writer.WriteLine("uci");
            writer.WriteLine("ucinewgame");
        }

        public void AddMove(string move)
        {
            moves.Append(move + " ");
        }

        public void PlayMoveTime(string move, int mseconds)
        {
            writer.WriteLine("stop");

            moves.Append(move + " ");
            writer.WriteLine("position startpos moves " + moves);
            writer.WriteLine("go movetime " + mseconds.ToString());

            waitHandle.WaitOne();
            waitHandle.Reset();

            moves.Append(bestMove + " ");
        }

        public void PlayMoveDepth(string move, int depth)
        {
            writer.WriteLine("stop");

            moves.Append(move + " ");
            writer.WriteLine("position startpos moves " + moves);
            writer.WriteLine("go depth " + depth.ToString() + " movetime " + breakAfterMiliseconds.ToString()); //Maximum move time of 10 seconds

            waitHandle.WaitOne();
            waitHandle.Reset();

            moves.Append(bestMove + " ");
        }

        public void EvalPositionDepth(int depth)
        {
            writer.WriteLine("stop");
            writer.WriteLine("position startpos moves " + moves);
            writer.WriteLine("go depth " + depth.ToString() + " movetime " + breakAfterMiliseconds.ToString()); //Maximum move time of 10 seconds
        }

        private void HandleOutputReceive(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
            if (e.Data.IndexOf("bestmove") > -1)
            {
                string[] bestAndPonder = e.Data.Split();

                bestMove = bestAndPonder[1];
                ponder = bestAndPonder[3];
                foreach (UCIListener listener in listeners)
                {
                    listener.BestMove = bestMove;
                    listener.PonderMove = ponder;
                }
                waitHandle.Set();
            }
            else if(e.Data.IndexOf("score cp ") > -1)
            {
                string cp = e.Data.Substring(e.Data.IndexOf("score cp ") + 9).Split()[0];
                currentCPScore = int.Parse(cp);
                foreach( UCIListener listener in listeners)
                {
                    listener.CurrentCPScore = currentCPScore;
                }
                waitHandle.Set();
            }
        }
        
    }
}
