using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InstaSharp;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Auth;
using Instagram.Client;
using WhetherU.Models;

namespace WhetherU
{
    [Activity(Label = "InstagramSignIn")]
    public class InstagramLogin : Activity
    {

        public static OAuthSettings XamarinAuthSettings { get; private set; }

        /// <summary>
        /// A static intance of IInstagramClient.
        /// </summary>
        /// <value>The Instagram client.</value>
        /// <remarks>Not the cleanest way to use the Instagram, but it works. IoC/DI would be better (and recommended), but this app is meant to be simple.</remarks>
        public static IInstagramClient InstagramClient { get; private set; }

        public static bool IsAuthenticated
        {
            get { return (!String.IsNullOrWhiteSpace(Token)); }
        }
        

        /// <summary>
        /// The Instagram API token returned from a successful login. This token is unique to each Instagram user.
        /// </summary>
        /// <value>The Instagram API token.</value>
        public static string Token { get; private set; }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            // Set our view from the "main" layout resource
            var clientId = "33665d08e62942c6b1f484f422bbb7c1";
            var clientSecret = "ae94378b39e747b0ac5d6f8640636b02 ";
            var redirectUri = "https://elfsight.com/service/generate-instagram-access-token/";
            var realtimeUri = "";

            var auth = new OAuth2Authenticator(
                clientId: clientId,
                scope: "basic",
                authorizeUrl: new Uri("https://api.instagram.com/oauth/authorize/"),
                redirectUrl: new Uri(redirectUri));
            var token = "";
            auth.Completed += (s, ee) => {
                token = ee.Account.Properties["access_token"];

            };
            StartActivity(auth.GetUI(this));
            //PresentViewController(auth.GetUI(), true, null);
            IEnumerable<Account> accounts = AccountStore.Create(this).FindAccountsForService("Instagram");
            auth.Completed += async (sender, eventArgs) =>
            {
                // We presented the UI, so it's up to us to dimiss it on iOS.
                //DismissViewController(true, null);

                if (eventArgs.IsAuthenticated)
                {
                    // Use eventArgs.Account to do wonderful things
                    AccountStore.Create(this).Save(eventArgs.Account, "Instagram");
                    Console.WriteLine(token);
                    var InstagramClient = new InstagramClient(token);
                    await InstagramClient.GetMyUserAsync();
                    Console.Write(InstagramClient.GetType());
                    SetContentView(Resource.Layout.Main);
                }
                else
                {
                    // The user cancelled
                    StartActivity(typeof(StartUpActivity));
                }
            };
            // Create your application here
            //var clientId = "33665d08e62942c6b1f484f422bbb7c1";
            //var clientSecret = "ae94378b39e747b0ac5d6f8640636b02 ";
            //var redirectUri = "https://elfsight.com/service/generate-instagram-access-token/";
            //var realtimeUri = "";/////

            //InstagramConfig config = new InstagramConfig(clientId, clientSecret, redirectUri, realtimeUri);

            //var scopes = new List<OAuth.Scope>();
            //scopes.Add(InstaSharp.OAuth.Scope.Likes);
            //scopes.Add(InstaSharp.OAuth.Scope.Comments);

            //var link = InstaSharp.OAuth.AuthLink(config.OAuthUri + "authorize", config.ClientId, config.RedirectUri, scopes, InstaSharp.OAuth.ResponseType.Code);
            
        }

    }
}