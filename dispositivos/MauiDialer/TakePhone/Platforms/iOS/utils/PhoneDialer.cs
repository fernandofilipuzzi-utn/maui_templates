using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokePhone.Utils;
using UIKit;

namespace TokePhone.Platforms.iOS.utils
{
    public partial class PhoneDialer : PhoneDialerBase
    {
        override public bool CallPhone(string number)
        {
            var phoneNumber = number.Replace(" ", "").Replace("-", "");
            var url = new NSUrl($"tel:{number}");

            if (UIApplication.SharedApplication.CanOpenUrl(url) == true)
            {
                UIApplication.SharedApplication.OpenUrl(url, new UIApplicationOpenUrlOptions(), null);
                return true;
                /*
                if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                {
                    UIApplication.SharedApplication.OpenUrl(url, new UIApplicationOpenUrlOptions(), null);
                    return true;
                }
                else
                {
                    return UIApplication.SharedApplication.OpenUrl(url);
                }
                */
            }
            else
            {
                return false;
            }
        }
    }
}
