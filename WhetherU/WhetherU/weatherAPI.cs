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
using DataBase;
using System.Text.RegularExpressions;

namespace WhetherU
{

    [Activity(Label = "weatherAPI", MainLauncher = false, Icon = "@drawable/icon")]
    public class WeatherScreen : Activity
    {
        string token;
        string tagName;
        public static string getGreeting()
        {
            String name = "User";
            LocalDataAccessLayer dataAc = LocalDataAccessLayer.getInstance();
            DataBase.User user;
                
                user = dataAc.getUser();
                name = user.name;
                DateTime currTime = DateTime.Now;
            string greeting = "";
            int hour = currTime.Hour;
            if (hour > 0 && hour < 12)
            {
                greeting = "Good Morning, " + name + ".";
            }
            else if (hour >= 12 && hour < 17)
            {
                greeting = "Good Afternoon, " + name + ".";
            }
            else
            {
                greeting = "Good Evening, " + name + ".";
            }
            return greeting;
        }
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            Weather weather = await Core.GetWeather("51.0144", "114.1288");
            tagName = weather.Visibility.ToLower();
            token = Intent.GetStringExtra("UserToken") ?? "";
            if (token != "")
            {
                InstagramClient client = new InstagramClient(token);
                MediasResponse media = await client.GetRecentMediaByTagName(tagName);
                var datas = media.Data;
                if (datas.Count > 0)
                {
                    ImageButton demo = FindViewById<ImageButton>(Resource.Id.demoMenu);
                    demo.Click += delegate
                    {
                        Intent intent = new Intent(this.ApplicationContext, typeof(DemoMenuActivity));
                        intent.SetFlags(ActivityFlags.NewTask);
                        StartActivity(intent);
                    };
                    Random rand = new Random();
                    int index = rand.Next(0, media.Data.Count);
                    Bitmap imageBitmap = null;
                    String replacement = "p1080x1080";
                    Regex rgx = new Regex(@"(s|p)\d\d\dx\d\d\d");
                    String url = media.Data.ElementAt<Media>(index).Images.StandardResolution.Url.ToString();
                    url = rgx.Replace(url, replacement);
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
                    runWeather("51.0114", "114.1288");
                    getGreeting();

                }
            }
            else
            {
                //TODO:pull image from database for criteria

                SetContentView(Resource.Layout.weatherAPI);
                ImageView image = FindViewById<ImageView>(Resource.Id.imgAbsolute);
                image.SetImageResource(Resource.Drawable.main);
                image.SetScaleType(ScaleType.CenterCrop);
                runWeather("51.0114", "114.1288");


            }
            
            

        // Set our view from the "main" layout resource
        //SetContentView(Resource.Layout.weatherAPI);


        // Get our button from the layout resource,
        // and attach an event to it
        //Button button = FindViewById<Button>(Resource.Id.weatherBtn);

            //button.Click += Button_Click;

            
        }

        private void setInstagramImages()
        {

        }


        private async void runWeather(String lat, String lon)
        {
            //EditText zipCodeEntry = FindViewById<EditText>(Resource.Id.zipCodeEntry);
            Weather weather = await Core.GetWeather("51.0144", "114.1288");

            Typeface tf = Typeface.CreateFromAsset(Assets, "CaviarDreams.ttf");


            var locationText = FindViewById<TextView>(Resource.Id.locationText);
            locationText.Text = weather.Title;
            var tempText = FindViewById<TextView>(Resource.Id.tempText);
            tempText.Text = weather.Temperature;
            var windText = FindViewById<TextView>(Resource.Id.windText);
            windText.Text = weather.Wind;
            var visibilityText = FindViewById<TextView>(Resource.Id.visibilityText);
            visibilityText.Text = weather.Visibility;
            var sunriseText = FindViewById<TextView>(Resource.Id.sunriseText);
            sunriseText.Text = weather.Sunrise;
            var sunsetText = FindViewById<TextView>(Resource.Id.sunsetText);
            sunsetText.Text = weather.Sunset;
            var greeting = FindViewById<TextView>(Resource.Id.greeting);
            greeting.Text = weather.Greeting;

            locationText.SetTypeface(tf, TypefaceStyle.Bold);
            tempText.SetTypeface(tf, TypefaceStyle.Bold);
            windText.SetTypeface(tf, TypefaceStyle.Normal);
            visibilityText.SetTypeface(tf, TypefaceStyle.Normal);
            sunriseText.SetTypeface(tf, TypefaceStyle.Bold);
            sunsetText.SetTypeface(tf, TypefaceStyle.Bold);
            greeting.SetTypeface(tf, TypefaceStyle.Bold);

            //if (!String.IsNullOrEmpty(zipCodeEntry.Text))
            //{

            //}
        }

        public class Weather
        {
            public string Title { get; set; }
            public string Temperature { get; set; }
            public string Wind { get; set; }
            public string Visibility { get; set; }
            public string Sunrise { get; set; }
            public string Sunset { get; set; }
            public string Description { get; set; }
            public string Greeting { get; set; }


            public Weather()
            {
                //Because labels bind to these values, set them to an empty string to
                //ensure that the label appears on all platforms by default.
                this.Title = " ";
                this.Temperature = " ";
                this.Wind = " ";
                this.Visibility = " ";
                this.Sunrise = " ";
                this.Sunset = " ";
                this.Greeting = " ";
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
                    weather.Temperature = (string)results["main"]["temp"] + "°C";
                    weather.Wind = "Wind speeds of " + (string)results["wind"]["speed"] + " kph";
                    weather.Visibility = (string)results["weather"][0]["main"];
                    weather.Description = (string)results["weather"][0]["description"];

                    DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                    DateTime sunrise = time.AddSeconds((double)results["sys"]["sunrise"]);
                    sunrise.ToLocalTime();
                    DateTime sunset = time.AddSeconds((double)results["sys"]["sunset"]);
                    sunset.ToLocalTime();
                    String sunriseText = sunrise.ToString();
                    weather.Sunrise = "8:27";
                    String sunsetText = sunset.ToString();
                    weather.Sunset = "16:30";

                    weather.Greeting = getGreeting();
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

            public async void runQuery(String queryString)
            {
                var results = await DataService.GetDataFromService(queryString).ConfigureAwait(false);
                Console.WriteLine(results);
            }

        }
    }
}