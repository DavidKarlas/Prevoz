using System;
using Xamarin.Forms;

namespace Prevoz
{
	public class CityLabel : Label
	{
		private City city;
		public City City
		{
			get
			{
				return city;
			}
			set
			{
				if (city == value)
					return;
				city = value;
				Text = city.ToString();
			}
		}
	}
}
