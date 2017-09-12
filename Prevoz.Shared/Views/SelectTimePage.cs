using System;
using Xamarin.Forms;

namespace Prevoz
{
	class SelectTimePage : ContentPage
	{
		readonly Action<TimeSpan> callbackSetTime;

		public SelectTimePage(Action<TimeSpan> callbackSetTime)
		{
			Title = "Ura";
			this.callbackSetTime = callbackSetTime;
			var grid = new Grid() { RowSpacing = 0, ColumnSpacing = 0 };
			var tapHourGesture = new TapGestureRecognizer();
			tapHourGesture.Tapped += TapHourGesture_Tapped;
			for (int i = 0; i < 24; i++)
			{
				var label = new Label()
				{
					BindingContext = i,
					Text = i.ToString("00"),
					FontSize = 30,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					HorizontalOptions = LayoutOptions.CenterAndExpand
				};
				grid.Children.Add(label, i % 4, i / 4);
				label.GestureRecognizers.Add(tapHourGesture);

				var box = new ContentView()
				{
					BindingContext = i
				};
				grid.Children.Add(box, i % 4, i / 4);
				box.GestureRecognizers.Add(tapHourGesture);
			}
			Content = grid;
		}

		int hourSelected;

		void TapHourGesture_Tapped(object sender, EventArgs e)
		{
			Title = "Minuta";
			hourSelected = (int)((View)sender).BindingContext;

			var grid = new Grid() { RowSpacing = 0, ColumnSpacing = 0 };
			var tapMinuteGesture = new TapGestureRecognizer();
			tapMinuteGesture.Tapped += TapMinuteGesture_Tapped; ;
			for (int i = 0; i < 12; i++)
			{
				var label = new Label()
				{
					BindingContext = i * 5,
					Text = (i * 5).ToString("00"),
					FontSize = 30,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					HorizontalOptions = LayoutOptions.CenterAndExpand
				};
				grid.Children.Add(label, i % 3, i / 3);
				label.GestureRecognizers.Add(tapMinuteGesture);

				var box = new ContentView()
				{
					BindingContext = i * 5
				};
				grid.Children.Add(box, i % 3, i / 3);
				box.GestureRecognizers.Add(tapMinuteGesture);
			}
			Content = grid;
		}

		void TapMinuteGesture_Tapped(object sender, EventArgs e)
		{
			var minutes = (int)((View)sender).BindingContext;
			callbackSetTime(new TimeSpan(hourSelected, minutes, 0));
			Navigation.PopAsync();
		}
	}
}