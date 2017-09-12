using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Prevoz
{
	public class HomeMenuItem
	{
		Page page;
		Func<Page> createPage;

		public HomeMenuItem (string title, Func<Page> createPage, string icon)
		{
			this.createPage = createPage;
			this.Title = title;
		}

		public string Title { get; set; }
		public string Id { get; set; }

		public Page GetPage ()
		{
			if (page == null)
				page = createPage ();
			return page;
		}
	}

	public class HomeViewModel
	{
		public ObservableCollection<HomeMenuItem> MenuItems { get; set; }

		public HomeViewModel ()
		{
			MenuItems = new ObservableCollection<HomeMenuItem> ();
			MenuItems.Add (new HomeMenuItem ("Iskanje", () => new SearchPage (), null) { Id = nameof (SearchPage) });
			MenuItems.Add (new HomeMenuItem ("Moji prevozi", () => MyRidesPage.Instance, null) { Id = nameof (MyRidesPage) });
		}
	}
}
