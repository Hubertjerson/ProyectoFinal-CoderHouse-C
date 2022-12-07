using ApiGestionVenta.Repositories;
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
        [Route("TraerVentas/{IdUsuario}")]
        public IEnumerable<Venta> GetVentas(int idUsuario)
        {
            return VentaRepository.GetVentas(idUsuario);
        }

        [HttpPost]
        [Route("CargarVenta/{idUsuario}")]
        public void PostVenta(List<Producto> productos, int idUsuario)
        {
            VentaRepository.InsertVenta(productos, idUsuario);
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
                    return Ok("Se elimnino correctamente");
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
