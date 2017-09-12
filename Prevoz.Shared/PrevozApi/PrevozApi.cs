using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using SimpleAuth;

namespace Prevoz
{
	class PrevozApi : OAuthApi
	{
		private static PrevozApi Instance { get; } = new PrevozApi();

		public PrevozApi()
			: base("Prevoz.org",
				   "fc731983dfed079c61e0",
				   "af6269fcecfa91ea371ceb72c3589d13f18c3a98"
#if DEBUG
					, new LoggingHttpHandler()
#endif
				   )
		{
			Scopes = new[] { "read" };
			TokenUrl = "https://prevoz.org/oauth2/access_token/";
			Client.BaseAddress = new Uri("https://prevoz.org/api/");
#if __IOS__
			CurrentShowAuthenticator = NativeSafariAuthenticator.ShowAuthenticator;
#endif
		}

		public class PrevozAuthenticator : WebAuthenticator
		{
			public override string BaseUrl { get; set; } = "https://prevoz.org/oauth2/authorize/";
			public override Uri RedirectUrl { get; set; } = new Uri("si.karlas.prevoz://auth/done/");
			public override async Task<Dictionary<string, string>> GetTokenPostData(string clientSecret)
			{
				var data = await base.GetTokenPostData(clientSecret);
				data["redirect_uri"] = RedirectUrl.AbsoluteUri;
				return data;
			}
		}

		protected override WebAuthenticator CreateAuthenticator()
		{
			return new PrevozAuthenticator
			{
				ClientId = ClientId,
				ClearCookiesBeforeLogin = CalledReset,
				Scope = Scopes.ToList()
			};
		}

		public static async Task<IList<Ride>> GetCarShares(string startCountry, string startCity, string endCountry, string endCity, DateTime date, bool exact)
		{
			return (await Instance.Get<RidesResponse>("search/shares/" +
												$"?f={startCity}" +
												$"&fc={startCountry}" +
												$"&t={endCity}" +
												$"&tc={endCountry}" +
												$"&d={date:yyyy-MM-dd}" +
												$"&exact={exact}")).RidesList;
		}

		public static async Task<IList<string>> GetSuggestedCities(string text, string country)
		{

			return (await Instance.Get<GeonamesLookupResponse>($"geonames/lookup/?query={text}")).Suggestions;
		}

		public static async Task PostRide(Ride ride)
		{
			await Instance.Authenticate();
			Instance.RealVerifyCredentials();
			await Instance.Post<string>("carshare/create/", ride.ToJson());
		}

		public static async Task<IList<Ride>> GetMyRides()
		{
			await Instance.Authenticate();
			Instance.RealVerifyCredentials();
			return (await Instance.Get<RidesResponse>("carshare/list/")).RidesList;
		}

		public static async Task DeleteRideAsync(Ride ride)
		{
			await Instance.Authenticate();
			Instance.RealVerifyCredentials();
			await Instance.Client.DeleteAsync($"/api/carshare/delete/{ride.Id}/");
		}

		protected override Task VerifyCredentials()
		{
			return Task.FromResult(0);
		}

		void RealVerifyCredentials()
		{
			base.VerifyCredentials();
		}

#if DEBUG
		class LoggingHttpHandler : HttpClientHandler
		{
			protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
			{
				System.Diagnostics.Debug.WriteLine("Request:" + request.RequestUri);
				var response = await base.SendAsync(request, cancellationToken);
				System.Diagnostics.Debug.WriteLine("Response:" + await response.Content.ReadAsStringAsync());
				return response;
			}
		}
#endif
	}
}