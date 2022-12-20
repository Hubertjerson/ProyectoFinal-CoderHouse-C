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
        public ActionResult<List<Venta>> GetAll(int IdUsuario )
        {
            try
            {
                return repository.GetVentas(IdUsuario);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="venta"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RegistrarVenta")]
        public ActionResult Post([FromBody] Venta venta)
        {
            try
            {
                repository.RegistrarVenta(venta);
                return Ok("Se registro su Venta");
            }
            catch (Exception ex)
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
