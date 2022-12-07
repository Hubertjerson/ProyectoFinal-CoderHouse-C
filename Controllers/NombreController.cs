using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("controller")]
    public class NombreController : Controller
    {
        [HttpGet]
        [Route("NombreDeApi")]
        public string Get()
        {
            return "Sistema De Gestion de ventas-Jerson Huaman";
        }
    }
}
