using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejemplo3SQLite.DTOs
{
    public partial class EmpleadoDTO : ObservableObject
    {
        [ObservableProperty]
        public int idEmpleado;
        [ObservableProperty]
        public string nombreCompleto;
        [ObservableProperty]
        public string correo;
        [ObservableProperty]
        public decimal sueldo;
        [ObservableProperty]
        public DateTime fechaContrato;
    }
}
