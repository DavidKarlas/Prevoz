using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using System.Threading;

namespace Prevoz
{
	public partial class SearchPage : ContentPage
	{
		public SearchPage()
		{
			Title = "Iskanje";
			InitializeComponent();
			recentSearches.ItemsSource = Settings.RecentSearches;
			Day = DateTime.Now;
		}
		DateTime day;
		DateTime Day
		{
			get
			{
				return day;
			}
			set
			{
				if (day != value)
				{
					day = value;
					DateLabel.Text = day.ToPrevozDateStyle();
				}
			}
		}

		async void searchButtonClicked(object sender, EventArgs e)
		{
			//TODO: Support countries
			//TODO: Support non-exact matches
			await Navigation.PushAsync(new RidesResultsPage(StartLocation.City, EndLocation.City, Day, true));

			Settings.AddRecentSearches(new RecentSearch()
			{
				FromCity = StartLocation.City,
				ToCity = EndLocation.City
			});
		}

		void RecentSearches_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
				return;
			var item = e.SelectedItem as RecentSearch;
			StartLocation.City = item.FromCity;
			EndLocation.City = item.ToCity;
			recentSearches.SelectedItem = null;
		}

		void StartLocation_Tapped(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new SelectLocationPage((s) => StartLocation.City = s));
		}

		void EndLocation_Tapped(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new SelectLocationPage((s) => EndLocation.City = s));
		}

		void Date_Tapped(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new SelectDatePage((d) => Day = d));
		}
	}
}
