using System;
using Windows.UI.Xaml.Data;

namespace MoneySplitter.Win10.Converters
{
	public class DateTimeToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var date = (DateTime) value;

			return date.ToString(parameter.ToString());
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
