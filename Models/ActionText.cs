using Nexa.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexa.Models
{
    public class ActionsText : ViewModelBase
    {
        private int _action;
        public int Action
        {
            get => _action;
            set
            {
                _action = value;
                NotifyPropertyChanged(nameof(Action));
            }
        }
        private string _actionText;
        public string ActionText
        {
            get => _actionText;
            set
            {
                _actionText = value;
                NotifyPropertyChanged(nameof(ActionText));
            }
        }

        public override string ToString()
        {
            return _actionText;
        }
    }

}
