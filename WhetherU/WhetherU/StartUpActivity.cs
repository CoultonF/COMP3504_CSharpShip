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
    [Activity(Label = "WhetherU", MainLauncher = true, Icon = "@drawable/icon")]
    public class StartUpActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            // Set our view from the "main" layout resource
            string token;

            LocalDataAccessLayer dataAc = LocalDataAccessLayer.getInstance();
            User user;
            try
            {
                user = dataAc.getUser();
            }
            catch (Exception e)
            {
                dataAc.addUser("User", "");
                user = dataAc.getUser();
            }

            token = user.login;
            if (token != null && token != "")
            {

                Intent intent = new Intent(this, typeof(WeatherScreen));
                Bundle data = new Bundle();
                data.PutString("UserToken", token);
                intent.PutExtras(data);
                StartActivity(intent);
            }
            else
            {
                SetContentView(Resource.Layout.LoginPrompt);

                ImageButton withInsta = FindViewById<ImageButton>(Resource.Id.UseInstagram);
                ImageButton withoutInsta = FindViewById<ImageButton>(Resource.Id.NoInstagram);

                withInsta.Click += delegate {
                    Console.Write("Button Clicked");



                    //if token does not exist
                    if (user.login == "" || user.login == null)
                    {
                        StartActivity(typeof(InstagramLogin));
                    }


                    //if token exists pull from DB
                    else
                    {
                        token = user.login;
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
}

