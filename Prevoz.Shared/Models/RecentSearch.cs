using System;

namespace Prevoz
{
	public class RecentSearch
	{
		public City FromCity { get; set; }

		public City ToCity { get; set; }

		public int Count { get; set; }

		public string Text
		{
			get
			{
				return $"{FromCity} - {ToCity}";
			}
		}

		public override bool Equals(object obj)
		{
			return obj is RecentSearch o && o.FromCity.Equals(FromCity) && o.ToCity.Equals(ToCity);
		}

		public override int GetHashCode()
		{
			return FromCity.GetHashCode() ^ ToCity.GetHashCode();
		}
	}
}

