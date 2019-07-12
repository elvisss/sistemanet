using Microsoft.EntityFrameworkCore;
using Sistema.Datos.Mapping.Almacen;
using Sistema.Datos.Mapping.Usuarios;
using Sistema.Datos.Mapping.Ventas;
using Sistema.Entidades.Almacen;
using Sistema.Entidades.Usuarios;
using Sistema.Entidades.Ventas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos
{
    public class DbContextSistema : DbContext
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<DetalleIngreso> DetallesIngresos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVentas { get; set; }
        public DbContextSistema(DbContextOptions<DbContextSistema> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modeBuilder)
        {
            base.OnModelCreating(modeBuilder);
            modeBuilder.ApplyConfiguration(new CategoriaMap());
            modeBuilder.ApplyConfiguration(new ArticuloMap());
            modeBuilder.ApplyConfiguration(new RolMap());
            modeBuilder.ApplyConfiguration(new UsuarioMap());
            modeBuilder.ApplyConfiguration(new PersonaMap());
            modeBuilder.ApplyConfiguration(new IngresoMap());
            modeBuilder.ApplyConfiguration(new DetalleIngresoMap());
            modeBuilder.ApplyConfiguration(new VentaMap());
            modeBuilder.ApplyConfiguration(new DetalleVentaMap());
        }
    }
}
