using System;
using System.Globalization;
using System.Windows.Data;

namespace GamifiedLearningPlatform.Converters
{
    public class BoolToCompletedStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCompleted)
            {
                return isCompleted ? "Завдання виконано" : "Завдання не виконано";
            }
            return "Невідомий статус";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}