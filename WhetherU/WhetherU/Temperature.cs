using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;


namespace DataBase
{
    public class Temperature
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; } // auto set when isnerted to the db
        public int temperature { get; set; } // auto set when isnerted to the db
        public int tempCode { get; set; }


        public Temperature() { } // must have a default constructor to use SQLite methods 

        public Temperature(int temperature, int tempCode)
        {
            this.temperature = temperature;
            this.tempCode = tempCode;
        }




        public override string ToString() // called when object given to list for default list display
        {
            return temperature + " " + tempCode;
        }

    }
}