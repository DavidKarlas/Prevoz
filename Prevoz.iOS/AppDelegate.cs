using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using UIKit;

namespace Prevoz.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			MobileCenter.Start("6bd6890e-f42c-410a-813b-2bb02a2c6ce8",
				   typeof(Analytics), typeof(Crashes));
			
			global::Xamarin.Forms.Forms.Init ();
			LoadApplication (new Prevoz.Portable.App ());
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start ();
#endif
			SimpleAuth.NativeSafariAuthenticator.Activate();
			return base.FinishedLaunching (app, options);
		}

		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
			if (SimpleAuth.Native.OpenUrl(app, url, options))
				return true;
			return base.OpenUrl(app, url, options);
		}
	}
}
