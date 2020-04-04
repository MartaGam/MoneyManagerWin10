using System;
using Windows.UI.Xaml.Data;

namespace MoneySplitter.Win10.Converters
{
	public class DoubleToPlusMinusConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var result = Math.Round((double) value, 2);

			var finalResult = result.ToString("0.00");

			if (result >= 0)
			{
				finalResult = "+" + finalResult;
			}

			return finalResult;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
