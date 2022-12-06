using ApiSistemaDeVentas.Models;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal.Repositories;

namespace ProyectoFinal.Controllers
{
    [ApiController]
    [Route("controller")]
    public class LoginController : Controller
    {
        LoginRepository repository = new LoginRepository();

        [HttpPost]
        [Route("InicioDeSesion")]
        public ActionResult<Usuario>Login(Usuario usuario)
        {
            try
            {
                bool usuarioExiste = repository.verificarUser(usuario);
                return usuarioExiste ? Ok():NotFound();
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
