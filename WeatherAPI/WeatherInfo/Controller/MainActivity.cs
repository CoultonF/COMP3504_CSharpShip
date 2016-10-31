using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Newtonsoft.Json; 
using System.Threading.Tasks;
using System.Net;
using Android.Views.InputMethods;
using Android.Graphics;
using System.Collections.Generic;
using Android.Net;
using Android.Locations;
using Android.Provider;
using Geolocator.Plugin;
using System.Text;
using System.Linq;

namespace WeatherInfo
{
	[Activity ( Label = "WeatherCondition" , MainLauncher = true , Icon = "@drawable/icon" )]
	public class MainActivity : Activity
	{ 
		ImageView imgCondition;
		TextView lblTemp;
		TextView lblHumidity;
		TextView lblWindSpeed;
		TextView lblConditionText;
		TextView lblSunrise;
		TextView lblSunSet;
		TextView lblTitleCondition;
		TextView lblVisibility;
		TextView lblPoweredBy;
		ListView ListViewForeCast; 
		TextView lblForeCastHeadingTxt;

		AutoCompleteTextView txtLocation;
		ArrayAdapter adapter=null; 
		GoogleMapPlaceClass objMapClass;
		ForecastAdapterClass objForecastAdapterClass;
		string autoCompleteOptions;
		string[] strPredictiveText;
		int index = 0;
		ProgressDialog progress;
		string strLocation;
		List<Forecast> lstForeCast;
		WeatherClass objYahooWeatherClass;
		const string strGoogleApiKey="AIzaSyCaRfEwrLeUvdADPn_R9WQ_WrP3jBuFDfA";
		const string  strAutoCompleteGoogleApi="https://maps.googleapis.com/maps/api/place/autocomplete/json?input=";
		RelativeLayout rlTopMainContainer;
		const string strYahooApi=  "https://query.yahooapis.com/v1/public/yql?q=select * from weather.forecast where woeid in (select woeid from geo.places(1) where text='{0}') and u='c'&format=json";
		protected override void OnCreate ( Bundle bundle )
		{
			base.OnCreate ( bundle ); 
			// Set our view from the "main" layout resource
			SetContentView ( Resource.Layout.Main ); 
			InitializeControl ();
	         txtLocation.TextChanged += async delegate(object sender , Android.Text.TextChangedEventArgs e )
			 { 
				if(!string.IsNullOrEmpty(txtLocation.Text))
				await AutoCompleteLocation(); 
			 };
			txtLocation.ItemClick += AutoCompleteOption_Click;

			txtLocation.KeyPress += async delegate(object sender , View.KeyEventArgs e )
			{
				InputMethodManager inputManager = ( InputMethodManager ) this.GetSystemService ( Context.InputMethodService ); 
				inputManager.HideSoftInputFromWindow ( this.CurrentFocus.WindowToken , HideSoftInputFlags.NotAlways ); 
				if ( e.KeyCode == Keycode.Enter && e.Event.Action == KeyEventActions.Down )
				{  
					if ( !string.IsNullOrEmpty ( txtLocation.Text ) )
					{
						strLocation = txtLocation.Text;  
						txtLocation.Text = string.Empty;
						txtLocation.ClearFocus ();  

						bool isValid =	await LocationSelected ();
						if ( isValid )
						{
							BindData ();
						}
					}
				}
			}; 
			lblTitleCondition.Click += async delegate(object sender, EventArgs e) {

				bool isEnabled = await LocationSetting ();
				if ((isEnabled)) {
					RunOnUiThread (() => {
						progress = ProgressDialog.Show (this, "", "Please wait...");
					});
					//get the current location
					bool gotLocaion =	await GetCurrentPosition ();

					DismissActivityIndicator ();
					//download and process yahoo weather data
					bool isValid=false;
					if (gotLocaion) {
						isValid =	await LocationSelected ();
					} 
					//bind the data back to ui
					if (isValid) { 
						BindData ();
					} 
				}
			
			};
			lblPoweredBy.Click+= delegate(object sender , EventArgs e )
			{
				StartActivity(typeof(MyWebViewActivity));
			};
		}
 
		#region "Defined Function"
		async void AutoCompleteOption_Click(object sender,AdapterView.ItemClickEventArgs e)
		{
			if ( !string.IsNullOrEmpty ( txtLocation.Text ) )
			{
				strLocation = txtLocation.Text;  
				txtLocation.Text = string.Empty;
				txtLocation.ClearFocus ();
				bool isValid =	await LocationSelected ();
				if ( isValid )
				{
					BindData ();
				}
				//hide soft keyboard 
				InputMethodManager inputManager = ( InputMethodManager ) this.GetSystemService ( Context.InputMethodService );
				inputManager.HideSoftInputFromWindow ( txtLocation.WindowToken , HideSoftInputFlags.NotAlways );
			}
		}
		async Task<bool> LocationSelected()
		{
			string Location = strLocation.Replace ( "," , "" );
			if ( !isConnected () )
			{
				RunOnUiThread ( () =>
				{
					Toast.MakeText ( this , "Sorry you have no internet connection!!!" , ToastLength.Short ).Show ();
				} );
				return false;
			}

			try
			{
				RunOnUiThread ( () =>
				{
					progress=ProgressDialog.Show(this,"","Please wait...");
				});
				string strWeatherJson = await fnDownloadString ( string.Format ( strYahooApi , Location ) );

				DismissActivityIndicator();
				if ( strWeatherJson != "Exception" )
				{ 
					objYahooWeatherClass = JsonConvert.DeserializeObject<WeatherClass> ( strWeatherJson );
					if ( objYahooWeatherClass != null )
					{ 
						return true;
					} 
				}
				else
				{
					RunOnUiThread ( () =>
					{
						Toast.MakeText ( this , "Unable to touch server at this moment!!!" , ToastLength.Short ).Show ();
					} );
					return false;
				}
			}
			catch
			{
				RunOnUiThread ( () =>
				{
					Toast.MakeText ( this , "Sorry! Unable to process at this moment!!!" , ToastLength.Short ).Show ();
				} );
				return false;
			}
			return true;
		}
		void BindData()
		{
			imgCondition.Visibility = ViewStates.Visible;
			lblForeCastHeadingTxt.Visibility = ViewStates.Visible;
			ListViewForeCast.Visibility = ViewStates.Visible;
			lblSunrise.Visibility = ViewStates.Visible;
			lblSunSet.Visibility = ViewStates.Visible;
			rlTopMainContainer.Visibility = ViewStates.Visible;
			lblTemp.Text = string.Format ( "Temperature :{0}{1}" , objYahooWeatherClass.query.results.channel.item.condition.temp , objYahooWeatherClass.query.results.channel.units.temperature );
			lblHumidity.Text = string.Format ( "Humidity :{0}{1}" , objYahooWeatherClass.query.results.channel.atmosphere.humidity,"%" );
			lblWindSpeed.Text=string.Format ( "Wind Speed :{0}{1}" , objYahooWeatherClass.query.results.channel.wind.speed,objYahooWeatherClass.query.results.channel.units.speed );
			lblVisibility.Text=string.Format ( "Visibility :{0}{1}" , objYahooWeatherClass.query.results.channel.atmosphere.visibility,objYahooWeatherClass.query.results.channel.units.distance );
			lblConditionText.Text=string.Format ( " {0}" , objYahooWeatherClass.query.results.channel.item.condition.text );

			lblSunrise.Text =string.Format("Sunrise :{0}",objYahooWeatherClass.query.results.channel.astronomy.sunrise);
			lblSunSet.Text =string.Format("Sunset :{0}",objYahooWeatherClass.query.results.channel.astronomy.sunset);
			lblTitleCondition.Text = objYahooWeatherClass.query.results.channel.item.title; 
			//take weather icon url from parsing  	objYahooWeatherClass.query.results.channel.item.description
			string imageUrl = string.Format ( "http://l.yimg.com/a/i/us/we/52/{0}.gif" , objYahooWeatherClass.query.results.channel.item.condition.code );
			//Koush.UrlImageViewHelper.SetUrlDrawable ( imgCondition , imageUrl , Resource.Drawable.Icon );

			lstForeCast=objYahooWeatherClass.query.results.channel.item.forecast;
			objForecastAdapterClass = new ForecastAdapterClass ( this , lstForeCast );
			ListViewForeCast.Adapter = objForecastAdapterClass;
		}

		async Task<bool> AutoCompleteLocation()
		{
			try
			{
				if ( !isConnected () )
				{  
					Toast.MakeText ( this , "Sorry you have no internet connection!!!" , ToastLength.Short ).Show ();  
					return false;
				} 
				autoCompleteOptions = await fnDownloadString ( string.Format ( "{0}{1}&key={2}" , strAutoCompleteGoogleApi , txtLocation.Text , strGoogleApiKey ) );
				Console.WriteLine(autoCompleteOptions);
				if ( autoCompleteOptions == "Exception" )
				{ 
				Toast.MakeText ( this , "Unable to touch server at this moment!!!" , ToastLength.Short ).Show ();
			    return false;
				} 
				objMapClass = JsonConvert.DeserializeObject<GoogleMapPlaceClass> ( autoCompleteOptions ); 
				strPredictiveText = new string[objMapClass.predictions.Count];
				index = 0;
				foreach ( Prediction objPred  in objMapClass.predictions )
				{
					strPredictiveText [index] = objPred.description;
					index++; 
				} 
				adapter = new ArrayAdapter<string> ( this , Android.Resource.Layout.SimpleDropDownItem1Line , strPredictiveText );
				txtLocation.Adapter = adapter;  
			}
			catch
			{   
				DismissActivityIndicator();
				Toast.MakeText ( this , "Unable to touch server at this moment!!!" , ToastLength.Short ).Show (); 
			}
			return true;
		}

		void InitializeControl()
		{
			if ( !isConnected () )
			{
				Toast.MakeText ( this , "Please open internet connection!!!" , ToastLength.Short ).Show ();
			} 
			imgCondition = FindViewById<ImageView> ( Resource.Id.imgWeatherIcon ); 
			txtLocation = FindViewById<AutoCompleteTextView> ( Resource.Id.txtSearch );
			lblTemp = FindViewById<TextView> ( Resource.Id.lblTempWC );
			lblHumidity=FindViewById<TextView> ( Resource.Id.lblHumidity );
			lblWindSpeed=FindViewById<TextView> ( Resource.Id.lblWindSpeed );
			lblConditionText=FindViewById<TextView> ( Resource.Id.lblConditionText );
			lblSunrise=FindViewById<TextView> ( Resource.Id.lblSunrise );
			lblSunSet=FindViewById<TextView> ( Resource.Id.lblSunset ); 
			lblVisibility= FindViewById<TextView> ( Resource.Id.lblVisibility );
			lblTitleCondition = FindViewById<TextView> ( Resource.Id.lblCurrentLocation );
			lblPoweredBy = FindViewById<TextView> ( Resource.Id.lblPoweredBy );
			lstForeCast = new List<Forecast> ();
			ListViewForeCast = FindViewById<ListView> ( Resource.Id.ListViewForeCast );
			lblTitleCondition.Text="Tap here for your current Location"; 
			lblForeCastHeadingTxt = FindViewById<TextView> ( Resource.Id.lblForeCastHeadingTxt ); 
			rlTopMainContainer = FindViewById<RelativeLayout> ( Resource.Id.rlTopMainContainer );
			imgCondition.Visibility = ViewStates.Invisible; 
			lblForeCastHeadingTxt.Visibility = ViewStates.Invisible;
			ListViewForeCast.Visibility = ViewStates.Invisible;
			lblSunrise.Visibility = ViewStates.Invisible;
			lblSunSet.Visibility = ViewStates.Invisible;
			rlTopMainContainer.Visibility = ViewStates.Invisible;
			//rlTopMainContainer
		}

		async Task<string> fnDownloadString(string strUri)
		{ 
			WebClient webclient = new WebClient ();
			string strResultData;
			try
			{
				strResultData= await webclient.DownloadStringTaskAsync (new System.Uri(strUri)); 
			}
			catch
			{
				strResultData = "Exception"; 
				DismissActivityIndicator();
				RunOnUiThread ( () =>
				{ 
					Toast.MakeText ( this , "Unable to connect to server!!!" , ToastLength.Short ).Show ();
				} );
			}
			finally
			{
				webclient.Dispose ();
				webclient = null; 
			}

			return strResultData;
		}
		Boolean isConnected()
		{
			try
			{
				var connectionManager = (ConnectivityManager)this.GetSystemService (Context.ConnectivityService); 
				NetworkInfo networkInfo = connectionManager.ActiveNetworkInfo; 
				if (networkInfo != null && networkInfo.IsConnected) 
				{
					return true;
				}
			}
			catch 
			{ 
				//ensure access network state is enbled
				return false;
			}
			return false;
		}
		void DismissActivityIndicator()
		{
			RunOnUiThread ( () =>
			{
				if ( progress != null )
				{
					progress.Dismiss ();
					progress = null;
				}
			} );
		}
		#endregion

		#region " Current Location"
		async Task<bool> LocationSetting()
		{
			// Get Location Manager and check for GPS & Network location services
			LocationManager lm = (LocationManager) GetSystemService(LocationService);
			if(!lm.IsProviderEnabled(LocationManager.GpsProvider) || !lm.IsProviderEnabled(LocationManager.NetworkProvider)) 
			{
				RunOnUiThread ( () =>
				{
					fnAlertMsg ( "Location OFF" , "Please enable Location setting " , "Ok" , "Cancel" , this );
				} );
				await Task.Delay(1000);
				return false;
			} 
			return true;
		}
		void fnAlertMsg (string strTitle,string strMsg,string strOk,string strCancel,Context context)
		{
			AlertDialog alertMsg = new AlertDialog.Builder (context).Create ();
			alertMsg.SetCancelable (false);
			alertMsg.SetTitle (strTitle);
			alertMsg.SetMessage (strMsg);
			alertMsg.SetButton2(strOk, delegate (object sender, DialogClickEventArgs e) 
			{
				if (e.Which  == -2) 
				{
					Intent intent =new Intent(Settings.ActionLocationSourceSettings);
					StartActivity(intent);
					fnAlertMsgOne(GetString(Resource.String.app_name),"Try again now","OK",this);
				}
			});
			alertMsg.SetButton(strCancel,delegate (object sender, DialogClickEventArgs e) 
			{
				if (e.Which  == -1) 
				{
					alertMsg.Dismiss ();
					alertMsg=null;
				}
			});
			alertMsg.Show ();
		} 
		void fnAlertMsgOne(string strTitle,string strMsg,string strOk,Context context)
		{
			AlertDialog alertMsg = new AlertDialog.Builder (context).Create ();
			alertMsg.SetCancelable (false);
			alertMsg.SetTitle (strTitle);
			alertMsg.SetMessage (strMsg);
			alertMsg.SetButton (strOk, delegate (object sender, DialogClickEventArgs e) 
			{
				if (e.Which  == -1) 
				{
					alertMsg.Dismiss ();
					alertMsg=null;
					//GetCurrentPosition();
//					updateCameraPosition(CurrentPosition);
				}
			});
			alertMsg.Show ();
		} 
		async Task<bool>GetCurrentPosition()
		{
			bool done = false;
			try {
				var Locator = CrossGeolocator.Current;
				Locator.DesiredAccuracy = 50;
				var position = await Locator.GetPositionAsync (timeout: 10000); 
			
				Console.WriteLine (position.Heading);
				if (position != null) {
					Geocoder geocoder = new Geocoder (this);
					IList<Address> addressList = await geocoder.GetFromLocationAsync (position.Latitude, position.Longitude, 10);

					Address address = addressList.FirstOrDefault ();
					if (address != null) {
						done = true;
						if (address.MaxAddressLineIndex > 1) {
							RunOnUiThread (() => {
								txtLocation.Text = address.GetAddressLine (1);
							});
						} else {
							RunOnUiThread (() => {
								txtLocation.Text = address.GetAddressLine (0);
							});
						}
						RunOnUiThread (() => {
							strLocation = txtLocation.Text;   
							txtLocation.ClearFocus ();
						});
						
					} else {
						done = false;
						DismissActivityIndicator();
						RunOnUiThread (() => {
							Toast.MakeText (this, "Unable to process at this moment!!!", ToastLength.Short).Show (); 
						});
					}
				} else {
					done = false;
				}
			} catch {

				DismissActivityIndicator();
				RunOnUiThread (() => {
					Toast.MakeText (this, "Unable to process at this moment!!!", ToastLength.Short).Show (); 
				});
				done = false;
			}
			return done;
		}
		#endregion

	}
}


