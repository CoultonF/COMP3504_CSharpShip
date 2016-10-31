
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;

namespace WeatherInfo
{
	[Activity ( Label = "Powered by Suchith" )]			
	public class MyWebViewActivity : Activity
	{
		protected override void OnCreate ( Bundle bundle )
		{ 
			base.OnCreate ( bundle );
			SetContentView ( Resource.Layout.MyWebViewLayout );
			WebView localWebView = FindViewById<WebView>(Resource.Id.LocalWebView);
			localWebView.SetWebViewClient (new WebViewClient ()); // stops request going to Web Browser
			localWebView.Settings.LoadWithOverviewMode = true;
			localWebView.Settings.UseWideViewPort = true; 
			localWebView.LoadUrl("http://google.com/+SuchithM");
			// Create your application here
		}
	}
}

