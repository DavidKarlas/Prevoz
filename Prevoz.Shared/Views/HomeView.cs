using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Prevoz
{
	public class HomeView : MasterDetailPage
	{
		private HomeViewModel ViewModel {
			get { return BindingContext as HomeViewModel; }
		}

		HomeMasterView master;
		private Dictionary<Page, NavigationPage> pages;

		public HomeView ()
		{
			pages = new Dictionary<Page, NavigationPage> ();
			BindingContext = new HomeViewModel ();

			Master = master = new HomeMasterView (ViewModel);

			var homeNav = new NavigationPage (master.PageSelection) {
				BarBackgroundColor = Color.FromHex ("#3498DB"),
				BarTextColor = Color.White
			};
			Detail = homeNav;

			master.PageSelectionChanged = (menuType) => {
				NavigationPage newPage;
				if (pages.ContainsKey (menuType)) {
					newPage = pages [menuType];
				} else {
					newPage = new NavigationPage (master.PageSelection) {
						BarBackgroundColor = Color.FromHex ("#3498DB"),
						BarTextColor = Color.White
					};
					pages.Add (menuType, newPage);
				}
				Detail = newPage;
				Detail.Title = master.PageSelection.Title;
				if (Device.Idiom != TargetIdiom.Tablet)
					IsPresented = false;
			};

			this.Icon = "hamburger.png";
		}
	}


	public class HomeMasterView : ContentPage
	{
		public Action<Page> PageSelectionChanged;
		private Page pageSelection;

		public Page PageSelection {
			get { return pageSelection; }
			set {
				pageSelection = value;
				if (PageSelectionChanged != null)
					PageSelectionChanged (value);
			}
		}

		public HomeMasterView (HomeViewModel viewModel)
		{
			this.Icon = "hamburger.png";
			BindingContext = viewModel;
			Title = "Prevoz";

			var layout = new StackLayout { Spacing = 15 };
			layout.Padding = 5;
			var label = new ContentView {
				Padding = new Thickness (10, 20, 0, 5),
				BackgroundColor = Color.Transparent,
				Content = new Label {
					Text = "Prevoz",
					FontSize = Device.GetNamedSize (NamedSize.Large, typeof (Label))
				}
			};

			layout.Children.Add (label);
			layout.Children.Add (new BoxView () {
				HeightRequest = 1,
				Color = Color.FromHex ("#3498DB")
			});

			foreach (var item in viewModel.MenuItems) {
				var menuItem = new Label () {
					Text = item.Title,
					FontSize = Device.GetNamedSize (NamedSize.Large, typeof (Label))
				};
				var labelTap = new TapGestureRecognizer ();
				labelTap.Command = new Command (() => {
					PageSelection = item.GetPage ();
					Settings.LastPageId = item.Id;
				});
				menuItem.GestureRecognizers.Add (labelTap);
				layout.Children.Add (menuItem);
			}
			var lastPageId = Settings.LastPageId;
			PageSelection = (viewModel.MenuItems.FirstOrDefault (m => m.Id == lastPageId) ?? viewModel.MenuItems.First ()).GetPage ();

			Content = layout;
		}
	}

}
