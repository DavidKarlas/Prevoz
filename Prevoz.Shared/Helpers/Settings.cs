using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Prevoz
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		public static bool InsuredDefault
		{
			get
			{
				return AppSettings.GetValueOrDefault(nameof(InsuredDefault), false);
			}
			set
			{
				AppSettings.AddOrUpdateValue(nameof(InsuredDefault), value);
			}
		}

		public static string LastPageId
		{
			get
			{
				return AppSettings.GetValueOrDefault(nameof(LastPageId), "");
			}
			set
			{
				AppSettings.AddOrUpdateValue(nameof(LastPageId), value);
			}
		}

		public static string PhoneNumber
		{
			get
			{
				return AppSettings.GetValueOrDefault(nameof(PhoneNumber), "");
			}
			set
			{
				AppSettings.AddOrUpdateValue(nameof(PhoneNumber), value);
			}
		}

		public static bool DisplayCountryCode
		{
			get
			{
				return AppSettings.GetValueOrDefault(nameof(DisplayCountryCode), false);
			}
			set
			{
				AppSettings.AddOrUpdateValue(nameof(DisplayCountryCode), value);
			}
		}

		public static int PassangersCountDefault
		{
			get
			{
				return AppSettings.GetValueOrDefault(nameof(PassangersCountDefault), 3);
			}
			set
			{
				AppSettings.AddOrUpdateValue(nameof(PassangersCountDefault), value);
			}
		}

		static ObservableCollection<RecentSearch> recentSearches;

		public static ObservableCollection<RecentSearch> RecentSearches
		{
			get
			{
				if (recentSearches != null)
					return recentSearches;
				var str = AppSettings.GetValueOrDefault(nameof(RecentSearches), null);
				if (string.IsNullOrEmpty(str))
				{
					recentSearches = new ObservableCollection<RecentSearch>();
				}
				else
				{
					try
					{
						recentSearches = JsonConvert.DeserializeObject<ObservableCollection<RecentSearch>>(str);
					}
					catch (Exception e)
					{
						//TODO: Reporting
						recentSearches = new ObservableCollection<RecentSearch>();
					}
				}
				recentSearches.CollectionChanged += delegate
				{
					AppSettings.AddOrUpdateValue(nameof(RecentSearches), JsonConvert.SerializeObject(recentSearches));
				};
				return recentSearches;
			}
		}

		public static string LastCarInfo
		{
			get
			{
				return AppSettings.GetValueOrDefault(nameof(LastCarInfo), "");
			}
			set
			{
				AppSettings.AddOrUpdateValue(nameof(LastCarInfo), value);
			}
		}

		public static void AddRecentSearches(RecentSearch recentSearch)
		{
			if (string.IsNullOrWhiteSpace(recentSearch.FromCity.Name) || string.IsNullOrWhiteSpace(recentSearch.ToCity.Name))
				return;
			//Notice that RecentSearchesModel has custom Equals witch excludes Count
			if (RecentSearches.Contains(recentSearch))
			{
				recentSearch = RecentSearches[RecentSearches.IndexOf(recentSearch)];
				recentSearch.Count++;
			}
			else
			{
				recentSearch.Count = 1;
				RecentSearches.Add(recentSearch);
			}
			var list = RecentSearches.ToList();
			list.Sort((x, y) => y.Count - x.Count);
			//Limit to 10 most frequently used
			if (list.Count > 10)
				list.RemoveRange(10, list.Count - 10);
			RecentSearches.Clear();
			foreach (var i in list)
				RecentSearches.Add(i);
		}
	}
}