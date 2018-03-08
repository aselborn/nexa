using Nexa.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static Nexa.Models.DeviceWrapper;

namespace Nexa.ViewModels
{
   
    class MainWindowViewModel : ViewModelBase
    {
        private DataApi _dataApi = new DataApi();
        public ObservableCollection<DeviceWrapper> MyDeviceWrapper { get; } = new ObservableCollection<DeviceWrapper>();
        public ObservableCollection<Device> Devices { get; } = new ObservableCollection<Device>();

        public ObservableCollection<WeekDays> WeekDaysCollection { get; } = new ObservableCollection<WeekDays>();

        private Boolean IsAllowed = false;
        private Boolean IsSaveAllowed = false;

        public List<string> VeckoDagar = new List<string> { "Måndag", "Tisdag", "Onsdag", "Torsdag", "Fredag", "Lördag", "Söndag" };

        public MainWindowViewModel()
        {
            _dataApi.GetDbDevices.ForEach(Devices.Add);
            for (int n = 0; n < 7; n++)
            {
                WeekDaysCollection.Add(new ViewModels.WeekDays() { WeekDayName = WeekDayName(n), WeekDayId = n });
            }
        }

      

        public ICommand SaveDevice => new RelayCommand(x => DoSaveNewDevice(), x => IsSaveAllowed);
        public ICommand SaveSchema => new RelayCommand(x => DoSaveNewSchema(), x => IsAllowed);
        public ICommand PrepareNew => new RelayCommand(x => DoPrepareNew(), x => IsAllowed);

        private void DoPrepareNew()
        {
            
        }

        private void DoSaveNewSchema()
        {

            Schema schema = new Schema(_selectedDevice);
            schema.ActionText = _selectedAction.Content.ToString();
            schema.TimePoint = DateTime.Parse(_TextBoxTimePoint);
            schema.WeekDay = _selWeekDay.WeekDayId;

            DeviceWrapper wrapper = new DeviceWrapper(schema);

            MyDeviceWrapper.Add(wrapper);
            MyDeviceWrapper.OrderBy(p => p.TimePoint);

            _dataApi.SaveSchemaForDevice(schema);

            TextBoxTimePoint = string.Empty;



        }

        private void DoSaveNewDevice()
        {
            Random r = new Random();
            int rInt = r.Next(0, 100); //for ints

            if (_TextBoxDeviceName.Length == 0)
            {
                return;
            }

            Device device = new Device() { DeviceDescription = _TextBoxDescription, DeviceName = _TextBoxDeviceName, DeviceId=rInt };
            Devices.Add(device);

            TextBoxDescription = string.Empty;
            TextBoxDeviceName = string.Empty;

            _dataApi.SaveNewDevice(device);
            
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

        private Device _selectedDevice;
        public Device SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                _selectedDevice = value;
                NotifyPropertyChanged(nameof(SelectedDevice));

                MyDeviceWrapper.Clear();
                
                List<DeviceWrapper> forThisDevice = _dataApi.GetWrappers(_selectedDevice.DeviceId);
                forThisDevice.OrderBy(x => x.WeekDay).ThenBy(n => n.TimePoint);
                forThisDevice.ForEach(MyDeviceWrapper.Add);
                IsSaveAllowed = true;
            }
        }

        

        private DeviceWrapper _selectedWrapperSchema;
        public DeviceWrapper SelectedWrapperSchema
        {
            get => _selectedWrapperSchema;
            set
            {
                _selectedWrapperSchema = value;
                NotifyPropertyChanged(nameof(SelectedWrapperSchema));

                ViewInformation();

            }
        }

        private void ViewInformation()
        {
            EnumDayOfWeek veckodag = (EnumDayOfWeek)_selectedWrapperSchema.WeekDay;
            string s = veckodag.ToString();

            string p = Enum.GetName(typeof(EnumDayOfWeek), _selectedWrapperSchema.WeekDay);

            SelWeekDay = new ViewModels.WeekDays() { WeekDayId = _selectedWrapperSchema.WeekDay, WeekDayName = p };
            
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

        private ComboBoxItem _selectedAction;
        public ComboBoxItem SelectedAction
        {
            get => _selectedAction;
            set
            {
                _selectedAction= value;
                NotifyPropertyChanged(nameof(SelectedAction));
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


    }
}
