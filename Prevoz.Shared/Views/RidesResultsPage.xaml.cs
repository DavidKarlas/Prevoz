using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Prevoz
{
	public partial class RidesResultsPage : ContentPage
	{
#if DEBUG
		public RidesResultsPage()
		{
			InitializeComponent();
			Title = DateTime.Now.Date.ToPrevozDateStyle();
			var r = new List<Ride>(new Ride[]{
				new Ride(){ DateAndTime = DateTime.Now.AddHours(1), From="Ljubljana", To="Maribor", Price=5, Author="Janja"},
				new Ride(){ DateAndTime = DateTime.Now.AddHours(2), From="Ljubljana", To="Ljutomer", Price=7, Author="David"},
				new Ride(){ DateAndTime = DateTime.Now.AddHours(3), From="Ljubljana", To="Sežana", Price=5,Author="Veronica"},
				new Ride(){ DateAndTime = DateTime.Now.AddHours(4), From="Koper", To="Sežana", Price=3, Author="Tomaž"}
			});
			r.Sort(new Comparison<Ride>((x, y) => x.DateAndTime.CompareTo(y.DateAndTime)));
			listView.ItemsSource = r.GroupBy(g => g.Relation);
			loadingIndicator.IsVisible = false;
			loadingIndicator.IsRunning = false;
			listView.IsVisible = true;
		}
#endif
		public RidesResultsPage(City start, City end, DateTime date, bool exact)
		{
			InitializeComponent();
			Title = date.Date.ToPrevozDateStyle();

			var task = PrevozApi.GetCarShares(start.CountryCode, start.Name, end.CountryCode, end.Name, date, exact);

			task.ContinueWith((t) =>
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					if (t.IsFaulted)
					{
						//Xamarin.Insights.Report (t.Exception);
						//TODO: Inform user of network failure, tell him to retry...
						//Return user to previous view :(
						Navigation.PopAsync();
					}
					else if (!t.IsCanceled)
					{
						var r = t.Result.ToList();
						r.Sort(new Comparison<Ride>((x, y) => x.DateAndTime.CompareTo(y.DateAndTime)));
						listView.ItemsSource = r.GroupBy(g => g.Relation);
						loadingIndicator.IsRunning = false;
						loadingIndicator.IsVisible = false;
						listView.IsVisible = true;
						//TODO: In case of non exact search make sure exact match group is 1st
					}
				});
			});
		}

		async void listItemTapped(object sender, ItemTappedEventArgs args)
		{
			await Navigation.PushAsync(new RideDetailsPage((Ride)args.Item));
		}
	}
}

