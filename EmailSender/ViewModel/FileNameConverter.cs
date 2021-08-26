using System;
using System.Globalization;
using System.Windows.Data;

namespace EmailSender.ViewModel
{
    public class FileNameConverter : IValueConverter
    {
        //Конвертёр получает полный путь к файлу и возвращает имя файла 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new string((value as string).ToCharArray(), (value as string).LastIndexOf('\\') + 1, (value as string).Length - (value as string).LastIndexOf('\\') - 1);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
