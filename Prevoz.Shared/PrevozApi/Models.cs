using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Prevoz
{
	public class Ride
	{
#if DEBUG
		internal static Ride DesignTimeRide = new Ride()
		{
			From = "Ljubljana",
			To = "Maribor",
			Added = DateTime.Now,
			DateAndTime = DateTime.Now,
			NumPeople = 3,
			Price = 5,
			Contact = "041 700 700",
			CarInfo = "Rumen fičo"
		};
#endif
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("type")]
		public int Type { get; set; }

		[JsonProperty("from_id")]
		public string FromId { get; set; }

		[JsonProperty("from_country")]
		public string FromCountry { get; set; }

		[JsonProperty("from_country_name")]
		public string FromCountryName { get; set; }

		[JsonProperty("to_id")]
		public string ToId { get; set; }

		[JsonProperty("to")]
		public string To { get; set; }

		[JsonProperty("to_country")]
		public string ToCountry { get; set; }

		[JsonProperty("to_country_name")]
		public string ToCountryName { get; set; }

		[JsonProperty("date_iso8601")]
		public DateTime DateAndTime { get; set; }

		[JsonProperty("added")]
		public DateTime Added { get; set; }

		[JsonProperty("price")]
		public decimal? Price { get; set; }

		public string Relation
		{
			get
			{
				return $"{From} - {To}";
			}
		}

		//TODO: Fix Prevoz.org API...
		//This is needed because Prevoz.org send us "3.0" in /api/carshare/list/ but doesn't accept "3.0"(wants "3") in /api/carshare/create/
		class ParseIntWithDecimals : JsonConverter
		{
			public override bool CanConvert(Type objectType)
			{
				return objectType == typeof(int);
			}

			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{
				writer.WriteValue(value);
			}

			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				return Convert.ToInt32(reader.Value);
			}
		}

		[JsonProperty("num_people")]
		[JsonConverter(typeof(ParseIntWithDecimals))]
		public int NumPeople { get; set; }

		[JsonProperty("author")]
		public string Author { get; set; }

		[JsonProperty("is_author")]
		public bool IsAuthor { get; set; }

		[JsonProperty("comment")]
		public string Comment { get; set; }

		[JsonProperty("contact")]
		public string Contact { get; set; }

		[JsonProperty("full")]
		public bool Full { get; set; }

		[JsonProperty("insured")]
		public bool Insured { get; set; }

		[JsonProperty("confirmed_contact")]
		public bool ConfirmedContact { get; set; }

		[JsonProperty("bookmark")]
		public object Bookmark { get; set; }

		[JsonProperty("from")]
		public string From { get; set; }

		[JsonProperty("carshare_id")]
		public object CarshareId { get; set; }

		[JsonProperty("car_info")]
		public string CarInfo { get; set; }
	}

	public class RidesResponse
	{
		[JsonProperty("search_type")]
		public string SearchType { get; set; }

		[JsonProperty("carshare_list")]
		public IList<Ride> RidesList { get; set; }
	}

	public class GeonamesLookupResponse
	{

		[JsonProperty("query")]
		public string Query { get; set; }

		[JsonProperty("suggestions")]
		public IList<string> Suggestions { get; set; }

		[JsonProperty("data")]
		public IList<object> Data { get; set; }
	}
}

