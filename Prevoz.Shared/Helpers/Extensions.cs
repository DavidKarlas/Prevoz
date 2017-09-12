using System;
using System.Globalization;
using System.Threading.Tasks;
namespace Prevoz
{
	public static class Extensions
	{
		static CultureInfo sloCulture = new CultureInfo("sl-SI");
		public static string ToPrevozDateStyle(this DateTime date)
		{
			//if (date.Date == DateTime.Now.Date)
			//	return "Danes, " + date.Date.ToString("d", sloCulture);
			//if (date.Date == DateTime.Now.Date.AddDays(1))
			//	return "Jutri, " + date.Date.ToString("d", sloCulture);
			return date.Date.ToString("dddd(d. MMMM)", sloCulture);
		}
		public static string ToPrevozDayName(this DateTime date)
		{
			//if (date.Date == DateTime.Now.Date)
			//	return "Danes, " + date.Date.ToString("d", sloCulture);
			//if (date.Date == DateTime.Now.Date.AddDays(1))
			//	return "Jutri, " + date.Date.ToString("d", sloCulture);
			return date.Date.ToString("dddd", sloCulture);
		}

		public static string ToPrevozMoney(this decimal? price)
		{
			if (!price.HasValue)
				return "";
			return price.Value.ToPrevozMoney();
		}
		public static string ToPrevozMoney(this decimal price)
		{
			return string.Format(sloCulture, "{0:0.0} €", price);
		}

		public static void FireAndForget(this Task task)
		{
			task.ContinueWith(t => {
				//if (t.IsFaulted)
				//TODO: Logging exceptions	
			});
		}

	}
}

