using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InstaSharp;
using Auth0;
using Xamarin.Auth;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace WhetherU
{
    [Activity(Label = "WhetherU", MainLauncher = true, Icon = "@drawable/icon")]
    public class StartUpActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.LoginPrompt);

            // Get our button from the layout resource,
            // and attach an event to it
            ImageButton withInsta = FindViewById<ImageButton>(Resource.Id.UseInstagram);
            ImageButton withoutInsta = FindViewById<ImageButton>(Resource.Id.NoInstagram);

            //button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
            withInsta.Click += delegate {
                Console.Write("Button Clicked");
                String token = "4156801757.33665d0.fb93e6ef964f4c5dbc665147ebab12a7";
                if (token == "")
                {
                    StartActivity(typeof(InstagramLogin));
                }
                else
                {
                    Intent intent = new Intent(this, typeof(WeatherScreen));
                    Bundle data = new Bundle();
                    data.PutString("UserToken", token);
                    intent.PutExtras(data);
                    StartActivity(intent);
                }
              
            };
            withoutInsta.Click += delegate {
                Console.Write("Button Clicked");
                StartActivity(typeof(WeatherScreen));
            };
        }
       
    }
}

