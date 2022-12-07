using Microsoft.AspNetCore.Mvc;
using ApiSistemaDeVentas.Models;
using ApiGestionVenta.Repositories;

namespace ApiSistemaDeVentas.Controllers
{
    [ApiController]
    [Route("controller")]
    public class ProductoController : Controller
    {
        private ProductosRepository repository = new ProductosRepository();

        [HttpGet]
        [Route("TraerProductos")]
        public ActionResult<List<Producto>> listarProducto()
        {
            try
            {
                List<Producto> lista = repository.listarProductos();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpPost]
        [Route("CrearProducto")]
        public ActionResult CrearProducto([FromBody] Producto producto)
        {
            try
            {
                repository.crearProducto(producto);
                return StatusCode(StatusCodes.Status201Created,producto);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpGet]
        [Route("Producto/{id}")]
        public ActionResult<Producto> Get(int id)
        {
            try
            {
                Producto? producto = repository.obtenerProducto(id);
                if (producto != null)
                {
                    return Ok(producto);
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
        [Route("ModificarProducto/{id}")]
        public ActionResult<Producto> Actualizar(int id, [FromBody] Producto productoModif)
        {
            try
            {
                Producto? productoNuevo = repository.modificarProducto(id, productoModif);
                if(productoNuevo != null)
                {
                    return Ok(productoNuevo);
                }
                else
                {
                    return NotFound("El producto no fue actualizado");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
            
        [HttpDelete]
        [Route("EliminarProducto/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                bool seElimino = repository.eliminarProducto(id);
                if (seElimino)
                {
                    return Ok("Se elimino correctamente");
                }
                else
                {
                    return NotFound("Id ingresado Ya fue eliminado");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
