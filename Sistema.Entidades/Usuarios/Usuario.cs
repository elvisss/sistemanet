using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Sistema.Entidades.Almacen;
using Sistema.Entidades.Ventas;

namespace Sistema.Entidades.Usuarios
{
    public class Usuario
    {
        public int idusuario { get; set; }
        [Required]
        public int idrol { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre no debe tener más de 50 carácteres ni menos de 3")]
        public string nombre { get; set; }
        public string tipo_documento { get; set; }
        public string num_documento { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public byte[] password_hash { get; set; }
        [Required]
        public byte[] password_salt { get; set; }
        public bool condicion { get; set; }
        public Rol rol { get; set; }
        public ICollection<Ingreso> ingresos { get; set; }
        public ICollection<Venta> ventas { get; set; }
    }
}
