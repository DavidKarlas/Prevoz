using System;

using Xamarin.Forms;
using System.Linq;

namespace Prevoz.Shared.Views
{
	public class SelectNumberOfPeople : ContentPage
	{
		public Action<int> SelectionCallback { get; }

		public SelectNumberOfPeople(Action<int> selectionCallback)
		{
			SelectionCallback = selectionCallback;
			var list = new ListView();
			Content = list;
			list.ItemsSource = Enumerable.Range(1, 8);
			list.ItemTapped += List_ItemTapped;
		}

		void List_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			SelectionCallback((int)e.Item);
			Navigation.PopAsync();
		}
	}
}

