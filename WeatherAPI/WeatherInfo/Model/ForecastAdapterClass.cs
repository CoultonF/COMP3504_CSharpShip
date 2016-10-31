using System;
using Android.Widget;
using Android.App;
using System.Collections.Generic;
using Android.Views;

namespace WeatherInfo
{
	public class ForecastAdapterClass : BaseAdapter
	{
		Activity context;
		List<Forecast> lstForecast; 
		string imageUrl =  "http://l.yimg.com/a/i/us/we/52/{0}.gif" ;
		public ForecastAdapterClass (Activity c,List<Forecast> _lstForecast )
		{
			context = c;
			lstForecast = _lstForecast;
		}
		public override int Count {
			get { return lstForecast.Count; }
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return null;
		}

		public override long GetItemId (int position)
		{
			return 0;
		}
		// create a new ImageView for each item referenced by the Adapter
		public override View GetView (int position, View convertView, ViewGroup parent)
		{ 
			View view = convertView;
			ViewHolderClass objViewHolderClass;
			if ( view == null )
			{
				view = context.LayoutInflater.Inflate ( Resource.Layout.ForecastCustomLayout , parent , false );  
				objViewHolderClass = new ViewHolderClass ();
				objViewHolderClass.imgWeather=view.FindViewById<ImageView> ( Resource.Id.imgWeatherIconFCL );
				objViewHolderClass.txtTextCondition=view.FindViewById<TextView> ( Resource.Id.lblTextFCL );
				objViewHolderClass.txtHigh=view.FindViewById<TextView> ( Resource.Id.lblHighFCL );
				objViewHolderClass.txtLow=view.FindViewById<TextView> ( Resource.Id.lblLowFCL );
				objViewHolderClass.txtDay=view.FindViewById<TextView> ( Resource.Id.lbldateDayFCL );
				view.Tag = objViewHolderClass;
			}
			else
			{
				objViewHolderClass =(ViewHolderClass) view.Tag;
			}
			if ( position == 0 )
			{
				view.SetBackgroundColor ( Android.Graphics.Color.ParseColor ( "#D6D6D6" ) );
			}
			else if ( position % 2 == 0 )
			{
				view.SetBackgroundColor ( Android.Graphics.Color.ParseColor ( "#D6D6D6" ) );
			}
			objViewHolderClass.txtDay.Text = string.Format ( "{0},{1}" , lstForecast [position].day, lstForecast [position].date  );
			objViewHolderClass.txtTextCondition.Text = lstForecast [position].text;
			objViewHolderClass.txtHigh.Text =string.Format ( "High : {0}C" ,  lstForecast [position].high);
			objViewHolderClass.txtLow.Text = string.Format ( "Low : {0}C" ,  lstForecast [position].low);
			//Koush.UrlImageViewHelper.SetUrlDrawable ( objViewHolderClass.imgWeather , string.Format( imageUrl,lstForecast[position].code), Resource.Drawable.Icon );
			return view;
		} 
		public  class ViewHolderClass :Java.Lang.Object
		{
			public  ImageView imgWeather;
			public  TextView txtDay;
			public  TextView txtTextCondition;
			public  TextView txtHigh;
			public  TextView txtLow;
		}
	}


}

