using ApiGestionVenta.Repositories;
using ApiSistemaDeVentas.Models;
using System.Data.SqlClient;

namespace ProyectoFinal.Repositories
{
    public class LoginRepository
    {
        public bool verificarUser(Usuario usuario)
        {
            using (SqlConnection conexion = new SqlConnection(Conexion.cadenaConexion))
                try
                {
                    using(SqlCommand cmd = new SqlCommand("SELECT * FROM Usuario WHERE NombreUsuario = @NombreUsuario AND Contraseña = @Contrasenia", conexion))
                    {
                        conexion.Open();
                        cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                        cmd.Parameters.AddWithValue("Contrasenia", usuario.Contrasenia);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            return reader.HasRows;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    conexion.Close();
                }
        }
    }
}
