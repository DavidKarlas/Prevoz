using System;

namespace Prevoz
{
	public class City
	{
		public string CountryCode { get; set; }
		public string Name { get; set; }

		public City(string countryCode, string name)
		{
			CountryCode = countryCode;
			Name = name;
		}

		public override bool Equals(object obj)
		{
			return obj is City city && city.Name == Name && city.CountryCode == CountryCode;
		}

		public override int GetHashCode()
		{
			return CountryCode?.GetHashCode() ?? 0 ^ Name?.GetHashCode() ?? 0;
		}

		public override string ToString()
		{
			if (Settings.DisplayCountryCode)
				return $"{Name}({CountryCode}) )";
			else
				return Name;
		}
	}
}
