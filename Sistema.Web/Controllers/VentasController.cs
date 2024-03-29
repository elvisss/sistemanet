﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Datos;
using Sistema.Entidades.Ventas;
using Sistema.Web.Models.Ventas.Venta;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public VentasController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Ventas/Listar
        [Authorize(Roles = "Vendedor, Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<VentaViewModel>> Listar()
        {
            var venta = await _context.Ventas
                .Include(i => i.usuario)
                .Include(i => i.persona)
                .OrderByDescending(i => i.idventa)
                .Take(100)
                .ToListAsync();

            return venta.Select(i => new VentaViewModel
            {
                idventa = i.idventa,
                idcliente = i.idcliente,
                cliente = i.persona.nombre,
                num_documento = i.persona.num_documento,
                direccion = i.persona.direccion,
                telefono = i.persona.telefono,
                email = i.persona.email,
                idusuario = i.idusuario,
                usuario = i.usuario.nombre,
                tipo_comprobante = i.tipo_comprobante,
                serie_comprobante = i.serie_comprobante,
                num_comprobante = i.num_comprobante,
                fecha_hora = i.fecha_hora,
                impuesto = i.impuesto,
                total = i.total,
                estado = i.estado
            });
        }

        // GET: api/Ventas/VentasMes12
        [Authorize(Roles = "Almacenero, Vendedor, Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<ConsultaViewModel>> VentasMes12()
        {
            var consulta = await _context.Ventas
                .GroupBy(v => v.fecha_hora.Month)
                .Select(x => new { etiqueta = x.Key, valor = x.Sum(v => v.total) })
                .OrderByDescending(x => x.etiqueta)
                .Take(12)
                .ToListAsync();

            return consulta.Select(i => new ConsultaViewModel
            {
                etiqueta = i.etiqueta.ToString(),
                valor = i.valor
            });
        }

        // GET: api/Ventas/ListarFiltro/texto
        [Authorize(Roles = "Vendedor, Administrador")]
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<VentaViewModel>> ListarFiltro([FromRoute] string texto)
        {
            var venta = await _context.Ventas
                .Include(i => i.usuario)
                .Include(i => i.persona)
                .Where(i => i.num_comprobante.Contains(texto))
                .OrderByDescending(i => i.idventa)
                .ToListAsync();

            return venta.Select(i => new VentaViewModel
            {
                idventa = i.idventa,
                idcliente = i.idcliente,
                cliente = i.persona.nombre,
                num_documento = i.persona.num_documento,
                direccion = i.persona.direccion,
                telefono = i.persona.telefono,
                email = i.persona.email,
                idusuario = i.idusuario,
                usuario = i.usuario.nombre,
                tipo_comprobante = i.tipo_comprobante,
                serie_comprobante = i.serie_comprobante,
                num_comprobante = i.num_comprobante,
                fecha_hora = i.fecha_hora,
                impuesto = i.impuesto,
                total = i.total,
                estado = i.estado
            });
        }

        // GET: api/Ventas/ConsultarFechas/FechaInicio/FechaFin
        [Authorize(Roles = "Vendedor, Administrador")]
        [HttpGet("[action]/{FechaInicio}/{FechaFin}")]
        public async Task<IEnumerable<VentaViewModel>> ConsultarFechas([FromRoute] DateTime FechaInicio, [FromRoute] DateTime FechaFin)
        {
            var venta = await _context.Ventas
                .Include(i => i.usuario)
                .Include(i => i.persona)
                .Where(i => i.fecha_hora >= FechaInicio)
                .Where(i => i.fecha_hora <= FechaFin)
                .OrderByDescending(i => i.idventa)
                .Take(100)
                .ToListAsync();

            return venta.Select(i => new VentaViewModel
            {
                idventa = i.idventa,
                idcliente = i.idcliente,
                cliente = i.persona.nombre,
                num_documento = i.persona.num_documento,
                direccion = i.persona.direccion,
                telefono = i.persona.telefono,
                email = i.persona.email,
                idusuario = i.idusuario,
                usuario = i.usuario.nombre,
                tipo_comprobante = i.tipo_comprobante,
                serie_comprobante = i.serie_comprobante,
                num_comprobante = i.num_comprobante,
                fecha_hora = i.fecha_hora,
                impuesto = i.impuesto,
                total = i.total,
                estado = i.estado
            });
        }

        // GET: api/Ventas/ListarDetalles
        [Authorize(Roles = "Vendedor, Administrador")]
        [HttpGet("[action]/{idventa}")]
        public async Task<IEnumerable<DetalleViewModel>> ListarDetalles([FromRoute] int idventa)
        {
            var detalle = await _context.DetallesVentas
                .Include(d => d.articulo)
                .Where(d => d.idventa == idventa)
                .ToListAsync();

            return detalle.Select(d => new DetalleViewModel
            {
                idarticulo = d.idarticulo,
                articulo = d.articulo.nombre,
                cantidad = d.cantidad,
                precio = d.precio,
                descuento = d.descuento
            });
        }

        // POST: api/Ventas/Crear
        [Authorize(Roles = "Vendedor, Administrador")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fechaHora = DateTime.Now;

            Venta venta = new Venta
            {
                idcliente = model.idcliente,
                idusuario = model.idusuario,
                tipo_comprobante = model.tipo_comprobante,
                num_comprobante = model.num_comprobante,
                serie_comprobante = model.serie_comprobante,
                fecha_hora = fechaHora,
                impuesto = model.impuesto,
                total = model.total,
                estado = "Aceptado"
            };

            try
            {
                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                var id = venta.idventa;
                foreach (var det in model.detalles)
                {
                    DetalleVenta detalle = new DetalleVenta
                    {
                        idventa = id,
                        idarticulo = det.idarticulo,
                        cantidad = det.cantidad,
                        precio = det.precio,
                        descuento = det.descuento
                    };
                    _context.DetallesVentas.Add(detalle);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }

        // PUT: api/Ventas/Anular/1
        [Authorize(Roles = "Vendedor, Administrador")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Anular([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var venta = await _context.Ventas.FirstOrDefaultAsync(i => i.idventa == id);

            if (venta == null)
            {
                return NotFound();
            }

            venta.estado = "Anulado";

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Guardar Excepción
                return BadRequest();
            }

            return Ok();
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.idventa == id);
        }
    }
}
