﻿using Nexa.library;
using Nexa.Models;
using Nexa.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private Boolean _isNewDeviceEnabled=true;
        private Boolean _isDeleteEnabled;
        private Boolean _isDeleteRecordEnabled;
        private string  _saveUpdateText;

        public MainWindowViewModel()
        {
            //_dataApi.GetDbDevices.ForEach(Devices.Add);
            _dataApi.GetDbDevices.OrderBy(p=>p.DeviceName).ToList().ForEach(Devices.Add);
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
        public ICommand PrepareNewDevice => new RelayCommand(x => DoPrepareNewDevice(), x => IsNewDeviceEnabled);
        public ICommand DeleteDevice => new RelayCommand(p => DoDelete(p), p => IsDeleteEnabled);
        public ICommand ExitApplication => new RelayCommand(p => DoExit(), p => true);
        public ICommand DeleteTimeSchema => new RelayCommand(v => DoDeleteTimeSchema(v),v=> IsDeleteTimeschemaEnabled);
        public ICommand WriteConfigFile => new RelayCommand(z => DoWriteConfigurationFile(), z => true);
        public ICommand ShowSettingsWindow => new RelayCommand(_ => DoShowSettings(), _ => true);
        public ICommand RemoveMulipleTimeschemas => new RelayCommand(DoRemoveMulpleSchemas, c => true);
       
        

        private void DoRemoveMulpleSchemas(object ItemsInList)
        {
            var selectedItems = (IList)ItemsInList;
            List<NexaTimeSchema> selectedDevices = selectedItems.Cast<NexaTimeSchema>().ToList();

            TimeschemasCount = selectedDevices.Count.ToString();

            foreach (NexaTimeSchema schema in selectedDevices)
            {
                _dataApi.DeleteTimeSchema(schema);
            }

            ClearAndFillDevices();
        }

        private void ClearAndFillDevices()
        {
            Devices.Clear();
            _dataApi.GetDbDevices.ForEach(Devices.Add);
        }

        private void DoShowSettings()
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            settingsWindow.ShowDialog();
        }

        private void DoWriteConfigurationFile()
        {
            _dataApi.WriteConfig();
        }

        private void DoDeleteTimeSchema(object v)
        {
            NexaTimeSchema item = (NexaTimeSchema)v;
            SelectedDevice.timeschemas.Remove(item);

            if (_dataApi.DeleteTimeSchema(item))
            {
                ClearAndFillDevices();
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
            IsSaveEnabled = false;
            
        }

        private void DoPrepareNewDevice()
        {

            TextBoxDeviceName = "";
            TextBoxNexaId = 0;
            TextBoxDescription = "";
        }
        private void DoSaveNewSchema()
        {
            
            NexaTimeSchema timeSchema = new NexaTimeSchema();
            timeSchema.Action = _selectedAction;
            timeSchema.Dayofweek = _selectedWeekIndex + 1;
            timeSchema.DeviceId = _selectedDevice.DeviceId;
            timeSchema.TimePoint = DateTime.Parse(TextBoxTimePoint);

            //This has to be unhacked!
            if (SaveUpdateText.CompareTo("Spara") == 0)
            {
                _dataApi.SaveNexaTimeschema(timeSchema);
                TextBoxTimePoint = string.Empty;
                IsSaveEnabled = false;

                ClearAndFillDevices();

            }
            else
            {
                if (_selectedDevice != null)
                {
                    timeSchema.Id = _selectedNexaTimeschema.Id;
                    SelectedNexaTimeschema = _dataApi.UpdateNexaTimeschema(timeSchema);

                    NexaTimeschemas.Clear();
                    _selectedDevice.timeschemas.OrderBy(z=>z.TimePoint).ToList().ForEach(NexaTimeschemas.Add);
                }
                
            }
        }


        private void DoSaveNewDevice()
        {
            NexaDevice nexaDevice = new NexaDevice() { DeviceName = _TextBoxDeviceName, DeviceType = _TextBoxDescription, NexaId=_TextBoxNexaId };

            
            TextBoxDescription = string.Empty;
            TextBoxDeviceName = string.Empty;

            if (IsNewDeviceEnabled)
            {
                _dataApi.SaveNewDevice(nexaDevice);
            }
            else
            {
                nexaDevice.DeviceId = _selectedDevice.DeviceId;
                _dataApi.UpdateDevice(nexaDevice);
            }

            Devices.Clear();
            _dataApi.GetDbDevices.ForEach(Devices.Add);

            IsNewDeviceEnabled = true;
            IsSaveEnabled = false;
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

        public bool IsNewDeviceEnabled
        {
            get => _isNewDeviceEnabled;
            set
            {
                _isNewDeviceEnabled = value;
                NotifyPropertyChanged(nameof(IsNewDeviceEnabled));
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
            
        public Boolean IsDeleteTimeschemaEnabled
        {
            get => _isDeleteRecordEnabled;
            set
            {
                _isDeleteRecordEnabled = value;
                NotifyPropertyChanged(nameof(IsDeleteTimeschemaEnabled));
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

                if (_TextBoxTimePoint.Length == 5)
                {
                    IsSaveEnabled = isValidTime(_TextBoxTimePoint) ? true : false;
                }
                
            }
        }

        private bool isValidTime(string inputTime)
        {
            Regex chkTime = new Regex(@"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
            return chkTime.IsMatch(inputTime);
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

        private int? _TextBoxNexaId;
        public int? TextBoxNexaId
        {
            get => _TextBoxNexaId;
            set
            {
                _TextBoxNexaId = value;
                NotifyPropertyChanged(nameof(TextBoxNexaId));
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

                IsDeleteEnabled = true;
                
                if (_selectedDevice.timeschemas != null)
                {
                    
                    _selectedDevice.timeschemas.OrderBy(p => p.Dayofweek).ThenBy(x => x.TimePoint).ToList().ForEach(NexaTimeschemas.Add);
                    IsDeleteEnabled = false;

                    IsDeleteEnabled = _selectedDevice.timeschemas.Count == 0 ? true : false;
                    IsDeleteTimeschemaEnabled= _selectedDevice.timeschemas.Count > 0 ? true : false;
                }

                TimeschemasCount = _selectedDevice.timeschemas != null ? _selectedDevice.timeschemas.Count.ToString() : "";
                IsNewEnabled = true;


                TextBoxDeviceName = _selectedDevice.DeviceName;
                TextBoxNexaId = _selectedDevice.NexaId == null ? 0 : _selectedDevice.NexaId;
                TextBoxDescription = _selectedDevice.DeviceType == null ? "" : _selectedDevice.DeviceType;

                IsNewDeviceEnabled = false;
            }
        }



        private NexaTimeSchema _selectedNexaTimeschema;
        public NexaTimeSchema SelectedNexaTimeschema
        {
            get => _selectedNexaTimeschema;
            set
            {
                _selectedNexaTimeschema = value == null ? _selectedNexaTimeschema : value;
                IsSaveEnabled = value != null ? true : false;
                NotifyPropertyChanged(nameof(SelectedNexaTimeschema));

                
                ViewInformation();

                SaveUpdateText = "Uppdatera";

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
            IsDeleteTimeschemaEnabled = true;

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

        private string _timeSchemasCount;
        public string TimeschemasCount
        {
            get => _timeSchemasCount;
            set
            {
                _timeSchemasCount = $"Configured count:  { value }";
                NotifyPropertyChanged(nameof(TimeschemasCount));
            }
        }
        
    }
}
