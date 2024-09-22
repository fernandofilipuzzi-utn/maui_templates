using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlueTooth.Helpers
{
    public interface IPermissionHelper
    {
        Task<PermissionStatus> RequestAllPermissionsAsync();
    }
}
