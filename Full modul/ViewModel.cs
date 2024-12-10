using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Full_modul
{    
    public class ViewModel : INotifyPropertyChanged
    {
        private bool _isDropDownOpen;
        private bool _isCalendarVisible; // Новое свойство

        public bool IsDropDownOpen
        {
            get => _isDropDownOpen;
            set
            {
                if (_isDropDownOpen != value)
                {
                    _isDropDownOpen = value;
                    OnPropertyChanged(nameof(IsDropDownOpen));
                    OnPropertyChanged(nameof(ArrowAngle)); // Уведомляем об изменении угла стрелки
                    IsCalendarVisible = _isDropDownOpen; // Устанавливаем видимость календаря
                }
            }
        }

        public bool IsCalendarVisible // Свойство для управления видимостью календаря
        {
            get => _isCalendarVisible;
            set
            {
                if (_isCalendarVisible != value)
                {
                    _isCalendarVisible = value;
                    OnPropertyChanged(nameof(IsCalendarVisible));
                }
            }
        }

        public double ArrowAngle => IsDropDownOpen ? 180 : 0; // Угол стрелки

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }      

    }

}
