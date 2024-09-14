using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokePhone.Utils;
//
using AndroidApp = Android.App.Application;
using AndroidNet = Android.Net;

namespace TokePhone.Platforms.Android.utils
{
    public partial class PhoneDialer : PhoneDialerBase
    {
        override public bool CallPhone(string number)
        {
            Intent callIntent = new Intent(Intent.ActionCall);
            callIntent.SetFlags(ActivityFlags.NewTask);

            var url = AndroidNet.Uri.Parse(string.Format("tel:{0}", number));
            callIntent.SetData(url);
            AndroidApp.Context.StartActivity(callIntent);

            return true;
        }
    }
}
