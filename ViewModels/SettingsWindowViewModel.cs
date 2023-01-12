using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nexa.ViewModels
{
    public class SettingsWindowViewModel : ViewModelBase
    {

        public ICommand UnloadWindow => new RelayCommand(x => DoUnloadWindow(), x => true);
        public Action CloseAction { get; set; }

        private void DoUnloadWindow()
        {
            
        }

        public SettingsWindowViewModel()
        {
            
        }

    }
}
