using System;
using System.Collections.Generic;
using System.Linq;
using Plugin.Messaging;
using Xamarin.Forms;

namespace Prevoz
{
	public partial class RideDetailsPage : ContentPage
	{
		Ride ride;

		public RideDetailsPage(Ride ride)
		{
			this.ride = ride;
			BindingContext = ride;
			InitializeComponent();
			if (ride.IsAuthor)
			{
				leftButton.Text = "Uredi";
				rightButton.Text = "Izbriši";
			}
			else
			{
				leftButton.Text = "Pokliči";
				rightButton.Text = "Pošlji SMS";
			}
		}
#if DEBUG
		public RideDetailsPage()
			: this(Ride.DesignTimeRide)
		{

		}
#endif

		async void leftButtonClicked(object sender, EventArgs args)
		{
			if (ride.IsAuthor)
			{
				await Navigation.PushAsync(new AddEditRideView(ride));
				Navigation.RemovePage(this);//So when user finishes editing, it goes back to list
				return;
			}
			if (CrossMessaging.Current.PhoneDialer.CanMakePhoneCall)
				CrossMessaging.Current.PhoneDialer.MakePhoneCall(ride.Contact.Replace(" ", ""), ride.Author);
		}

		async void rightButtonClicked(object sender, EventArgs args)
		{
			if (ride.IsAuthor)
			{
				foreach (var child in grid.Children)
				{
					child.IsVisible = false;
				}
				var loader = new ActivityIndicator()
				{
					IsRunning = true
				};
				grid.Children.Add(loader, 0, grid.ColumnDefinitions.Count, 0, grid.RowDefinitions.Count);
				await PrevozApi.DeleteRideAsync(ride);
				await MyRidesPage.Instance.RefreshData();
				try
				{
					await Navigation.PopAsync(true);
				}
				catch { }//If user presses back button while processing network request this throws exception
			}
			else
			{
				if (CrossMessaging.Current.SmsMessenger.CanSendSms)
				{
					string dan = null;
					if (ride.DateAndTime.Date == DateTime.Now.Date)
					{
						dan = $"danes({ride.DateAndTime.ToPrevozDayName()})";
					}
					else if (ride.DateAndTime.Date == DateTime.Now.Date.AddDays(1))
					{
						dan = $"jutri({ride.DateAndTime.ToPrevozDayName()})";
					}
					else
					{
						dan = "v " + ride.DateAndTime.ToPrevozDateStyle();
					}
					CrossMessaging.Current.SmsMessenger.SendSms(ride.Contact, $"Imaš še prosto mesto {dan} ob {ride.DateAndTime:HH:mm} na relaciji {ride.From}-{ride.To}?");
				}
			}
		}
	}
}

