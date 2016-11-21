using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Instagram.Client;
using Instagram.Models;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Instagram.Models.Responses;

namespace WhetherU
{
    [Activity(Label = "MainScreen")]
    public class MainScreen : Activity {
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            string token = Intent.GetStringExtra("UserToken") ?? "False";
            Console.WriteLine(token);

            var InstagramClient = new InstagramClient(token);

            MediasResponse imageResponse = new MediasResponse();

            imageResponse = await InstagramClient.GetMyUserAsync();

            Console.WriteLine("this");
        }

    }





}