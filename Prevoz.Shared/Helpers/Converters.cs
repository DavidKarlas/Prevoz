using System;
using System.Globalization;
using Xamarin.Forms;

namespace Prevoz
{
	public class DateConverter : IValueConverter
	{
		public object Convert (object value, Type targetType,
							  object parameter, CultureInfo culture)
		{
			if (targetType != typeof (string))
				throw new ArgumentException (nameof (targetType));
			return ((DateTime)value).ToPrevozDateStyle ();
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException ();
		}
	}

	class MoneyConverter : IValueConverter
	{
		public object Convert (object value, Type targetType,
							  object parameter, CultureInfo culture)
		{
			if (targetType != typeof (string))
				throw new ArgumentException (nameof (targetType));
			return (value as decimal?).ToPrevozMoney ();
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException ();
		}
	}

	public class BooleanNegationConverter : IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return !(bool)value;
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return !(bool)value;
		}
	}
}

