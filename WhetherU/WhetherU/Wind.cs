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
    public class Wind
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; } // auto set when isnerted to the db
        public int windSp { get; set; } // auto set when isnerted to the db
        public int windCode { get; set; }


        public Wind() { } // must have a default constructor to use SQLite methods 

        public Wind(int windSp, int windCode)
        {
            this.windSp = windSp;
            this.windCode = windCode;
        }




        public override string ToString() // called when object given to list for default list display
        {
            return windSp + " " + windCode;
        }

    }
}