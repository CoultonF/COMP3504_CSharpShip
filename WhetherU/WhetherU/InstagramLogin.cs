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

namespace WhetherU
{
    [Activity(Label = "Activity1")]
    public class InstagramLogin : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            var clientId = "33665d08e62942c6b1f484f422bbb7c1";
            var clientSecret = "ae94378b39e747b0ac5d6f8640636b02 ";
            var redirectUri = "https://elfsight.com/service/generate-instagram-access-token/";
            var realtimeUri = "";

            InstagramConfig config = new InstagramConfig(clientId, clientSecret, redirectUri, realtimeUri);

            var scopes = new List<OAuth.Scope>();
            scopes.Add(InstaSharp.OAuth.Scope.Likes);
            scopes.Add(InstaSharp.OAuth.Scope.Comments);

            var link = InstaSharp.OAuth.AuthLink(config.OAuthUri + "authorize", config.ClientId, config.RedirectUri, scopes, InstaSharp.OAuth.ResponseType.Code);
            
        }

    }
}