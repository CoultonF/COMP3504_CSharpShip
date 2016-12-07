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

using SQLite;
using DataBase;

namespace WhetherU
{
    [Activity(Label = "WhetherU")]
    public class DemoMenuActivity : Activity
    {
        Button signOut;
        Button nxtImg;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.demoMenu);
            signOut = FindViewById<Button>(Resource.Id.SignOut);
            nxtImg = FindViewById<Button>(Resource.Id.NextImage);

            LocalDataAccessLayer data = LocalDataAccessLayer.getInstance();

            signOut.Click += delegate
            {
                data.deleteUser();
            };

            nxtImg.Click += delegate
            {
                //Coulton add code here
            };

        }
    }
}