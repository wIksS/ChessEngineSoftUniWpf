using ChessEngine.Data;
using ChessEngine.Data.Common.Events;
using ChessEngine.Services.Events.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine.ViewModels
{
    public class EvaluationViewModel : BaseNotifyPropertyChanged
    {
        private readonly IEventService<EvaluationChangeEventArgs> evaluationService;
        private string evaluation = "0";
        private int counter = 0;

        public EvaluationViewModel(IEventService<EvaluationChangeEventArgs> evaluationService)
        {
            this.Moves = new ObservableCollection<string>();
            this.evaluationService = evaluationService;
            evaluationService.StateChanged += EvaluationChangedHandler;
        }

        private void EvaluationChangedHandler(object sender, EvaluationChangeEventArgs e)
        {
            var result = float.Parse(e.Evaluation) / 100 * -1;
            string sign = result > 0 ? "+" : "";

            this.Evaluation =  sign + result;
            this.Moves.Add(++counter + ". " + e.Move);
        }

        public ObservableCollection<string> Moves { get; set; }

        public string Evaluation
        {
            get
            {
                return this.evaluation;
            }
            set
            {
                this.evaluation = value;
                NotifyChanged("Evaluation");
            }
        }

    }
}
