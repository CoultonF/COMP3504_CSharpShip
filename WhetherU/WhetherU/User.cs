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
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; } // auto set when isnerted to the db
        public string name { get; set; }
        public string login { get; set; }
        public int cldRngStart { get; set; }
        public int cldRngEnd { get; set; }
        public int hotRngStart { get; set; }
        public int hotRngEnd { get; set; }

        public User() { } // must have a default constructor to use SQLite methods 

        public User(string name, string login)
        {
            this.name = name;
            this.login = login;
            //this.cldRngStart = cldRngStart;
            //this.cldRngEnd = cldRngEnd;
            //this.hotRngStart = hotRngStart;
            //this.hotRngEnd = hotRngEnd;
            cldRngStart = createCStart();
            cldRngEnd = createCEnd();
            hotRngStart = createHStart();
            hotRngEnd = createHEnd();
        }

        public override string ToString() // called when object given to list for default list display
        {
            return name;
        }
        public void updateRng()
        {
            cldRngStart = createCStart();
            cldRngEnd = createCEnd();
            hotRngStart = createHStart();
            hotRngEnd = createHEnd();
        }
        private int createCStart()
        {
            LocalDataAccessLayer data = null;
            data = LocalDataAccessLayer.getInstance();
            List<UILog> logEntries = new List<UILog>();
            logEntries = data.getAllLogEntries();
            int num = 0;
            if (logEntries.Count != 0)
            {
                num = logEntries.Where<UILog>(p => p.type == false).Min<UILog>(p => p.totalState);
            }
            return num;
        }
        private int createHStart()
        {
            LocalDataAccessLayer data = null;
            data = LocalDataAccessLayer.getInstance();
            List<UILog> logEntries = new List<UILog>();
            logEntries = data.getAllLogEntries();
            int num = 0;
            if (logEntries.Count != 0)
            {
                num = logEntries.Where<UILog>(p => p.type == true).Min<UILog>(p => p.totalState);
            }
            return num;
        }
        private int createCEnd()
        {
            LocalDataAccessLayer data = null;
            data = LocalDataAccessLayer.getInstance();
            List<UILog> logEntries = new List<UILog>();
            logEntries = data.getAllLogEntries();
            int num = 0;
            if (logEntries.Count != 0)
            {
                num = logEntries.Where<UILog>(p => p.type == false).Max<UILog>(p => p.totalState);
            }
            return num;
        }
        private int createHEnd()
        {
            LocalDataAccessLayer data = null;
            data = LocalDataAccessLayer.getInstance();
            List<UILog> logEntries = new List<UILog>();
            logEntries = data.getAllLogEntries();
            int num = 0;
            if (logEntries.Count != 0)
            {
                num = logEntries.Where<UILog>(p => p.type == true).Max<UILog>(p => p.totalState);
            }
            return num;
        }
    }
}