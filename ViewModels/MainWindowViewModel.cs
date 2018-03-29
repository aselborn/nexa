using Nexa.library;
using Nexa.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;


namespace Nexa.ViewModels
{

    class MainWindowViewModel : ViewModelBase
    {
       
        private DataApi _dataApi = new DataApi();
        
        public ObservableCollection<NexaDevice> Devices { get; } = new ObservableCollection<NexaDevice>();

        public ObservableCollection<WeekDays> WeekDaysCollection { get; } = new ObservableCollection<WeekDays>();
        public ObservableCollection<ActionsText> ActionCollection { get; } = new ObservableCollection<ActionsText>();


        public ObservableCollection<NexaTimeSchema> NexaTimeschemas { get; set; } = new ObservableCollection<NexaTimeSchema>();

        

        private Boolean _isAllowed;
        private Boolean _isSaveEnabled ;
        private Boolean _isNewEnabled;
        private Boolean _isDeleteEnabled;
        private string  _saveUpdateText;

        public MainWindowViewModel()
        {
            _dataApi.GetDbDevices.ForEach(Devices.Add);
            for (int n = 0; n < 7; n++)
            {
                WeekDaysCollection.Add(new ViewModels.WeekDays() { NameOfWeekDay = WeekDayName(n), WeekDayId = n });
            }

            ActionCollection.Add(new ActionsText() { Action = 0, ActionText = "AV" });
            ActionCollection.Add(new ActionsText() { Action = 1, ActionText = "PÅ" });

            SaveUpdateText = "Spara";
        }


        public ICommand SaveDevice => new RelayCommand(x => DoSaveNewDevice(), x => IsAllowed);
        public ICommand SaveSchema => new RelayCommand(x => DoSaveNewSchema(), x => IsSaveEnabled);
        public ICommand PrepareNew => new RelayCommand(x => DoPrepareNew(), x => IsNewEnabled);

        public ICommand DeleteDevice => new RelayCommand(p => DoDelete(p), p => IsDeleteEnabled);
        public ICommand ExitApplication => new RelayCommand(p => DoExit(), p => true);
        public ICommand DeleteTimeSchema => new RelayCommand(v => DoDeleteTimeSchema(v),v=> IsDeleteEnabled);

        private void DoDeleteTimeSchema(object v)
        {
            NexaTimeSchema item = (NexaTimeSchema)v;

            if (_dataApi.DeleteTimeSchema(item))
            {
                SelectedDevice.timeschemas.Remove(item);
                
            }

        }

        

        private void DoExit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void DoDelete(object x)
        {
            _dataApi.DeleteDevice((NexaDevice)x);
            Devices.Remove((NexaDevice)x);
        }

        private void DoPrepareNew()
        {
            TextBoxTimePoint = "";
            SaveUpdateText = "Spara";
            IsSaveEnabled = true;
            
        }

        private void DoSaveNewSchema()
        {

            NexaTimeSchema timeSchema = new NexaTimeSchema();
            timeSchema.Action = _selectedAction;
            timeSchema.Dayofweek = _selectedWeekIndex + 1;
            timeSchema.DeviceId = _selectedDevice.DeviceId;
            timeSchema.TimePoint = DateTime.Parse(TextBoxTimePoint);

            
            if (SaveUpdateText.CompareTo("Save") == 0)
            {
                _dataApi.SaveNexaTimeschema(timeSchema);
                TextBoxTimePoint = string.Empty;
                IsSaveEnabled = false;
            }
            else
            {
                if (_selectedDevice != null)
                {
                    timeSchema.Id = _selectedNexaTimeschema.Id;
                    SelectedNexaTimeschema = _dataApi.UpdateNexaTimeschema(timeSchema);

                    NexaTimeschemas.Clear();
                    _selectedDevice.timeschemas.ToList().ForEach(NexaTimeschemas.Add);
                }
                
            }
        }

        private void DoSaveNewDevice()
        {
            NexaDevice nexaDevice = new NexaDevice() { DeviceName = _TextBoxDeviceName, DeviceType = _TextBoxDescription };

            
            TextBoxDescription = string.Empty;
            TextBoxDeviceName = string.Empty;

            _dataApi.SaveNewDevice(nexaDevice);

            Devices.Clear();
            _dataApi.GetDbDevices.ForEach(Devices.Add);
            

        }

        public bool IsAllowed
        {
            get => _isAllowed;
            set
            {
                _isAllowed = value;
                NotifyPropertyChanged(nameof(IsAllowed));
            }
        }
        public bool IsNewEnabled
        {
            get => _isNewEnabled;
            set
            {
                _isNewEnabled = value;
                NotifyPropertyChanged(nameof(IsNewEnabled));

            }
        }

        public Boolean IsDeleteEnabled
        {
            get => _isDeleteEnabled;
            set
            {
                _isDeleteEnabled = value;
                NotifyPropertyChanged(nameof(IsDeleteEnabled));
            }
        }
            


        public bool IsSaveEnabled
        {
            get => _isSaveEnabled;
            set
            {
                _isSaveEnabled = value;
                NotifyPropertyChanged(nameof(IsSaveEnabled));
            }
        }

        private string _TextBoxTimePoint;
        public string TextBoxTimePoint
        {
            get => _TextBoxTimePoint;
            set
            {
                _TextBoxTimePoint = value;
                NotifyPropertyChanged(nameof(TextBoxTimePoint));
            }
        }

        private string _TextBoxDeviceName;
        public string TextBoxDeviceName
        {
            get => _TextBoxDeviceName;
            set
            {
                _TextBoxDeviceName = value;
                NotifyPropertyChanged(nameof(TextBoxDeviceName));

                IsAllowed = value.Length > 0 ? true : false;
            }
        }

        private string _TextBoxDescription;
        public string TextBoxDescription
        {
            get => _TextBoxDescription;
            set
            {
                _TextBoxDescription = value;
                NotifyPropertyChanged(nameof(TextBoxDescription));
            }
        }

        private NexaDevice _selectedDevice;
        public NexaDevice SelectedDevice
        {
            get => _selectedDevice;
            set
            {

                _selectedDevice = value == null ? _selectedDevice : value;
                NotifyPropertyChanged(nameof(SelectedDevice));
                NexaTimeschemas.Clear();

                IsDeleteEnabled = false;

                if (_selectedDevice.timeschemas != null)
                {
                    _selectedDevice.timeschemas.ToList().ForEach(NexaTimeschemas.Add);
                    IsDeleteEnabled = true;
                }
                    
                
                IsNewEnabled = true;
                
            }
        }



        private NexaTimeSchema _selectedNexaTimeschema;
        public NexaTimeSchema SelectedNexaTimeschema
        {
            get => _selectedNexaTimeschema;
            set
            {
                _selectedNexaTimeschema = value == null ? _selectedNexaTimeschema : value;
                NotifyPropertyChanged(nameof(SelectedNexaTimeschema));

                
                ViewInformation();

                SaveUpdateText = "Update";

            }
        }

        private void ViewInformation()
        {

            SelWeekDay = new ViewModels.WeekDays()
            {
                WeekDayId = _selectedNexaTimeschema.Dayofweek,
                NameOfWeekDay = WeekDayName(_selectedNexaTimeschema.Dayofweek)
            };

            SelectedWeekIndex = _selectedNexaTimeschema.Dayofweek - 1;
            SelectedAction = _selectedNexaTimeschema.Action;
            TextBoxTimePoint = _selectedNexaTimeschema.TimePointAsString;

            IsSaveEnabled = true;

        }

        private string WeekDayName(int weekDay)
        {
            return Enum.GetName(typeof(EnumDayOfWeek), weekDay);
        }

        private ComboBoxItem _selectedWeekday;
        public ComboBoxItem SelectedWeekDay
        {
            get => _selectedWeekday;

            set
            {
                _selectedWeekday = value;
                NotifyPropertyChanged(nameof(SelectedWeekDay));
            }
        }

        private WeekDays _selWeekDay;
        public WeekDays SelWeekDay
        {
            get { return _selWeekDay; }
            set
            {
                _selWeekDay = value;
                NotifyPropertyChanged(nameof(SelWeekDay));

            }
        }

        private int _selectedWeekIndex;
        public int SelectedWeekIndex
        {
            get { return _selectedWeekIndex; }
            set
            {
                _selectedWeekIndex = value;
                NotifyPropertyChanged(nameof(SelectedWeekIndex));
            }
        }

        private int _selectedAction;
        public int SelectedAction
        {
            get =>_selectedAction; 
            set
            {
                _selectedAction = value;
                NotifyPropertyChanged(nameof(SelectedAction));
            }
        }

        public string SaveUpdateText
        {
            get => _saveUpdateText;
            set
            {
                _saveUpdateText = value;
                NotifyPropertyChanged(nameof(SaveUpdateText));
            }
        }

        
    }
}
