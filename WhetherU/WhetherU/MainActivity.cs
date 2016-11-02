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
    public class MainActivity : Activity
    {
        public Intent MainLaunch { get; private set; }

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            var clientId = "33665d08e62942c6b1f484f422bbb7c1";
            var clientSecret = "ae94378b39e747b0ac5d6f8640636b02 ";
            var redirectUri = "https://elfsight.com/service/generate-instagram-access-token/";
            var realtimeUri = "";

            var auth = new OAuth2Authenticator(
                clientId: "33665d08e62942c6b1f484f422bbb7c1",
                scope: "basic",
                authorizeUrl: new Uri("https://api.instagram.com/oauth/authorize/"),
                redirectUrl: new Uri("https://elfsight.com/service/generate-instagram-access-token/"));

            StartActivity(auth.GetUI(this));
            //PresentViewController(auth.GetUI(), true, null);
            IEnumerable<Account> accounts = AccountStore.Create(this).FindAccountsForService("Instagram");
            auth.Completed += (sender, eventArgs) => {
                    // We presented the UI, so it's up to us to dimiss it on iOS.
                    //DismissViewController(true, null);

                    if (eventArgs.IsAuthenticated)
                    {
                    // Use eventArgs.Account to do wonderful things
                    AccountStore.Create(this).Save(eventArgs.Account, "Instagram");

                    var request = new OAuth2Request("GET", new Uri("https://api.instagram.com/v1/tags/{tag-name}?access_token=ACCESS-TOKEN"), null, eventArgs.Account);
                    request.GetResponseAsync().ContinueWith(t => {
                        if (t.IsFaulted)
                            Console.WriteLine("Error: " + t.Exception.InnerException.Message);
                        else
                        {
                            string json = t.Result.GetResponseText();
                            Console.WriteLine(json);
                        }
                    });
                    Console.WriteLine(request);
                    SetContentView(Resource.Layout.Main);
                }
                    else
                    {
                        // The user cancelled
                    }
                };

            
        }
    }
}

