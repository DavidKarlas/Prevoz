using System;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Globalization;

namespace Prevoz
{
	public class SelectPricePage : ContentPage
	{
		public SelectPricePage(Action<decimal?> callbackPriceSet)
		{
			Title = "Stroški";
			var entry = new Entry
			{
				Keyboard = Keyboard.Numeric,
				Placeholder = "Vnesi strošek"
			};
			var buttonConfirm = new Button
			{
				Text = "Potrdi"
			};
			var grid = new Grid();
			grid.RowSpacing = 10;
			//TODO: Instead of this Brez, 0€-16€ options do MFU
			var tapLabelGesture = new TapGestureRecognizer();
			var valueMapper = new Dictionary<object, decimal?>();
			for (int i = -1; i < 19; i++)
			{
				var val = i;
				if (val > 15)
					val = 15 + (val - 15) * 5;//20,25,30

				var label = new Label()
				{
					Text = ((decimal)val).ToPrevozMoney(),
					FontSize = 22,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					HorizontalOptions = LayoutOptions.CenterAndExpand,
				};
				if (i == -1)
				{
					label.Text = "Brez";
					valueMapper.Add(label, null);
				}
				else {
					valueMapper.Add(label, val);
				}
				grid.Children.Add(label, (i + 1) % 4, (i + 1) / 4);
				label.GestureRecognizers.Add(tapLabelGesture);
			}
			tapLabelGesture.Tapped += (s, e) =>
			{
				callbackPriceSet(valueMapper[s]);
				Navigation.PopAsync();
			};
			var stackLayout = new StackLayout
			{
				Padding = new Thickness(4),
				Spacing = 4
			};
			stackLayout.Children.Add(entry);
			stackLayout.Children.Add(buttonConfirm);
			stackLayout.Children.Add(grid);
			buttonConfirm.Clicked += delegate
			{
				decimal val;
				if (decimal.TryParse(entry.Text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out val))
					callbackPriceSet(val);
				else
					callbackPriceSet(null);
				Navigation.PopAsync();
			};
			Content = stackLayout;
		}
	}
}


