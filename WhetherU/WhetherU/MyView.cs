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

namespace WhetherU
{
    public class MyView : View
    {
        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            Paint green = new Paint
            {
                AntiAlias = true,
                Color = Color.Rgb(0x99, 0xcc, 0),
            };
            green.SetStyle(Paint.Style.FillAndStroke);

            Paint red = new Paint
            {
                AntiAlias = true,
                Color = Color.Rgb(0xff, 0x44, 0x44)
            };
            red.SetStyle(Paint.Style.FillAndStroke);

            float middle = canvas.Width * 0.25f;
            canvas.DrawPaint(red);
            canvas.DrawRect(0, 0, middle, canvas.Height, green);
        }
    }
}