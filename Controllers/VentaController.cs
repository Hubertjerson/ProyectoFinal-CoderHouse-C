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

        [HttpGet]
        [Route("TraerVentas")]
        public IEnumerable<Venta> GetVentas(int idUsuario)
        {
            return VentaRepository.listarVenta(idUsuario);
        }

        [HttpPost]
        [Route("CargarVenta/{idUsuario}")]
        public void PostVenta(List<Producto> productos, int idUsuario)
        {
            try
            {
                VentaRepository.InsertVenta(productos, idUsuario);
            }
            catch (Exception ex)
            {
                Problem(ex.Message);
            }
        }
    }
}
