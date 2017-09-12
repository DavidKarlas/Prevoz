using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Prevoz.UITest
{
	public class AppInitializer
	{
		public static IApp StartApp (Platform platform)
		{
			if (platform == Platform.Android) {
				return ConfigureApp
					.Android
					.ApkFile ("../../../Prevoz.Droid/bin/Release/si.karlas.prevoz.apk")
					.StartApp ();
			}

			return ConfigureApp
				.iOS
				.AppBundle ("../../../Prevoz.iOS/bin/iPhoneSimulator/Debug/Prevoz.app")
				.StartApp ();
		}
	}
}

