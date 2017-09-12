using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Prevoz
{
	public partial class SelectDatePage : ContentPage
	{
		readonly Action<DateTime> callbackSetDate;
		class Day
		{
			public string Text { get; set; }
			public DateTime Value { get; set; }
		}

#if DEBUG
		public SelectDatePage()
			: this(null)
		{
		}
#endif

		public SelectDatePage(Action<DateTime> callbackSetDate)
		{
			this.callbackSetDate = callbackSetDate;
			InitializeComponent();
			var listOfDays = new List<Day>();
			for (var day = DateTime.Today; day < DateTime.Today.AddDays(14); day = day.AddDays(1))
			{
				listOfDays.Add(new Day()
				{
					Value = day,
					Text = day.ToPrevozDateStyle()
				});
			}
			listView.ItemsSource = listOfDays;
		}

		void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			callbackSetDate(((Day)e.Item).Value);
			Navigation.PopAsync();
		}
	}
}

