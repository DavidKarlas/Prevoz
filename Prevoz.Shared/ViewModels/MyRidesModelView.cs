using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin;
using System.ComponentModel;

namespace Prevoz
{
	public class MyRidesModelView : INotifyPropertyChanged
	{
		public IEnumerable<IGrouping<string, Ride>> Items { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public async Task LoadItemsAsync ()
		{
			try {
				Items = (await PrevozApi.GetMyRides ()).OrderBy (r => r.DateAndTime).GroupBy (r => r.DateAndTime.ToPrevozDateStyle ());
			} catch (Exception ex) {
				//Insights.Report (ex);
				Items = null;
			}
			PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (nameof (Items)));
		}
	}
}

