using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;

namespace Prevoz
{
	public partial class SelectLocationPage : ContentPage
	{
		readonly Action<City> callbackToSetLocation;

		public SelectLocationPage()
		{
		}

		public SelectLocationPage(Action<City> callbackToSetLocation)
		{
			this.callbackToSetLocation = callbackToSetLocation;
			InitializeComponent();
			SetDefaultListItems();
			//TODO: Support multiple countries
			//ToolbarItems.Add(new ToolbarItem("SI", "countries flag.png", () => System.Diagnostics.Debug.WriteLine("Change country")));
		}

		async void Handle_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(searchBar.Text))
			{
				SetDefaultListItems();
			}
			else
			{
				listView.ItemsSource = (await PrevozApi.GetSuggestedCities(searchBar.Text, "SI")).Select(c => new City("SI", c));
			}
		}

		void SetDefaultListItems()
		{
			var fromCities = Settings.RecentSearches.Select(r => Tuple.Create(r.FromCity, r.Count));
			var toCities = Settings.RecentSearches.Select(r => Tuple.Create(r.ToCity, r.Count));
			var citiesWithUseCount = fromCities.Concat(toCities).GroupBy(c => c.Item1).Select(g => (City: g.Key, UseCount: g.Sum(s => s.Item2)));
			listView.ItemsSource = citiesWithUseCount.OrderByDescending(c => c.UseCount).Select(c => c.City);
		}

		void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			callbackToSetLocation((City)e.Item);
			Navigation.PopAsync();
		}
	}
}

