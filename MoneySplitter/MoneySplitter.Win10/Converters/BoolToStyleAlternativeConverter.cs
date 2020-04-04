using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MoneySplitter.Win10.Converters
{
	public class BoolToStyleAlternativeConverter : IValueConverter
	{
		public Style TrueStyle { get; set; }
		public Style FalseStyle { get; set; }

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return (bool) value ? TrueStyle : FalseStyle;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
