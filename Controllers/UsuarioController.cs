using ApiGestionVenta.Repositories;
using ApiSistemaDeVentas.Models;
using Microsoft.AspNetCore.Mvc;

namespace SistemaVentasApi.Controllers
{
    [ApiController]
    [Route("controller")]
    public class UsuarioController : Controller
    {
        private UsuarioRepository repository = new UsuarioRepository();

        [HttpGet]
        [Route("TraerUsuarios")]
        public ActionResult<List<Usuario>> listarUsuario()
        {
            try
            {
                List<Usuario> lista = repository.listarUsuarios();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("CrearUsuarios")]
        public ActionResult CrearUsuarios([FromBody] Usuario usuario)
        {
            try
            {
                repository.crearUsuario(usuario);
                return StatusCode(StatusCodes.Status201Created, usuario);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpGet]
        [Route("Usuario/{id}")]
        public ActionResult<Usuario> Get(int id)
        {
            try
            {
                Usuario? usuario = repository.obtenerUsuario(id);
                if(usuario != null)
                {
                    return Ok(usuario);
                }
                else
                {
                    return NotFound("El usuario no fue encontrado");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpPut]
        [Route("ModificarUsuario/{id}")]
        public ActionResult<Usuario> actualizar (int id, [FromBody] Usuario usuariomodif)
        {
            try
            {
                Usuario? usuarioNuevo = repository.modificarUsuario(id, usuariomodif);
                if(usuarioNuevo != null)
                {
                    return Ok(usuarioNuevo);
                }
                else
                {
                    return NotFound("No se pudo actualizar");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpDelete]
        [Route("EliminarUsuario")]
        public ActionResult Delete([FromBody]int id)
        {
            try
            {
                bool seElimino = repository.eliminarUsuario(id);
                if(seElimino)
                {
                    return Ok("Se elimnino correctamente");
                }
                else
                {
                    return NotFound("Id ingresado ya fue eliminado");
                }
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
