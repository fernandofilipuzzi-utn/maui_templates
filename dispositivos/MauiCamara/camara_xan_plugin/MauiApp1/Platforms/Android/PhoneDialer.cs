using Android.Content;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using Android.App;
using AndroidApp = Android.App.Application;
using AndroidNet = Android.Net;
using MauiApp1.Utils;

namespace MauiApp1
{
    public partial class PhoneDialer : PhoneDialerUtils
    {
        override public void CallPhone(string number)
        {

            Intent callIntent = new Intent(Intent.ActionCall);
            callIntent.SetFlags(ActivityFlags.NewTask);

            var url = AndroidNet.Uri.Parse(string.Format("tel:{0}", number));
            callIntent.SetData(url);
            AndroidApp.Context.StartActivity(callIntent);

        }
    }
}
