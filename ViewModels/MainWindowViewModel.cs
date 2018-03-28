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

        public ObservableCollection<NexaTimeSchema> NexaTimeschemas { get; set; } = new ObservableCollection<NexaTimeSchema>();

        private Boolean _isAllowed;
        private Boolean _isSaveEnabled ;
        private Boolean _isNewEnabled;
        private Boolean _isDeleteEnabled;
        public MainWindowViewModel()
        {
            _dataApi.GetDbDevices.ForEach(Devices.Add);
            for (int n = 0; n < 7; n++)
            {
                WeekDaysCollection.Add(new ViewModels.WeekDays() { NameOfWeekDay = WeekDayName(n), WeekDayId = n });
            }
        }


        public ICommand SaveDevice => new RelayCommand(x => DoSaveNewDevice(), x => IsAllowed);
        public ICommand SaveSchema => new RelayCommand(x => DoSaveNewSchema(), x => IsSaveEnabled);
        public ICommand PrepareNew => new RelayCommand(x => DoPrepareNew(), x => IsNewEnabled);

        public ICommand DeleteDevice => new RelayCommand(p => DoDelete(p), p => IsDeleteEnabled);
        public ICommand ExitApplication => new RelayCommand(p => DoExit(), p => true);

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

        }

        private void DoSaveNewSchema()
        {

            Schema schema = new Schema(_selectedDevice);
            schema.ActionText = _selectedAction.ToString();
            schema.TimePoint = DateTime.Parse(_TextBoxTimePoint);
            schema.WeekDay = _selWeekDay.WeekDayId;

            

            _dataApi.SaveSchemaForDevice(schema);

            TextBoxTimePoint = string.Empty;

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

                _selectedDevice.timeschemas.ToList().ForEach(NexaTimeschemas.Add);

               
                
                IsNewEnabled = true;
                //IsDeleteEnabled = forThisDevice.Count > 0 ? false : true;
            }
        }



        private NexaDevice _selectedNexaDevice;
        public NexaDevice SelectedNexaDevice
        {
            get => _selectedNexaDevice;
            set
            {
                _selectedNexaDevice = value == null ? _selectedNexaDevice : value;
                NotifyPropertyChanged(nameof(SelectedNexaDevice));

                
                ViewInformation();

            }
        }

        private void ViewInformation()
        {
            
            //SelWeekDay = new ViewModels.WeekDays()
            //{
            //    WeekDayId = _selectedWrapperSchema.WeekDay, NameOfWeekDay = WeekDayName(_selectedWrapperSchema.WeekDay)
            //};

            //SelectedWeekIndex = _selectedWrapperSchema.WeekDay - 1;
            //SelectedAction = _selectedWrapperSchema.ActionText == "PÅ" ? "0" : "1";
            ////SelectedAction = _selectedWrapperSchema.ActionText;
            //TextBoxTimePoint = _selectedWrapperSchema.TimePointAsString;
            
            //IsSaveEnabled = true;

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

        private string _selectedAction;

        public string SelectedAction
        {
            get =>_selectedAction; 
            set
            {
                _selectedAction = value;
                NotifyPropertyChanged(nameof(SelectedAction));
            }
        }

    }
}
