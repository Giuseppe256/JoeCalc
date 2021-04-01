using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using JoeCalc.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// https://stackoverflow.com/questions/35279403/toast-equivalent-for-xamarin-forms

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace JoeCalc.Droid
{
    public class MessageAndroid : IMessage
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}