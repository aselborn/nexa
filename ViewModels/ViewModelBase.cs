using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexa.ViewModels
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private readonly HashSet<string> _errorList = new HashSet<string>();
        public HashSet<string> ErrorList
        {
            get { return _errorList; }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(params string[] propertyName)
        {
            foreach (var prop in propertyName)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(prop));
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, args);
            }
        }

        protected virtual void OnValidationError(string propertyName, string validationError)
        {
            _errorList.Add(propertyName);
            throw new ValidationException(validationError);
        }

        public virtual bool HasValidationErrors => _errorList.Count > 0;

        protected void ClearValidationErrors()
        {
            _errorList.Clear();
        }
    }
}
