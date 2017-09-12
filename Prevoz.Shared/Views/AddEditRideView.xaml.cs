using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Globalization;
using Prevoz.Portable;
using Prevoz.Shared.Views;
using System.Linq;

namespace Prevoz
{
	public partial class AddEditRideView : ContentPage
	{
		void Focus_TelEntry(object sender, System.EventArgs e)
		{
			TelEntry.Focus();
		}

		void Focus_CarInfoEntry(object sender, System.EventArgs e)
		{
			CarInfoEntry.Focus();
		}

		void Focus_CommentsEditor(object sender, System.EventArgs e)
		{
			CommentsEditor.Focus();
		}

#if DEBUG
		public AddEditRideView()
			: this(Ride.DesignTimeRide)
		{
		}
#endif

		Ride editingRide;
		public AddEditRideView(Ride ride)
		{
			InitializeComponent();
			if (ride != null)
			{
				editingRide = ride;
				InitializeComponent();
				Title = "Uredi Prevoz";
				okButton.Text = "Popravi";

				StartLocation.City = new City(ride.FromCountry, ride.From);
				EndLocation.City = new City(ride.ToCountry, ride.To);
				DateAndTimeOfRide = ride.DateAndTime;
				PassangersCountLabel.Text = ride.NumPeople.ToString();
				Price = ride.Price;
				TelEntry.Text = ride.Contact;
				InsuredSwitch.IsToggled = ride.Insured;
				CommentsEditor.Text = ride.Comment;
				CarInfoEntry.Text = ride.CarInfo;

			}
			else
			{
				Title = "Dodaj Prevoz";
				okButton.Text = "Dodaj";
				InsuredSwitch.IsToggled = Settings.InsuredDefault;
				TelEntry.Text = Settings.PhoneNumber;
				PassangersCountLabel.Text = Settings.PassangersCountDefault.ToString();
				CarInfoEntry.Text = Settings.LastCarInfo;
			}
		}

		DateTime dateAndTimeOfRide;
		DateTime DateAndTimeOfRide
		{
			get
			{
				return dateAndTimeOfRide;
			}
			set
			{
				if (dateAndTimeOfRide != value)
				{
					dateAndTimeOfRide = value;
					DateLabel.Text = dateAndTimeOfRide.ToPrevozDateStyle();
					TimeLabel.Text = $"{dateAndTimeOfRide.TimeOfDay:hh\\:mm}";
				}
			}
		}

		void StartLocation_Tapped(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new SelectLocationPage((s) =>
			{
				//TODO: Use Country
				StartLocation.City = s;
				UpdateCosts();
			}));
		}

		void EndLocation_Tapped(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new SelectLocationPage((s) =>
			{
				//TODO: Use Country
				EndLocation.City = s;
				UpdateCosts();
			}));
		}

		private bool costsAlreadySet;
		private void UpdateCosts()
		{
			if (costsAlreadySet)
				return;

		}

		void Date_Tapped(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new SelectDatePage((d) => DateAndTimeOfRide = d.AddMinutes(DateAndTimeOfRide.TimeOfDay.TotalMinutes)));
		}

		void Time_Tapped(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new SelectTimePage((t) => DateAndTimeOfRide = DateAndTimeOfRide.Date.AddMinutes(t.TotalMinutes)));
		}

		void PassangersCountLabel_Tapped(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new SelectNumberOfPeople((t) => PassangersCountLabel.Text = t.ToString()));
		}

		decimal? price;

		public decimal? Price
		{
			get
			{
				return price;
			}

			set
			{
				if (price != value)
				{
					price = value;
					CostsLabel.Text = value.ToPrevozMoney();
				}
			}
		}

		async void Handle_Clicked(object sender, System.EventArgs e)
		{
			Ride ride;
			if (editingRide != null)
			{
				ride = editingRide;
			}
			else
			{
				ride = new Ride();
			}

			ride.IsAuthor = true;
			ride.Added = DateTime.Now;
			ride.FromCountry = StartLocation.City.CountryCode;
			ride.From = StartLocation.City.Name;
			ride.ToCountry = EndLocation.City.CountryCode;
			ride.To = EndLocation.City.Name;
			ride.Price = Price;
			ride.DateAndTime = DateAndTimeOfRide;
			ride.NumPeople = int.Parse(PassangersCountLabel.Text);
			ride.Contact = TelEntry.Text;
			ride.Insured = InsuredSwitch.IsToggled;
			ride.Comment = CommentsEditor.Text;
			ride.CarInfo = CarInfoEntry.Text;
			foreach (var child in grid.Children)
			{
				child.IsVisible = false;
			}
			var loader = new ActivityIndicator()
			{
				IsRunning = true
			};
			grid.Children.Add(loader, 0, grid.ColumnDefinitions.Count, 0, grid.RowDefinitions.Count);
			await PrevozApi.PostRide(ride);
			Settings.PhoneNumber = TelEntry.Text;
			Settings.InsuredDefault = InsuredSwitch.IsToggled;
			Settings.LastCarInfo = CarInfoEntry.Text;
			await MyRidesPage.Instance.RefreshData();
			try
			{
				await Navigation.PopAsync(true);
			}
			catch { }//If user presses back button while processing network request this throws exception

			//This is temporary hack so Locations also have entries for people that don't search
			//Proper solution would be to store rides in sqlite and pull costs based on From-To city...
			Settings.AddRecentSearches(new RecentSearch()
			{
				FromCity = StartLocation.City,
				ToCity = EndLocation.City
			});
		}

		void Costs_Tapped(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new SelectPricePage((p) =>
			{
				Price = p;
				costsAlreadySet = true;
			}));
		}
	}
}

