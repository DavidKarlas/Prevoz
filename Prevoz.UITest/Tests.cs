using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using Xamarin.UITest.iOS;

namespace Prevoz.UITest
{
	[TestFixture (Platform.Android)]
	[TestFixture (Platform.iOS)]
	public class Tests
	{
		IApp app;
		Platform platform;

		public Tests (Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public void BeforeEachTest ()
		{
			app = AppInitializer.StartApp (platform);
		}

		[Test]
		public void BasicSearch ()
		{
			app.Screenshot ("App started.");
			app.Tap (x => x.Text ("Od:"));
			app.Screenshot ("Tapped on view FormsTextView with Text: 'Od:'");
			if (app is iOSApp) {
				app.Tap (x => x.Class ("UISearchBarTextField"));
				app.EnterText (x => x.Class ("UISearchBarTextField"), "se");
			} else {
				app.Tap (x => x.Id ("search_src_text"));
				app.EnterText (x => x.Id ("search_src_text"), "se");
			}
			app.Screenshot ("Entered 'se' into search box.");
			app.Tap (x => x.Text ("Sežana"));
			app.Screenshot ("Tapped on view TextView with Text: 'Sežana'");
			app.Tap (x => x.Text ("Do:"));
			app.Screenshot ("Tapped on view Platform_DefaultRenderer with Text: 'Do:'");
			if (app is iOSApp) {
				app.Tap (x => x.Class ("UISearchBarTextField"));
				app.EnterText (x => x.Class ("UISearchBarTextField"), "lj");
			} else {
				app.Tap (x => x.Id ("search_src_text"));
				app.EnterText (x => x.Id ("search_src_text"), "lj");
			}
			app.Screenshot ("Entered 'lj' into search box.");
			app.Tap (x => x.Text ("Ljubljana"));
			app.Screenshot ("Tapped on view TextView with Text: 'Ljubljana'");
			app.Tap (x => x.Text ("Išči"));
			app.Screenshot ("Tapped on view AppCompatButton with Text: 'Išči'");
			app.Tap (x => x.Text ("16:00"));
			app.Screenshot ("Tapped on view FormsTextView with Text: '16:00'");
			app.WaitForElement ("Pokliči");
			app.Screenshot ("Finished.");
		}
	}
}

