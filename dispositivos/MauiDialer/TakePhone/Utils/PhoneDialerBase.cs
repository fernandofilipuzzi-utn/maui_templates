using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokePhone.Utils
{
    public class PhoneDialerBase
    {
        virtual public bool CallPhone(string number)
        {
            return false;
        }
    }
}
