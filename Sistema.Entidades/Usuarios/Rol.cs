using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sistema.Entidades.Usuarios
{
    public class Rol
    {
        [Required]
        public int idrol { get; set; }
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre no debe tener más de 50 carácteres ni menos de 3")]
        public string nombre { get; set; }
        [StringLength(256)]
        public string descripcion { get; set; }
        public bool condicion { get; set; }
    }
}
