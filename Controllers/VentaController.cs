using ApiSistemaDeVentas.Models;
using Microsoft.AspNetCore.Mvc;
using SistemaVentasApi.Repositories;
using System;

namespace SistemaVentasApi.Controllers
{
    [ApiController]
    [Route("controller")]
    public class VentaController : Controller
    {
        private VentaRepository repository = new VentaRepository();

        [HttpGet]
        [Route("TraerVentas")]
        public ActionResult<List<Venta>> listarVenta()
        {
            try
            {
                List<Venta> lista = new List<Venta>();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpPost]
        [Route("CrearVenta")]
        public ActionResult CrearUsuario([FromBody] Venta venta)
        {
            try
            {
                repository.crearVenta(venta);
                return StatusCode(StatusCodes.Status201Created, venta);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpGet]
        [Route("Ventas/{id}")]
        public ActionResult<Venta> Get(int id)
        {
            try
            {
                Venta? venta = repository.obtenerVenta(id);
                if(venta != null)
                {
                    return Ok(venta);
                }
                else
                {
                    return NotFound("La venta no fue encontrada");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpPut]
        [Route("ActualizarVenta/{id}")]
        public ActionResult<Venta> Actualizar(int id ,[FromBody] Venta ventamodif)
        {
            try
            {
                Venta? ventaNuevo = repository.modificarVenta(id, ventamodif);
                if(ventaNuevo != null)
                {
                    return Ok(ventaNuevo);
                }
                else
                {
                    return NotFound("No se pudo actualizar venta");
                }
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete]
        [Route("EliminarVentas")]
        public ActionResult Delete([FromBody] int id)
        {
            try
            {
                bool seElimino = repository.eliminarVenta(id);
                if (seElimino)
                {
                    return Ok("Se elimino correctamente");
                }
                else
                {
                    return NotFound("Id ingresado ya fue eliminado");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
