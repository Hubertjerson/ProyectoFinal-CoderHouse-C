using Microsoft.AspNetCore.Mvc;
using ApiSistemaDeVentas.Models;
using SistemaVentasApi.Repositories;

namespace SistemaVentasApi.Controllers
{
    [ApiController]
    [Route("Controller")]
    public class ProductoVendidoController : Controller
    {
        private ProductoVentaRepository repository = new ProductoVentaRepository();
        [HttpGet]
        [Route("TraerProductosVendidos")]
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
    }
}
