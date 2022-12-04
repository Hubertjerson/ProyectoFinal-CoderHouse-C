using Microsoft.AspNetCore.Mvc;
using ApiSistemaDeVentas.Models;
using ApiGestionVenta.Repositories;
using SistemaVentasApi.Repositories;

namespace SistemaVentasApi.Controllers
{
    [ApiController]
    [Route("Controller")]
    public class ProductoVendidoController : Controller
    {
        private ProductoVentaRepository repository = new ProductoVentaRepository();
        [HttpGet]
        [Route("ProductosVendidos")]
        public ActionResult<List<ProductoVendido>> listarProductoVendido()
        {
            try
            {
                List<ProductoVendido> lista = repository.listarProductoVendido();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpPost]
        [Route("CrearProductoVendido")]
        public ActionResult CrearProductoVendido([FromBody] ProductoVendido vendido)
        {
            try
            {
                repository.crearProductoVendido(vendido);
                return StatusCode(StatusCodes.Status201Created, vendido);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpGet]
        [Route("ProductoVendido/{id}")]
        public ActionResult<ProductoVendido> Get(int id)
        {
            try
            {
                ProductoVendido? productoVendido = repository.obtenerProductoVendido(id);
                if(productoVendido != null)
                {
                    return Ok(productoVendido);
                }
                else
                {
                    return NotFound("El producto no fue encontrado");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpPut]
        [Route("ActualizarProductoVendido/{id}")]
        public ActionResult<ProductoVendido> actualizar(int id, [FromBody] ProductoVendido vendidomodif)
        {
            try
            {
                ProductoVendido? productoNew = repository.modificarProductoVendido(id, vendidomodif);
                if(productoNew != null)
                {
                    return Ok(productoNew);
                }
                else
                {
                    return NotFound("El producto no pudo actualizar");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete]
        [Route("EliminarProductoVendido")]
        public ActionResult Delete([FromBody] int id)
        {
            try
            {
                bool seElimino = repository.eliminarProductoVendido(id);
                if(seElimino)
                {
                    return Ok("Se elimino correctamente");
                }
                else
                {
                    return NotFound("Id ingresado Ya fue eliminado");
                }
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
