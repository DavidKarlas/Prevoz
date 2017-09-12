using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Prevoz
{
	public partial class MyRidesPage : ContentPage
	{
		public static MyRidesPage Instance { get; } = new MyRidesPage ();
		void ToolBarAddClick (object sender, System.EventArgs e)
		{
			Navigation.PushAsync (new AddEditRideView (null), true);
		}

		public MyRidesPage()
		{
			Title = "Moji prevozi";
			BindingContext = new MyRidesModelView();
			InitializeComponent();
			RefreshData().FireAndForget();
		}

		public Task RefreshData()
		{
			listView.BeginRefresh();
			return RefreshDataWithoutBegin();
		}

		async Task RefreshDataWithoutBegin()
		{
			await ((MyRidesModelView)BindingContext).LoadItemsAsync();
			listView.EndRefresh();
		}

		async void listItemTapped(object sender, ItemTappedEventArgs args)
		{
			await Navigation.PushAsync(new RideDetailsPage((Ride)args.Item));
		}

		void Handle_Refreshing(object sender, System.EventArgs e)
		{
			RefreshDataWithoutBegin().FireAndForget();
		}
	}
}

