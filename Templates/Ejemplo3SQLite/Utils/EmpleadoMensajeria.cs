using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejemplo3SQLite.Utils
{
    public class EmpleadoMensajeria : ValueChangedMessage<EmpleadoMensaje>
    {
        public EmpleadoMensajeria(EmpleadoMensaje value) : base(value)
        {

        }
    }
}
