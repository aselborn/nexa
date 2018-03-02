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
        public ObservableCollection<DeviceWrapper> MyDeviceWrapper { get; } = new ObservableCollection<DeviceWrapper>();
        public ObservableCollection<Device> Devices { get; } = new ObservableCollection<Device>();
        private Boolean IsAllowed = true;
        public MainWindowViewModel()
        {
            //DataApi.GetAllItems().ForEach(DeviceWrapper.Add);
            //DataApi.GetDevices().ForEach(Devices.Add);
            _dataApi.Devices.ForEach(Devices.Add);
            //_dataApi.AllItems.ForEach(DeviceWrapper.Add);
        }


        public ICommand SaveDevice => new RelayCommand(x => DoSaveNewDevice(), x => IsAllowed);
        public ICommand SaveSchema => new RelayCommand(x => DoSaveNewSchema(), x => IsAllowed);

        private void DoSaveNewSchema()
        {
            Schema schema = new Schema(_selectedDevice);
            schema.ActionText = _selectedAction.Content.ToString();
            schema.TimePoint = _TextBoxTimePoint;
            schema.WeekDay = _selectedWeekday.Content.ToString();

            DeviceWrapper wrapper = new DeviceWrapper(schema);
            MyDeviceWrapper.Add(wrapper);

            MyDeviceWrapper.OrderBy(p => p.TimePoint);

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
            }
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

        
    }
}
