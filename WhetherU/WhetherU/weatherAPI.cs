using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;

//instagram packages
using Instagram.Client;
using Instagram.Models;
using Instagram.Models.Responses;

using System.Linq;


using System.Net;
//using System.Text;
using Android.Graphics;
using static Android.Widget.ImageView;

namespace WhetherU
{
    

    [Activity(Label = "weatherAPI", MainLauncher = false, Icon = "@drawable/icon")]
    public class WeatherScreen : Activity
    {
        string token;

        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);

            //Make token be pulled from DB
            token = Intent.GetStringExtra("UserToken") ?? "False";
            if (token != "False")
            {
                InstagramClient client = new InstagramClient(token);
                MediasResponse media = await client.GetRecentMediaByTagName("cold");
                var datas = media.Data;
                if (datas.Count > 0)
                {
                    Bitmap imageBitmap = null;
                    String url = media.Data.ElementAt<Media>(0).Images.StandardResolution.Url.ToString();
                    using (var webClient = new WebClient())
                    {
                        var imageBytes = webClient.DownloadData(url);
                        if (imageBytes != null && imageBytes.Length > 0)
                        {
                            imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                        }
                    }

                    SetContentView(Resource.Layout.weatherAPI);
                    ImageView image = FindViewById<ImageView>(Resource.Id.imgAbsolute);
                    image.SetImageBitmap(imageBitmap);
                    image.SetScaleType(ScaleType.CenterCrop);
                    runWeather("51.0114","114.1288");
                    
                
                    
                }
                else
                {
                    //TODO:pull image from database for criteria

                    SetContentView(Resource.Layout.weatherAPI);
                    ImageView image = FindViewById<ImageView>(Resource.Id.imgAbsolute);
                    //image.SetImageResource(Resource.Drawable.main);
                    runWeather("51.0114", "114.1288");

                    
                }
            }

            // Set our view from the "main" layout resource
            //SetContentView(Resource.Layout.weatherAPI);


            // Get our button from the layout resource,
            // and attach an event to it
            //Button button = FindViewById<Button>(Resource.Id.weatherBtn);


            //button.Click += Button_Click;
        }


        private async void runWeather(String lat, String lon)
        {
            //EditText zipCodeEntry = FindViewById<EditText>(Resource.Id.zipCodeEntry);
            Weather weather = await Core.GetWeather("51.0144", "114.1288");
            FindViewById<TextView>(Resource.Id.locationText).Text = weather.Title;
            FindViewById<TextView>(Resource.Id.tempText).Text = weather.Temperature;
            FindViewById<TextView>(Resource.Id.windText).Text = weather.Wind;
            FindViewById<TextView>(Resource.Id.visibilityText).Text = weather.Visibility;
            FindViewById<TextView>(Resource.Id.humidityText).Text = weather.Humidity;
            FindViewById<TextView>(Resource.Id.sunriseText).Text = weather.Sunrise;
            FindViewById<TextView>(Resource.Id.sunsetText).Text = weather.Sunset;

            //if (!String.IsNullOrEmpty(zipCodeEntry.Text))
            //{

            //}
        }


        public class Weather
        {
            public string Title { get; set; }
            public string Temperature { get; set; }
            public string Wind { get; set; }
            public string Humidity { get; set; }
            public string Visibility { get; set; }
            public string Sunrise { get; set; }
            public string Sunset { get; set; }


            public Weather()
            {
                //Because labels bind to these values, set them to an empty string to
                //ensure that the label appears on all platforms by default.
                this.Title = " ";
                this.Temperature = " ";
                this.Wind = " ";
                this.Humidity = " ";
                this.Visibility = " ";
                this.Sunrise = " ";
                this.Sunset = " ";
            }
        }


        public class Core
        {
            public static async Task<Weather> GetWeather(string lat, string lon)
            {
                //Sign up for a free API key at http://openweathermap.org/appid
                string key = "5a4417e333b538728fea644bd7eb16cf";
                string queryString = "http://api.openweathermap.org/data/2.5/weather?lat=51.0144&lon=-114.1288&appid=5a4417e333b538728fea644bd7eb16cf&units=metric";


                //Make sure developers running this sample replaced the API key
                if (key == "YOUR API KEY HERE")
                {
                    throw new ArgumentException("You must obtain an API key from openweathermap.org/appid and save it in the 'key' variable.");
                }


                var results = await DataService.GetDataFromService(queryString).ConfigureAwait(false);


                if (results["weather"] != null)
                {
                    Weather weather = new Weather();
                    weather.Title = (string)results["name"];
                    weather.Temperature = (string)results["main"]["temp"] + " F";
                    weather.Wind = (string)results["wind"]["speed"] + " mph";
                    weather.Humidity = (string)results["main"]["humidity"] + " %";
                    weather.Visibility = (string)results["weather"][0]["main"];


                    DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                    DateTime sunrise = time.AddSeconds((double)results["sys"]["sunrise"]);
                    DateTime sunset = time.AddSeconds((double)results["sys"]["sunset"]);
                    weather.Sunrise = sunrise.ToString() + " UTC";
                    weather.Sunset = sunset.ToString() + " UTC";
                    return weather;
                }
                else
                {
                    return null;
                }
            }
        }


        public class DataService
        {
            public static async Task<JContainer> GetDataFromService(string queryString)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(queryString);


                JContainer data = null;
                if (response != null)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    data = (JContainer)JsonConvert.DeserializeObject(json);
                }


                return data;
            }
        }

        public class InstagramData
        {

            public async void  runQuery(String queryString)
            {
                var results = await DataService.GetDataFromService(queryString).ConfigureAwait(false);
                Console.WriteLine(results);
            }
            
        }
    }
}




